using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.RuneGhostItems
{
    public class RuneGhostBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 36));
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 32;
            item.height = 32;
            item.rare = 9;
            item.expert = true;
            //bossBagNPC = mod.NPCType("RuneSpector"); 
        }
        public override int BossBagNPC => mod.NPCType("RuneSpector");


        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {

            int runeCount = Main.rand.Next(30, 41);
            int selectScroll = Main.rand.Next(1, 5);
            if (Main.rand.Next(7) == 0)
                player.QuickSpawnItem(mod.ItemType("RunicRobe"));
            if (Main.rand.Next(7) == 0)
                player.QuickSpawnItem(mod.ItemType("RuneGhostMask"));

            if (selectScroll == 1)
            {
                player.QuickSpawnItem(mod.ItemType("IceScroll"));
            }
            if (selectScroll == 2)
            {
                player.QuickSpawnItem(mod.ItemType("PursuitScroll"));
            }
            if (selectScroll == 3)
            {
                player.QuickSpawnItem(mod.ItemType("LeechScroll"));
            }
            if (selectScroll == 4)
            {
                player.QuickSpawnItem(mod.ItemType("AggroScroll"));
            }


            player.QuickSpawnItem(mod.ItemType("ExpertItem"));
            player.QuickSpawnItem(73, 35);

            player.QuickSpawnItem(mod.ItemType("CraftingRune"), runeCount);

        }
    }
}
