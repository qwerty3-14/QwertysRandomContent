using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.StarSage
{
	[AutoloadEquip(EquipType.Body)]

	class StarSage : ModItem
	{
		
		public override bool Autoload(ref string name)
        {
		// All code below runs only if we're not loading on a server
			if (!Main.dedServ)
			{
				// Add certain equip textures
			mod.AddEquipTexture(new StarSageLegs(), null, EquipType.Legs, "StarSage_Legs", "QwertysRandomContent/Items/Armor/StarSage/StarSage_Legs");
			mod.AddEquipTexture(new StarSageLegsFemale(), null, EquipType.Legs, "StarSage_FemaleLegs", "QwertysRandomContent/Items/Armor/StarSage/StarSage_FemaleLegs");
			}
			return true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Sage Robes");
			
			
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 30;
			item.rare = 1;
            item.value = 10000;
            item.vanity = true;
		}
		
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			
			if (male) equipSlot = mod.GetEquipSlot("StarSage_Legs", EquipType.Legs);
			if (!male) equipSlot = mod.GetEquipSlot("StarSage_FemaleLegs", EquipType.Legs);
            
		}
		
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
			drawArms = true;
		}
        
		
        
		
		
		
	}
	
	public class StarSageLegs : EquipTexture
    {	
    }
	
	public class StarSageLegsFemale : EquipTexture
    {
	}
	
	
}
