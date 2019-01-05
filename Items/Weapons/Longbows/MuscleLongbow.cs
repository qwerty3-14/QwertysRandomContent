using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Longbows
{
	public class MuscleLongbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Muscle Longbow");
			Tooltip.SetDefault("Slower but more powerful than a standard tendon bow");
			
		}
		public override void SetDefaults()
		{
			item.damage = 38;
			item.ranged = true;
			
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 5;
			item.knockBack = 7;
			item.value = 28000;
			item.rare = 1;
			item.UseSound = SoundID.Item5;
			
			item.width = 26;
			item.height = 40;
			item.crit = 12;
			item.shoot = 40;
			item.useAmmo = 40;
			item.shootSpeed =50;
			item.noMelee=true;
			
			
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrimtaneBar, 12);
			recipe.AddIngredient(ItemID.TissueSample, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
		
	
}

