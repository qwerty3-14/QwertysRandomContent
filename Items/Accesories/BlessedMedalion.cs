using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
	
	
	public class BlessedMedalion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blessed Medallion");
			Tooltip.SetDefault("30% chance to dodge an otherwise lethal attack");
			
		}
		
		public override void SetDefaults()
		{
			
			
			item.rare = 1;

            item.value = 1000;
			item.width = 14;
			item.height = 14;
			
			item.accessory = true;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
			var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
			modPlayer.blessedMedalion = true;
			
			
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CursedMedalion"));
            recipe.AddIngredient(ItemID.PurificationPowder, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }


    }
		
	
}

