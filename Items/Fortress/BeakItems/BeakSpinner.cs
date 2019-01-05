using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.BeakItems
{
	public class BeakSpinner : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beak Spinner");
			Tooltip.SetDefault("Gains momentum as it hits enemies but wears out faster than other tops when sliding on the ground");
			
		}
		public override void SetDefaults()
		{
			item.damage = 51;
			item.thrown = true;
			item.knockBack = 5;
			item.value = 100;
			item.rare = 4;
			item.width = 26;
			item.height = 40;
            item.useStyle = 1;
			item.shootSpeed =3.5f;
            item.useTime = 23;
            item.useAnimation = 23;
			item.consumable = true;
			item.shoot = mod.ProjectileType("BeakSpinnerP");
            item.noUseGraphic = true;
            item.noMelee = true;
			item.maxStack = 999;
            item.autoReuse = true;
			
			
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 2);
            recipe.AddIngredient(mod.ItemType("FortressHarpyBeak"), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 999);
            recipe.AddRecipe();
        }
    }
	public class BeakSpinnerP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beak Spinner");
            Main.projFrames[projectile.type] = 2;

        }
		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
            
			projectile.width = 26;
			projectile.height = 40;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.thrown= true;
            
			
			projectile.tileCollide = true;
			
			
		}
        public bool runOnce=true;
        public float initVel;
        public bool hitGround;
        public int direction=1;
        public int timeOutTimer;
        public bool timeOutFirstStep;
        public bool timeOutSecondStep;
        public bool timeOutThirdStep;
        public float friction = .009f;
        public float enemyFriction = -.4f;
        public override void AI()
        {
            if(runOnce)
            {
                initVel = (float)Math.Abs(projectile.velocity.Length());
                friction = friction * (initVel - 2);
                runOnce = false;
            }
            projectile.frame = projectile.frame == 0 ? 1:0;
            if (hitGround)
                {
                if (projectile.velocity.X < 0)
                {
                    projectile.velocity.X = -initVel;

                }
                else
                {
                    projectile.velocity.X = initVel;
                }
                
                
                if(initVel < 2)
                {
                    projectile.friendly = false;
                    initVel = .5f;
                    timeOutTimer++;
                    if(timeOutTimer >325)
                    {
                        projectile.Kill();
                    }
                    else if(timeOutTimer > 255)
                    {
                        initVel = 0f;
                        projectile.rotation = (float)MathHelper.ToRadians(-45);
                    }
                    else if (timeOutTimer > 180)
                    {
                        projectile.rotation = (float)MathHelper.ToRadians(210 - timeOutTimer);
                    }
                    else if (timeOutTimer > 120)
                    {
                        projectile.rotation = (float)MathHelper.ToRadians(timeOutTimer-150);
                    }
                    else if (timeOutTimer > 60)
                    {
                        projectile.rotation = (float)MathHelper.ToRadians(90- timeOutTimer);
                    }
                    else if (timeOutTimer>30)
                    {
                        projectile.rotation = (float)MathHelper.ToRadians(timeOutTimer-30);
                    }
                    else
                    {
                        projectile.rotation = 0;
                        projectile.rotation += (float)MathHelper.ToRadians(1);
                    }

                    


                }
                else
                {
                    projectile.rotation = 0;
                    initVel -= friction;
                }
            }
            else
            {
                projectile.rotation = 0;
            }
        }
        public override bool OnTileCollide(Vector2 velocityChange)
        {
            hitGround = true;
            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.velocity.X = -velocityChange.X;
            }
            
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            knockback = 0;
            if (!target.immortal && !target.boss)
            {
                target.velocity.X += -projectile.velocity.X;
            }
            initVel -= enemyFriction;
        }







    }

        }

