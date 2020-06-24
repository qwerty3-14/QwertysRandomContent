using Terraria.ModLoader;

namespace QwertysRandomContent.Items.TileItems
{
    public class VFan : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vertical Fan");
            Tooltip.SetDefault("Blows things away" + "\nToggled by right clicking or by wire device" + "\nCrashes the game when broken while in use");
        }

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 30;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 30000;
            item.createTile = mod.TileType("VFan");
        }
    }
}