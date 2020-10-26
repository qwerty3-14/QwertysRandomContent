using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.TwistedDark
{
    [AutoloadEquip(EquipType.Body)]
    public class TwistedDarkBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Etimsic Half Plate");
            Tooltip.SetDefault("15% reduced cooldown on quick morphs");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(gold: 1);
            item.rare = 3;
            item.value = 120000;
            item.width = 22;
            item.height = 12;
            item.defense = 1;
            item.GetGlobalItem<ShapeShifterItem>().equipedMorphDefense = 7;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("EtimsMaterial"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("TwistedDarkMask") && legs.type == mod.ItemType("TwistedDarkLegs");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.DarkSet");
            if (player.GetModPlayer<ShapeShifterPlayer>().morphTime > 0)
            {
                player.lifeRegen += 8;
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration *= .85f;
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;

            drawArms = true;
        }
    }
}