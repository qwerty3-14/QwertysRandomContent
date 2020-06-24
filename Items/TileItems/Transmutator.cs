using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.TileItems
{
    public class Transmutator : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Transmutator");
            Tooltip.SetDefault("Used to transmute blocks with the night's lunic energy" + "\nCan transmute up to 5 blocks placed directly above it" + "\nSupported Block and Results" + "\nDemonite/Crimtane ore -> Lune ore" + "\nDirt -> Clay" + "\nHardened Sand -> Desert Fossil" + "\nHoney Block -> Hive" + "\nCopper/Tin -> Iron/Lead" + "\nSilver/Tungsten -> Gold/Platinum");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.value = 2000;
            item.rare = 1;
            item.createTile = mod.TileType("Transmutator");
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

            recipe.AddIngredient(ItemID.MeteoriteBar, 4);
            recipe.AddIngredient(ItemID.FallenStar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 4);
            recipe.AddRecipe();
        }
    }
}