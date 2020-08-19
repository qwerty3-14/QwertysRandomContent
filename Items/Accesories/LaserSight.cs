using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class LaserSight : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Laser Sight");
            Tooltip.SetDefault("GIves you a laser sight");
        }

        public override void SetDefaults()
        {
            item.value = 50000;
            item.rare = 3;
            item.width = 38;
            item.height = 34;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<LaserSightPlayer>().effect = true;
        }
    }

    public class LaserSightPlayer : ModPlayer
    {
        public bool effect = false;

        public override void ResetEffects()
        {
            effect = false;
        }

        public override void PostUpdateEquips()
        {
            if (effect)
            {
                //Main.NewText("Laser");
                Projectile.NewProjectile(player.Center, QwertyMethods.PolarVector(4, (Main.MouseWorld - player.Center).ToRotation()), mod.ProjectileType("SightBeam"), 0, 0, player.whoAmI);
            }
        }
    }

    public class SightBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sight Beam");
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 6;
            projectile.penetrate = -1;
            projectile.timeLeft = 350;
            projectile.extraUpdates = 349;
        }

        private bool decaying = false;

        private void StartDecay()
        {
            if (!decaying)
            {
                projectile.extraUpdates = 0;
                projectile.timeLeft = 3;
                projectile.velocity = Vector2.Zero;
                decaying = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            return false;
        }

        private Vector2 start;
        private bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                runOnce = false;
                start = projectile.Center;
            }
            if (projectile.timeLeft == 2 && !decaying)
            {
                StartDecay();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (!runOnce)
            {
                for (int d = 0; d < (projectile.Center - start).Length(); d += 4)
                {
                    float rot = (projectile.Center - (Vector2)start).ToRotation();
                    spriteBatch.Draw(Main.projectileTexture[projectile.type], start + QwertyMethods.PolarVector(d, rot) - Main.screenPosition, null, Color.White, rot, Vector2.UnitY * 3, Vector2.One, SpriteEffects.None, 0);
                }
            }
            return false;
        }
    }
}