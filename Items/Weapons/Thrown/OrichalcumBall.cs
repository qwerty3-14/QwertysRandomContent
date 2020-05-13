using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Thrown
{
	public class OrichalcumBall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orichalcum Ball");
			Tooltip.SetDefault("Behold! The terrifying power of bouncy balls!!");
		}

		public override void SetDefaults()
		{
			item.damage = 45;
			item.crit = 20;
			item.thrown = true;
			item.knockBack = 0;
			item.value = 60;
			item.rare = 3;
			item.width = 8;
			item.height = 8;
			item.useStyle = 1;
			item.shootSpeed = 12f;
			item.useTime = 21;
			item.useAnimation = 21;
			item.consumable = true;
			item.shoot = mod.ProjectileType("OrichalcumBallP");
			item.noUseGraphic = true;
			item.noMelee = true;
			item.maxStack = 999;
			item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.OrichalcumBar, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 333);
			recipe.AddRecipe();
		}
	}

	public class OrichalcumBallP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orichalcum Ball");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 1;

			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.thrown = true;

			projectile.tileCollide = true;
		}

		public int bounceCount;
		public int spinDirection = 1;

		public override void AI()
		{
			projectile.rotation += (float)Math.PI / 30 * spinDirection;
			if (bounceCount >= 10)
			{
				projectile.Kill();
			}
		}

		public override bool OnTileCollide(Vector2 velocityChange)
		{
			bounceCount++;
			spinDirection *= -1;
			if (projectile.velocity.X != velocityChange.X)
			{
				projectile.velocity.X = -velocityChange.X;
			}
			if (projectile.velocity.Y != velocityChange.Y)
			{
				projectile.velocity.Y = -.9f * velocityChange.Y;
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			bounceCount++;

			projectile.velocity.X = -projectile.velocity.X;
			projectile.velocity.Y = -projectile.velocity.Y;
			spinDirection *= -1;
		}
	}
}