using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items
{
	public class VarguleBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Bar");
			Tooltip.SetDefault("");
		}
		
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 24;
			item.maxStack = 999;
			item.value = 50000;
			item.rare = 8;
			item.createTile = mod.TileType("VarguleBar");
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
			recipe.AddIngredient(mod, "RhuthiniumBar", 2);
			recipe.AddIngredient(1508, 1);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}

		

	}
}
