using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Rhuthinium
{
	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	
	public class RhuthiniumBracelet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Bracelet");
			
			
		}
		
		public override void SetDefaults()
		{
			
			item.value = 10000;
			item.rare = 3;
			
			
			item.width = 28;
			item.height = 22;
			item.vanity = true;
			item.accessory = true;
			
			
			
		}
		
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
			
	}
		
	
}

