using QwertysRandomContent.Config;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items
{
	public class DnasTransmutator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dnas Transmutator");
			Tooltip.SetDefault("Sand placed on top of this will be turned into dnas over time");
		}

		public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicFortress ? base.Texture + "_Classic" : base.Texture;

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.value = 2000;
			item.rare = 1;
			item.createTile = mod.TileType("DnasTransmutator");
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
			recipe.AddIngredient(mod.ItemType("FortressBrick"), 4);
			recipe.AddIngredient(mod.ItemType("CaeliteCore"), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
}