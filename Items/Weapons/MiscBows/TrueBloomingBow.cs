using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscBows
{
    public class TrueBloomingBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Blooming Bow");
            Tooltip.SetDefault("Randomly picks 1-11 randomly random arrows to fire in randomly random directions at randomly random velocities randomly!");
        }

        public override void SetDefaults()
        {
            item.damage = 27;
            item.ranged = true;

            item.useTime = 50;
            item.useAnimation = 45;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = 496036;
            item.rare = 6;
            item.UseSound = SoundID.Item5;

            item.width = 32;
            item.height = 74;

            item.shoot = 40;
            item.useAmmo = 40;
            item.shootSpeed = 4;
            item.noMelee = true;
            item.autoReuse = true;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("BloomingBow"));
            recipe.AddIngredient(mod.ItemType("WornPrehistoricBow"));
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int numberProjectiles = 1 + Main.rand.Next(11);
            for (int i = 0; i < numberProjectiles; i++)
            {
                QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
                Vector2 trueSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
                float scale = Main.rand.NextFloat(.7f, 2.3f);
                trueSpeed = trueSpeed * scale;
                bool yes = true;
                float anotherSpeedVariable = trueSpeed.Length();
                int currentDmg = (int)(item.damage * player.rangedDamage);
                float currentKnockBack = item.knockBack * knockBack;
                modPlayer.PickRandomAmmo(item, ref type, ref anotherSpeedVariable, ref yes, ref currentDmg, ref currentKnockBack, Main.rand.Next(2) == 0);
                Projectile.NewProjectile(position.X + Main.rand.Next(-18, 18), position.Y + Main.rand.Next(-18, 18), trueSpeed.X, trueSpeed.Y, type, currentDmg, currentKnockBack, player.whoAmI);
            }
            return false;
        }
    }
}