using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.DevItems.qwerty
{
	[AutoloadEquip(EquipType.Body)]

	class Toga : ModItem
	{
		
		public override bool Autoload(ref string name)
        {
		// All code below runs only if we're not loading on a server
			if (!Main.dedServ)
			{
				// Add certain equip textures
			mod.AddEquipTexture(new TogaLegs(), null, EquipType.Legs, "Toga_Legs", "QwertysRandomContent/Items/DevItems/qwerty/Toga_Legs");
			mod.AddEquipTexture(new TogaLegsFemale(), null, EquipType.Legs, "Toga_FemaleLegs", "QwertysRandomContent/Items/DevItems/qwerty/Toga_FemaleLegs");
			}
			return true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toga");
            Tooltip.SetDefault("Because pants are so barbaric!"+ "\nDev Item");
			
		}
		public override void SetDefaults()
		{
            item.vanity = true;
            item.width = 18;
			item.height = 14;
			item.rare = 10;
			item.vanity = true;
		}
		
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			
			if (male) equipSlot = mod.GetEquipSlot("Toga_Legs", EquipType.Legs);
			if (!male) equipSlot = mod.GetEquipSlot("Toga_FemaleLegs", EquipType.Legs);
            
		}
		
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
			drawArms = true;
		}
		
		
		
		
	}
	
	public class TogaLegs : EquipTexture
    {	
    }
	public class TogaLegsFemale : EquipTexture
    {
	}
	
}
