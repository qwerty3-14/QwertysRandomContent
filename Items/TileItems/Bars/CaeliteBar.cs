using Terraria.ModLoader;

namespace QwertysRandomContent.Items.TileItems.Bars
{
    public class CaeliteBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Bar");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 24;
            item.maxStack = 999;
            item.value = 20000;
            item.rare = 3;
            item.createTile = mod.TileType("CaeliteBar");
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
        }
    }
}