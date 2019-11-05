using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using QwertysRandomContent;
using Terraria.Localization;

namespace QwertysRandomContent.Items.Armor.Vargule
{
	[AutoloadEquip(EquipType.Head)]
	public class VarguleVisor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Visor");
			Tooltip.SetDefault("+25% melee and ranged crit" + "\n+10% melee speed");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 50000;
			item.rare = 8;
			
			
			item.width = 22;
			item.height = 10;
			item.defense = 9;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
			player.rangedCrit += 25;
			player.meleeCrit += 25;
			player.meleeSpeed += .1f;
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
			player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.VVisorSet");
			var modPlayer = player.GetModPlayer<QwertyPlayer>();
			modPlayer.VarguleCrownSetBonus = true;
			player.rangedDamage += modPlayer.VarguleRangedBoost; 
			
			
			
			
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

