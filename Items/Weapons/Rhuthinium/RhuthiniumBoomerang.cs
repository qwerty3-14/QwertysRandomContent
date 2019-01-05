using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.Weapons.Rhuthinium 
{
	public class RhuthiniumBoomerang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Boomerang");
			Tooltip.SetDefault("Hold down the throw button to make it fly further" + "\nRight click to make the boomerang stationary");


        }
		public override void SetDefaults()
		{
			item.damage = 30;
			item.melee = true;
			item.noMelee = true;
			
			item.useTime = 14;
			item.useAnimation = 14;
			item.useStyle = 5;
			item.knockBack = 0;
			item.value = 25000;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.noUseGraphic = true;
			item.width = 28;
			item.height = 32;
			item.crit = 5;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("RhuthiniumBoomerangP");
			item.shootSpeed =10;
            item.channel = true;
			
			
			
			
		}
		

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 8);
			recipe.AddTile(TileID.Anvils);
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

	public class RhuthiniumBoomerangP : ModProjectile
	{
		public override void SetDefaults()
		{
            //projectile.aiStyle = ProjectileID.WoodenBoomerang;
            //aiType = 52;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.width = 28;
			projectile.height = 32;
            projectile.melee = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("RhuthiniumBoomerang");

		}
        public int timer;
        public bool runOnce =true;
        public int spinDirection;
        public Vector2 origonalVelocity;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if(runOnce)
            {
                spinDirection = player.direction;
                origonalVelocity = projectile.velocity;
                runOnce = false;
                
            }
            projectile.rotation += MathHelper.ToRadians(20 * spinDirection);
            timer++;
            if (Main.mouseRight)
            {
                projectile.velocity.X = 0;
                projectile.velocity.Y = 0;
            }
            
            else if (player.channel)
            {

                projectile.velocity = origonalVelocity;

            }
            else
            {
                projectile.tileCollide = false;
                float speed = 10;
                float direction = (player.Center - projectile.Center).ToRotation();
                projectile.velocity.X = speed * (float)Math.Cos(direction);
                projectile.velocity.Y = speed * (float)Math.Sin(direction);
                float distance = (float)Math.Sqrt((player.Center.X - projectile.Center.X) * (player.Center.X - projectile.Center.X) + (player.Center.Y - projectile.Center.Y) * (player.Center.Y - projectile.Center.Y));
                if (distance < 10)
                {
                    projectile.Kill();
                }

            }
            


        }
        /*
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            timer = 40;

        }
        */
        public override bool OnTileCollide(Vector2 velocityChange)
        {
            return false;
        }
    }

	
}

