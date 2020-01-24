using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Rhuthinium
{
    [AutoloadEquip(EquipType.Head)]
    public class RhuthiniumHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Hat");
            Tooltip.SetDefault("+11% magic damage and critical strike chance");

        }


        public override void SetDefaults()
        {

            item.value = 50000;
            item.rare = 3;


            item.width = 26;
            item.height = 12;
            item.defense = 2;



        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += .11f;
            player.magicCrit += 11;
        }
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("RhuthiniumChestplate") && legs.type == mod.ItemType("RhuthiniumGreaves");

        }



        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.RHatSet");
            player.magicDamage += ((player.statMana * 1.0f) / (player.statManaMax2 * 1.0f) * .2f);


        }




        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 12);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }


}

