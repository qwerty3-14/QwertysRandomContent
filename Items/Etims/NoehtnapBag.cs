using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Etims
{
    public class NoehtnapBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 48;
            item.height = 32;
            item.rare = 9;
            item.expert = true;
            //bossBagNPC = mod.NPCType("CloakedDarkBoss");
        }
        public override int BossBagNPC => mod.NPCType("CloakedDarkBoss");


        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {



            player.QuickSpawnItem(73, 8);
            player.QuickSpawnItem(mod.ItemType("Doppleganger"));
            if(Main.rand.Next(5)==0)
            {
                player.QuickSpawnItem(mod.ItemType("EyeOfDarkness"));
            }
            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("NoScope"));
            }
            player.QuickSpawnItem(mod.ItemType("EtimsMaterial"), 20 + Main.rand.Next(17));
        }
    }
}
