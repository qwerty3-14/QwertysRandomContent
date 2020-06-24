using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.TileItems.FortressFurniture
{
    public class FortressBathtub : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 26;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 250;
            item.createTile = mod.TileType("FortressBathtub");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("FortressBrick"), 14);
            recipe.AddTile(TileID.Sawmill);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}