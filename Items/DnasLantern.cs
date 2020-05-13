using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items
{
	public class DnasLantern : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dnas Lantern");
			Tooltip.SetDefault("Reverses the gravity of nearby players");
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
			item.value = 250;
			item.createTile = mod.TileType("DnasLantern");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("FortressBrick"), 6);
			recipe.AddIngredient(mod.ItemType("ReverseSand"), 6);
			recipe.AddIngredient(mod.ItemType("CaeliteCore"), 2);
			//recipe.AddIngredient(ItemID.Torch, 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}