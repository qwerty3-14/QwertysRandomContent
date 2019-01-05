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
			bossBagNPC = mod.NPCType("BladeBoss");
		}

		

		public override bool CanRightClick()
		{
			return true;
		}

		public override void OpenBossBag(Player player)
		{
			
			
				
				player.QuickSpawnItem(73, 15);
				
				player.QuickSpawnItem(mod.ItemType("ImperiousSheath"));
            switch(Main.rand.Next(3))
            {
                case 0:
                    player.QuickSpawnItem(mod.ItemType("SwordStormStaff"));
                    break;
                case 1:
                    player.QuickSpawnItem(mod.ItemType("ImperiousTheIV"));
                    break;
                case 2:
                    player.QuickSpawnItem(mod.ItemType("FlailSword"));
                    break;
            }
		}
	}
}
