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
			int scaleCount = Main.rand.Next(30, 41);
			int arrowCount = Main.rand.Next(120, 241);
			int weaponLoot = Main.rand.Next(1, 4);
			int getHook = Main.rand.Next(0, 100);
			int getHydrator = Main.rand.Next(0, 100);

			if (weaponLoot == 1)
			{
				player.QuickSpawnItem(mod.ItemType("Hydrent"));
			}
			if (weaponLoot == 2)
			{
				player.QuickSpawnItem(mod.ItemType("HydraBeam"));
			}
			if (weaponLoot == 3)
			{
				player.QuickSpawnItem(mod.ItemType("HydraCannon"));
			}
			if (getHook < 15)
			{
				player.QuickSpawnItem(mod.ItemType("HydraHook"));
			}
			if (getHydrator < 15)
			{
				player.QuickSpawnItem(mod.ItemType("Hydrator"));
			}

			player.QuickSpawnItem(73, 12);
			player.QuickSpawnItem(mod.ItemType("HydraWings"));
			player.QuickSpawnItem(mod.ItemType("HydraHeadStaff"));
			player.QuickSpawnItem(mod.ItemType("HydraArrow"), arrowCount);
			player.QuickSpawnItem(mod.ItemType("HydraScale"), scaleCount);
		}
	}
}