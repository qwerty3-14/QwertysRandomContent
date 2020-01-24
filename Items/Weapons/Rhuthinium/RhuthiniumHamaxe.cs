using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Rhuthinium
{
    public class RhuthiniumHamaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Hamaxe");
            Tooltip.SetDefault("");

        }
        public override void SetDefaults()
        {
            item.damage = 11;
            item.melee = true;

            item.useTime = 14;
            item.useAnimation = 26;
            item.useStyle = 1;
            item.knockBack = 3;
            item.value = 50000;
            item.rare = 3;
            item.UseSound = SoundID.Item1;

            item.width = 74;
            item.height = 70;
            item.crit = 5;
            item.autoReuse = true;
            item.hammer = 75;
            item.axe = 33;
            item.tileBoost = 1;




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

