using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class UltraManuverabilityBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frog Sprung Boots");
            Tooltip.SetDefault("Allows super fast running" + "\nAllows flight" + "\nIncreases jump speed" + "\nAllows auto jump" + "\nProvides immunity to fall damage");
        }

        public override void SetDefaults()
        {
            item.value = 10000;
            item.rare = 1;

            item.width = 32;
            item.height = 28;

            item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.accRunSpeed = 6.75f;
            player.rocketBoots = 3;
            player.moveSpeed += 0.1f;
            player.maxRunSpeed += 0.1f;
            player.noFallDmg = true;
            player.autoJump = true;
            player.jumpSpeedBoost += 2.5f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FrogLeg);
            recipe.AddIngredient(ItemID.LightningBoots);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}