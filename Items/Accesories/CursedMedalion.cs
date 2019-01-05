using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
	
	
	public class CursedMedalion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed medallion");
			Tooltip.SetDefault("Disables life regeneration");
			
		}
		
		public override void SetDefaults()
		{
			
			
			item.rare = 1;
			
			
			item.width = 14;
			item.height = 14;
			
			item.accessory = true;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
			var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
			modPlayer.noRegen = true;
			
			
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("IronBar", 2);
            
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }


    }
		
	
}

