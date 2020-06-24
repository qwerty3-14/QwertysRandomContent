using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using QwertysRandomContent.Items.Etims;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.CloakedDarkBoss
{
    public class EtimsicCannon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heaven Raider Cannon");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 34;
            projectile.hostile = true;
            projectile.tileCollide = false;
        }

        private int shootTimer = 0;
        private int laserLength = 2000;

        public override void AI()
        {
            projectile.rotation = projectile.ai[0];
            shootTimer++;
            if (shootTimer == 180)
            {
                if (Main.netMode != 2)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/QuickBeam").WithVolume(.8f).WithPitchVariance(.5f), projectile.Center);
                }

                projectile.ai[1] = 1;
            }

            if (shootTimer > 200)
            {
                projectile.Kill();
            }
        }

        private void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {
            for (int i = 0; i < laserLength; i += 4)
            {
                spriteBatch.Draw(texture, projectile.Center + QwertyMethods.PolarVector(17 + i, projectile.rotation) - Main.screenPosition, null, color, projectile.rotation, Vector2.UnitY * texture.Height * .5f, 1f, SpriteEffects.None, 0);
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (shootTimer > 180)
            {
                DrawLaser(spriteBatch, mod.GetTexture("NPCs/CloakedDarkBoss/CannonBeam" + (ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? "_Old" : "")), Color.White);
            }
            /*
            else if (shootTimer > 150)
            {
                DrawLaser(spriteBatch, mod.GetTexture("NPCs/CloakedDarkBoss/WarningLaser"), (shootTimer % 10 > 5 ? Color.White : Color.Red));
            }
            else if (shootTimer > 60)
            {
                DrawLaser(spriteBatch, mod.GetTexture("NPCs/CloakedDarkBoss/WarningLaser"), (shootTimer%20 > 10 ? Color.White : Color.Red));
            }*/
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return shootTimer > 180 && Collision.CheckAABBvLineCollision(targetHitbox.Location.ToVector2(), targetHitbox.Size(), projectile.Center, projectile.Center + QwertyMethods.PolarVector(laserLength, projectile.rotation), 10, ref point);
        }
    }

    public class EtimsicWall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Etimsic Barrier");
            Main.projFrames[projectile.type] = 2;
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 38;
            projectile.hostile = true;
            projectile.tileCollide = false;
        }

        private int shootTimer = 0;
        private int laserLength = 2000;

        public override void AI()
        {
            projectile.rotation = projectile.ai[0];
            shootTimer++;
            if (shootTimer == 30)
            {
                if (Main.netMode != 2)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/QuickBeam").WithVolume(.8f).WithPitchVariance(.5f), projectile.Center);
                }

                projectile.ai[1] = 1;
            }
            if (shootTimer > 30 && shootTimer % 10 == 0)
            {
                projectile.frame = projectile.frame == 0 ? 1 : 0;
            }
        }

        private void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {
            for (int i = 0; i < laserLength; i += 4)
            {
                spriteBatch.Draw(texture, projectile.Center + QwertyMethods.PolarVector(22 + i, projectile.rotation) - Main.screenPosition, null, color, projectile.rotation, Vector2.UnitY * texture.Height * .5f, 1f, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, projectile.Center + QwertyMethods.PolarVector(-22 - i, projectile.rotation) - Main.screenPosition, null, color, projectile.rotation + (float)Math.PI, Vector2.UnitY * texture.Height * .5f, 1f, SpriteEffects.None, 0);
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (shootTimer > 30)
            {
                DrawLaser(spriteBatch, mod.GetTexture("NPCs/CloakedDarkBoss/WallBeam" + (ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? "_Old" : "")), Color.White);
            }
            /*
            else if (shootTimer > 20)
            {
                DrawLaser(spriteBatch, mod.GetTexture("NPCs/CloakedDarkBoss/WarningLaser"), (shootTimer % 10 > 5 ? Color.White : Color.Red));
            }
            else if (shootTimer > 0)
            {
                DrawLaser(spriteBatch, mod.GetTexture("NPCs/CloakedDarkBoss/WarningLaser"), (shootTimer % 20 > 10 ? Color.White : Color.Red));
            }
            */
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return shootTimer > 30 && Collision.CheckAABBvLineCollision(targetHitbox.Location.ToVector2(), targetHitbox.Size(), projectile.Center + QwertyMethods.PolarVector(-laserLength, projectile.rotation), projectile.Center + QwertyMethods.PolarVector(laserLength * 2, projectile.rotation), 10, ref point);
        }
    }

    public class EtimsicRay : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Etimsic Ray");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.extraUpdates = 1;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            projectile.tileCollide = false;
            projectile.light = 1f;
            projectile.GetGlobalProjectile<Etims>().effect = true;
        }
    }
}