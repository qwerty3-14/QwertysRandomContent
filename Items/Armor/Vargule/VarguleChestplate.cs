using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Vargule
{
	[AutoloadEquip(EquipType.Body)]
	public class VarguleChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Chestplate");
			Tooltip.SetDefault("20% increased damage" + "\n+1 max minions");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 50000;
			item.rare = 8;
			
			
			item.width = 22;
			item.height = 12;
			item.defense = 15;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{

            player.allDamage += .2f;
			player.maxMinions +=1;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawArms=true;
			drawHands=true;
			
		}
		
		
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VarguleBar"), 18);
			
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
			
	}
		
	
}

