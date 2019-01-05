using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.DevItems.ShockedHorizon
{
	[AutoloadEquip(EquipType.Body)]
	public class DragonScaleBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("DragonScale Chestplate");
			Tooltip.SetDefault("Good for shocking the horizons" + "\nDev Item");
			
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

