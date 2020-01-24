using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Vanity
{
    [AutoloadEquip(EquipType.Legs)]
    public class Miniskirt : ModItem
    {
        public override bool Autoload(ref string name)
        {
            if (!Main.dedServ)
            {
                // Add the female leg variant
                mod.AddEquipTexture(null, EquipType.Legs, "Miniskirt_Female", "QwertysRandomContent/Items/Vanity/Miniskirt_FemaleLegs");
            }
            return true;

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Miniskirt");
            Tooltip.SetDefault("");

        }


        public override void SetDefaults()
        {

            item.value = 1000;
            item.rare = 1;
            item.vanity = true;

            item.width = 22;
            item.height = 18;
            item.vanity = true;



        }


        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = mod.GetEquipSlot("Miniskirt", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("Miniskirt_Female", EquipType.Legs);
        }




    }


}

