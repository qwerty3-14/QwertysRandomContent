
using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons
{
    public class ChargingShotgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Charging Shotgun");
            Tooltip.SetDefault("Right click to add an extra bullet to your next fire");

        }
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicGunChakram ? base.Texture + "_Old" : base.Texture;
        public override void SetDefaults()
        {
            item.damage = 100;
            item.ranged = true;

            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = 500000;
            item.rare = 9;
            item.UseSound = SoundID.Item11;

            item.width = 56;
            item.height = 34;

            item.shoot = 97;
            item.useAmmo = 97;
            item.shootSpeed = 6f;
            item.noMelee = true;
            item.autoReuse = true;
            item.noUseGraphic = false;


        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public int numberProjectiles = 1;
        public float colorProgress = .02f;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.shoot = 0;
                item.useAmmo = -1;
                item.useTime = 12;
                item.useAnimation = 12;
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/click").WithVolume(.8f).WithPitchVariance(.5f);

                numberProjectiles++;
                item.useStyle = 1;
                item.noUseGraphic = true;
                if (numberProjectiles > 50)
                {
                    numberProjectiles = 50;
                    CombatText.NewText(player.getRect(), new Color(colorProgress, colorProgress, colorProgress), "MAX!", true, false);
                }
                else
                {
                    colorProgress += .02f;
                    CombatText.NewText(player.getRect(), new Color(colorProgress, colorProgress, colorProgress), numberProjectiles, true, false);
                }
            }
            else
            {
                item.shoot = 97;
                item.useAmmo = 97;

                item.useTime = 60;
                item.useAnimation = 60;
                if (numberProjectiles > 1)
                {
                    item.UseSound = SoundID.Item38;
                }
                else
                {
                    item.UseSound = SoundID.Item11;
                }
                item.useStyle = 5;
                item.noUseGraphic = false;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            if (player.altFunctionUse == 2)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < numberProjectiles; i++)
                {

                    Vector2 trueSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
                    float scale = Main.rand.NextFloat(.9f, 1.1f);
                    trueSpeed *= scale;

                    float shellShift = MathHelper.ToRadians(-50);
                    float SVar = shellShift + MathHelper.ToRadians(Main.rand.Next(-100, 301) / 10);
                    float Sspeed = .05f * Main.rand.Next(15, 41);
                    Projectile.NewProjectile(position.X, position.Y, (float)Math.Cos(SVar) * Sspeed * -player.direction, (float)Math.Sin(SVar) * Sspeed, mod.ProjectileType("Shell"), 0, 0, Main.myPlayer);
                    Projectile.NewProjectile(position.X, position.Y, trueSpeed.X, trueSpeed.Y, type, damage, knockBack, player.whoAmI);
                }
                colorProgress = .02f;
                numberProjectiles = 1;
                return false;
            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, -0);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Shotgun);
            recipe.AddIngredient(mod.ItemType("CraftingRune"), 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }



    }


}

