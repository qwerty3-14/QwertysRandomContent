using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.BeakItems
{
    public class BeakTippedFlechettes : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beak Tipped Flechettes");
            Tooltip.SetDefault("Flechettes do more damage as they pick up speed from gravity" + "\nDrifts toward enemies");

        }
        public override void SetDefaults()
        {
            item.damage = 37;
            item.thrown = true;
            item.knockBack = 5;
            item.value = 100;
            item.rare = 4;
            item.width = 14;
            item.height = 26;
            item.useStyle = 1;
            item.shootSpeed = 6f;
            item.useTime = 14;
            item.useAnimation = 28;
            item.consumable = true;
            item.shoot = mod.ProjectileType("BeakTippedFlechetteP");
            item.noUseGraphic = true;
            item.noMelee = true;
            item.maxStack = 999;
            item.autoReuse = true;
            item.UseSound = SoundID.Item39;

        }
        public override bool ConsumeItem(Player player)
        {
            return Main.rand.Next(2) == 0;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 2);
            recipe.AddIngredient(mod.ItemType("FortressHarpyBeak"), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 999);
            recipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float speed = new Vector2(speedX, speedY).Length();
            int numberOfProjectiles = 2 + Main.rand.Next(4);

            for (int p = 0; p < numberOfProjectiles; p++)
            {
                float direction = Main.rand.NextFloat(5 * (float)Math.PI / 8, 3 * (float)Math.PI / 8);
                Projectile.NewProjectile(position, QwertyMethods.PolarVector(speed, direction), type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }
    public class BeakTippedFlechetteP : Flechette
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beak Tipped Flechette");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;

        }
        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 6;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.thrown = true;

            projectile.tileCollide = true;
            acceleration = .1f;
            maxVerticalSpeed = 12f;

        }

        bool runOnce = true;
        float initialVerticalVelocity;
        NPC target;
        NPC possibleTarget;
        bool foundTarget;
        float maxDistance = 10000f;
        float distance;
        float horizontalMaxSpeed = 6f;
        float horizontalAcceleration = .15f;
        public override void ExtraAI()
        {

            for (int k = 0; k < 200; k++)
            {
                possibleTarget = Main.npc[k];
                distance = (possibleTarget.Center.Y - projectile.Center.Y);
                if (distance < maxDistance && distance > 0 && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0))
                {
                    target = Main.npc[k];
                    foundTarget = true;


                    maxDistance = (possibleTarget.Center.Y - projectile.Center.Y); ;
                }

            }
            if (foundTarget)
            {
                if (target.Center.X > projectile.Center.X)
                {
                    projectile.velocity.X += horizontalAcceleration;
                }
                else
                {
                    projectile.velocity.X -= horizontalAcceleration;
                }
                if (projectile.velocity.X > horizontalMaxSpeed)
                {
                    projectile.velocity.X = horizontalMaxSpeed;
                }
                if (projectile.velocity.X < -horizontalMaxSpeed)
                {
                    projectile.velocity.X = -horizontalMaxSpeed;
                }
            }

            maxDistance = 10000f;
            foundTarget = false;
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

