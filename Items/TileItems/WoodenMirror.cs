using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.TileItems
{
    public class WoodenMirror : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wooden Mirror");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 38;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 10, 0);
            item.createTile = mod.TileType("Mirrors");
            item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 12);
            recipe.AddIngredient(ItemID.Glass, 8);
            recipe.AddTile(TileID.Sawmill);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}