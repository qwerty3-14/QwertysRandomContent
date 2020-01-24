using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items
{
    public class RhuthiniumBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Bar");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {

            item.width = 30;
            item.height = 24;
            item.maxStack = 999;
            item.value = 10000;
            item.rare = 3;
            item.createTile = mod.TileType("RhuthiniumBar");
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "RhuthiniumOre", 6);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }



    }
}
