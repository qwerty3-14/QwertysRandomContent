using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using QwertysRandomContent;
using Terraria.Localization;

namespace QwertysRandomContent.Items.Armor.Vargule
{
	[AutoloadEquip(EquipType.Head)]
	public class VarguleCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Charm");
			Tooltip.SetDefault("+1 max minions and sentries");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 50000;
			item.rare = 8;
			
			
			item.width = 14;
			item.height = 14;
			item.defense = 9;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
			player.maxMinions +=1;
			player.maxTurrets += 1;
			
		}
		public override void DrawHair(ref bool  drawHair, ref bool  drawAltHair )
		{
			drawHair=true;
			
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("VarguleChestplate") && legs.type == mod.ItemType("VarguleLeggings");
			
		}
		
	
		
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.VCharmSet");
			player.maxMinions +=1;
			player.maxTurrets +=1;
			
		}
		
		
		
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VarguleBar"), 12);
			
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
			
	}
		
	
}

