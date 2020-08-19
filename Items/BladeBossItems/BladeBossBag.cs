using QwertysRandomContent.Config;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.BladeBossItems
{
    public class BladeBossBag : ModItem
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicImperious ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 36;
            item.height = 34;
            item.rare = 9;
            item.expert = true;
            //bossBagNPC = mod.NPCType("BladeBoss");
        }

        public override int BossBagNPC => mod.NPCType("Imperious");

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            string[] spawnThese = QwertysRandomContent.ImperiousLoot.Draw(3);
            player.QuickSpawnItem(mod.ItemType(spawnThese[0]));
            player.QuickSpawnItem(mod.ItemType(spawnThese[1]));
            player.QuickSpawnItem(mod.ItemType(spawnThese[2]));
            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("SwordsmanBadge"));
            }

            player.QuickSpawnItem(73, 15);

            player.QuickSpawnItem(mod.ItemType("ImperiousSheath"));
        }
    }
}