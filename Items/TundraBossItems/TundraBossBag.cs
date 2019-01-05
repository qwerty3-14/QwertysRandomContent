using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.TundraBossItems
{
	public class TundraBossBag : ModItem
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
			bossBagNPC = mod.NPCType("PolarBear"); 
		}

		

		public override bool CanRightClick()
		{
			return true;
		}

		public override void OpenBossBag(Player player)
		{




            switch (Main.rand.Next(3))
            {
                case 0:
                    player.QuickSpawnItem(mod.ItemType("PenguinClub"), 1);
                    break;
                case 1:
                    player.QuickSpawnItem(mod.ItemType("PenguinLauncher"), 1);
                    break;
                case 2:
                    player.QuickSpawnItem(mod.ItemType("PenguinWhistle"), 1);
                    break;

            }
            player.QuickSpawnItem(mod.ItemType("PenguinGenerator"), 1);
            player.QuickSpawnItem(ItemID.Penguin, Main.rand.Next(40, 81));
            /*
            if (Main.rand.Next(7) == 0)
                player.QuickSpawnItem(mod.ItemType("DivineLightMask"));
                */
            player.QuickSpawnItem(73, 4);
            
        }
	}
}
