using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Vargule
{
	public class VarguleTwinShooter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Mega Shot");
			Tooltip.SetDefault("Shoots a MASSIVE bullet");
			
		}
		public override void SetDefaults()
		{
			item.damage = 30;
			item.ranged = true;
			
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 250000;
            item.rare = 8;
            item.UseSound = SoundID.Item11;
			
			item.width = 50;
			item.height = 32;
			
			item.shoot = 97;
			item.useAmmo = 97;
			item.shootSpeed =36;
			item.noMelee=true;
			item.autoReuse = true;
			
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VarguleBar"), 12);
			
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 5);
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			
			
			
			for (int s =-5; s<=5; s++)
            {
                Vector2 normalizedSpeed = new Vector2(speedX, speedY).SafeNormalize(-Vector2.UnitY);
                float direction = new Vector2(speedX, speedY).ToRotation();
                Vector2 shiftedPosition = new Vector2(position.X + (float)Math.Cos(direction + Math.PI/2) * s *2, position.Y + (float)Math.Sin(direction+ Math.PI/2) *s *2);
                Projectile.NewProjectile(shiftedPosition.X, shiftedPosition.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                //Main.NewText(s*2);
            }
            
            ////
            /*
			Vector2 muzzleOffset = new Vector2(speedX, speedY).SafeNormalize(-Vector2.UnitY);
			position += new Vector2(muzzleOffset.Y * player.direction, muzzleOffset.X * -player.direction) * 5.5f; // change to change the height
			muzzleOffset *= 0f; // change to change the offset from the player
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
				
					float rotation = new Vector2(speedX, speedY).ToRotation();
					Projectile.NewProjectile(position.X+(float)Math.Cos(rotation)*0, position.Y+(float)Math.Sin(rotation)*0, speedX, speedY, type, damage, knockBack, player.whoAmI);
					
					Projectile.NewProjectile(position.X, position.Y+2f, speedX, speedY, type, damage, knockBack, player.whoAmI);
					
					Projectile.NewProjectile(position.X, position.Y+4f, speedX, speedY, type, damage, knockBack, player.whoAmI);
					
					Projectile.NewProjectile(position.X, position.Y+6f, speedX, speedY, type, damage, knockBack, player.whoAmI);
					
					Projectile.NewProjectile(position.X, position.Y+8f, speedX, speedY, type, damage, knockBack, player.whoAmI);
					
					Projectile.NewProjectile(position.X, position.Y+10f, speedX, speedY, type, damage, knockBack, player.whoAmI);
					
					Projectile.NewProjectile(position.X, position.Y+12f, speedX, speedY, type, damage, knockBack, player.whoAmI);
					
					Projectile.NewProjectile(position.X, position.Y+14f, speedX, speedY, type, damage, knockBack, player.whoAmI);
					
					Projectile.NewProjectile(position.X, position.Y+16f, speedX, speedY, type, damage, knockBack, player.whoAmI);
					
					Projectile.NewProjectile(position.X, position.Y+18f, speedX, speedY, type, damage, knockBack, player.whoAmI);
					
					Projectile.NewProjectile(position.X, position.Y+20f, speedX, speedY, type, damage, knockBack, player.whoAmI);
					
					
				
			}
			*/




            return false;
		}

		
	}
		
	
}

