using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.FortressBoss
{
    public class FortressBossBag : ModItem
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
            item.width = 60;
            item.height = 34;
            item.rare = 9;
            item.expert = true;
            //bossBagNPC = mod.NPCType("FortressBoss");
        }

        public override int BossBagNPC => mod.NPCType("FortressBoss");

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.QuickSpawnItem(mod.ItemType("CaeliteBar"), Main.rand.Next(18, 31));
            player.QuickSpawnItem(mod.ItemType("CaeliteCore"), Main.rand.Next(9, 16));
            player.QuickSpawnItem(mod.ItemType("ExpertChalice"));
            if (Main.rand.Next(7) == 0)
                player.QuickSpawnItem(mod.ItemType("DivineLightMask"));
            player.QuickSpawnItem(73, 10);
            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("Lightling"));
            }
            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("SkywardHilt"));
            }
        }
    }
}