using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
	public class GlassBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glass Bullet");
			Tooltip.SetDefault("Shatters upon firing, releasing 9 short-ranged shards dealing 40% damage each");
		}

		public override void SetDefaults()
		{
			item.damage = 7;
			item.ranged = true;
			item.knockBack = 2;
			item.value = 1;
			item.rare = 0;
			item.width = 16;
			item.height = 18;

			item.shootSpeed = 8;

			item.consumable = true;
			item.shoot = mod.ProjectileType("GlassBulletP");
			item.ammo = 97;
			item.maxStack = 999;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Glass, 1);
			recipe.AddTile(TileID.GlassKiln);
			recipe.SetResult(this, 40);
			recipe.AddRecipe();
		}
	}

	public class GlassBulletP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glass Bullet");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.ranged = true;

			projectile.timeLeft = 1;
		}

		public override void Kill(int timeLeft)
		{
			if (projectile.owner == Main.myPlayer)
			{
				float V = 30;
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation + MathHelper.ToRadians(-90)) * V, (float)Math.Sin(projectile.rotation + MathHelper.ToRadians(-90)) * V, mod.ProjectileType("GlassBulletShard"), (int)(projectile.damage * .4f), projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation + MathHelper.ToRadians(-82)) * V, (float)Math.Sin(projectile.rotation + MathHelper.ToRadians(-82)) * V, mod.ProjectileType("GlassBulletShard"), (int)(projectile.damage * .4f), projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation + MathHelper.ToRadians(-74)) * V, (float)Math.Sin(projectile.rotation + MathHelper.ToRadians(-74)) * V, mod.ProjectileType("GlassBulletShard"), (int)(projectile.damage * .4f), projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation + MathHelper.ToRadians(-66)) * V, (float)Math.Sin(projectile.rotation + MathHelper.ToRadians(-66)) * V, mod.ProjectileType("GlassBulletShard"), (int)(projectile.damage * .4f), projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation + MathHelper.ToRadians(-58)) * V, (float)Math.Sin(projectile.rotation + MathHelper.ToRadians(-58)) * V, mod.ProjectileType("GlassBulletShard"), (int)(projectile.damage * .4f), projectile.knockBack, Main.myPlayer);

				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation + MathHelper.ToRadians(-98)) * V, (float)Math.Sin(projectile.rotation + MathHelper.ToRadians(-98)) * V, mod.ProjectileType("GlassBulletShard"), (int)(projectile.damage * .4f), projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation + MathHelper.ToRadians(-106)) * V, (float)Math.Sin(projectile.rotation + MathHelper.ToRadians(-106)) * V, mod.ProjectileType("GlassBulletShard"), (int)(projectile.damage * .4f), projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation + MathHelper.ToRadians(-114)) * V, (float)Math.Sin(projectile.rotation + MathHelper.ToRadians(-114)) * V, mod.ProjectileType("GlassBulletShard"), (int)(projectile.damage * .4f), projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation + MathHelper.ToRadians(-122)) * V, (float)Math.Sin(projectile.rotation + MathHelper.ToRadians(-122)) * V, mod.ProjectileType("GlassBulletShard"), (int)(projectile.damage * .4f), projectile.knockBack, Main.myPlayer);
			}
		}
	}

	public class GlassBulletShard : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glass Bullet Shard");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.ranged = true;
			projectile.timeLeft = 10;
		}
	}
}