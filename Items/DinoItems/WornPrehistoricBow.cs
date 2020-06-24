using Terraria.ModLoader;

namespace QwertysRandomContent.Items.DinoItems
{
    public class WornPrehistoricBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Worn Prehistoric Bow");
            Tooltip.SetDefault("This Bow is over 65 million years old!" + "\nBut then... who made it?");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 40;
            item.maxStack = 999;
            item.value = 100;
            item.rare = 3;
            item.value = 500;
            item.rare = 6;
        }
    }
}