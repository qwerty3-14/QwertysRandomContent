using Terraria.ModLoader;

namespace QwertysRandomContent.Items
{
	public class Illuminator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminator");
			Tooltip.SetDefault("Placing illuminators on top of each other makes columns." + "\nThese columns will light the tiles above them." + "\nThe number of tiles they light is 4x the height of the column");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.value = 2000;
			item.rare = 3;
			item.createTile = mod.TileType("Illuminator");
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
		}
	}
}