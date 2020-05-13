using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.AncientItems
{
	public class AncientNuke : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shape shift: Ancient Nuke");
			Tooltip.SetDefault("Breifly turns you into an ancient nuke that causes a massive explosion when you collide with something... don't worry you'll live");
		}

		public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicAncient ? base.Texture + "_Old" : base.Texture;
		public const int dmg = 300;
		public const int crt = 0;
		public const float kb = 9f;
		public const int def = -1;

		public override void SetDefaults()
		{
			item.damage = dmg;
			item.crit = crt;
			item.knockBack = kb;
			item.GetGlobalItem<ShapeShifterItem>().morph = true;
			item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
			item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.QuickShiftType;
			item.GetGlobalItem<ShapeShifterItem>().morphCooldown = 27;
			item.noMelee = true;

			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 5;

			item.value = 150000;
			item.rare = 1;

			item.noUseGraphic = true;
			item.width = 18;
			item.height = 32;

			//item.autoReuse = true;
			item.shoot = mod.ProjectileType("AncientNukeMorph");
			item.shootSpeed = 0f;
			item.channel = true;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/AncientNuke_Glow_Old") : mod.GetTexture("Items/AncientItems/AncientNuke_Glow");
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

		public override bool CanUseItem(Player player)
		{
			for (int i = 0; i < 1000; ++i)
			{
				if ((Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot) || player.HasBuff(mod.BuffType("MorphCooldown")))
				{
					return false;
				}
			}
			//player.AddBuff(mod.BuffType("MorphCooldown"), (int)(item.GetGlobalItem<ShapeShifterItem>().morphCooldown * 60 * player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration * item.GetGlobalItem<ShapeShifterItem>().PrefixorphCooldownModifier));
			//Main.PlaySound(SoundID.Roar, player.position, 0);

			return true;
		}
	}

	public class AncientNukeMorph : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Nuke");
			Main.projFrames[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 600;
			projectile.usesLocalNPCImmunity = true;
			projectile.extraUpdates = 1;
		}

		private float dustYoffset;

		public override void AI()
		{
			//projectile.timeLeft = 2;

			Player player = Main.player[projectile.owner];
			player.Center = projectile.Center;
			player.immune = true;
			player.immuneTime = 2;
			player.statDefense = 0;
			player.itemAnimation = 2;
			player.itemTime = 2;
			player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
			player.AddBuff(mod.BuffType("HealingHalt"), 10);
			player.AddBuff(mod.BuffType("MorphCooldown"), (int)((18 * player.HeldItem.GetGlobalItem<ShapeShifterItem>().PrefixorphCooldownModifier * player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration) * 60f));
			//projectile.timeLeft = 2;
			//player.mount = null;
			//player.noItems = true;
			//player.buffTime[mod.BuffType("MorphCooldown")]++;
			if (player.controlLeft)
			{
				projectile.rotation -= (float)Math.PI / 60;
			}
			if (player.controlRight)
			{
				projectile.rotation += (float)Math.PI / 60;
			}
			projectile.velocity = QwertyMethods.PolarVector(10, projectile.rotation - (float)Math.PI / 2);
			dustYoffset = 20;
			for (int i = 0; i < 2; i++)
			{
				Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(dustYoffset, projectile.rotation + (float)Math.PI / 2) + QwertyMethods.PolarVector(Main.rand.Next(-9, 9), projectile.rotation), mod.DustType("AncientGlow"));
			}
		}

		public override void Kill(int timeLeft)
		{
			if (timeLeft == 0)
			{
				Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("AncientFallout"), projectile.damage, projectile.knockBack, projectile.owner)];
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = -1;
			target.immune[projectile.owner] = 0;
			Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("AncientFallout"), projectile.damage, projectile.knockBack, projectile.owner)];
			e.localNPCImmunity[target.whoAmI] = -1;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("AncientFallout"), projectile.damage, projectile.knockBack, projectile.owner)];
			return true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			spriteBatch.Draw(ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/AncientNukeMorph_Old") : mod.GetTexture("Items/AncientItems/AncientNukeMorph"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
						new Rectangle(0, 0, 38, 56), drawColor, projectile.rotation,
						new Vector2(46 * 0.5f, 56 * 0.5f), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/AncientNukeMorph_Glow_Old") : mod.GetTexture("Items/AncientItems/AncientNukeMorph_Glow"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
						new Rectangle(0, 0, 38, 56), Color.White, projectile.rotation,
						new Vector2(46 * 0.5f, 56 * 0.5f), 1f, SpriteEffects.None, 0f);
			return false;
		}
	}

	public class AncientFallout : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Fallout");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.width = 1000;
			projectile.height = 1000;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 2;
			projectile.usesLocalNPCImmunity = true;
			projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			Main.PlaySound(SoundID.Item62, projectile.position);

			for (int i = 0; i < 1600; i++)
			{
				float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
				Dust dust = Dust.NewDustPerfect(projectile.Center, mod.DustType("AncientGlow"), QwertyMethods.PolarVector(Main.rand.Next(2, 120), theta));
				dust.noGravity = true;
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Collision.CanHit(projectile.Center, 1, 1, targetHitbox.Location.ToVector2(), targetHitbox.Width, targetHitbox.Height))
			{
				return base.Colliding(projHitbox, targetHitbox);
			}
			return false;
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