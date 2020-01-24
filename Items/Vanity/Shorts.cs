using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Vanity
{
    [AutoloadEquip(EquipType.Legs)]
    public class Shorts : ModItem
    {
        public override bool Autoload(ref string name)
        {
            if (!Main.dedServ)
            {
                // Add the female leg variant
                mod.AddEquipTexture(null, EquipType.Legs, "Shorts_Female", "QwertysRandomContent/Items/Vanity/Shorts_FemaleLegs");
            }
            return true;

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shorts");
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
            if (male) equipSlot = mod.GetEquipSlot("Shorts", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("Shorts_Female", EquipType.Legs);
        }




    }


}

