using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons
{
	public class SandStorm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Storm");
			Tooltip.SetDefault("Good for drowning your enemies in sand!");
		}

		public override void SetDefaults()
		{
			item.damage = 40;
			item.ranged = true;

			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 500000;
			item.rare = 9;
			item.UseSound = SoundID.Item11;

			item.width = 44;
			item.height = 24;

			item.shoot = 42;
			item.useAmmo = 169;
			item.shootSpeed = 8f;
			item.noMelee = true;
			item.autoReuse = true;
			item.noUseGraphic = false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sandgun, 1);
			recipe.AddIngredient(ItemID.SoulofFright, 20);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 11 + Main.rand.Next(4);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 trueSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(25));
				float scale = Main.rand.NextFloat(.6f, 1.4f);
				trueSpeed *= scale;

				Projectile.NewProjectile(position.X, position.Y, trueSpeed.X, trueSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			Vector2 trueSpeedB = new Vector2(speedX, speedY);

			Projectile.NewProjectile(position.X, position.Y, trueSpeedB.X, trueSpeedB.Y, mod.ProjectileType("BigSandBall"), damage, knockBack, player.whoAmI);

			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, -0);
		}
	}

	public class BigSandBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Big Sand Ball");
		}

		public override void SetDefaults()
		{
			projectile.knockBack = 8f;
			projectile.width = 28;
			projectile.height = 18;

			projectile.friendly = true;
			projectile.extraUpdates = 1;
			projectile.aiStyle = 1;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}

		public override void AI()
		{
			if (Main.rand.Next(2) == 0)
			{
				int num130 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 32, 0f, projectile.velocity.Y / 2f, 0, default(Color), 1f);
				Dust dust12 = Main.dust[num130];
				dust12.velocity.X = dust12.velocity.X * 0.4f;
			}
			projectile.rotation += (float)Math.PI / 8;
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			Projectile.NewProjectile(projectile.Center.X + 4, projectile.Center.Y, 3, 0, mod.ProjectileType("FriendlySand"), projectile.damage, projectile.knockBack, player.whoAmI);
			Projectile.NewProjectile(projectile.Center.X - 4, projectile.Center.Y, -3, 0, mod.ProjectileType("FriendlySand"), projectile.damage, projectile.knockBack, player.whoAmI);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y + 4, 0, 3, mod.ProjectileType("FriendlySand"), projectile.damage, projectile.knockBack, player.whoAmI);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 4, 0, -3, mod.ProjectileType("FriendlySand"), projectile.damage, projectile.knockBack, player.whoAmI);

			Projectile.NewProjectile(projectile.Center.X + 4, projectile.Center.Y, 3 * (float)(Math.Sqrt(2) / 2), 3 * (float)(Math.Sqrt(2) / 2), mod.ProjectileType("FriendlySand"), projectile.damage, projectile.knockBack, player.whoAmI);
			Projectile.NewProjectile(projectile.Center.X - 4, projectile.Center.Y, 3 * (float)(Math.Sqrt(2) / 2), -3 * (float)(Math.Sqrt(2) / 2), mod.ProjectileType("FriendlySand"), projectile.damage, projectile.knockBack, player.whoAmI);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y + 4, -3 * (float)(Math.Sqrt(2) / 2), 3 * (float)(Math.Sqrt(2) / 2), mod.ProjectileType("FriendlySand"), projectile.damage, projectile.knockBack, player.whoAmI);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 4, -3 * (float)(Math.Sqrt(2) / 2), -3 * (float)(Math.Sqrt(2) / 2), mod.ProjectileType("FriendlySand"), projectile.damage, projectile.knockBack, player.whoAmI);
		}
	}

	public class FriendlySand : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Ball");
		}

		public override void SetDefaults()
		{
			projectile.knockBack = 6f;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = 1;

			projectile.friendly = true;

			projectile.penetrate = -1;
		}

		/*
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.CloneDefaults(31);

            return true;
        }
        */

		public override void AI()
		{
			if (Main.rand.Next(2) == 0)
			{
				int num130 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 32, 0f, projectile.velocity.Y / 2f, 0, default(Color), 1f);
				Dust dust12 = Main.dust[num130];
				dust12.velocity.X = dust12.velocity.X * 0.4f;
			}
			projectile.rotation += (float)Math.PI / 8;
			projectile.velocity.Y += .1f;
		}

		public override void Kill(int timeLeft)
		{
			int num835 = -1;
			int num836 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
			int num837 = (int)(projectile.position.Y + (float)(projectile.width / 2)) / 16;

			int num839 = 2;
			if (!Main.tile[num836, num837].active() && 53 >= 0)
			{
				bool flag5 = false;
				if (num837 < Main.maxTilesY - 2 && Main.tile[num836, num837 + 1] != null && Main.tile[num836, num837 + 1].active() && Main.tile[num836, num837 + 1].type == 314)
				{
					flag5 = true;
				}
				if (!flag5)
				{
					WorldGen.PlaceTile(num836, num837, 53, false, true, -1, 0);
				}
				if (!flag5 && Main.tile[num836, num837].active() && (int)Main.tile[num836, num837].type == 53)
				{
					if (Main.tile[num836, num837 + 1].halfBrick() || Main.tile[num836, num837 + 1].slope() != 0)
					{
						WorldGen.SlopeTile(num836, num837 + 1, 0);
						if (Main.netMode == 2)
						{
							NetMessage.SendData(17, -1, -1, null, 14, (float)num836, (float)(num837 + 1), 0f, 0, 0, 0);
						}
					}
					if (Main.netMode != 0)
					{
						NetMessage.SendData(17, -1, -1, null, 1, (float)num836, (float)num837, (float)53, 0, 0, 0);
					}
				}
			}
		}
	}
}