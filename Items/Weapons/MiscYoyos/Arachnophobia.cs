using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscYoyos
{
    public class Arachnophobia : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aracnoyo");
            Tooltip.SetDefault("Throws 8 yoyos at once!");
            ItemID.Sets.Yoyo[item.type] = true;
            ItemID.Sets.GamepadExtraRange[item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.width = 30;
            item.height = 26;
            item.useAnimation = 25;
            item.useTime = 25;
            item.shootSpeed = 16f;
            item.knockBack = 2.5f;
            item.damage = 38;
            item.value = 50000;
            item.rare = 4;
            item.melee = true;
            item.channel = true;
            item.noMelee = true;
            item.noUseGraphic = true;

            item.UseSound = SoundID.Item1;

            item.shoot = mod.ProjectileType("ArachnophobiaP");
        }

        private Projectile yoyo;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int n = 0; n < 8; n++)
            {
                yoyo = Main.projectile[Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI)];
                yoyo.localAI[1] = n;
            }

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.SpiderFang, 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class ArachnophobiaP : QwertyYoyo
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            // aiStyle 99 is used for all yoyos, and is Extremely suggested, as yoyo are extremely difficult without them
            //projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1f;

            yoyoCount = 8;
            time = -1f;
            range = 120;
            speed = 16f;
            counterWeightId = mod.ProjectileType("SpiderCounterweight");
        }

        public override void YoyoHit(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 360);
        }
    }

    public class SpiderCounterweight : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // The following sets are only applicable to yoyo that use aiStyle 99.
            // YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player.
            // Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
            //ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = -1f;
            // YoyosMaximumRange is the maximum distance the yoyo sleep away from the player.
            // Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
            //ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 120f;
            // YoyosTopSpeed is top speed of the yoyo projectile.
            // Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
            //ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 13f;
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 10;
            projectile.height = 10;
            // aiStyle 99 is used for all yoyos, and is Extremely suggested, as yoyo are extremely difficult without them
            //projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1f;
        }

        // notes for aiStyle 99:
        // localAI[0] is used for timing up to YoyosLifeTimeMultiplier
        // localAI[1] can be used freely by specific types
        // ai[0] and ai[1] usually point towards the x and y world coordinate hover point
        // ai[0] is -1f once YoyosLifeTimeMultiplier is reached, when the player is stoned/frozen, when the yoyo is too far away, or the player is no longer clicking the shoot button.
        // ai[0] being negative makes the yoyo move back towards the player
        // Any AI method can be used for dust, spawning projectiles, etc specific to your yoyo.
        private float range = 120;

        private float speed = 13f;
        private float time = -1f;
        private Vector2 modifiedMousePosition;
        private int frameTimer;

        public override void AI()
        {
            frameTimer++;
            if (frameTimer > 40)
            {
                frameTimer = 0;
                projectile.frame = 0;
            }
            else if (frameTimer > 30)
            {
                projectile.frame = 2;
            }
            else if (frameTimer > 20)
            {
                projectile.frame = 0;
            }
            else if (frameTimer > 10)
            {
                projectile.frame = 1;
            }
            else
            {
                projectile.frame = 0;
            }
            projectile.timeLeft = 6;
            bool flag = true;
            float num = 250f;
            float scaleFactor = 0.1f;
            float num2 = 15f;
            float num3 = 12f;
            num *= 0.5f;
            num2 *= 0.8f;
            num3 *= 1.5f;
            if (projectile.owner == Main.myPlayer)
            {
                bool flag2 = false;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && Main.projectile[i].type == mod.ProjectileType("ArachnophobiaP"))
                    {
                        flag2 = true;
                    }
                }
                if (!flag2)
                {
                    projectile.ai[0] = -1f;
                    projectile.netUpdate = true;
                }
            }
            if (Main.player[projectile.owner].yoyoString)
            {
                num += num * 0.25f + 10f;
            }
            //projectile.rotation += 0.5f;
            if (Main.player[projectile.owner].dead)
            {
                projectile.Kill();
                return;
            }
            if (!flag)
            {
                Main.player[projectile.owner].heldProj = projectile.whoAmI;
                Main.player[projectile.owner].itemAnimation = 2;
                Main.player[projectile.owner].itemTime = 2;
                if (projectile.position.X + (float)(projectile.width / 2) > Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2))
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
            if (projectile.ai[0] >= 0f)
            {
                num *= projectile.ai[0];
                num3 *= 0.5f;
                bool flag3 = false;
                Vector2 vector = Main.player[projectile.owner].Center - projectile.Center;
                if ((double)vector.Length() > (double)num * 0.9)
                {
                    flag3 = true;
                }
                if (vector.Length() > num)
                {
                    float num4 = vector.Length() - num;
                    Vector2 vector2;
                    vector2.X = vector.Y;
                    vector2.Y = vector.X;
                    vector.Normalize();
                    vector *= num;
                    projectile.position = Main.player[projectile.owner].Center - vector;
                    projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                    projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
                    float num5 = projectile.velocity.Length();
                    projectile.velocity.Normalize();
                    if (num4 > num5 - 1f)
                    {
                        num4 = num5 - 1f;
                    }
                    projectile.velocity *= num5 - num4;
                    num5 = projectile.velocity.Length();
                    Vector2 vector3 = new Vector2(projectile.Center.X, projectile.Center.Y);
                    Vector2 vector4 = new Vector2(Main.player[projectile.owner].Center.X, Main.player[projectile.owner].Center.Y);
                    if (vector3.Y < vector4.Y)
                    {
                        vector2.Y = Math.Abs(vector2.Y);
                    }
                    else if (vector3.Y > vector4.Y)
                    {
                        vector2.Y = -Math.Abs(vector2.Y);
                    }
                    if (vector3.X < vector4.X)
                    {
                        vector2.X = Math.Abs(vector2.X);
                    }
                    else if (vector3.X > vector4.X)
                    {
                        vector2.X = -Math.Abs(vector2.X);
                    }
                    vector2.Normalize();
                    vector2 *= projectile.velocity.Length();
                    new Vector2(vector2.X, vector2.Y);
                    if (Math.Abs(projectile.velocity.X) > Math.Abs(projectile.velocity.Y))
                    {
                        Vector2 vector5 = projectile.velocity;
                        vector5.Y += vector2.Y;
                        vector5.Normalize();
                        vector5 *= projectile.velocity.Length();
                        if ((double)Math.Abs(vector2.X) < 0.1 || (double)Math.Abs(vector2.Y) < 0.1)
                        {
                            projectile.velocity = vector5;
                        }
                        else
                        {
                            projectile.velocity = (vector5 + projectile.velocity * 2f) / 3f;
                        }
                    }
                    else
                    {
                        Vector2 vector6 = projectile.velocity;
                        vector6.X += vector2.X;
                        vector6.Normalize();
                        vector6 *= projectile.velocity.Length();
                        if ((double)Math.Abs(vector2.X) < 0.2 || (double)Math.Abs(vector2.Y) < 0.2)
                        {
                            projectile.velocity = vector6;
                        }
                        else
                        {
                            projectile.velocity = (vector6 + projectile.velocity * 2f) / 3f;
                        }
                    }
                }
                if (Main.myPlayer == projectile.owner)
                {
                    if (Main.player[projectile.owner].channel)
                    {
                        Vector2 vector7 = new Vector2((float)(Main.mouseX - Main.lastMouseX), (float)(Main.mouseY - Main.lastMouseY));
                        if (projectile.velocity.X != 0f || projectile.velocity.Y != 0f)
                        {
                            if (flag)
                            {
                                vector7 *= -1f;
                            }
                            if (flag3)
                            {
                                if (projectile.Center.X < Main.player[projectile.owner].Center.X && vector7.X < 0f)
                                {
                                    vector7.X = 0f;
                                }
                                if (projectile.Center.X > Main.player[projectile.owner].Center.X && vector7.X > 0f)
                                {
                                    vector7.X = 0f;
                                }
                                if (projectile.Center.Y < Main.player[projectile.owner].Center.Y && vector7.Y < 0f)
                                {
                                    vector7.Y = 0f;
                                }
                                if (projectile.Center.Y > Main.player[projectile.owner].Center.Y && vector7.Y > 0f)
                                {
                                    vector7.Y = 0f;
                                }
                            }
                            projectile.velocity += vector7 * scaleFactor;
                            projectile.netUpdate = true;
                        }
                    }
                    else
                    {
                        projectile.ai[0] = 10f;
                        projectile.netUpdate = true;
                    }
                }
                if (flag)
                {
                    float num6 = 800f;
                    Vector2 vector8 = default(Vector2);
                    bool flag4 = false;

                    for (int j = 0; j < 200; j++)
                    {
                        if (Main.npc[j].CanBeChasedBy(projectile, false))
                        {
                            float num7 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                            float num8 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                            float num9 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num7) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num8);
                            if (num9 < num6 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[j].position, Main.npc[j].width, Main.npc[j].height) && (double)(Main.npc[j].Center - Main.player[projectile.owner].Center).Length() < (double)num * 0.9)
                            {
                                num6 = num9;
                                vector8.X = num7;
                                vector8.Y = num8;
                                flag4 = true;
                            }
                        }
                    }
                    if (flag4)
                    {
                        vector8 -= projectile.Center;
                        vector8.Normalize();

                        vector8 *= 6f;
                        projectile.velocity = (projectile.velocity * 7f + vector8) / 8f;
                    }
                }
                if (projectile.velocity.Length() > num2)
                {
                    projectile.velocity.Normalize();
                    projectile.velocity *= num2;
                }
                if (projectile.velocity.Length() < num3)
                {
                    projectile.velocity.Normalize();
                    projectile.velocity *= num3;
                    return;
                }
            }
            else
            {
                projectile.tileCollide = false;
                Vector2 vector9 = Main.player[projectile.owner].Center - projectile.Center;
                if (vector9.Length() < 40f || vector9.HasNaNs())
                {
                    projectile.Kill();
                    return;
                }
                float num10 = num2 * 1.5f;

                float num11 = 12f;
                vector9.Normalize();
                vector9 *= num10;
                projectile.velocity = (projectile.velocity * (num11 - 1f) + vector9) / num11;
            }
            projectile.rotation = (projectile.Center - Main.player[projectile.owner].Center).ToRotation() + (float)Math.PI / 2;
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 360);
            int num8 = Main.DamageVar((float)projectile.damage);
            //Main.player[projectile.owner].GetModPlayer<CustomYoyoPlayer>().Counterweight(target.Center, projectile.damage, projectile.knockBack, projectile, this);
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
                projectile.localAI[0] += 20f;
                if (!Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.player[projectile.owner].position, Main.player[projectile.owner].width, Main.player[projectile.owner].height))
                {
                    projectile.localAI[0] += 40f;
                    num8 = (int)((double)num8 * 0.75);
                }
            }
        }
    }
}