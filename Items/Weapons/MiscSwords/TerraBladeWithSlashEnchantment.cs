using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSwords
{
    public class TerraBladeWithSlashEnchantment : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Blade With Slash Enchantment");
            Tooltip.SetDefault("Right click for a powerful slash attack");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.TerraBlade);
        }

        public override bool OnlyShootOnSwing => true;

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useStyle = 5;
                item.noUseGraphic = true;
                item.useAnimation = 8;
                item.useTime = 8;
                item.noMelee = true;
            }
            else
            {
                item.useAnimation = 16;
                item.useTime = 16;
                item.useStyle = 1;
                item.noUseGraphic = false;
                item.noMelee = false;
            }
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.TerraBlade);
            recipe.AddIngredient(mod.ItemType("CraftingRune"), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public int useAlt = 1;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                if (useAlt == 1)
                {
                    Projectile.NewProjectile(new Vector2(player.MountedCenter.X + (float)Math.Cos(new Vector2(speedX, speedY).ToRotation()) * 60, player.MountedCenter.Y + (float)Math.Sin(new Vector2(speedX, speedY).ToRotation()) * 60), new Vector2(speedX * .01f, speedY * .01f), mod.ProjectileType("TerraSlash"), (int)(damage * 5f), knockBack, player.whoAmI);
                }
                else
                {
                    Projectile.NewProjectile(new Vector2(player.MountedCenter.X + (float)Math.Cos(new Vector2(speedX, speedY).ToRotation()) * 60, player.MountedCenter.Y + (float)Math.Sin(new Vector2(speedX, speedY).ToRotation()) * 60), new Vector2(speedX * .01f, speedY * .01f), mod.ProjectileType("TerraSlashB"), (int)(damage * 5f), knockBack, player.whoAmI);
                }
                Projectile.NewProjectile(new Vector2(player.MountedCenter.X + (float)Math.Cos(new Vector2(speedX, speedY).ToRotation()) * 30, player.MountedCenter.Y + (float)Math.Sin(new Vector2(speedX, speedY).ToRotation()) * 30), new Vector2(speedX * .01f, speedY * .01f), mod.ProjectileType("TerraSlashInv"), (int)(damage * 5f), knockBack, player.whoAmI);
                Projectile.NewProjectile(new Vector2(player.MountedCenter.X + (float)Math.Cos(new Vector2(speedX, speedY).ToRotation()) * 00, player.MountedCenter.Y + (float)Math.Sin(new Vector2(speedX, speedY).ToRotation()) * 00), new Vector2(speedX * .01f, speedY * .01f), mod.ProjectileType("TerraSlashInv"), (int)(damage * 5f), knockBack, player.whoAmI);
                useAlt *= -1;
                return false;
            }
            else
            {
                damage = (int)(damage * 1.25f);
                return true;
            }
        }
    }

    public class TerraSlash : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Slash");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 40;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 4;
        }

        public bool runOnce = true;
        public Vector2 oriVel;

        public override void AI()
        {
            projectile.rotation = (projectile.velocity).ToRotation() + (float)(Math.PI / 2);
        }
    }

    public class TerraSlashB : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Slash");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 40;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 4;
        }

        public bool runOnce = true;
        public Vector2 oriVel;

        public override void AI()
        {
            projectile.rotation = (projectile.velocity).ToRotation() + (float)(Math.PI / 2);
        }
    }

    public class TerraSlashInv : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Slash");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 40;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 4;
        }

        public bool runOnce = true;
        public Vector2 oriVel;

        public override void AI()
        {
            projectile.rotation = (projectile.velocity).ToRotation() + (float)(Math.PI / 2);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
    }
}