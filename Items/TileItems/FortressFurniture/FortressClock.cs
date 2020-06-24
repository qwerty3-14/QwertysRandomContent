using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.TileItems.FortressFurniture
{
    public class FortressClock : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 22;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 250;
            item.createTile = mod.TileType("FortressClock");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("FortressBrick"), 10);
            //recipe.AddIngredient(ItemID.Torch, 3);
            recipe.AddRecipeGroup("IronBar");
            recipe.AddTile(TileID.Sawmill);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}