using Microsoft.Xna.Framework;
using QwertysRandomContent.AbstractClasses;
using System;
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
            item.UseSound = SoundID.Item39;

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

            for (int p = 0; p < numberOfProjectiles; p++)
            {
                float direction = Main.rand.NextFloat(5 * (float)Math.PI / 8, 3 * (float)Math.PI / 8);
                Projectile.NewProjectile(position, QwertyMethods.PolarVector(speed, direction), type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }
    public class PlagueFlechetteP : Flechette
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FlechetteP");
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
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.tileCollide = true;
            acceleration = .1f;
            maxVerticalSpeed = 12f;

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 0;
            projectile.localNPCImmunity[target.whoAmI] = -1;
        }

    }

}

