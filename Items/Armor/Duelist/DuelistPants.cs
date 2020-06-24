using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Duelist
{
    [AutoloadEquip(EquipType.Legs)]
    public class DuelistPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Duelist Pants");
            Tooltip.SetDefault("Melee attacks reduce the cooldown on quick morphs \n4% increased melee damage");
        }

        public override bool Autoload(ref string name)
        {
            if (!Main.dedServ)
            {
                // Add the female leg variant
                mod.AddEquipTexture(new DuelistLegs(), null, EquipType.Legs, "DuelistPants_Legs", "QwertysRandomContent/Items/Armor/Duelist/DuelistPants_Legs");
                mod.AddEquipTexture(new DuelistLegsFemale(), null, EquipType.Legs, "DuelistPants_FemaleLegs", "QwertysRandomContent/Items/Armor/Duelist/DuelistPants_FemaleLegs");
            }
            return true;
        }

        public override void SetDefaults()
        {
            item.value = 50000;
            item.rare = 1;
            item.width = 22;
            item.height = 12;
            item.defense = 5;
        }

        public override bool DrawLegs()
        {
            return false;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<DuelistEffects>().legs = true;
            player.meleeDamage += .04f;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = mod.GetEquipSlot("DuelistPants_Legs", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("DuelistPants_FemaleLegs", EquipType.Legs);
        }
    }

    public class DuelistLegs : EquipTexture
    {
    }

    public class DuelistLegsFemale : EquipTexture
    {
        public override bool DrawLegs()
        {
            return false;
        }
    }

    public class DuelestRobeDrawing : ModPlayer
    {
        public static readonly PlayerLayer Pants = LayerDrawing.DrawOnLegs("DuelistPants_Legs", "Items/Armor/Duelist/DuelistRobeFront", "DuelistPants_FemaleLegs", "Items/Armor/Duelist/DuelistRobeFront_Female", "Pants", false, 1);

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int legLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Legs"));
            if (legLayer != -1)
            {
                Pants.visible = true;
                layers.Insert(legLayer + 1, Pants);
            }
        }

        public override void ModifyDrawHeadLayers(List<PlayerHeadLayer> layers)
        {
            base.ModifyDrawHeadLayers(layers);
        }
    }
}