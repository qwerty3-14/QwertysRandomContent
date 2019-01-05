using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
	[AutoloadEquip(EquipType.HandsOff)]
	
	public class WhiteGloveBackHand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("White Glove (Back Hand)");
			Tooltip.SetDefault("For romantic occasions and cartoon mouse impersonations");
			
		}
		
		public override void SetDefaults()
		{
			
			item.value = 10000;
			item.rare = 1;
			
			
			item.width = 22;
			item.height = 28;
			item.vanity = true;
			item.accessory = true;
			
			
			
		}
		
		
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 2);
			recipe.AddTile(TileID.Loom);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
			
	}
		
	
}

