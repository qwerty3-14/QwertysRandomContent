using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.B4Items
{
	public class ExplosivePierce : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Cannon");
			Tooltip.SetDefault("Fires projectiles that pierce through enemies exploding every time they hit something!");
		}

		public override void SetDefaults()
		{
			item.damage = 175;
			item.magic = true;

			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 5;
			item.knockBack = 1;
			item.value = 750000;
			item.rare = 10;
			item.UseSound = SoundID.Item91;
			item.autoReuse = true;
			item.width = 92;
			item.height = 30;
			item.crit = 20;
			item.mana = 11;
			item.shoot = mod.ProjectileType("EPShot");
			item.shootSpeed = 27;
			item.noMelee = true;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/B4Items/ExplosivePierce_Glowmask");
			}
			item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -15;
			item.GetGlobalItem<ItemUseGlow>().glowOffsetY = -2;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-15, -2);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/B4Items/ExplosivePierce_Glowmask");
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

	public class EPShot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("EPShot");
			Main.projFrames[projectile.type] = 18;
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 0;
			projectile.magic = true;
			projectile.width = 44;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 600;
			projectile.tileCollide = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.light = 1f;
		}

		public bool runOnce = true;

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.frameCounter++;
			if (projectile.frameCounter % 1 == 0)
			{
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
				{
					projectile.frame = 0;
				}
			}
			for (int i = 0; i < 1; i++)
			{
				Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("EPDust"), 0f, 0f, 100, default(Color), 1f)];
			}
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("EPexplosion"), projectile.damage, projectile.knockBack, player.whoAmI);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("EPexplosion"), projectile.damage, projectile.knockBack, player.whoAmI);
			projectile.localNPCImmunity[target.whoAmI] = -1;
			target.immune[projectile.owner] = 0;
		}
	}

	public class EPexplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("EP explosion");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.width = 100;
			projectile.height = 100;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 2;
			projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.width = 100;
			projectile.height = 100;

			Main.PlaySound(SoundID.Item91, projectile.position);

			for (int i = 0; i < 40; i++)
			{
				Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("EPDust"), 0f, 0f, 100, default(Color), 1f)];
				dust.noGravity = true;
				dust.velocity *= 5f;
				float distFromCenter = Main.rand.NextFloat(0, 1f);
				float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);

				dust.position = projectile.Center + new Vector2((float)Math.Cos(theta) * distFromCenter * projectile.width / 2, (float)Math.Sin(theta) * distFromCenter * projectile.height / 2);
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