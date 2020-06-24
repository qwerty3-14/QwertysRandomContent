using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Rhuthinium
{
    public class RhuthiniumDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Dart");
            Tooltip.SetDefault("Flies around you and fires 3 beams before breaking");
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 18;
            item.damage = 11;
            item.ranged = true;
            item.ammo = AmmoID.Dart;
            item.shoot = mod.ProjectileType("RhuthiniumDartP");
            item.shootSpeed = 3;
            item.knockBack = 0;
            item.rare = 3;
            item.consumable = true;
            item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 1);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }

    public class RhuthiniumDartP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Dart");
        }

        public override void SetDefaults()
        {
            projectile.ranged = true;
            projectile.width = projectile.height = 14;
            projectile.tileCollide = false;
            projectile.penetrate = 3;
        }

        private bool start = true;
        private Vector2 flyOffset;
        private float acceleration = .4f;
        private float maxSpeed = 10f;

        private void SetFlyOffset()
        {
            Player player = Main.player[projectile.owner];
            flyOffset = QwertyMethods.PolarVector(100, (player.Center - projectile.Center).ToRotation() + Main.rand.NextFloat(-(float)Math.PI / 2, (float)Math.PI / 2));
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (start)
            {
                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;
                projectile.velocity *= .8f;
                if (projectile.velocity.Length() < .1f)
                {
                    start = false;
                    SetFlyOffset();
                }
            }
            else
            {
                if (Main.LocalPlayer == player)
                {
                    projectile.rotation.SlowRotation((Main.MouseWorld - projectile.Center).ToRotation() + (float)Math.PI / 2, (float)Math.PI / 30);
                }
                projectile.velocity -= projectile.velocity.SafeNormalize(Vector2.UnitY) * acceleration / 2;
                projectile.velocity += (player.Center + flyOffset - projectile.Center).SafeNormalize(Vector2.UnitY) * acceleration;

                if (projectile.velocity.Length() > maxSpeed)
                {
                    projectile.velocity = (player.Center + flyOffset - projectile.Center).SafeNormalize(Vector2.UnitY) * maxSpeed;
                }
                if ((player.Center + flyOffset - projectile.Center).Length() < projectile.velocity.Length())
                {
                    projectile.Center = player.Center + flyOffset;
                    projectile.velocity = Vector2.Zero;
                    Projectile.NewProjectile(projectile.Center + QwertyMethods.PolarVector(10, projectile.rotation - (float)Math.PI / 2), QwertyMethods.PolarVector(4, projectile.rotation - (float)Math.PI / 2), mod.ProjectileType("DartBeam"), projectile.damage, projectile.knockBack, projectile.owner);
                    SetFlyOffset();
                    projectile.penetrate--;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustPerfect(projectile.Center, mod.DustType("RhuthiniumDust"));
                d.velocity *= 2;
                d.noGravity = true;
            }
        }
    }

    public class DartBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Dart");
        }

        public override void SetDefaults()
        {
            projectile.ranged = true;
            projectile.width = projectile.height = 6;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.timeLeft = 350;
            projectile.extraUpdates = 349;
            projectile.friendly = true;
        }

        private bool decaying = false;
        private bool cantHit = false;

        private void StartDecay()
        {
            if (!decaying)
            {
                projectile.extraUpdates = 0;
                projectile.timeLeft = 30;
                projectile.velocity = Vector2.Zero;
                decaying = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
            cantHit = true;
            projectile.velocity = Vector2.Zero;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            cantHit = true;
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
                    int c = decaying ? (int)(255f * projectile.timeLeft / 30f) : 255;
                    spriteBatch.Draw(Main.projectileTexture[projectile.type], start + QwertyMethods.PolarVector(d, rot) - Main.screenPosition, null, new Color(c, c, c, c), rot, Vector2.UnitY * 3, Vector2.One, SpriteEffects.None, 0);
                }
            }
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (cantHit)
            {
                return false;
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
    }
}