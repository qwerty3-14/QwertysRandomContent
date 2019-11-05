using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Vitallum
{
    public class Vitallum : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vitallum (WIP)");
            Tooltip.SetDefault("");

        }
        /*
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 20);
            recipe.AddIngredient(ItemID.LifeCrystal, 1);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.SetResult(this, 40);
            recipe.AddRecipe();
        }
        */
        public override void SetDefaults()
        {
            item.value = 30000;
            item.width = 18;
            item.height = 24;
            item.maxStack = 999;
            item.rare = 7;
        }
    }
}
