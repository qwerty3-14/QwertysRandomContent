using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using QwertysRandomContent;
using Terraria.Localization;

namespace QwertysRandomContent.Items.Armor.Vargule
{
	[AutoloadEquip(EquipType.Head)]
	public class VarguleSombrero : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Sombrero");
			Tooltip.SetDefault("+35% ranged and magic damage" + "\nDisables magic and ranged crits" + "\n20% chance not to consume ammo");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 50000;
			item.rare = 8;
			
			
			item.width = 40;
			item.height = 12;
			item.defense = 9;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
			player.rangedDamage += .35f;
			player.magicDamage += .35f;
			player.magicCrit = -999;
			player.rangedCrit = -999;
            player.GetModPlayer<QwertyPlayer>().ammoReduction *= .8f;


        }
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.magicCrit = -999;
			player.rangedCrit = -999;
		}
		public override void DrawHair(ref bool  drawHair, ref bool  drawAltHair )
		{
			drawAltHair=true;
			
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("VarguleChestplate") && legs.type == mod.ItemType("VarguleLeggings");
			
		}
		
	
		
		public override void UpdateArmorSet(Player player)
		{
			 player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.VSombreroSSet");
			var modPlayer = player.GetModPlayer<QwertyPlayer>();
			modPlayer.SombreroSetBonus = true;
			player.ammoCost75 = true;
			
			
			
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

