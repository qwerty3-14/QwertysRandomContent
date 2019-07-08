using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Rhuthinium
{
	
	[AutoloadEquip(EquipType.Legs)]
	public class RhuthiniumGreaves : ModItem
	{
		public override bool Autoload(ref string name)
        {
			if (!Main.dedServ)
                {
                    // Add the female leg variant
                    mod.AddEquipTexture(null, EquipType.Legs, "RhuthiniumGreaves_Female", "QwertysRandomContent/Items/Armor/Rhuthinium/RhuthiniumGreaves_FemaleLegs");
                }
				return true;
			
		}
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Greaves");
			Tooltip.SetDefault("Lets you dash (4 dash power)" + "\n10% chance not to consume ammo" + "\n15% reduced mana usage" + "\n+60 mana" + "\n20% chance not to consume thrown items");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 50000;
			item.rare = 3;
			
			
			item.width = 22;
			item.height = 18;
			item.defense = 4;
			
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{

            player.GetModPlayer<QwertyPlayer>(mod).ammoReduction *= .9f;
            player.GetModPlayer<QwertyPlayer>(mod).throwReduction *= .8f;
            
			player.manaCost -= .15f;
			player.statManaMax2 += 60;
            var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            if (modPlayer.customDashSpeed < 4f)
            {
                modPlayer.customDashSpeed = 4f;
            }
        }
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
			if (male) equipSlot = mod.GetEquipSlot("RhuthiniumGreaves", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("RhuthiniumGreaves_Female", EquipType.Legs);
		}
		
		
		
		
		

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 14);
			
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
			
	}
		
	
}

