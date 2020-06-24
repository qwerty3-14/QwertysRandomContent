using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.RuneGhostItems
{
    [AutoloadEquip(EquipType.Body)]
    internal class RunicRobe : ModItem
    {
        public override bool Autoload(ref string name)
        {
            // All code below runs only if we're not loading on a server
            if (!Main.dedServ)
            {
                // Add certain equip textures
                mod.AddEquipTexture(new DressLegs(), null, EquipType.Legs, "RunicRobe_Legs", "QwertysRandomContent/Items/RuneGhostItems/RunicRobe_Legs");
                mod.AddEquipTexture(new DressLegsFemale(), null, EquipType.Legs, "RunicRobe_FemaleLegs", "QwertysRandomContent/Items/RuneGhostItems/RunicRobe_FemaleLegs");
            }
            return true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Runic Robe");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.rare = 1;
            item.vanity = true;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            robes = true;

            if (male) equipSlot = mod.GetEquipSlot("RunicRobe_Legs", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("RunicRobe_FemaleLegs", EquipType.Legs);
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
            drawArms = true;
        }
    }

    public class DressLegs : EquipTexture
    {
    }

    public class DressLegsFemale : EquipTexture
    {
    }
}