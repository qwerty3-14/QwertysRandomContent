using System;


using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
    public class FinnedArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Finned Arrow");
            Tooltip.SetDefault("Excels in water");

        }
        public override void SetDefaults()
        {
            item.damage = 6;
            item.ranged = true;
            item.knockBack = 2;
            item.value = 20;
            item.rare = 2;
            item.width = 22;
            item.height = 32;

            item.shootSpeed = 1;

            item.consumable = true;
            item.shoot = mod.ProjectileType("FinnedArrowP");
            item.ammo = 40;
            item.maxStack = 999;


        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WoodenArrow, 200);
            recipe.AddIngredient(ItemID.SharkFin, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 200);
            recipe.AddRecipe();
        }


    }
    public class FinnedArrowP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Finned Arrow");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.ranged = true;
            projectile.arrow = true;




        }
        public float swimSpeed = 10;
        public float swimDirection;


        public int wanderTimer = 61;
        public bool runOnce = true;
        public float actDirection;
        public int f = 1;
        public float wiggle;
        public float wiggleTime;
        public float maxDistance = 1000f;
        public NPC prey;


        public override void AI()
        {
            if (runOnce)
            {

                actDirection = projectile.velocity.ToRotation();
                projectile.velocity /= 5;
                runOnce = false;
            }

            wanderTimer++;
            if (wanderTimer > 60)
            {
                if (Main.netMode == 1 && projectile.owner == Main.myPlayer)
                {
                    projectile.ai[1] = Main.rand.NextFloat(2 * (float)Math.PI);

                    if (Main.netMode == 1)
                    {
                        QwertysRandomContent.ProjectileAIUpdate(projectile);
                    }

                    projectile.netUpdate = true;
                }
                else if (Main.netMode == 0)
                {
                    projectile.ai[1] = Main.rand.NextFloat(2 * (float)Math.PI);
                }
                wanderTimer = 0;

            }
            if (projectile.wet)
            {

                if (QwertyMethods.ClosestNPC(ref prey, 10000, projectile.Center))
                {
                    swimDirection = (projectile.Center - prey.Center).ToRotation() - (float)Math.PI;
                }
                else
                {
                    swimDirection = projectile.ai[1] - (float)Math.PI;
                }



                actDirection = QwertyMethods.SlowRotation(actDirection, swimDirection, 4);
                projectile.velocity.X = (float)Math.Cos(actDirection) * swimSpeed;
                projectile.velocity.Y = (float)Math.Sin(actDirection) * swimSpeed;
                projectile.rotation = actDirection + (float)Math.PI / 2;
                actDirection = projectile.velocity.ToRotation();

            }
            else
            {
                actDirection = projectile.velocity.ToRotation();
            }








        }

        public override void Kill(int timeLeft)
        {



        }


    }

}

