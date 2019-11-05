using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static QwertysRandomContent.Items.Accesories.SkywardHilt;
using static QwertysRandomContent.Items.BladeBossItems.SwordsmanBadge;

namespace QwertysRandomContent.Items.Accesories
{
    public class SwordmastersSeal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordmasters Seal");
            Tooltip.SetDefault("Greatly enhances swordplay performance!" + "\nMakes your sword much larger" + "\nHitting things with your sword while airborne does more damage" + "\nSwing your sword faster when standing still");

        }

        public override void SetDefaults()
        {
            item.rare = 7;
            item.value = Item.sellPrice(0, 50, 0, 0);
            item.width = 18;
            item.height = 16;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SkywardHiltEffect>().effect = true;
            player.GetModPlayer<BigSword>().Enlarger += 1f;
            player.GetModPlayer<SwordsmanBadgeEffect>().effect = true;

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("SwordsmanBadge"), 1);
            recipe.AddIngredient(mod.ItemType("SkywardHilt"), 1);
            recipe.AddIngredient(mod.ItemType("SwordEmbiggenner"), 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
