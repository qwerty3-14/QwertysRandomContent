using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscBows
{
    public class AerousLongbowWithRandomEnchantment : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Irregular Enchanted Aerous");
            Tooltip.SetDefault("The crazy bows are cool and all, but sometimes you just need a fairly normal bow with good stats" + "\nRight click to shoot a random arrow");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(mod.ItemType("AerousLongbow"));
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
                Vector2 trueSpeed = new Vector2(speedX, speedY);

                bool yes = true;
                float anotherSpeedVariable = trueSpeed.Length();
                damage = (int)(item.damage * player.rangedDamage);
                knockBack = item.knockBack * knockBack;
                modPlayer.PickRandomAmmo(item, ref type, ref anotherSpeedVariable, ref yes, ref damage, ref knockBack, Main.rand.Next(2) == 0);
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);

                return false;
            }
            else
            {
                return true;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddRecipeGroup("QwertysrandomContent:AerousBow");
            recipe.AddIngredient(mod.ItemType("CraftingRune"), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}