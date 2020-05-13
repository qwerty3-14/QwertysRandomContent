using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items
{
	public class TheBeetle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Beetle");
			Tooltip.SetDefault("A beetle-shaped device with Emerald plating and Gold frames, with a ridiculously sharp blade on the mandibles, capable of breaking pots and cutting vines.");
		}

		public override void SetDefaults()
		{
			item.width = 80;
			item.height = 84;
			item.rare = 1;
			item.value = Item.sellPrice(gold: 1);
			item.shoot = mod.ProjectileType("TheBeetleP");
			item.channel = true;
			item.useAnimation = item.useTime = 6;
			item.autoReuse = true;
			item.useStyle = 5;
			item.shootSpeed = 2f;
			item.noMelee = true;
			item.noUseGraphic = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[mod.ProjectileType("TheBeetleP")] <= 0;
		}
	}

	public class TheBeetleP : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = projectile.height = 80;
			projectile.penetrate = -1;
			projectile.timeLeft = 30 * 60;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (player.channel)
			{
				FlyTo(QwertysRandomContent.LocalCursor[projectile.owner]);
			}
			else
			{
				acceleration = maxSpeed;
				projectile.tileCollide = false;
				FlyTo(player.Center);
				if (Collision.CheckAABBvAABBCollision(player.position, player.Size, projectile.position, projectile.Size))
				{
					projectile.Kill();
				}
			}
			projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;
		}

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 unit = projectile.velocity;
			Utils.PlotTileLine(projectile.Top, projectile.Bottom, projectile.width, DelegateMethods.CutTiles);
		}

		private float acceleration = .4f;
		private float maxSpeed = 8f;

		private void FlyTo(Vector2 target)
		{
			projectile.velocity += QwertyMethods.PolarVector(acceleration, (target - projectile.Center).ToRotation());
			if (projectile.velocity.Length() > maxSpeed)
			{
				projectile.velocity = projectile.velocity.SafeNormalize(Vector2.UnitY) * maxSpeed;
			}
		}
	}
}