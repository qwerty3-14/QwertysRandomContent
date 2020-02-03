using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress
{

    public class ExpertChalice : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chalice of the Light");
            Tooltip.SetDefault("Reusable" + "\nGives you 2 seconds of invinsibility after drinking");
        }

        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item3;
            item.healLife = 100;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.maxStack = 1;
            item.consumable = false;
            item.width = 14;
            item.height = 24;
            item.rare = 3;
            item.potion = true;
            item.value = 50000;
            item.expert = true;

        }
        public override bool ConsumeItem(Player player)
        {
            return false;

        }
        public override bool UseItem(Player player)
        {
            player.immune = true;
            player.immuneTime = 120;
            return true;
        }






    }
}