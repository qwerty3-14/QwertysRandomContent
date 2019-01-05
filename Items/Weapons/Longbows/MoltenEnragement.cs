using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Longbows
{
	public class MoltenEnragement : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Molten Enragement");
			Tooltip.SetDefault("A very slow, but very powerful bow");
			
		}
		public override void SetDefaults()
		{
			item.damage = 80;
			item.ranged = true;
			
			item.useTime = 120;
			item.useAnimation = 120;
			item.useStyle = 5;
			item.knockBack = 8;
			item.value = 50000;
			item.rare = 3;
			item.UseSound = SoundID.Item5;
			item.width = 20;
			item.height = 40;
			item.crit = 26;
			item.shoot = 40;
			item.useAmmo = 40;
			item.shootSpeed =800;
			item.noMelee=true;
			
			
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellstoneBar, 24);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
		
	
}

