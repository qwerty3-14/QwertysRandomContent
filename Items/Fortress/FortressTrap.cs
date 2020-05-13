using QwertysRandomContent.Config;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress
{
	public class FortressTrap : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caelite Pulse Trap");
			Tooltip.SetDefault("Shoots 2 pulses");
		}

		public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicFortress ? base.Texture + "_Classic" : base.Texture;

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.value = 0;
			item.rare = 3;
			item.createTile = mod.TileType("FortressTrap");
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
			recipe.AddIngredient(mod.ItemType("FortressBrick"), 2);
			recipe.AddIngredient(mod.ItemType("CaeliteBar"), 1);
			recipe.AddIngredient(mod.ItemType("CaeliteCore"), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
}