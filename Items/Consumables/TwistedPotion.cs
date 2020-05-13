using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Consumables
{
	public class TwistedPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twisted Potion");
			Tooltip.SetDefault("14% increased morph critical strike chance");
		}

		public override void SetDefaults()
		{
			item.UseSound = SoundID.Item3;
			item.useStyle = 2;
			item.useTurn = true;
			item.useAnimation = 17;
			item.useTime = 17;
			item.maxStack = 30;
			item.consumable = true;
			item.width = 16;
			item.height = 30;
			item.buffType = mod.BuffType("Twisted");
			item.buffTime = 60 * 60 * 10;
			item.value = 1000;
			item.rare = 1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(mod.ItemType("EtimsMaterial"), 1);
			recipe.AddIngredient(ItemID.Deathweed, 1);
			recipe.AddIngredient(ItemID.Shiverthorn, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}