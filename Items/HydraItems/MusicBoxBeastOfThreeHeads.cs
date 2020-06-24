using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
    public class MusicBoxBeastOfThreeHeads : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Music Box (Beast of Three Heads)");
        }

        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = mod.TileType("MusicBoxBeastOfThreeHeads");
            item.width = 24;
            item.height = 24;
            item.rare = 5;
            item.value = 100000;
            item.accessory = true;
        }
    }
}