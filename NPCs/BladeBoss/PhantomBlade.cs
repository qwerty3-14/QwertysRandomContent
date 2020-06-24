using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.BladeBoss
{
    public class PhantomBlade : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicImperious ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Blade");
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 10;
            projectile.tileCollide = false;
            projectile.hostile = true;
        }

        private int totalLength = 398;
        private int bladeLength = 308;
        private int bladeWidth = 82;
        private int a = 80;

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, new Color(a, a, a, a), projectile.rotation, new Vector2(18, texture.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + new Vector2(-10 + Main.rand.Next(21), -10 + Main.rand.Next(21)), null, new Color(a, a, a, a), projectile.rotation, new Vector2(18, texture.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float CP = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center + QwertyMethods.PolarVector(-18 + totalLength - bladeLength, projectile.rotation), projectile.Center + QwertyMethods.PolarVector(-18 + totalLength, projectile.rotation), bladeWidth, ref CP);
        }

        public override void Kill(int timeLeft)
        {
            for (int d = 0; d < 100; d++)
            {
                int lengthOffset = -18 + Main.rand.Next(bladeLength);
                int widthOffset = +Main.rand.Next(bladeWidth) - bladeWidth / 2;
                Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(lengthOffset, projectile.rotation) + QwertyMethods.PolarVector(widthOffset, projectile.width + (float)Math.PI / 2), 15);
                dust.noGravity = true;
            }
        }

        public override void AI()
        {
            projectile.rotation = projectile.ai[0];
        }
    }
}