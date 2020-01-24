using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Consumables
{

    public class Luneshine : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luneshine");
            Tooltip.SetDefault("+8 recovery");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.maxStack = 30;
            item.rare = 3;
            item.useAnimation = 17;
            item.useTime = 45;
            item.useStyle = 2;
            item.UseSound = SoundID.Item3;
            item.consumable = true;
            //item.potion = true;
            item.useTurn = true;
            item.buffType = mod.BuffType("LuneRecovery");
            item.buffTime = 9 * 60 * 60;

        }


        public override bool CanUseItem(Player player)
        {

            return true;
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.Moonglow);
            recipe.AddIngredient(ItemID.Daybloom);
            recipe.AddIngredient(mod.ItemType("LuneBar"));
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }


    }
}