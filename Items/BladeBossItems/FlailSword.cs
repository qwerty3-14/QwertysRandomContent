using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.BladeBossItems
{
    public class FlailSword : ModItem
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicImperious ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Impelial");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.useAnimation = 20;
            item.useTime = 20;
            item.shootSpeed = 24f;
            item.knockBack = 2f;
            item.width = 16;
            item.height = 16;
            item.UseSound = SoundID.Item101;
            item.shoot = mod.ProjectileType("FlailSwordP");
            item.rare = 7;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.noMelee = true;
            item.noUseGraphic = true;
            item.channel = true;
            item.autoReuse = true;
            item.melee = true;
            item.damage = 75;
        }
    }

    /*
    public class flailSwordPlayer : ModPlayer
    {
        public override void PreUpdate()
        {
            Item item = player.inventory[player.selectedItem];
            if (item.type == mod.ItemType("FlailSword") && player.channel  && player.itemAnimation > 0 && player.itemTime == 0)
            {
                float ai3 = (Main.rand.NextFloat() - 0.5f) * 0.7853982f;
                Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
                float num81 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                float num82 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                Vector2 vector13 = new Vector2(num81, num82);
                Projectile.NewProjectile(vector2.X, vector2.Y, vector13.X, vector13.Y, item.shoot, item.damage, item.knockBack, player.whoAmI, 0f, ai3);
                player.itemTime = item.useTime;
            }
        }
    }
    */

    public class FlailSwordP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            //projectile.aiStyle = 75;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.hide = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
            Texture2D texture2D22 = Main.projectileTexture[projectile.type];
            Color alpha = projectile.GetAlpha(lightColor);
            if (projectile.velocity == Vector2.Zero)
            {
                return false;
            }
            float ChainLength = projectile.velocity.Length() + 16f;
            bool shortChain = ChainLength < 100f;
            Vector2 normalisedVelocity = Vector2.Normalize(projectile.velocity);
            Rectangle frame = new Rectangle(0, 2, texture2D22.Width, 30); //handle
            Vector2 playerHillClimbOffset = new Vector2(0f, Main.player[projectile.owner].gfxOffY);
            //Main.NewText(playerHillClimbOffset);
            float drawRotation = projectile.rotation + 3.14159274f;
            Main.spriteBatch.Draw(texture2D22, projectile.Center.Floor() - Main.screenPosition + playerHillClimbOffset, new Rectangle?(frame), alpha, drawRotation, new Vector2(5, 17), projectile.scale, SpriteEffects.None, 0f);
            ChainLength -= 40f * projectile.scale;
            Vector2 shiftingPosition = projectile.Center.Floor();
            shiftingPosition += normalisedVelocity * projectile.scale * 11f;

            float segment = 0f;

            int segmentCount = 10;

            frame = new Rectangle(0, 74, texture2D22.Width, 10);

            if (ChainLength > 0f)
            {
                float spacing = ChainLength / (segmentCount + 1);
                while (segment < segmentCount * spacing)
                {
                    segment += (float)spacing * projectile.scale;
                    shiftingPosition += normalisedVelocity * spacing * projectile.scale;
                    Main.spriteBatch.Draw(texture2D22, shiftingPosition - Main.screenPosition + playerHillClimbOffset, new Microsoft.Xna.Framework.Rectangle?(frame), alpha, drawRotation + (float)Math.PI / 2, texture2D22.Frame(1, 1, 0, 0).Top(), projectile.scale, SpriteEffects.None, 0f);
                    Main.spriteBatch.Draw(texture2D22, shiftingPosition - Main.screenPosition + playerHillClimbOffset, new Microsoft.Xna.Framework.Rectangle?(frame), alpha, drawRotation - (float)Math.PI / 2, texture2D22.Frame(1, 1, 0, 0).Top(), projectile.scale, SpriteEffects.None, 0f);
                }
            }
            frame = new Rectangle(0, 32, texture2D22.Width, 10);
            shiftingPosition = projectile.Center.Floor();
            shiftingPosition += normalisedVelocity * projectile.scale * 11f;
            segment = 0f;
            if (ChainLength > 0f)
            {
                while (segment + 1f < ChainLength / 2)
                {
                    if (ChainLength - segment < (float)frame.Height)
                    {
                        frame.Height = (int)(ChainLength - segment);
                    }
                    Main.spriteBatch.Draw(texture2D22, shiftingPosition - Main.screenPosition + playerHillClimbOffset, new Rectangle?(frame), alpha, drawRotation, new Vector2((float)(frame.Width / 2), 0f), projectile.scale, SpriteEffects.None, 0f);
                    segment += (float)frame.Height * projectile.scale;
                    shiftingPosition += normalisedVelocity * (float)frame.Height * projectile.scale;
                }
            }

            frame = new Rectangle(0, 52, texture2D22.Width, 10);

            float CL2 = ChainLength;
            if (ChainLength > 0f)
            {
                while (segment + 1f < ChainLength)
                {
                    if (ChainLength - segment < (float)frame.Height)
                    {
                        frame.Height = (int)(ChainLength - segment);
                    }
                    Main.spriteBatch.Draw(texture2D22, shiftingPosition - Main.screenPosition + playerHillClimbOffset, new Rectangle?(frame), alpha, drawRotation, new Vector2((float)(frame.Width / 2), 0f), projectile.scale, SpriteEffects.None, 0f);
                    segment += (float)frame.Height * projectile.scale;
                    shiftingPosition += normalisedVelocity * (float)frame.Height * projectile.scale;
                }
            }
            Vector2 tipPosition = shiftingPosition;
            frame = new Rectangle(0, 72, texture2D22.Width, 12);

            Main.spriteBatch.Draw(texture2D22, tipPosition - Main.screenPosition + playerHillClimbOffset, new Microsoft.Xna.Framework.Rectangle?(frame), alpha, drawRotation + (float)Math.PI / 4, texture2D22.Frame(1, 1, 0, 0).Top(), projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture2D22, tipPosition - Main.screenPosition + playerHillClimbOffset, new Microsoft.Xna.Framework.Rectangle?(frame), alpha, drawRotation - (float)Math.PI / 4, texture2D22.Frame(1, 1, 0, 0).Top(), projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture2D22, tipPosition - Main.screenPosition + playerHillClimbOffset, new Microsoft.Xna.Framework.Rectangle?(frame), alpha, drawRotation + (float)Math.PI / 8, texture2D22.Frame(1, 1, 0, 0).Top(), projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture2D22, tipPosition - Main.screenPosition + playerHillClimbOffset, new Microsoft.Xna.Framework.Rectangle?(frame), alpha, drawRotation - (float)Math.PI / 8, texture2D22.Frame(1, 1, 0, 0).Top(), projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture2D22, tipPosition - Main.screenPosition + playerHillClimbOffset, new Microsoft.Xna.Framework.Rectangle?(frame), alpha, drawRotation, texture2D22.Frame(1, 1, 0, 0).Top(), projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        private float r;
        private bool runOnce = true;

        public override void AI()
        {
            //copied from vanilla solar eruption AI
            Player player = Main.player[projectile.owner];
            float halfPi = 1.57079637f;
            Vector2 playerPoint = player.RotatedRelativePoint(player.MountedCenter, true);
            //Main.NewText(playerPoint);

            if (projectile.localAI[1] > 0f)
            {
                projectile.localAI[1] -= 1f;
            }
            projectile.alpha -= 42;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = projectile.velocity.ToRotation();
            }
            float num32 = (float)((projectile.localAI[0].ToRotationVector2().X >= 0f) ? 1 : -1);
            if (projectile.ai[1] <= 0f)
            {
                num32 *= -1f;
            }
            Vector2 vector17 = (num32 * (projectile.ai[0] / 30f * 6.28318548f - 1.57079637f)).ToRotationVector2();
            vector17.Y *= (float)Math.Sin((double)projectile.ai[1]);
            if (projectile.ai[1] <= 0f)
            {
                vector17.Y *= -1f;
            }
            vector17 = vector17.RotatedBy((double)projectile.localAI[0], default(Vector2));
            projectile.ai[0] += 1f;
            if (projectile.ai[0] < 30f)
            {
                projectile.velocity += 24f * vector17;
            }
            else
            {
                projectile.Kill();
            }

            projectile.position = player.RotatedRelativePoint(player.MountedCenter, true) - projectile.Size / 2f;
            projectile.rotation = projectile.velocity.ToRotation() + halfPi;
            projectile.spriteDirection = projectile.direction;
            projectile.timeLeft = 2;
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2((double)(projectile.velocity.Y * (float)projectile.direction), (double)(projectile.velocity.X * (float)projectile.direction));

            Vector2 vector24 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector24.X = (float)player.bodyFrame.Width - vector24.X;
            }
            if (player.gravDir != 1f)
            {
                vector24.Y = (float)player.bodyFrame.Height - vector24.Y;
            }
            vector24 -= new Vector2((float)(player.bodyFrame.Width - player.width), (float)(player.bodyFrame.Height - 42)) / 2f;

            //I added this part since I couldn't get the rotation working
            if (runOnce)
            {
                r = projectile.rotation = projectile.velocity.ToRotation();
                runOnce = false;
            }
            if (player.whoAmI == Main.myPlayer)
            {
                r = QwertyMethods.SlowRotation(r, (Main.MouseWorld - player.MountedCenter).ToRotation(), 2);
            }

            projectile.velocity = QwertyMethods.PolarVector(projectile.velocity.Length(), r);

            projectile.Center = player.RotatedRelativePoint(player.position + vector24, true) - projectile.velocity;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = 8;
            target.immune[projectile.owner] = 0;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float num8 = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity, 16f * projectile.scale, ref num8))
            {
                return true;
            }
            return false;
        }
    }
}