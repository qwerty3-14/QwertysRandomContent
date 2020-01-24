using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    [AutoloadEquip(EquipType.HandsOn)]

    public class LeatherGloveMainHand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leather Glove (Front Hand)");
            Tooltip.SetDefault("Just a regular old leather glove");

        }

        public override void SetDefaults()
        {

            item.value = 10000;
            item.rare = 1;


            item.width = 22;
            item.height = 28;
            item.vanity = true;
            item.accessory = true;



        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }


}

