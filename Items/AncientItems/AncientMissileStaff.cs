using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.AncientItems
{
	public class AncientMissileStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Missile Staff");
			Tooltip.SetDefault("Fires explosive Ancient Missiles!");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicAncient ? base.Texture + "_Old" : base.Texture;

		public override void SetDefaults()
		{
			item.damage = 32;
			item.magic = true;

			item.useTime = 21;
			item.useAnimation = 21;
			item.useStyle = 5;
			item.knockBack = 2;
			item.value = 150000;
			item.rare = 3;
			//item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.width = 72;
			item.height = 72;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/AncientMissileStaff_Glow_Old") : mod.GetTexture("Items/AncientItems/AncientMissileStaff_Glow");
			}
			item.mana = 7;
			item.shoot = mod.ProjectileType("AncientMissileP");
			item.shootSpeed = 9;
			item.noMelee = true;
			//item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/AncientItems/AncientWave_Glow");
		}

		public override bool CanUseItem(Player player)
		{
			if (player.statMana > item.mana)
			{
				Main.PlaySound(25, player.position, 0);
			}
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 70f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/AncientMissileStaff_Glow_Old") : mod.GetTexture("Items/AncientItems/AncientMissileStaff_Glow");
			spriteBatch.Draw
			(
				texture,
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}
	}

	public class AncientMissileP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Missile");

			Main.projFrames[projectile.type] = 2;
		}

		public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicAncient ? base.Texture + "_Old" : base.Texture;

		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.width = 20;
			projectile.height = projectile.width;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 240;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.usesLocalNPCImmunity = true;
			projectile.magic = true;
		}

		public int dustTimer;
		private float direction;
		private float missileAcceleration = .5f;
		private float topSpeed = 10f;
		private int timer;

		private NPC target;
		private NPC possibleTarget;
		private bool foundTarget;
		private float maxDistance = 10000f;
		private float distance;

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.frameCounter++;
			if (projectile.frameCounter % 30 == 0)
			{
				projectile.frame++;
				if (projectile.frame >= 2)
				{
					projectile.frame = 0;
				}
			}
			timer++;
			if (timer > 30)
			{
				//Player player = Main.player[projectile.owner];

				for (int k = 0; k < 200; k++)
				{
					possibleTarget = Main.npc[k];
					distance = (possibleTarget.Center - player.Center).Length();
					if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0))
					{
						target = Main.npc[k];
						foundTarget = true;

						maxDistance = (target.Center - player.Center).Length();
					}
				}
				if (foundTarget)
				{
					projectile.velocity += QwertyMethods.PolarVector(missileAcceleration, (target.Center - projectile.Center).ToRotation());
					if (projectile.velocity.Length() > topSpeed)
					{
						projectile.velocity = projectile.velocity.SafeNormalize(-Vector2.UnitY) * topSpeed;
					}
				}
			}
			//int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("AncientGlow"), 0, 0, 0, default(Color), .4f);
			Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(26, projectile.rotation + (float)Math.PI / 2) + QwertyMethods.PolarVector(Main.rand.Next(-6, 6), projectile.rotation), mod.DustType("AncientGlow"));
			maxDistance = 10000;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = -1;
			target.immune[projectile.owner] = 0;
			Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("AncientBlastFriendly"), projectile.damage, projectile.knockBack, projectile.owner, 1f)];
			e.localNPCImmunity[target.whoAmI] = -1;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("AncientBlastFriendly"), projectile.damage, projectile.knockBack, projectile.owner);
			return true;
		}

		/*
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("AncientBlast"), projectile.damage, projectile.knockBack, player.whoAmI);
        }*/

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			spriteBatch.Draw(Main.projectileTexture[projectile.type], new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
						new Rectangle(0, projectile.frame * Main.projectileTexture[projectile.type].Height / 2, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 2), drawColor, projectile.rotation,
						new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/AncientMissileP_Glow_Old") : mod.GetTexture("NPCs/AncientMachine/AncientMissile_Glow"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
						new Rectangle(0, projectile.frame * Main.projectileTexture[projectile.type].Height / 2, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 2), Color.White, projectile.rotation,
						new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
			return false;
		}
	}

	public class AncientBlastFriendly : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Blast");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.width = 150;
			projectile.height = 150;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 2;
			projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.width = 150;
			projectile.height = 150;
			projectile.FriendlyFire();

			Main.PlaySound(SoundID.Item62, projectile.position);

			for (int i = 0; i < 400; i++)
			{
				float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
				Dust dust = Dust.NewDustPerfect(projectile.Center, mod.DustType("AncientGlow"), QwertyMethods.PolarVector(Main.rand.Next(2, 20), theta));
				dust.noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = -1;
			target.immune[projectile.owner] = 0;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
	}
}