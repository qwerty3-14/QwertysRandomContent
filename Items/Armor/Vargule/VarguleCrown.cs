using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using QwertysRandomContent;
namespace QwertysRandomContent.Items.Armor.Vargule
{
	[AutoloadEquip(EquipType.Head)]
	public class VarguleCrown : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Crown");
			Tooltip.SetDefault("+40% magic crit" + "\n +15% melee damage and speed" + "\nDisables mana regen, and -180 max mana, but melee attacks recover mana");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 50000;
			item.rare = 8;
			
			
			item.width = 18;
			item.height = 14;
			item.defense = 9;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
			
			
			player.meleeDamage += .15f;
			player.magicCrit += 40;
			var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
			modPlayer.siphon2 = true;
			
			player.manaRegenBonus = -999;
			player.statManaMax2 += -180;
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
			player.setBonus = "Melee attacks boost magic damage and max mana" + "\nMagic attacks boost melee damage";
			var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
			modPlayer.visorSetBonus = true;
			
			
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

