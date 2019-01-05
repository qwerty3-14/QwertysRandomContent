using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Longbows
{
	public class DemonLongbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demon Longbow");
			Tooltip.SetDefault("Slower but more powerful than a standard demon bow");
			
		}
		public override void SetDefaults()
		{
			item.damage = 28;
			item.ranged = true;
			
			item.useTime = 48;
			item.useAnimation = 48;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 2000;
			item.rare = 1;
			item.UseSound = SoundID.Item5;
			
			item.width = 20;
			item.height = 40;
			item.crit = 11;
			item.shoot = 40;
			item.useAmmo = 40;
			item.shootSpeed =40;
			item.noMelee=true;
			
			
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemoniteBar, 12);
			recipe.AddIngredient(ItemID.ShadowScale, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
		
	
}

