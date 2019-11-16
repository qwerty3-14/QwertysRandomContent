using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
	
	
	public class UltraManuverabilityBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ultra Manuverability Boots");
			Tooltip.SetDefault("Allows super fast running" + "\nAllows flight"+ "\nIncreases jump speed"+ "\nAllows auto jump"+ "\nAllows swimming"+ "\nProvides immunity to fall damage" + "\nDouble tap to dash (6.9 dash power)");
			
		}
		
		public override void SetDefaults()
		{
			
			item.value = 10000;
			item.rare = 1;
			
			
			item.width = 32;
			item.height = 28;
			
			item.accessory = true;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
			player.accRunSpeed = 6.75f;
			player.rocketBoots = 3;
			player.moveSpeed += 0.1f;
			player.maxRunSpeed += 0.1f;
			player.noFallDmg = true;
			player.autoJump = true;
			
			player.jumpSpeedBoost += 2.5f;
			player.accFlipper = true;

            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (modPlayer.customDashSpeed < 6.9f)
            {
                modPlayer.customDashSpeed = 6.9f;
            }

        }
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Flipper);
			recipe.AddIngredient(ItemID.Tabi);
			recipe.AddIngredient(ItemID.FrogLeg);
			recipe.AddIngredient(ItemID.LightningBoots);
			recipe.AddIngredient(ItemID.LuckyHorseshoe);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
			
	}
		
	
}

