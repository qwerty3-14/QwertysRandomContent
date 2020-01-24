using Microsoft.Xna.Framework;
using QwertysRandomContent.AbstractClasses;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Thrown
{
    public class PalladiumFlechettes : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Palladium Flechettes");
            Tooltip.SetDefault("Flechettes do more damage as they pick up speed from gravity");

        }
        public override void SetDefaults()
        {
            item.damage = 27;
            item.thrown = true;
            item.knockBack = 5;
            item.value = 50;
            item.rare = 3;
            item.width = 14;
            item.height = 26;
            item.useStyle = 1;
            item.shootSpeed = 6f;
            item.useTime = 9;
            item.useAnimation = 18;
            item.consumable = true;
            item.shoot = mod.ProjectileType("PalladiumFlechetteP");
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
            recipe.AddIngredient(ItemID.PalladiumBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 333);
            recipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float speed = new Vector2(speedX, speedY).Length();
            int numberOfProjectiles = 3 + Main.rand.Next(2);

            for (int p = 0; p < numberOfProjectiles; p++)
            {
                float direction = Main.rand.NextFloat(5 * (float)Math.PI / 8, 3 * (float)Math.PI / 8);
                Projectile.NewProjectile(position, QwertyMethods.PolarVector(speed, direction), type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }
    public class PalladiumFlechetteP : Flechette
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Palladium Flechette");
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

    }

}

