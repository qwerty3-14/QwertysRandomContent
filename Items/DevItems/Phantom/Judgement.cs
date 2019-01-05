using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using QwertysRandomContent;
using System.Collections.Generic;

namespace QwertysRandomContent.Items.DevItems.Phantom
{
	public class Judgement : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Judgement");
			Tooltip.SetDefault("Phantom454545 really wanted me to add this");
			
		}
		public override void SetDefaults()
		{
			item.damage = 9999;
			item.magic = true;
			
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 5;
			item.knockBack = 100;
			item.value = 250000000;
			item.rare = 9;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.width = 120;
			item.height = 138;
			
			item.mana =12;
			item.shoot = mod.ProjectileType("Cross");
			item.shootSpeed =9;
			item.noMelee=true;
			
			
			
			
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            
            for (int l = 0; l < Main.projectile.Length; l++)
            {                                                                  //this make so you can only spawn one of this projectile at the time,
                Projectile proj = Main.projectile[l];
                if (proj.active && proj.type == item.shoot && proj.owner == player.whoAmI)
                {
                    proj.active = false;
                }
            }
            return true;
        }

		
	}
		public class Cross : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cross");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet; 
			projectile.width = 84;
			projectile.height = 136;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.magic= true;
			projectile.tileCollide= false;
			
			
			
			
		}
		public int varTime;
		public int Yvar =0;
		public int YvarOld =0;
		public int Xvar =0;
		public int XvarOld =0;
		public int f=1;
		public float targetAngle =90;
		public float s =1;
		public float tarX;
		public float tarY;
		public int start;
        public override void AI()
        {
			Player player = Main.player[projectile.owner];
           if(start <60)
		   {
			   projectile.velocity.X=0;
			   projectile.velocity.Y=-10;
			   start++;
			   projectile.rotation=MathHelper.ToRadians(90);
			   
		   }
		   else
		   {
			   projectile.position.X=player.Center.X-(projectile.width/2);
			   projectile.position.Y=player.Center.Y-600-(projectile.height/2);
           
			projectile.rotation = (Main.MouseWorld - projectile.Center).ToRotation();
			tarX=(float)Math.Cos(projectile.rotation)*10;
			tarY=(float)Math.Sin(projectile.rotation)*10;             
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, tarX, tarY, mod.ProjectileType("HolyBeam"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f); 
			
		   }
 
        }

		
	}
		public class HolyBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Beam");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			
			projectile.width = 18;
			projectile.height = 120;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.magic= true;
			projectile.tileCollide= false;
			projectile.timeLeft=40;
			
			
		}
		public float V =0;
		public Projectile Cross;
		public override void AI()
        {
			foreach(Projectile projSearch in Main.projectile)
			{	
				if(projSearch.type == mod.ProjectileType("Cross"))
				Cross = projSearch;
			}
			Player player = Main.player[projectile.owner];
			
			
			projectile.rotation = Cross.rotation+MathHelper.ToRadians(90);
			projectile.position.X= Cross.Center.X+ V*(float)Math.Cos(Cross.rotation)-(projectile.width/2);
			projectile.position.Y= Cross.Center.Y+ V*(float)Math.Sin(Cross.rotation)-(projectile.height/2);
			V += 30f;
			CreateDust();

		}
		public virtual void CreateDust()
		{
			
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HolyGlow"));
				
				Main.dust[dust].velocity.X =projectile.velocity.X;
			
		}

		
	}
	
}

