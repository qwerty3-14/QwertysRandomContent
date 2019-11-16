using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Items.B4Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Vargule
{
	public class VarguleHelixShot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Helix Bullet");
			Tooltip.SetDefault("Shoots two bullets in a helix pattern each doing 80% damage");
			
		}
		public override void SetDefaults()
		{
			item.damage = 10;
			item.ranged = true;
			item.knockBack = 2;
			item.value = 5;
            item.rare = 8;
            item.width = 20;
			item.height = 26;
			
			item.shootSpeed =3;
			
			item.consumable = true;
			item.shoot = mod.ProjectileType("VarguleHelixShotP");
			item.ammo = 97;
			item.maxStack = 999;
			
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VarguleBar"), 1);
			
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
	public class VarguleHelixShotP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Helix Shot");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.ranged= true;
			projectile.arrow=false;
			projectile.timeLeft=1;
			
			
		}
		public override void AI()
		{
			CreateDust();
		}
		public virtual void CreateDust()
		{
			
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("VarguleDust"));
				
				
			
		}
		public override void Kill( int timeLeft)
		{
		if (projectile.owner == Main.myPlayer)
			{
				
				float V =8;
				Projectile b = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation+MathHelper.ToRadians(-90))*V, (float)Math.Sin(projectile.rotation+MathHelper.ToRadians(-90))*V, mod.ProjectileType("Helix1"), (int)(projectile.damage * .8f), projectile.knockBack, Main.myPlayer)];
                Projectile b2 = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation+MathHelper.ToRadians(-90))*V, (float)Math.Sin(projectile.rotation+MathHelper.ToRadians(-90))*V, mod.ProjectileType("Helix2"), (int)(projectile.damage * .8f), projectile.knockBack, Main.myPlayer)];
                if (projectile.GetGlobalProjectile<arrowgigantism>().GiganticArrow)
                {
                    b.scale *= 3;
                    b2.scale *= 3;
                    b.GetGlobalProjectile<arrowgigantism>().GiganticArrow = true;
                    b2.GetGlobalProjectile<arrowgigantism>().GiganticArrow = true;
                    projectile.GetGlobalProjectile<arrowgigantism>().GiganticArrow = false;

                }

            }
		}
		
		

		
	}
	public class Helix1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helix");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet; 
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.ranged= true;
			projectile.arrow=false;
			projectile.timeLeft=300;
			
			
		}
		public bool runOnce=true;
		
		public float period; // changes the period of the cosine funstion, makes the projectiles twist more
		public float wave; //the cosine function that dictates the projectiles velocity perpendicular to it's main velocity
		public float Orir;//origional rotation used for deciding the projectile's general direction
		public float amplitude=16; //amplitude of the cosine function, makes the projectiles twist higher/lower
		public float speed = 14; //increases the projectile's speed toward it's general direction
		public override void AI()
		{
			
			if(runOnce)
			{
				
				Orir =projectile.rotation+MathHelper.ToRadians(-90);
				runOnce=false;
			}
			
			CreateDust();
			period+=18;
			wave=(float)Math.Cos(MathHelper.ToRadians(period))*amplitude;
			projectile.velocity.X = (float)Math.Cos(Orir)*speed+((float)Math.Cos(Orir+MathHelper.ToRadians(90))*wave);
			projectile.velocity.Y = (float)Math.Sin(Orir)*speed+((float)Math.Sin(Orir+MathHelper.ToRadians(90))*wave);
			
			
			
		}
		public virtual void CreateDust()
		{
			
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Helix1Dust"));
				
				
			
		}
		
		
		

		
	}
	public class Helix2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helix");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet; 
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.ranged= true;
			projectile.arrow=false;
			projectile.timeLeft=300;
			
			
		}
		public bool runOnce=true;
		
		public float period;
		public float wave;
		public float Orir;
		public float amplitude=16;
		public float speed = 14;
		public override void AI()
		{
			
			if(runOnce)
			{
				
				Orir =projectile.rotation+MathHelper.ToRadians(-90);
				runOnce=false;
			}
			
			CreateDust();
			period+=18;
			wave=(float)Math.Cos(MathHelper.ToRadians(period))*amplitude;
			projectile.velocity.X = (float)Math.Cos(Orir)*speed+((float)Math.Cos(Orir+MathHelper.ToRadians(-90))*wave);
			projectile.velocity.Y = (float)Math.Sin(Orir)*speed+((float)Math.Sin(Orir+MathHelper.ToRadians(-90))*wave);
			
			
			
		}
		public virtual void CreateDust()
		{
			
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Helix2Dust"));
				
				
			
		}
		
		
		

		
	}
	
}

