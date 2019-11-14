using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.RuneGhostItems
{
	public class CraftingRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rune");
			Tooltip.SetDefault("");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(54, 4));
            ItemID.Sets.ItemNoGravity[item.type] = true; 
        }
		
		public override void SetDefaults()
		{

			item.width = 54;
			item.height = 54;
			item.maxStack = 999;
			item.value = 100;
			item.rare = 3;
			item.value = 500;
			item.rare = 9;  
			
		}
		
	

		

	}
}
