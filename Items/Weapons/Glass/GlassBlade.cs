using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.Weapons.Glass
{
    public class GlassBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glass Blade");
            Tooltip.SetDefault("Breaks into shards upon striking an enemy");

        }
        public override void SetDefaults()
        {
            item.damage = 14;
            item.thrown = true;

            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = 1;
            item.knockBack = 3;
            item.value = 1;
            item.rare = 0;
            item.UseSound = SoundID.Item1;

            item.width = 64;
            item.height = 64;
            item.maxStack = 999;
            item.autoReuse = true;
            //item.scale = 5;



        }
        int useCounter = 0;
        public override bool CanUseItem(Player player)
        {
            return useCounter == 0;
        }
        public override void UpdateInventory(Player player)
        {
            if (useCounter > 0)
            {
                useCounter--;

            }
        }
        public override bool ConsumeItem(Player player)
        {
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Glass, 1);
            recipe.AddTile(TileID.GlassKiln);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }


        public override bool? CanHitNPC(Player player, NPC target)
        {
            float swordLength = item.Size.Length() * item.scale;
            float r = player.direction == 1 ? player.itemRotation - (float)Math.PI / 4 : player.itemRotation + 5 * (float)Math.PI / 4;
            if (player.gravDir == -1)
            {
                r += MathHelper.PiOver2 * player.direction;
            }
            return Collision.CheckAABBvLineCollision(target.position, target.Size, player.MountedCenter, player.MountedCenter + new Vector2((float)Math.Cos(r), (float)Math.Sin(r)) * swordLength);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (player.itemAnimation < player.itemAnimationMax - 5)
            {
                player.itemAnimation = 0;
                useCounter = 10;
                if (player.GetModPlayer<QwertyPlayer>().throwReduction > Main.rand.NextFloat())
                {
                    item.stack--;
                }

                float swordLength = item.Size.Length() * item.scale;


                float r = player.direction == 1 ? player.itemRotation - (float)Math.PI / 4 : player.itemRotation + 5 * (float)Math.PI / 4;
                if (player.gravDir == -1)
                {
                    r += MathHelper.PiOver2 * player.direction;
                }
                for (int p = 0; p < 20; p++)
                {
                    float distance = Main.rand.NextFloat(swordLength);
                    Projectile g = Main.projectile[Projectile.NewProjectile(player.MountedCenter + new Vector2((float)Math.Cos(r), (float)Math.Sin(r)) * distance, QwertyMethods.PolarVector(8, Main.rand.NextFloat(-1, 1) * (float)Math.PI), mod.ProjectileType("GlassBulletShard"), (int)(item.damage * .7f), item.knockBack, player.whoAmI)];
                    g.thrown = true;
                    g.ranged = false;
                }
            }
        }
    }




}

