using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
	public class TitaniumBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanium Bullet");
			Tooltip.SetDefault("Takes its time to build up speed but hits hard");
			
		}
		public override void SetDefaults()
		{
			item.damage = 30;
			item.ranged = true;
			item.knockBack = 2;
			item.value = 1;
			item.rare = 3;
			item.width = 18;
			item.height = 18;
			
			item.shootSpeed =1;
			
			item.consumable = true;
			item.shoot = mod.ProjectileType("TitaniumBulletP");
			item.ammo = 97;
			item.maxStack = 999;
			
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TitaniumBar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
		
	}
	public class TitaniumBulletP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanium Bullet");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 0;
			aiType = ProjectileID.Bullet; 
			projectile.width = 18;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.ranged= true;
			
			
			
			
		}
		public bool runOnce = true;
		public float targetRotation;
		public float speed =.1f;
		public override void AI()
		{
			if(runOnce)
			{
				targetRotation = (projectile.velocity).ToRotation();
				projectile.velocity.X = 0;
				projectile.velocity.Y = 0;
				runOnce=false;
			}

			speed+=.06f*speed;
			projectile.velocity.X=(float)Math.Cos(targetRotation)*speed;
			projectile.velocity.Y=(float)Math.Sin(targetRotation)*speed;
			projectile.rotation += MathHelper.ToRadians(speed*10);
		}
		
		
		
		

		
	}
	
	
	
}

