using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Vanity
{
	[AutoloadEquip(EquipType.Body)]
	public class SwimsuitTop : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Swimsuit top");
			Tooltip.SetDefault("");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 1000;
			item.rare = 1;
			
			item.vanity = true;
			item.width = 18;
			item.height = 12;
			item.vanity = true;
			
			
			
		}
		
		
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands=true;
			drawArms=true;
		}
		
		

		
			
	}
		
	
}

