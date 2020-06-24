using Terraria.ModLoader;

namespace QwertysRandomContent.Items.TileItems
{
    public class DecorativePlant : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Decorative Plant");
        }

        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = mod.TileType("DecorativePlant");
            item.width = 32;
            item.height = 24;
            item.rare = 1;
            item.value = 10000;
        }
    }
}