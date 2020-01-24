using Terraria.ModLoader;

namespace QwertysRandomContent.Items
{
    public class ReverseSand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dnas");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {

            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.value = 0;
            item.rare = 1;
            item.createTile = mod.TileType("ReverseSand");
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
        }



    }
}
