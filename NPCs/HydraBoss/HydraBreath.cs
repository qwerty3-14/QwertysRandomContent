using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.HydraBoss
{
    public class HydraBreath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Breath");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 240;
            projectile.tileCollide = false;
            projectile.light = 1f;
        }

        private int frameCounter;
        private float trigCounter;
        private float amplitude = 10;
        private Vector2[] pseudoProjectileVelocities = new Vector2[2];

        public override void AI()
        {
            trigCounter += (float)Math.PI / 30;

            pseudoProjectileVelocities[0] = projectile.velocity + QwertyMethods.PolarVector((float)Math.Cos(trigCounter) * amplitude, projectile.rotation);
            pseudoProjectileVelocities[1] = projectile.velocity + QwertyMethods.PolarVector((float)Math.Cos(trigCounter + (float)Math.PI) * amplitude, projectile.rotation);
            Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector((float)Math.Sin(trigCounter) * amplitude, projectile.rotation), mod.DustType("HydraBreathGlow"), Vector2.Zero);
            Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector((float)Math.Sin(trigCounter) * amplitude, projectile.rotation - (float)Math.PI), mod.DustType("HydraBreathGlow"), Vector2.Zero);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            if (Math.Cos(trigCounter) > 0)
            {
                spriteBatch.Draw(texture, (projectile.Center + QwertyMethods.PolarVector((float)Math.Sin(trigCounter) * amplitude, projectile.rotation)) - Main.screenPosition,
                            texture.Frame(), Color.White, pseudoProjectileVelocities[0].ToRotation() + (float)Math.PI / 2,
                            texture.Size() / 2f, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(texture, (projectile.Center + QwertyMethods.PolarVector((float)Math.Sin(trigCounter + (float)Math.PI) * amplitude, projectile.rotation)) - Main.screenPosition,
                                texture.Frame(), Color.White, pseudoProjectileVelocities[1].ToRotation() + (float)Math.PI / 2,
                                texture.Size() / 2f, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(texture, (projectile.Center + QwertyMethods.PolarVector((float)Math.Sin(trigCounter + (float)Math.PI) * amplitude, projectile.rotation)) - Main.screenPosition,
                                texture.Frame(), Color.White, pseudoProjectileVelocities[1].ToRotation() + (float)Math.PI / 2,
                                texture.Size() / 2f, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(texture, (projectile.Center + QwertyMethods.PolarVector((float)Math.Sin(trigCounter) * amplitude, projectile.rotation)) - Main.screenPosition,
                            texture.Frame(), Color.White, pseudoProjectileVelocities[0].ToRotation() + (float)Math.PI / 2,
                            texture.Size() / 2f, 1f, SpriteEffects.None, 0f);
            }

            return false;
        }
    }
}