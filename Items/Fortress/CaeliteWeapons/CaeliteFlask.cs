using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.CaeliteWeapons
{

    public class CaeliteFlask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flask of Caelite Wrath");
            Tooltip.SetDefault("Melee attacks inflict caelite wrath");
        }

        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.maxStack = 30;
            item.consumable = true;
            item.width = 14;
            item.height = 24;
            item.buffType = mod.BuffType("CaeliteImbune");
            item.buffTime = 72000;
            item.value = Item.sellPrice(0, 0, 5, 0);
            item.rare = 4;

        }


        public override bool CanUseItem(Player player)
        {

            return true;
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"));
            recipe.AddTile(TileID.ImbuingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }


    }
}