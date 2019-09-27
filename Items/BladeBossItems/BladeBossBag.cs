using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.BladeBossItems
{
	public class BladeBossBag : ModItem
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
			item.width = 36;
			item.height = 34;
			item.rare = 9;
			item.expert = true;
			//bossBagNPC = mod.NPCType("BladeBoss");
		}
        public override int BossBagNPC => mod.NPCType("BladeBoss");


        public override bool CanRightClick()
		{
			return true;
		}

        public override void OpenBossBag(Player player)
        {
            Deck<string> itemNames = new Deck<string>();
            itemNames.Add("SwordStormStaff");
            itemNames.Add("ImperiousTheIV");
            itemNames.Add("FlailSword");
            itemNames.Add("SwordMinionStaff");
            itemNames.Add("SwordsmanBadge");
            itemNames.Add("BladedArrowShaft");

            string[] spawnThese = itemNames.Draw(2);
            player.QuickSpawnItem(mod.ItemType(spawnThese[0]));
            player.QuickSpawnItem(mod.ItemType(spawnThese[1]));

            player.QuickSpawnItem(73, 15);

            player.QuickSpawnItem(mod.ItemType("ImperiousSheath"));

        }
	}
}
