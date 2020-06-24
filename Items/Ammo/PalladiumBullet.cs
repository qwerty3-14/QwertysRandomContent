using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
    public class PalladiumBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Palladium Bullet");
            Tooltip.SetDefault("Right click to redirect in flight!");
        }

        public override void SetDefaults()
        {
            item.damage = 10;
            item.ranged = true;
            item.knockBack = 2;
            item.value = 1;
            item.rare = 3;
            item.width = 16;
            item.height = 22;

            item.shootSpeed = 16;

            item.consumable = true;
            item.shoot = mod.ProjectileType("PalladiumBulletP");
            item.ammo = 97;
            item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PalladiumBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }

    public class PalladiumBulletP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Palladium Bullet");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.ranged = true;
            projectile.timeLeft = 300;
        }

        public bool runOnce = true;
        public bool HasRightClicked = false;

        public float targetRotation;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (Main.mouseRight && projectile.timeLeft <= 290 || HasRightClicked)
            {
                projectile.alpha = 0;
                if (runOnce)
                {
                    HasRightClicked = true;
                    projectile.timeLeft = 3600;
                    runOnce = false;
                    projectile.netUpdate = true;
                }

                projectile.velocity.X = (float)Math.Cos(targetRotation + MathHelper.ToRadians(-90)) * 20f;
                projectile.velocity.Y = (float)Math.Sin(targetRotation + MathHelper.ToRadians(-90)) * 20f;
            }
            else
            {
                projectile.alpha = (int)(255f - ((float)projectile.timeLeft / 300f) * 255f);

                if (Main.LocalPlayer == player)
                {
                    projectile.ai[0] = Main.MouseWorld.X;
                    projectile.ai[1] = Main.MouseWorld.Y;
                    projectile.netUpdate = true;

                    //projectile.netUpdate = true;
                }
                targetRotation = (new Vector2(projectile.ai[0], projectile.ai[1]) - projectile.Center).ToRotation() + (float)Math.PI / 2;
            }
            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(HasRightClicked);
            writer.Write(runOnce);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            HasRightClicked = reader.ReadBoolean();
            runOnce = reader.ReadBoolean();
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
        }
    }
}