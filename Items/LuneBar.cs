using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items
{
	public class LuneBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lune Bar");
			Tooltip.SetDefault("");
		}
		
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 24;
			item.maxStack = 999;
			item.value = 20000;
			item.rare = 1;
			item.createTile = mod.TileType("LuneBar");
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, "LuneOre", 3);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		

	}
}
