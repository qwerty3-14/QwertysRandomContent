using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Ranger
{
	[AutoloadEquip(EquipType.Legs)]
	public class RangerPants : ModItem
	{
		public override bool Autoload(ref string name)
        {
			if (!Main.dedServ)
                {
                    // Add the female leg variant
                    mod.AddEquipTexture(null, EquipType.Legs, "RangerPants_Female", "QwertysRandomContent/Items/Armor/Ranger/RangerPants_FemaleLegs");
                }
				return true;
			
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ranger Pants");
			Tooltip.SetDefault("10% increased move speed" + "\nSet bonus with Ranger Vest");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 10000;
			item.rare = 1;
			
			
			item.width = 22;
			item.height = 18;
			item.defense = 2;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.10f;
			
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return  body.type == mod.ItemType("RangerVest");
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "10% ranged crit";
			player.rangedCrit += 10;
			
        }
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
			if (male) equipSlot = mod.GetEquipSlot("RangerPants", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("RangerPants_Female", EquipType.Legs);
		}
		

		public override void AddRecipes()
		{
			
			
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 6);
			recipe.AddIngredient(331, 2);
			recipe.AddTile(TileID.Loom);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			
			
			
		}
			
	}
		
	
}

