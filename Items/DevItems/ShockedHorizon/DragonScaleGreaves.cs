using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.DevItems.ShockedHorizon
{
	
	[AutoloadEquip(EquipType.Legs)]
	public class DragonScaleGreaves : ModItem
	{
		public override bool Autoload(ref string name)
        {
			if (!Main.dedServ)
                {
                    // Add the female leg variant
                    //mod.AddEquipTexture(null, EquipType.Legs, "RhuthiniumGreaves_Female", "QwertysRandomContent/Items/Armor/Rhuthinium/RhuthiniumGreaves_FemaleLegs");
                }
				return true;
			
		}
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dragonscale Leggings");
			Tooltip.SetDefault("Good for shocking the horizons" + "\nDev Item");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 0;
			item.rare = 10;

            item.vanity = true;
            item.width = 20;
			item.height = 20;
			
			
			
			
			
		}
		
		
			
	}
		
	
}

