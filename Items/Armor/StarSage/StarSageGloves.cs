using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.StarSage
{
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]

    public class StarSageGloves : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Sage Gloves");


        }

        public override void SetDefaults()
        {

            item.value = 10000;
            item.rare = 3;


            item.width = 28;
            item.height = 22;
            item.vanity = true;
            item.accessory = true;



        }

    }


}

