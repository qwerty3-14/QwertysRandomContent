using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Ranger
{
	[AutoloadEquip(EquipType.Body)]
	public class RangerVest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ranger Vest");
			Tooltip.SetDefault("15% increased ranged damage" + "\nSet bonus with Ranger Pants");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 10000;
			item.rare = 1;
			
			
			item.width = 34;
			item.height = 14;
			item.defense = 3;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
			player.rangedDamage  +=0.15f;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands=true;
			drawArms=true;
		}
		
		

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 6);
			recipe.AddIngredient(209, 2);
			recipe.AddTile(TileID.Loom);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
			
	}
		
	
}

