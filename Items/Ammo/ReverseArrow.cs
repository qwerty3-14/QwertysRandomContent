using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Items.B4Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
	public class ReverseArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reverse Arrow");
			Tooltip.SetDefault("Flys backwards into your bow!");
			
		}
		public override void SetDefaults()
		{
			item.damage = 7;
			item.ranged = true;
			item.knockBack = 2;
			item.value = 5;
			item.rare = 1;
			item.width = 14;
			item.height = 32;
			
			item.shootSpeed =6;
			
			item.consumable = true;
			item.shoot = mod.ProjectileType("ReverseArrowS");
			item.ammo = 40;
			item.maxStack = 999;
			
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenArrow, 50);
			recipe.AddIngredient(mod.ItemType("ReverseSand"), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}

		
	}
	
	public class ReverseArrowS : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reverse Arrow");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet; 
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.ranged= true;
			projectile.arrow=true;
			projectile.timeLeft=1;
			
			
		}
		public int startDistance = 1000;
		public float startX;
		public float startY;
		public override void AI()
		{
			startX = (float)Math.Cos(projectile.rotation+MathHelper.ToRadians(-90))*startDistance;
			startY = (float)Math.Sin(projectile.rotation+MathHelper.ToRadians(-90))*startDistance;
		}
		public override void Kill( int timeLeft)
		{
		if (projectile.owner == Main.myPlayer)
			{
				
				float V =30;
                Projectile arrow1 = Main.projectile[Projectile.NewProjectile(projectile.Center.X+startX, projectile.Center.Y+startY, -(float)Math.Cos(projectile.rotation+MathHelper.ToRadians(-90))*V, -(float)Math.Sin(projectile.rotation+MathHelper.ToRadians(-90))*V, mod.ProjectileType("ReverseArrowP"), projectile.damage, projectile.knockBack, Main.myPlayer)];
                if (projectile.GetGlobalProjectile<arrowHoming>(mod).B4HomingArrow)
                {
                    arrow1.GetGlobalProjectile<arrowHoming>(mod).B4HomingArrow = true;
                    

                }
                if (projectile.GetGlobalProjectile<arrowgigantism>(mod).GiganticArrow)
                {
                    arrow1.scale *= 3;
                    
                    arrow1.GetGlobalProjectile<arrowgigantism>(mod).GiganticArrow = true;
                    
                    projectile.GetGlobalProjectile<arrowgigantism>(mod).GiganticArrow = false;
                }


            }
		}
		
		

		
	}
	public class ReverseArrowP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reverse Arrow");
			
			
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
			projectile.arrow=true;
			
			
			
		}
		public float travelDistance = 1000;
		public float distance;
		public float startX;
		public float startY;
		public bool runOnce=true;
		public float distanceX;
		public float distanceY;
		
		public override void AI()
		{
			if(runOnce)
			{
			startX = projectile.position.X;
			startY = projectile.position.Y;
			runOnce=false;
			}
			distanceX = projectile.position.X -startX;
			distanceY = projectile.position.Y -startY;
			distance = (float)Math.Sqrt(distanceX*distanceX+distanceY*distanceY);
			
			if(distance>=travelDistance)
			{
				projectile.timeLeft=0;
			}
			
		}
		
		
		
		

		
	}
	
}

