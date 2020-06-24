using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class HydratedSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrated Skull");
            Tooltip.SetDefault("Increases your maximun number of minions by 1" + "\nMakes minion attacks trigger melee effects" + "\nAllows most minions to summon more minions to fill empty minion slots" + "\nHide the accesory to disable this effect");
        }

        public override void SetDefaults()
        {
            item.value = 200000;
            item.rare = 5;

            item.width = 18;
            item.height = 16;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (!hideVisual)
            {
                modPlayer.hydraCharm = true;
            }
            modPlayer.minionFang = true;
            player.maxMinions++;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("HydraCharm"), 1);
            recipe.AddIngredient(mod.ItemType("MinionFang"), 1);
            recipe.AddIngredient(1158, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}