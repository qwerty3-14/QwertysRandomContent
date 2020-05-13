using Terraria.ModLoader;

namespace QwertysRandomContent.Items.BladeBossItems
{
	public class BladeBossTrophy : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Imperious Trophy");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = 50000;
			item.createTile = mod.TileType("BladeBossTrophy");
			item.placeStyle = 0;
		}
	}
}