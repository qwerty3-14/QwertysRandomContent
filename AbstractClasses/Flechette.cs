using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.AbstractClasses
{
    public abstract class Flechette : ModProjectile
    {
        protected float acceleration = .1f;
        protected float maxVerticalSpeed = 12f;
        bool runOnce = true;
        float initialVerticalVelocity;
        public override void AI()
        {
            if (runOnce)
            {
                initialVerticalVelocity = projectile.velocity.Y;
                runOnce = false;
            }
            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;
            projectile.velocity.Y += acceleration;
            if (projectile.velocity.Y > maxVerticalSpeed)
            {
                projectile.velocity.Y = maxVerticalSpeed;
            }
            ExtraAI();
        }
        public virtual void ExtraAI()
        {

        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = damage + (int)(((projectile.velocity.Y - initialVerticalVelocity) / (maxVerticalSpeed - initialVerticalVelocity)) * .5f * (float)damage);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.velocity.Y == maxVerticalSpeed)
            {
                Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                    Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                    spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
                }
            }
            return true;
        }
    }
}
