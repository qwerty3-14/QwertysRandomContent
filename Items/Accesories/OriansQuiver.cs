using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class OriansQuiver : ModItem //whoops spelling error
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orion's Quiver");
            Tooltip.SetDefault("When shooting a bow a second arrow will be shot in the opposite direction \n10% increased arrow damage \n20% chance not to consume arrows and increased arrow velocity");

        }

        public override void SetDefaults()
        {
            item.rare = 1;
            item.value = 10000;
            item.width = 16;
            item.height = 16;
            item.accessory = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("Gemini"));
            recipe.AddIngredient(ItemID.MagicQuiver);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GeminiEffect>().effect = true;
            player.magicQuiver = true;
            player.arrowDamage += 0.1f;
        }
    }
}
