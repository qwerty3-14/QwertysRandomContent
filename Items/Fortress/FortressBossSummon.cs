using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress
{
    public class FortressBossSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pendant of the Sky God");
            Tooltip.SetDefault("Can be used at the altar");
            ItemID.Sets.SortingPriorityBossSpawns[item.type] = 13; // This helps sort inventory know this is a boss summoning item.
        }

        public override void SetDefaults()
        {

            item.width = 30;
            item.height = 24;
            item.maxStack = 999;
            item.value = 10000;
            item.rare = 3;

            item.useTurn = true;

            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 1;
            item.consumable = true;
        }





    }
}
