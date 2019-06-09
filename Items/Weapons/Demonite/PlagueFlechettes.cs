using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Demonite
{
    public class PlagueFlechettes : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plague Flechettes");
            Tooltip.SetDefault("Flechettes do more damage as they pick up speed from gravity" + "\nMore numerous than ordinary flechettes");

        }
        public override void SetDefaults()
        {
            item.damage = 8;
            item.thrown = true;
            item.knockBack = 5;
            item.value = 15;
            item.rare = 1;
            item.width = 14;
            item.height = 26;
            item.useStyle = 1;
            item.shootSpeed = 5f;
            item.useTime = 8;
            item.useAnimation = 16;
            item.consumable = true;
            item.shoot = mod.ProjectileType("PlagueFlechetteP");
            item.noUseGraphic = true;
            item.noMelee = true;
            item.maxStack = 999;
            item.autoReuse = true;
            

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DemoniteBar, 1);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 200);
            recipe.AddRecipe();
        }
        public override bool ConsumeItem(Player player)
        {
            return Main.rand.Next(2) == 0;
        }
        
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float speed = new Vector2(speedX, speedY).Length();
            int numberOfProjectiles = 3 + Main.rand.Next(3);
            
            for(int p =0; p <numberOfProjectiles; p++)
            {
                float direction = Main.rand.NextFloat(5 * (float)Math.PI / 8, 3* (float)Math.PI / 8);
                Projectile.NewProjectile(position, QwertyMethods.PolarVector(speed, direction), type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }
    public class PlagueFlechetteP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FlechetteP");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;

        }
        public override void SetDefaults()
        {
            //projectile.aiStyle = 1;
            //aiType = ProjectileID.Shuriken;
            projectile.width = 6;
            projectile.height = 6;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.thrown = true;
            //projectile.extraUpdates = 1;
            //projectile.scale = .5f;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.tileCollide = true;


        }
        float acceleration = .1f;
        float maxVerticalSpeed = 12f;
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
            //Main.NewText(projectile.velocity.Y);

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

