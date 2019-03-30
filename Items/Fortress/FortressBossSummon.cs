using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress
{
	public class FortressBossSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pendant of the sky god");
			Tooltip.SetDefault("Can be used at the altar");
		}
		
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 24;
			item.maxStack = 999;
			item.value = 10000;
			item.rare = 3;
			
			item.useTurn = true;
			
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 1;
			item.consumable = true;
		}
        




    }
}
