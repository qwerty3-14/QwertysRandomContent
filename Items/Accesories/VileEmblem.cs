using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
	public class VileEmblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vile Emblem");
			Tooltip.SetDefault("12% increased damage \n10% increased critical strike chance \nEnemies are less likely to target you");
		}

		public override void SetDefaults()
		{
			item.rare = 5;
			item.value = Item.sellPrice(gold: 15);
			item.width = 28;
			item.height = 28;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DestroyerEmblem);
			recipe.AddIngredient(ItemID.PutridScent);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.allDamage += .12f;
			player.magicCrit += 10;
			player.rangedCrit += 10;
			player.meleeCrit += 10;
			player.thrownCrit += 10;
			player.aggro -= 400;
		}
	}
}