using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.TileItems.FortressFurniture
{
    public class FortressDoor : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 34;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 250;
            item.createTile = mod.TileType("FortressDoorClosed");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("FortressBrick"), 6);
            //recipe.AddIngredient(ItemID.Torch, 3);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}