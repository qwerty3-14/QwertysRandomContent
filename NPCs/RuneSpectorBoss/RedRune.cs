using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.RuneSpectorBoss
{
	internal class RedRune : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;

			projectile.width = 200;
			projectile.height = 200;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.tileCollide = false;
			projectile.light = 1f;
		}

		public bool runOnce = true;
		public Projectile aggroRune;
		public int runeCounter;

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.velocity = new Vector2(0, 0);
			if (projectile.alpha > 0)
				projectile.alpha -= 2;
			else
				projectile.alpha = 0;
			runeCounter++;
			if (runeCounter >= 128)
			{
				if (runOnce)
				{
					if (Main.netMode != 2)
						aggroRune = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("AggroRune"), projectile.damage, 3f, Main.myPlayer)];
					runOnce = false;
				}
				else if (!aggroRune.active)
				{
					projectile.Kill();
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int d = 0; d <= 100; d++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("AggroRuneLash"));
			}
		}
	}

	internal class AggroRune : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;

			projectile.width = 62;
			projectile.height = 62;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.tileCollide = false;
			projectile.light = 1f;
		}

		public bool runOnce = true;
		public int time;

		public override void AI()
		{
			if (runOnce)
			{
				if (Main.netMode == 0)
				{
					time = Main.rand.Next(300, 481);
				}
				else
				{
					time = Main.rand.Next(300, 481);
				}

				projectile.timeLeft = (int)time;
				runOnce = false;
			}
			Player player = Main.player[projectile.owner];

			if (projectile.alpha > 0)
				projectile.alpha--;
			else
				projectile.alpha = 0;

			if (projectile.timeLeft <= 2)
			{
				projectile.alpha = 255;
				Rectangle myRect = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);

				Rectangle value = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
				if (myRect.Intersects(value))
				{
					if (player.position.X + (float)(player.width / 2) < projectile.position.X + (float)(projectile.width / 2))
					{
						projectile.direction = -1;
					}
					else
					{
						projectile.direction = 1;
					}
					int num4 = Main.DamageVar((float)projectile.damage);
					projectile.StatusPlayer(Main.myPlayer);
					if (Main.expertMode)
					{
						player.Hurt(PlayerDeathReason.ByProjectile(mod.NPCType("RuneSpector"), projectile.whoAmI), (int)(num4 * 1.4f), projectile.direction, true, false, false, -1);
					}
					else
					{
						player.Hurt(PlayerDeathReason.ByProjectile(mod.NPCType("RuneSpector"), projectile.whoAmI), num4, projectile.direction, true, false, false, -1);
					}
				}
				for (int d = 0; d <= 100; d++)
				{
					Dust.NewDust(projectile.position, myRect.Width, myRect.Height, mod.DustType("AggroRuneLash"));
				}
			}
			if (projectile.timeLeft <= 70)
			{
				projectile.velocity = new Vector2(0, 0);
			}
			else
			{
				projectile.position.X = player.Center.X - 31;
				projectile.position.Y = player.Center.Y - 31;
				projectile.rotation += MathHelper.ToRadians(3);
			}
		}
	}
}