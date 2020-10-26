using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Bionic
{
    [AutoloadEquip(EquipType.Legs)]
    public class BionicLimbs : ModItem
    {
        public override bool Autoload(ref string name)
        {
            return true;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bionic Limbs");
            Tooltip.SetDefault("15% increased morph damage\n10% reduced cooldown on quick morphs");
        }

        public override void SetDefaults()
        {
            item.rare = 5;
            item.value = Item.sellPrice(gold: 5);
            item.defense = 7;
            item.GetGlobalItem<ShapeShifterItem>().equipedMorphDefense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().morphDamage += .15f;
            player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration *= .9f;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = mod.GetEquipSlot("BionicLimbs_Legs", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("BionicLimbs_FemaleLegs", EquipType.Legs);
        }

        public override bool DrawLegs()
        {
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofMight, 8);
            recipe.AddIngredient(ItemID.HallowedBar, 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
