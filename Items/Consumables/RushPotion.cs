using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Consumables
{
	public class RushPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rush Potion");
			Tooltip.SetDefault("+2 dash power (doesn't work with shield of cthulu or solar shield)");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.maxStack = 30;
			item.rare = 3;
			item.useAnimation = 17;
			item.useTime = 45;
			item.useStyle = 2;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			//item.potion = true;
			item.useTurn = true;
			item.buffType = mod.BuffType("Rush");
			item.buffTime = 21600;
		}

		public override bool CanUseItem(Player player)
		{
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater);
			recipe.AddIngredient(ItemID.Fireblossom);
			recipe.AddIngredient(mod.ItemType("RhuthiniumBar"));
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}