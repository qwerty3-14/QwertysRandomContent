using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.BladeBossItems
{
    public class BladedArrowShaft : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Used to craft a powerful arrow!");
        }
        public override void SetDefaults()
        {
            item.rare = 7;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.width = 26;
            item.height = 14;
        }
    }
}
