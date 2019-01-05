using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Lune
{
	public class LuneTop : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lune Top");
			Tooltip.SetDefault("Inflicts Lune curse making enemies more vulnerable to critical hits");
			
		}
		public override void SetDefaults()
		{
			item.damage = 28;
			item.thrown = true;
			item.knockBack = 5;
			item.value = 5;
			item.rare = 1;
			item.width = 30;
			item.height = 38;
            item.useStyle = 1;
			item.shootSpeed =4.5f;
            item.useTime = 32;
            item.useAnimation = 32;
			item.consumable = true;
			item.shoot = mod.ProjectileType("LuneTopP");
            item.noUseGraphic = true;
            item.noMelee = true;
			item.maxStack = 999;
            item.autoReuse = true;
			
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("LuneBar"), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
	public class LuneTopP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lune Top");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
            
			projectile.width = 30;
			projectile.height = 38;
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
        public float friction = .004f ;
        public float enemyFriction = .08f;
        public override void AI()
        {
            if(runOnce)
            {
                initVel = (float)Math.Abs(projectile.velocity.Length());
                friction = friction * (initVel - 2);
                runOnce = false;
            }
            
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
            target.AddBuff(mod.BuffType("LuneCurse"), 120);
        }







    }

        }

