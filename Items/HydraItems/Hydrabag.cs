using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
    public class HydraBag : ModItem
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
            item.width = 76;
            item.height = 35;
            item.rare = 9;
            item.expert = true;
            //bossBagNPC = mod.NPCType("Hydra");
        }

        public override int BossBagNPC => mod.NPCType("Hydra");

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            int getHook = Main.rand.Next(0, 100);

            string[] spawnThese = QwertysRandomContent.HydraLoot.Draw(3);
            player.QuickSpawnItem(mod.ItemType(spawnThese[0]));
            player.QuickSpawnItem(mod.ItemType(spawnThese[1]));
            player.QuickSpawnItem(mod.ItemType(spawnThese[2]));

            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("HydraHook"));
            }
            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("Hydrator"));
            }

            player.QuickSpawnItem(73, 12);
            player.QuickSpawnItem(mod.ItemType("HydraWings"));
            player.QuickSpawnItem(mod.ItemType("HydraScale"), Main.rand.Next(30, 41));
        }
    }
}