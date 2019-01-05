using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.DevItems.SUS
{
	[AutoloadEquip(EquipType.Head)]
	public class SUSsFishbowl : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SUS's Fishbowl");
			Tooltip.SetDefault("Somehow not fatal" + "\nDev Item");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 0;
			item.rare = 10;

            item.vanity = true;
			item.width = 20;
			item.height = 20;
			
			
			
			
		}
		
		
			
	}
		
	
}

