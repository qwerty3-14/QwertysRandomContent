using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Clay
{
    [AutoloadEquip(EquipType.Body)]
    public class ClayPlate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clay Plate");
            Tooltip.SetDefault("10% reduced cooldown on quick morphs");
        }

        public override void SetDefaults()
        {
            item.value = 30000;
            item.rare = 1;

            item.width = 22;
            item.height = 12;
            item.defense = 2;
            item.GetGlobalItem<ShapeShifterItem>().equipedMorphDefense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration *= .9f;
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = true;
            drawHands = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ClayBlock, 30);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}