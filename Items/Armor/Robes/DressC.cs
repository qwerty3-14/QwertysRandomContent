using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Robes
{
    [AutoloadEquip(EquipType.Body)]

    class DressC : ModItem
    {

        public override bool Autoload(ref string name)
        {
            // All code below runs only if we're not loading on a server
            if (!Main.dedServ)
            {
                // Add certain equip textures
                mod.AddEquipTexture(new DressLegsC(), null, EquipType.Legs, "DressC_Legs", "QwertysRandomContent/Items/Armor/Robes/DressC_Legs");
                mod.AddEquipTexture(new DressLegsCFemale(), null, EquipType.Legs, "DressC_FemaleLegs", "QwertysRandomContent/Items/Armor/Robes/DressC_FemaleLegs");
            }
            return true;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dress style C");


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

            if (male) equipSlot = mod.GetEquipSlot("DressC_Legs", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("DressC_FemaleLegs", EquipType.Legs);

        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
            drawArms = true;
        }






    }

    public class DressLegsC : EquipTexture
    {
    }

    public class DressLegsCFemale : EquipTexture
    {
    }


}
