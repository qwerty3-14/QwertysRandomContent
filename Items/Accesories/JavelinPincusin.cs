using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class JavelinPincusin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Javelin Pincusin");
            Tooltip.SetDefault("Doubles the amount of javelins you can stick into an enemy \nOnly works on javelins from this qwerty's bosses and items");
        }

        public override void SetDefaults()
        {
            item.value = 10000;
            item.rare = 1;
            item.width = 20;
            item.height = 18;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<QwertyPlayer>().PincusionMultiplier *= 2f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.AddRecipeGroup("IronBar", 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}