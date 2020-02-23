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
    public class GuardEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guard Emblem");
            Tooltip.SetDefault("10 defense \n10% increased damage \nEnemies are more likely to target you");
        }

        public override void SetDefaults()
        {
            item.rare = 5;
            item.value = Item.sellPrice(gold: 15);
            item.width = 28;
            item.height = 28;
            item.accessory = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AvengerEmblem);
            recipe.AddIngredient(ItemID.FleshKnuckles);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.allDamage += .1f;
            player.statDefense += 10;
            player.aggro += 400;
        }
    }
}
