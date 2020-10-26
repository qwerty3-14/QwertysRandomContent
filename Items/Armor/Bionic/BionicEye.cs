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
    [AutoloadEquip(EquipType.Head)]
    public class BionicEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bionic Eye");
            Tooltip.SetDefault("15% increased morph critical strike chance\n10% reduced cooldown on quick morphs\nEmpowers the spazer's eye");
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
            player.GetModPlayer<ShapeShifterPlayer>().morphCrit += 15;
            player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration *= .9f;
            player.GetModPlayer<BionicEffects>().eyeEquiped = true;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("BionicImplants") && legs.type == mod.ItemType("BionicLimbs");
        }

        public override void UpdateArmorSet(Player player)
        {
            String s = "Please go to conrols and bind the 'Yet another special ability key'";
            foreach (String key in QwertysRandomContent.YetAnotherSpecialAbility.GetAssignedKeys()) //get's the string of the hotkey's name
            {
                s = "Press " + key + " to activate overrdrive doubling the speed of your stable morphs!.";
            }
            player.setBonus = s;
            player.GetModPlayer<BionicEffects>().setBonus = true;
        }
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofMight, 8);
            recipe.AddIngredient(ItemID.HallowedBar, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
