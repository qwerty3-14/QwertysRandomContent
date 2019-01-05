using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.Weapons.Vargule 
{
	public class VarguleBoomerangDuo : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Boomerang Duo");
			Tooltip.SetDefault("Flys toward you cursor returning if it reaches it or hits something" + "\nThe second boomerang will be thrown when the origonal is on its way back" + "\nright click to make the boomerangs orbit you");


        }
		public override void SetDefaults()
		{
			item.damage = 120;
			item.melee = true;
			item.noMelee = true;
			
			item.useTime = 14;
			item.useAnimation = 14;
			item.useStyle = 5;
			item.knockBack = 6f;
			item.value = 250000;
            item.rare = 8;
            item.UseSound = SoundID.Item1;
			item.noUseGraphic = true;
			item.width = 30;
			item.height = 30;
			item.crit = 5;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("VarguleBoomerangGreen");
			item.shootSpeed =15;
            
			
			
			
			
		}
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }


        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VarguleBar"), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool CanUseItem(Player player)
		{
			for (int i = 0; i < 1000; ++i)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
				{
					return false;
				}
			}
			return true;
		}
	}

	public class VarguleBoomerangGreen : ModProjectile
	{
		public override void SetDefaults()
		{
            //projectile.aiStyle = ProjectileID.WoodenBoomerang;
            //aiType = 52;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.width = 14;
			projectile.height = 30;
            projectile.melee = true;
            //projectile.extraUpdates = 2;
            projectile.timeLeft = 240;
            projectile.usesLocalNPCImmunity=true;
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Boomerang");

		}
        public int timer;
        public bool runOnce =true;
        public int spinDirection;
        public float origonalVelocity;
        public float direction;
        public bool returnToPlayer = false;
        public float speed;
        public bool orbitting;
        public bool orbitEnd;
        public float radius;
        public bool createCompanion = true;
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            
            if (runOnce)
            {
                spinDirection = player.direction;
                origonalVelocity = (float)Math.Sqrt((projectile.velocity.X * projectile.velocity.X) + (projectile.velocity.Y * projectile.velocity.Y));
                radius = origonalVelocity * 3;
                if (Main.mouseRight)
                {
                    orbitting = true;
                    projectile.position.X = player.Center.X ;
                    projectile.position.Y = player.Center.Y - radius - (projectile.height / 2);
                }
                runOnce = false;


            }
            projectile.rotation += MathHelper.ToRadians(2 * spinDirection* origonalVelocity);
            timer++;
            if (orbitting)
            {
                if (createCompanion)
                {
                    //Main.PlaySound(SoundID.Item1, player.position, 0);
                    //player.useStyle = 5;
                    Projectile.NewProjectile(player.Center.X, player.Center.Y, origonalVelocity, 0, mod.ProjectileType("VarguleBoomerangPurple"), (int)(projectile.damage), projectile.knockBack, Main.myPlayer);
                    createCompanion = false;
                }
                projectile.tileCollide = false;
                speed = origonalVelocity/2;
                if (orbitEnd)
                {
                    

                    direction = (player.Center - projectile.Center).ToRotation();

                    float distance = (float)Math.Sqrt((player.Center.X - projectile.Center.X) * (player.Center.X - projectile.Center.X) + (player.Center.Y - projectile.Center.Y) * (player.Center.Y - projectile.Center.Y));
                    if (distance < 10 * (speed/15))
                    {
                        projectile.Kill();
                    }

                }
                else
                {
                    direction += (float)((2 * Math.PI) / (Math.PI * 2*radius / speed));
                }
                
                if (!Main.mouseRight)
                {
                    orbitEnd = true;
                }
                projectile.velocity.X = speed * (float)Math.Cos(direction) + player.velocity.X;
                projectile.velocity.Y = speed * (float)Math.Sin(direction) + player.velocity.Y;
            }
            else
            {
                speed = origonalVelocity;
                if (returnToPlayer)
                {
                    if (createCompanion)
                    {
                        //Main.PlaySound(SoundID.Item1, player.position, 0);
                        //player.useStyle = 5;
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, origonalVelocity, 0, mod.ProjectileType("VarguleBoomerangPurple"), (int)(projectile.damage), projectile.knockBack, Main.myPlayer);
                        createCompanion = false;
                    }
                    projectile.tileCollide = false;

                    direction = (player.Center - projectile.Center).ToRotation();

                    float distance = (float)Math.Sqrt((player.Center.X - projectile.Center.X) * (player.Center.X - projectile.Center.X) + (player.Center.Y - projectile.Center.Y) * (player.Center.Y - projectile.Center.Y));
                    if (distance < 10 * (speed / 15))
                    {
                        projectile.Kill();
                    }

                }
                else
                {
                    direction = (Main.MouseWorld - projectile.Center).ToRotation();
                }
               
                float distanceC = (float)Math.Sqrt((Main.MouseWorld.X - projectile.Center.X) * (Main.MouseWorld.X - projectile.Center.X) + (Main.MouseWorld.Y - projectile.Center.Y) * (Main.MouseWorld.Y - projectile.Center.Y));
                if (distanceC < 10 * (speed / 15))
                {
                    returnToPlayer = true;
                }
                projectile.velocity.X = speed * (float)Math.Cos(direction);
                projectile.velocity.Y = speed * (float)Math.Sin(direction);
            }

            CreateDust();


        }
        public virtual void CreateDust()
        {

            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Helix2Dust"));



        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = 10;
            target.immune[projectile.owner] = 0;
            returnToPlayer = true;

        }
        
        public override bool OnTileCollide(Vector2 velocityChange)
        {
            returnToPlayer = true;
            return false;
        }
    }


    public class VarguleBoomerangPurple : ModProjectile
    {
        public override void SetDefaults()
        {
            //projectile.aiStyle = ProjectileID.WoodenBoomerang;
            //aiType = 52;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.width = 14;
            projectile.height = 30;
            projectile.melee = true;
            //projectile.extraUpdates = 2;
            projectile.timeLeft = 240;
            projectile.usesLocalNPCImmunity = true;

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vargule Boomerang");

        }
        public int timer;
        public bool runOnce = true;
        public int spinDirection;
        public float origonalVelocity;
        public float direction = (float)Math.PI;
        public bool returnToPlayer = false;
        public float speed;
        public bool orbitting;
        public bool orbitEnd;
        public float radius;
        public Projectile companion;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            foreach (Projectile projSearch in Main.projectile)
            {
                if (projSearch.type == mod.ProjectileType("VarguleBoomerangGreen"))
                    companion = projSearch;
            }
            if (runOnce)
            {
                spinDirection = player.direction;
                origonalVelocity = (float)Math.Sqrt((projectile.velocity.X * projectile.velocity.X) + (projectile.velocity.Y * projectile.velocity.Y));
                radius = origonalVelocity * 3;
                if (Main.mouseRight)
                {
                    orbitting = true;
                    projectile.position.X = player.Center.X ;
                    projectile.position.Y = player.Center.Y + radius - (projectile.height/2);
                }
                projectile.velocity.X = 0;
                projectile.velocity.Y = 0;
                runOnce = false;


            }
            projectile.rotation += MathHelper.ToRadians(2 * spinDirection * origonalVelocity);
            timer++;
            if (orbitting)
            {
                if(!companion.active)
                {
                    projectile.Kill();
                }
                projectile.tileCollide = false;
                speed = origonalVelocity / 2;
                if (orbitEnd)
                {


                    direction = (player.Center - projectile.Center).ToRotation();

                    float distance = (float)Math.Sqrt((player.Center.X - projectile.Center.X) * (player.Center.X - projectile.Center.X) + (player.Center.Y - projectile.Center.Y) * (player.Center.Y - projectile.Center.Y));
                    if (distance < 10 * (speed / 15))
                    {
                        projectile.Kill();
                    }

                }
                else
                {
                    direction += (float)((2 * Math.PI) / (Math.PI * 2 * radius / speed));
                }

                if (!Main.mouseRight)
                {
                    orbitEnd = true;
                }
                projectile.velocity.X = speed * (float)Math.Cos(direction) + player.velocity.X;
                projectile.velocity.Y = speed * (float)Math.Sin(direction) + player.velocity.Y;
            }
            else
            {
                
                speed = origonalVelocity;
                if (returnToPlayer)
                {
                    projectile.tileCollide = false;

                    direction = (player.Center - projectile.Center).ToRotation();

                    float distance = (float)Math.Sqrt((player.Center.X - projectile.Center.X) * (player.Center.X - projectile.Center.X) + (player.Center.Y - projectile.Center.Y) * (player.Center.Y - projectile.Center.Y));
                    if (distance < 10 * (speed / 15))
                    {
                        projectile.Kill();
                    }

                }
                else
                {
                    direction = (Main.MouseWorld - projectile.Center).ToRotation();
                }

                float distanceC = (float)Math.Sqrt((Main.MouseWorld.X - projectile.Center.X) * (Main.MouseWorld.X - projectile.Center.X) + (Main.MouseWorld.Y - projectile.Center.Y) * (Main.MouseWorld.Y - projectile.Center.Y));
                if (distanceC < 10 * (speed / 15))
                {
                    returnToPlayer = true;
                }
                projectile.velocity.X = speed * (float)Math.Cos(direction);
                projectile.velocity.Y = speed * (float)Math.Sin(direction);
            }

            CreateDust();


        }
        public virtual void CreateDust()
        {

            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Helix1Dust"));



        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            returnToPlayer = true;
            projectile.localNPCImmunity[target.whoAmI] = 10;
            target.immune[projectile.owner] = 0;

        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            returnToPlayer = true;
            return false;
        }
    }


}

