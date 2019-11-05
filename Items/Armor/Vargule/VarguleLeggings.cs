using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Vargule
{
	
	[AutoloadEquip(EquipType.Legs)]
	public class VarguleLeggings : ModItem
	{
		public override bool Autoload(ref string name)
        {
			if (!Main.dedServ)
                {
                    // Add the female leg variant
                    mod.AddEquipTexture(null, EquipType.Legs, "VarguleLeggings_Female", "QwertysRandomContent/Items/Armor/Vargule/VarguleLeggings_FemaleLegs");
                }
				return true;
			
		}
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Leggings");
			Tooltip.SetDefault("Lets you dash (5.4 dash power)" + "\n20% chance not to consume ammo" + "\n20% reduced mana usage" + "\n+80 max mana" + "\n30% chance not to consume thrown items"+ "\n+12% melee and movement speed");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 50000;
			item.rare = 8;
			
			
			item.width = 22;
			item.height = 18;
			item.defense = 12;
			
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{


            player.GetModPlayer<QwertyPlayer>().ammoReduction *= .8f;
            player.GetModPlayer<QwertyPlayer>().throwReduction *= .7f;

           
			player.manaCost -= .2f;
			player.statManaMax2 += 80;
			player.meleeSpeed += .12f;
			player.moveSpeed += .12f;
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (modPlayer.customDashSpeed < 5.4f)
            {
                modPlayer.customDashSpeed = 5.4f;
            }

        }
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
			
			if (male) equipSlot = mod.GetEquipSlot("VarguleLeggings", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("VarguleLeggings_Female", EquipType.Legs);
			
		}
		
		
		
		
		
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VarguleBar"), 15);
			
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
			
	}
		
	
}

