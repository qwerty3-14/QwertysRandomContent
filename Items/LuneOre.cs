using Terraria.ModLoader;

namespace QwertysRandomContent.Items
{
    public class LuneOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Ore");
            Tooltip.SetDefault("Use a transmutator to turn demonite/crimtane into lune ore");
        }

        public override void SetDefaults()
        {

            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.value = 100;
            item.rare = 1;
            item.createTile = mod.TileType("LuneOre");
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
        }



    }
}
