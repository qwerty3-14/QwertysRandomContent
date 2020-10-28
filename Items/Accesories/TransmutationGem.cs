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
    class TransmutationGem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Transmutation Gem");
            Tooltip.SetDefault("Increases morph damage by 15%\nReduces the cooldown on quick morphs by 10%");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(gold: 5);
            item.rare = 5;
            item.width = 26;
            item.height = 32;
            item.accessory = true;
            item.GetGlobalItem<ShapeShifterItem>().equipedMorphDefense = 10;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ShapeShifterPlayer>().morphDamage += .15f;
            player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration *= .9f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("MorphGem"), 1);
            recipe.AddIngredient(mod.ItemType("ModelBone"), 1);
            recipe.AddIngredient(ItemID.AvengerEmblem, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
