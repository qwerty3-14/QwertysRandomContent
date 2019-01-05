using Terraria.ModLoader;

namespace QwertysRandomContent.Items
{
	public class RhuthiniumOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Ore");
			Tooltip.SetDefault("");
		}
		
		public override void SetDefaults()
		{

			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.value = 100;
			item.rare = 3;
			item.createTile = mod.TileType("RhuthiniumOre");
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
		}

		

	}
}
