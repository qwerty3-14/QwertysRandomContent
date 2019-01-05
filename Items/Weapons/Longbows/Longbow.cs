using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Longbows
{
	public class Longbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Longbow");
			Tooltip.SetDefault("Slower but more powerful than a standard wood bow");
			
		}
		public override void SetDefaults()
		{
			item.damage = 12;
			item.ranged = true;
			
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 100;
			item.rare = 1;
			item.UseSound = SoundID.Item5;
			
			item.width = 16;
			item.height = 40;
			item.crit = 1;
			item.shoot = 40;
			item.useAmmo = 40;
			item.shootSpeed =36;
			item.noMelee=true;
			
			
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 12);
			recipe.AddIngredient(ItemID.StoneBlock, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
		
	
}

