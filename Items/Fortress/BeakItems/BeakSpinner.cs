using QwertysRandomContent.AbstractClasses;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.BeakItems
{
    public class BeakSpinner : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beak Spinner");
            Tooltip.SetDefault("Gains momentum as it hits enemies but wears out faster than other tops");
        }

        public override void SetDefaults()
        {
            item.damage = 51;
            item.melee = true;
            item.knockBack = 5;
            item.value = Item.sellPrice(gold: 3);
            item.rare = 4;
            item.width = 26;
            item.height = 40;
            item.useStyle = 1;
            item.shootSpeed = 3.5f;
            item.useTime = 23;
            item.useAnimation = 23;
            item.shoot = mod.ProjectileType("BeakSpinnerP");
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 12);
            recipe.AddIngredient(mod.ItemType("FortressHarpyBeak"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class BeakSpinnerP : Top
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beak Spinner");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;

            projectile.width = 26;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;

            projectile.tileCollide = true;
            friction = .009f;
            enemyFriction = -.4f;
        }
    }
}