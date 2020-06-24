using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Consumables
{
    public class DodgePotion : ModItem
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicPotions ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dodge Potion");
            Tooltip.SetDefault("10% dodge chance");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 32;
            item.maxStack = 30;
            item.rare = 3;
            item.useAnimation = 17;
            item.useTime = 45;
            item.useStyle = 2;
            item.UseSound = SoundID.Item3;
            item.consumable = true;
            //item.potion = true;
            item.useTurn = true;
            item.buffType = mod.BuffType("DodgeBuff");
            item.buffTime = 60 * 60 * 10;
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
            recipe.AddIngredient(ItemID.Fireblossom);
            recipe.AddIngredient(mod.ItemType("FortressHarpyBeak"));
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}