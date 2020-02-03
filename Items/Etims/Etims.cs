using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Etims
{
    public class EtimsMaterial : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Etims");
            Tooltip.SetDefault("Forged from the blood of those slain by gods!");
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }
        public override void SetDefaults()
        {
            item.value = 10000;
            item.width = item.height = 32;
            item.maxStack = 999;
            item.rare = 3;
        }


    }
}
