using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Vanity
{
	[AutoloadEquip(EquipType.Legs)]
	public class SwimsuitBottom : ModItem
	{
		public override bool Autoload(ref string name)
        {
			if (!Main.dedServ)
                {
                    // Add the female leg variant
                    mod.AddEquipTexture(null, EquipType.Legs, "SwimsuitBottom_Female", "QwertysRandomContent/Items/Vanity/SwimsuitBottom_FemaleLegs");
                }
				return true;
			
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Swimsuit Bottom");
			Tooltip.SetDefault("");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 1000;
			item.rare = 1;
			
			
			item.width = 22;
			item.height = 10;
			item.vanity = true;
			
			
			
		}
		
		
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
			if (male) equipSlot = mod.GetEquipSlot("SwimsuitBottom", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("SwimsuitBottom_Female", EquipType.Legs);
		}
		

		
			
	}
		
	
}

