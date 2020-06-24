using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class NaturesStar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nature's Star");
            Tooltip.SetDefault("increases mana by 20 \n10% reduced mana usage");
        }

        public override void SetDefaults()
        {
            item.rare = 1;
            item.value = 10000;
            item.width = 28;
            item.height = 26;
            item.accessory = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.NaturesGift);
            recipe.AddIngredient(ItemID.BandofStarpower);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.manaCost -= .1f;
            player.statManaMax2 += 20;
        }
    }
}