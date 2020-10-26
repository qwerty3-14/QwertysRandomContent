using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Rhuthinium
{
    public class RhuthiniumBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Doubow");
            Tooltip.SetDefault("");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;

            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            item.knockBack = 2;
            item.value = 25000;
            item.rare = 3;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.width = 26;
            item.height = 62;
            item.crit = 5;
            item.shoot = 40;
            item.useAmmo = 40;
            item.shootSpeed = 10;
            item.noMelee = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float rot = new Vector2(speedX, speedY).ToRotation();
            Projectile.NewProjectile(position + QwertyMethods.PolarVector(6, rot) + QwertyMethods.PolarVector(7.5f, rot + (float)Math.PI / 2), new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(position + QwertyMethods.PolarVector(6, rot) + QwertyMethods.PolarVector(-7.5f, rot + (float)Math.PI / 2), new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}