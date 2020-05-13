using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.NPCs.BladeBoss;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace QwertysRandomContent.Items.BladeBossItems
{
	public class Swordquake : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shape Shift: Swordquake");
			Tooltip.SetDefault("Turn into a sword that causes a deadly swordquake upon striking the ground!");
		}

		public const int dmg = 900;
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
			item.GetGlobalItem<ShapeShifterItem>().morphCooldown = 32;
			item.noMelee = true;

			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 5;

			item.rare = 7;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.noUseGraphic = true;
			item.width = 38;
			item.height = 38;

			//item.autoReuse = true;
			item.shoot = mod.ProjectileType("SwordquakeP");
			item.shootSpeed = 1f;
			item.channel = true;
		}
	}

	public class SwordquakeP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Swordquake");
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 90;
			projectile.usesLocalNPCImmunity = true;
			projectile.extraUpdates = 1;
		}

		private bool runOnce = true;

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			player.Center = projectile.Center;
			player.immune = true;
			player.immuneTime = 2;
			player.statDefense = 0;
			projectile.velocity = Vector2.Zero;
			player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
			if (runOnce)
			{
				projectile.rotation = player.direction == 1 ? (float)Math.PI : 0;
				runOnce = false;
			}

			if (projectile.ai[0] == 2)
			{
				player.GetModPlayer<SwordQuakeShake>().shake = true;
			}
			else
			{
				if (projectile.timeLeft < 30)
				{
					projectile.rotation += player.direction * (float)Math.PI / 15;
					if (!Collision.CanHit(projectile.Center, 0, 0, projectile.Center + QwertyMethods.PolarVector(180, projectile.rotation), 0, 0))
					{
						projectile.timeLeft = 30;
						projectile.ai[0] = 2;
						Vector2 start = projectile.Center + QwertyMethods.PolarVector(180, projectile.rotation) + player.direction * 14 * Vector2.UnitX;

						Point point;
						while (WorldUtils.Find(start.ToTileCoordinates(), Searches.Chain(new Searches.Up(1), new GenCondition[] { new Conditions.IsSolid() }), out point))
						{
							start += -Vector2.UnitY;
						}
						while (!WorldUtils.Find(start.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[] { new Conditions.IsSolid() }), out point))
						{
							start += Vector2.UnitY;
						}
						start += Vector2.UnitY * 20;
						Projectile.NewProjectile(start, Vector2.Zero, mod.ProjectileType("SwordlagmitePlayer"), projectile.damage, projectile.knockBack, projectile.owner, player.direction, 40);
					}
				}
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float CP = 0;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center + QwertyMethods.PolarVector(-12, projectile.rotation), projectile.Center + QwertyMethods.PolarVector(194 - 12, projectile.rotation), 34, ref CP);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, new Vector2(12, texture.Height / 2), 1f, 0, 0);
			return false;
		}
	}

	public class SwordlagmitePlayer : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Swordlagmite");
			ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true; // projectiles with hide but without this will draw in the lighting values of the owner player.
		}

		public override void SetDefaults()
		{
			projectile.width = 28;
			projectile.height = 2;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 61;
			projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
			projectile.usesIDStaticNPCImmunity = true;
		}

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			drawCacheProjsBehindNPCsAndTiles.Add(index);
		}

		private const int lingerTime = 60;
		private const int extendSpeed = 30;
		private int heightMax = 150;

		public override void AI()
		{
			if (projectile.timeLeft == lingerTime)
			{
				projectile.height += extendSpeed;
				projectile.position.Y -= extendSpeed;

				if (projectile.height < heightMax)
				{
					projectile.timeLeft++;
				}
			}
			if (projectile.timeLeft == lingerTime - 1)
			{
				Main.PlaySound(SoundID.Item69, projectile.Center);
				if (projectile.ai[1] > 0)
				{
					Vector2 start = projectile.Bottom + projectile.ai[0] * 28 * Vector2.UnitX;
					Point point;
					while (WorldUtils.Find(start.ToTileCoordinates(), Searches.Chain(new Searches.Up(1), new GenCondition[] { new Conditions.IsSolid() }), out point))
					{
						start += -Vector2.UnitY;
					}
					while (!WorldUtils.Find(start.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[] { new Conditions.IsSolid() }), out point))
					{
						start += Vector2.UnitY;
					}
					start += Vector2.UnitY * 20;
					Projectile.NewProjectile(start, Vector2.Zero, mod.ProjectileType("SwordlagmitePlayer"), projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1] - 1);
				}
			}
			if (projectile.timeLeft == 1)
			{
				projectile.height -= extendSpeed;
				projectile.position.Y += extendSpeed;
				if (projectile.height > extendSpeed)
				{
					projectile.timeLeft++;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			int tipHeight = 14;
			int segmentHeight = 12;
			Texture2D texture = Main.projectileTexture[projectile.type];
			int k = 0;
			spriteBatch.Draw(texture, projectile.position + Vector2.UnitY * (k * segmentHeight) - Main.screenPosition, new Rectangle(0, 0, projectile.width, 82), Lighting.GetColor((int)projectile.Center.X / 16, (int)(projectile.position.Y + tipHeight / 2) / 16), 0, Vector2.Zero, 1f, 0, 0);

			for (; k < ((projectile.height - tipHeight) / segmentHeight) - 1; k++)
			{
				spriteBatch.Draw(texture, projectile.position + Vector2.UnitY * (k * segmentHeight + tipHeight) - Main.screenPosition, new Rectangle(0, tipHeight, projectile.width, segmentHeight), Lighting.GetColor((int)projectile.Center.X / 16, (int)(projectile.position.Y + (k * segmentHeight) + tipHeight + segmentHeight / 2) / 16), 0, Vector2.Zero, 1f, 0, 0);
			}
			spriteBatch.Draw(texture, projectile.position + Vector2.UnitY * (k * segmentHeight + tipHeight) - Main.screenPosition, new Rectangle(0, tipHeight, projectile.width, (projectile.height - tipHeight) % segmentHeight), Lighting.GetColor((int)projectile.Center.X / 16, (int)(projectile.position.Y + (k * segmentHeight) + tipHeight + ((projectile.height - tipHeight) % segmentHeight) / 2) / 16), 0, Vector2.Zero, 1f, 0, 0);
			return false;
		}
	}
}