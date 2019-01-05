using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.DevItems.Phantom 
{
	public class GodsSmite : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("God's smite");
			Tooltip.SetDefault("I don't understand Havoc's obsession with giant god weapons" + "\nDev Item");


        }
		public override void SetDefaults()
		{
			item.damage = 800;
			item.melee = true;
			item.noMelee = true;
			
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.knockBack = 0;
			item.value = 0;
			item.rare = 10;
			item.UseSound = SoundID.Item1;
			item.noUseGraphic = true;
			item.width = 180;
			item.height = 166;
			item.crit = 5;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("GodsSmiteP");
			item.shootSpeed =10;
            //item.channel = true;
			
			
			
			
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

	public class GodsSmiteP : ModProjectile
	{
		public override void SetDefaults()
		{
            //projectile.aiStyle = ProjectileID.WoodenBoomerang;
            //aiType = 52;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.width = 180;
			projectile.height = 166;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.light = 100f;
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("God's Smite");

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
            
            
            if (timer>=120)
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
            CreateDust();


        }
        public virtual void CreateDust()
        {

            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HolyGlow"));



        }
        
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            timer = 120;

        }
        
        public override bool OnTileCollide(Vector2 velocityChange)
        {
            return false;
        }
    }

	
}

