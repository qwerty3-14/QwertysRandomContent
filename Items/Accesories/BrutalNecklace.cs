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
    public class BrutalNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brutal Necklace");
            Tooltip.SetDefault("Increases armor penetration by 6");
        }

        public override void SetDefaults()
        {
            item.rare = 1;
            item.defense = 2;
            item.value = Item.sellPrice(gold: 3);
            item.width = 28;
            item.height = 30;
            item.accessory = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Shackle);
            recipe.AddIngredient(ItemID.SharkToothNecklace);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.armorPenetration += 6;
        }
    }
}
