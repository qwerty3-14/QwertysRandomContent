using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress
{
    public class FortressBrick : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortress Brick");
            Tooltip.SetDefault("");
            if (ModContent.GetInstance<SpriteSettings>().ClassicFortress)
            {
                Main.itemTexture[item.type] = mod.GetTexture("Items/Fortress/FortressBrick_Classic");
            }
        }

        public override void SetDefaults()
        {

            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.value = 0;
            item.rare = 3;
            item.createTile = mod.TileType("FortressBrick");
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
            recipe.AddIngredient(mod.ItemType("FortressWall"), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("FortressPillar"), 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("FortressPlatform"), 2);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }


    }
}
