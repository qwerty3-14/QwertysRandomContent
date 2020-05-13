using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
	public class EyeOfTheAlchemist : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eye of the Alchemist");
			Tooltip.SetDefault("Emerald Eye Effect" + "\nReduces the time on healing potions");
		}

		public override void SetDefaults()
		{
			item.rare = 1;
			item.value = 10000;
			item.width = 16;
			item.height = 16;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EmeraldEye"));
			recipe.AddIngredient(ItemID.PhilosophersStone);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<ShapeShifterPlayer>().EyeEquiped = true;
			if (player.GetModPlayer<ShapeShifterPlayer>().morphTime > 600)
			{
				player.GetModPlayer<ShapeShifterPlayer>().EyeBlessing = true;
			}
			player.pStone = true;
		}
	}
}