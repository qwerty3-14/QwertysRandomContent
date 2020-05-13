using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSpells
{
	public class Steambath : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Steambath");
			Tooltip.SetDefault("Ignores defense");
		}

		public override void SetDefaults()
		{
			item.damage = 4;
			item.magic = true;

			item.useTime = 4;
			item.useAnimation = 4;
			item.useStyle = 5;
			item.knockBack = 0;
			item.value = 80000;
			item.rare = 5;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.width = 28;
			item.height = 30;

			item.mana = 3;
			item.shoot = mod.ProjectileType("Steam");
			item.shootSpeed = 25;
			item.noMelee = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int shots = Main.rand.Next(2, 5);

			for (int s = 0; s < shots; s++)
			{
				Vector2 trueSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
				trueSpeed.X *= Main.rand.NextFloat(.4f, 1.2f);
				trueSpeed.Y *= .3f;
				Projectile.NewProjectile(position.X, position.Y, trueSpeed.X, trueSpeed.Y, type, damage, knockBack, player.whoAmI);
			}

			return false;
		}
	}

	public class Steam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Steam");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			//projectile.aiStyle = 1;
			//aiType = ProjectileID.Bullet;
			projectile.width = 30;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.magic = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 60 * 15;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 30;
			projectile.GetGlobalProjectile<QwertyGlobalProjectile>().ignoresArmor = true;
		}

		public int dustTimer;
		private int frameTimer;

		public override void AI()
		{
			frameTimer++;
			if (frameTimer > 30)
			{
				projectile.frame++;
				if (projectile.frame >= 5)
				{
					projectile.frame = 0;
				}
			}
			projectile.velocity.X *= .95f;
			if (projectile.velocity.Y > -2)
			{
				projectile.velocity.Y -= .1f;
			}
			else
			{
				projectile.velocity.Y += .1f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
			//damage += target.defense / 2;
		}
	}
}