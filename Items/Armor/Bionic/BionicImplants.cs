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
    [AutoloadEquip(EquipType.Body)]
    public class BionicImplants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bionic Implants");
            Tooltip.SetDefault("10% increased morph damage and critical strike chance\n20% reduced cooldown on quick morphs");
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
            player.GetModPlayer<ShapeShifterPlayer>().morphDamage += .1f;
            player.GetModPlayer<ShapeShifterPlayer>().morphCrit += 10;
            player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration *= .8f;
        }
        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = true;
            drawHands = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofMight, 8);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
