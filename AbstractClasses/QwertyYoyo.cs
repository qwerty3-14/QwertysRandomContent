using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.AbstractClasses
{
    public abstract class QwertyYoyo : ModProjectile
    {
        protected float range = 120;

        protected float speed = 16f;
        protected float time = -1f;
        public int yoyoCount = 1;
        public int counterWeightId = -1;

        public override void AI()
        {
            PreYoyoAI();
            {
                bool notMain = false;
                bool Orbital = false;
                for (int i = 0; i < projectile.whoAmI; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && Main.projectile[i].type == projectile.type)
                    {
                        notMain = true;
                    }
                }
                if (projectile.localAI[1] >= yoyoCount)
                {
                    Orbital = true;
                }

                //time managment
                if (projectile.owner == Main.myPlayer)
                {
                    projectile.localAI[0] += 1f;
                    float num = projectile.localAI[0] / 60f;
                    num /= (1f + Main.player[projectile.owner].meleeSpeed) / 2f;
                    float num2 = time;
                    if (num2 != -1f && num > num2)
                    {
                        projectile.ai[0] = -1f;
                    }
                }

                if (Main.player[projectile.owner].dead)
                {
                    projectile.Kill();
                    return;
                }
                if (!notMain)
                {
                    Main.player[projectile.owner].heldProj = projectile.whoAmI;
                    Main.player[projectile.owner].itemAnimation = 2;
                    Main.player[projectile.owner].itemTime = 2;
                    if (projectile.Center.X > Main.player[projectile.owner].Center.X)
                    {
                        Main.player[projectile.owner].ChangeDir(1);
                        projectile.direction = 1;
                    }
                    else
                    {
                        Main.player[projectile.owner].ChangeDir(-1);
                        projectile.direction = -1;
                    }
                }
                if (projectile.velocity.HasNaNs())
                {
                    projectile.Kill();
                }
                projectile.timeLeft = 6;

                float useRange = range;
                float useSpeed = speed;
                if (Main.player[projectile.owner].yoyoString)
                {
                    useRange = useRange * 1.25f + 30f;
                }
                useRange /= (1f + Main.player[projectile.owner].meleeSpeed * 3f) / 4f;
                useSpeed /= (1f + Main.player[projectile.owner].meleeSpeed * 3f) / 4f;
                float num11 = 14f - useSpeed / 2f;
                float num12 = 5f + useSpeed / 2f;
                if (Orbital)
                {
                    num12 += 20f;
                }
                if (projectile.ai[0] >= 0f)
                {
                    if (projectile.velocity.Length() > useSpeed)
                    {
                        projectile.velocity *= 0.98f;
                    }
                    bool flag3 = false;
                    bool flag4 = false;
                    Vector2 vector3 = Main.player[projectile.owner].Center - projectile.Center;
                    if (vector3.Length() > useRange)
                    {
                        flag3 = true;
                        if ((double)vector3.Length() > (double)useRange * 1.3)
                        {
                            //flag4 = true;
                        }
                    }
                    if (projectile.owner == Main.myPlayer)
                    {
                        if (!Main.player[projectile.owner].channel || Main.player[projectile.owner].stoned || Main.player[projectile.owner].frozen)
                        {
                            projectile.ai[0] = -1f;
                            projectile.ai[1] = 0f;
                            projectile.netUpdate = true;
                        }
                        else
                        {
                            Vector2 vector4 = Main.ReverseGravitySupport(Main.MouseScreen, 0f) + Main.screenPosition;

                            float rotationDirection = (vector4 - Main.player[projectile.owner].Center).ToRotation() + (2 * (float)Math.PI / yoyoCount * projectile.localAI[1]);
                            float distance = (vector4 - Main.player[projectile.owner].Center).Length();
                            vector4 = new Vector2(Main.player[projectile.owner].Center.X + (float)Math.Cos(rotationDirection) * distance, Main.player[projectile.owner].Center.Y + (float)Math.Sin(rotationDirection) * distance);
                            //Dust.NewDust(vector4, 0, 0, 1);
                            //Projectile.NewProjectile(vector4, Vector2.Zero, ProjectileID.WoodenArrowFriendly, 10, 0, projectile.owner);
                            //Main.NewText(vector4);
                            float x = vector4.X;
                            float y = vector4.Y;
                            Vector2 distVector = new Vector2(x, y) - Main.player[projectile.owner].Center;
                            if (distVector.Length() > useRange)
                            {
                                distVector.Normalize();
                                distVector *= useRange;
                                distVector = Main.player[projectile.owner].Center + distVector;
                                x = distVector.X;
                                y = distVector.Y;
                            }
                            if (projectile.ai[0] != x || projectile.ai[1] != y)
                            {
                                Vector2 vector6 = new Vector2(x, y);
                                Vector2 vector7 = vector6 - Main.player[projectile.owner].Center;
                                if (vector7.Length() > useRange - 1f)
                                {
                                    vector7.Normalize();
                                    vector7 *= useRange - 1f;
                                    vector6 = Main.player[projectile.owner].Center + vector7;
                                    x = vector6.X;
                                    y = vector6.Y;
                                }
                                projectile.ai[0] = x;
                                projectile.ai[1] = y;
                                //Main.NewText(new Vector2(x, y));
                                projectile.netUpdate = true;
                            }
                        }
                    }
                    if (flag4 && projectile.owner == Main.myPlayer)
                    {
                        projectile.ai[0] = -1f;
                        projectile.netUpdate = true;
                    }
                    if (projectile.ai[0] >= 0f)
                    {
                        if (flag3)
                        {
                            num11 /= 2f;
                            useSpeed *= 2f;
                            if (projectile.Center.X > Main.player[projectile.owner].Center.X && projectile.velocity.X > 0f)
                            {
                                projectile.velocity.X = projectile.velocity.X * 0.5f;
                            }
                            if (projectile.Center.Y > Main.player[projectile.owner].Center.Y && projectile.velocity.Y > 0f)
                            {
                                projectile.velocity.Y = projectile.velocity.Y * 0.5f;
                            }
                            if (projectile.Center.X < Main.player[projectile.owner].Center.X && projectile.velocity.X > 0f)
                            {
                                projectile.velocity.X = projectile.velocity.X * 0.5f;
                            }
                            if (projectile.Center.Y < Main.player[projectile.owner].Center.Y && projectile.velocity.Y > 0f)
                            {
                                projectile.velocity.Y = projectile.velocity.Y * 0.5f;
                            }
                        }
                        Vector2 value = new Vector2(projectile.ai[0], projectile.ai[1]);
                        //Main.NewText(value);
                        Vector2 vector8 = value - projectile.Center;
                        projectile.velocity.Length();
                        float num13 = vector8.Length();
                        if (num13 > num12)
                        {
                            vector8.Normalize();
                            float scaleFactor = (num13 > useSpeed * 2f) ? useSpeed : (num13 / 2f);
                            vector8 *= scaleFactor;
                            projectile.velocity = (projectile.velocity * (num11 - 1f) + vector8) / num11;
                            //Main.NewText(projectile.velocity);
                        }
                        else if (Orbital)
                        {
                            if ((double)projectile.velocity.Length() < (double)useSpeed * 0.6)
                            {
                                vector8 = projectile.velocity;
                                vector8.Normalize();
                                vector8 *= useSpeed * 0.6f;
                                projectile.velocity = (projectile.velocity * (num11 - 1f) + vector8) / num11;
                            }
                        }
                        else
                        {
                            projectile.velocity *= 0.8f;
                        }
                        if (Orbital && !flag3 && (double)projectile.velocity.Length() < (double)useSpeed * 0.6)
                        {
                            projectile.velocity.Normalize();
                            projectile.velocity *= useSpeed * 0.6f;
                        }
                    }
                }
                else
                {
                    num11 = (float)((int)((double)num11 * 0.8));
                    useSpeed *= 1.5f;
                    projectile.tileCollide = false;
                    Vector2 vector9 = Main.player[projectile.owner].position - projectile.Center;
                    float num14 = vector9.Length();
                    if (num14 < useSpeed + 10f || num14 == 0f)
                    {
                        projectile.Kill();
                    }
                    else
                    {
                        vector9.Normalize();
                        vector9 *= useSpeed;
                        projectile.velocity = (projectile.velocity * (num11 - 1f) + vector9) / num11;
                    }
                }
                projectile.rotation += 0.45f;
            }
            PostYoyoAI();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            projectile.localAI[1] = reader.ReadSingle();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bool notMain10 = false;
            if (projectile.velocity.X != oldVelocity.X)
            {
                notMain10 = true;
                projectile.velocity.X = oldVelocity.X * -1f;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                notMain10 = true;
                projectile.velocity.Y = oldVelocity.Y * -1f;
            }
            if (notMain10)
            {
                Vector2 vector10 = Main.player[projectile.owner].Center - projectile.Center;
                vector10.Normalize();
                vector10 *= projectile.velocity.Length();
                vector10 *= 0.25f;
                projectile.velocity *= 0.75f;
                projectile.velocity += vector10;
                if (projectile.velocity.Length() > 6f)
                {
                    projectile.velocity *= 0.5f;
                }
            }
            return false;
        }

        public virtual void YoyoHit(NPC target, int damage, float knockback, bool crit)
        {
        }

        public virtual void PreYoyoAI()
        {
        }

        public virtual void PostYoyoAI()
        {
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //Main.player[projectile.owner].Counterweight(target.Center, projectile.damage, projectile.knockBack, this);
            Main.player[projectile.owner].GetModPlayer<CustomYoyoPlayer>().Counterweight(target.Center, projectile.damage, projectile.knockBack, projectile, this);
            if (target.Center.X < Main.player[projectile.owner].Center.X)
            {
                projectile.direction = -1;
            }
            else
            {
                projectile.direction = 1;
            }

            if (projectile.ai[0] >= 0f)
            {
                Vector2 value2 = projectile.Center - target.Center;
                value2.Normalize();
                float scaleFactor = 16f;
                projectile.velocity *= -0.5f;
                projectile.velocity += value2 * scaleFactor;
                projectile.netUpdate = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
            Vector2 vector = mountedCenter;
            vector.Y += Main.player[projectile.owner].gfxOffY;
            float num3 = projectile.Center.X - vector.X;
            float num4 = projectile.Center.Y - vector.Y;
            Math.Sqrt((double)(num3 * num3 + num4 * num4));
            float rotation = (float)Math.Atan2((double)num4, (double)num3) - 1.57f;
            if (!projectile.counterweight)
            {
                int num5 = -1;
                if (projectile.position.X + (float)(projectile.width / 2) < Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2))
                {
                    num5 = 1;
                }
                num5 *= -1;
                Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(num4 * (float)num5), (double)(num3 * (float)num5));
            }
            bool notMain = true;
            if (num3 == 0f && num4 == 0f)
            {
                notMain = false;
            }
            else
            {
                float num6 = (float)Math.Sqrt((double)(num3 * num3 + num4 * num4));
                num6 = 12f / num6;
                num3 *= num6;
                num4 *= num6;
                vector.X -= num3 * 0.1f;
                vector.Y -= num4 * 0.1f;
                num3 = projectile.position.X + (float)projectile.width * 0.5f - vector.X;
                num4 = projectile.position.Y + (float)projectile.height * 0.5f - vector.Y;
            }
            while (notMain)
            {
                float num7 = 12f;
                float num8 = (float)Math.Sqrt((double)(num3 * num3 + num4 * num4));
                float num9 = num8;
                if (float.IsNaN(num8) || float.IsNaN(num9))
                {
                    notMain = false;
                }
                else
                {
                    if (num8 < 20f)
                    {
                        num7 = num8 - 8f;
                        notMain = false;
                    }
                    num8 = 12f / num8;
                    num3 *= num8;
                    num4 *= num8;
                    vector.X += num3;
                    vector.Y += num4;
                    num3 = projectile.position.X + (float)projectile.width * 0.5f - vector.X;
                    num4 = projectile.position.Y + (float)projectile.height * 0.1f - vector.Y;
                    if (num9 > 12f)
                    {
                        float num10 = 0.3f;
                        float num11 = Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y);
                        if (num11 > 16f)
                        {
                            num11 = 16f;
                        }
                        num11 = 1f - num11 / 16f;
                        num10 *= num11;
                        num11 = num9 / 80f;
                        if (num11 > 1f)
                        {
                            num11 = 1f;
                        }
                        num10 *= num11;
                        if (num10 < 0f)
                        {
                            num10 = 0f;
                        }
                        num10 *= num11;
                        num10 *= 0.5f;
                        if (num4 > 0f)
                        {
                            num4 *= 1f + num10;
                            num3 *= 1f - num10;
                        }
                        else
                        {
                            num11 = Math.Abs(projectile.velocity.X) / 3f;
                            if (num11 > 1f)
                            {
                                num11 = 1f;
                            }
                            num11 -= 0.5f;
                            num10 *= num11;
                            if (num10 > 0f)
                            {
                                num10 *= 2f;
                            }
                            num4 *= 1f + num10;
                            num3 *= 1f - num10;
                        }
                    }
                    rotation = (float)Math.Atan2((double)num4, (double)num3) - 1.57f;
                    int stringColor = Main.player[projectile.owner].stringColor;
                    Microsoft.Xna.Framework.Color color = WorldGen.paintColor(stringColor);
                    if (color.R < 75)
                    {
                        color.R = 75;
                    }
                    if (color.G < 75)
                    {
                        color.G = 75;
                    }
                    if (color.B < 75)
                    {
                        color.B = 75;
                    }
                    if (stringColor == 13)
                    {
                        color = new Microsoft.Xna.Framework.Color(20, 20, 20);
                    }
                    else if (stringColor == 14 || stringColor == 0)
                    {
                        color = new Microsoft.Xna.Framework.Color(200, 200, 200);
                    }
                    else if (stringColor == 28)
                    {
                        color = new Microsoft.Xna.Framework.Color(163, 116, 91);
                    }
                    else if (stringColor == 27)
                    {
                        color = new Microsoft.Xna.Framework.Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
                    }
                    color.A = (byte)((float)color.A * 0.4f);
                    float num12 = 0.5f;
                    color = Lighting.GetColor((int)vector.X / 16, (int)(vector.Y / 16f), color);
                    color = new Microsoft.Xna.Framework.Color((int)((byte)((float)color.R * num12)), (int)((byte)((float)color.G * num12)), (int)((byte)((float)color.B * num12)), (int)((byte)((float)color.A * num12)));
                    Main.spriteBatch.Draw(Main.fishingLineTexture, new Vector2(vector.X - Main.screenPosition.X + (float)Main.fishingLineTexture.Width * 0.5f, vector.Y - Main.screenPosition.Y + (float)Main.fishingLineTexture.Height * 0.5f) - new Vector2(6f, 0f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.fishingLineTexture.Width, (int)num7)), color, rotation, new Vector2((float)Main.fishingLineTexture.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
                }
            }
            return true;
        }
    }

    public class CustomYoyoPlayer : ModPlayer
    {
        private bool farCounter = false;

        public void Counterweight(Vector2 hitPos, int dmg, float kb, Projectile parent, QwertyYoyo yoyoData)
        {
            if (!player.yoyoGlove && player.counterWeight <= 0)
            {
                return;
            }
            int num2 = 0;
            int num3 = 0;
            bool noChild = true;
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].localAI[1] == parent.localAI[1] + yoyoData.yoyoCount)
                {
                    noChild = false;
                }
            }
            if (parent.localAI[1] < yoyoData.yoyoCount && noChild && player.yoyoGlove)
            {
                Vector2 vector = hitPos - player.Center;
                vector.Normalize();
                vector *= 16f;
                for (int y = 0; y < yoyoData.yoyoCount; y++)
                {
                    Projectile yoyo = Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, vector.X, vector.Y, parent.type, parent.damage, parent.knockBack, player.whoAmI, 1f, 0f)];
                    yoyo.localAI[1] = yoyoData.yoyoCount + y;
                }
            }
            else if (yoyoData.counterWeightId != -1)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI)
                    {
                        if (Main.projectile[i].type == yoyoData.counterWeightId)
                        {
                            num3++;
                        }
                        else if (Main.projectile[i].type == yoyoData.projectile.type)
                        {
                            num2++;
                        }
                    }
                }

                if (num3 < num2)
                {
                    Vector2 vector2 = hitPos - player.Center;
                    vector2.Normalize();
                    vector2 *= 16f;
                    float knockBack = (kb + 6f) / 2f;
                    int counterCount = 0;
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == yoyoData.counterWeightId)
                        {
                            counterCount++;
                        }
                    }
                    Projectile.NewProjectile(player.Center.X, player.Center.Y, vector2.X, vector2.Y, yoyoData.counterWeightId, (int)((double)dmg * 0.8), knockBack, player.whoAmI, .5f + 1f * ((float)counterCount / (2f * yoyoData.yoyoCount)), 0f);
                    farCounter = true;
                }
            }
        }
    }
}