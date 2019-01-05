using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
	
	
	public class BloodyMedalion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody medallion");
			Tooltip.SetDefault("80% increased magic damage"+ "\nWhat normaly drains mana drains you instead!");


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
            player.magicDamage += .8f;
            var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            modPlayer.bloodyMedalion = true;
            if (player.statMana< player.statManaMax)
            {
                player.statLife -= (player.statManaMax - player.statMana);
                player.statMana = player.statManaMax;
            }
            



        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CursedMedalion"));
            recipe.AddRecipeGroup("QwertysrandomContent:EvilPowder", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        
			
	}
		
	
}

