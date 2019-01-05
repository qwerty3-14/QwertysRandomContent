using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items
{
	public class Fan : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Blows things away can be toggled on and of by right clicking or by wire");

        }

		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 30;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
            item.value = 30000;
            item.createTile = mod.TileType("Fan");
		}

		
	}
}