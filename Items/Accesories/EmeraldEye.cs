using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{


    public class EmeraldEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Emerald Eye");
            Tooltip.SetDefault("Using a quick morph or being in a stable morph for 10 seconds will grant you 'Eye's knowledge'" + "\n'Eye's knowledge' prevents morph sickness the next time you use a stable morph");

        }

        public override void SetDefaults()
        {


            item.rare = 1;
            item.value = 200;

            item.width = 14;
            item.height = 14;

            item.accessory = true;



        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Rope, 4);
            recipe.AddIngredient(ItemID.Emerald, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ShapeShifterPlayer>().EyeEquiped = true;

            if (player.GetModPlayer<ShapeShifterPlayer>().morphTime > 600)
            {
                player.GetModPlayer<ShapeShifterPlayer>().EyeBlessing = true;
            }


        }



    }


}

