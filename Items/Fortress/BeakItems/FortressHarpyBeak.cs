using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.BeakItems
{
	public class FortressHarpyBeak : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fortress Harpy Beak");
			Tooltip.SetDefault("Lightweight and sturdy, goes well with Caelite when making weapons");
            
        }
		
		public override void SetDefaults()
		{

			item.width = 18;
			item.height = 20;
            item.maxStack = 999;
			item.rare = 4;
			item.value = 2500;
			
			
		}
		
	

		

	}
}
