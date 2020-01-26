using QwertysRandomContent.Config;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress
{
    public class FortressPlatform : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortress Platform");
            if (ModContent.GetInstance<SpriteSettings>().ClassicFortress)
            {
                Main.itemTexture[item.type] = mod.GetTexture("Items/Fortress/FortressPlatform_Classic");
            }
        }

        public override void SetDefaults()
        {
            item.rare = 3;
            item.width = 8;
            item.height = 10;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = mod.TileType("FortressPlatform");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("FortressBrick"));
            recipe.SetResult(this, 2);
            recipe.AddRecipe();
        }


    }
}