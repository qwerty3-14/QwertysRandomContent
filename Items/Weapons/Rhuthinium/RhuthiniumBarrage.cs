using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Rhuthinium
{
	internal class RhuthiniumBarrage : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shape shift: Rhuthinium Barrage");
			Tooltip.SetDefault("Launches a HUGE barrage of darts dealing massive damage!");
		}

		public const int dmg = 26;
		public const int crt = 0;
		public const float kb = 0f;
		public const int def = 3;

		public override void SetDefaults()
		{
			item.damage = dmg;
			item.crit = crt;
			item.knockBack = kb;
			item.GetGlobalItem<ShapeShifterItem>().morph = true;
			item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
			item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.QuickShiftType;
			item.GetGlobalItem<ShapeShifterItem>().morphCooldown = 40;
			item.noMelee = true;

			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 5;

			item.value = 25000;
			item.rare = 3;
			item.crit = 5;
			item.noUseGraphic = true;
			item.width = 18;
			item.height = 32;

			//item.autoReuse = true;
			item.shoot = mod.ProjectileType("RhuthiniumBarrageLauncher");
			item.shootSpeed = 0f;
			item.channel = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
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

			return true;
		}
	}

	public class RhuthiniumBarrageLauncher : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Barrage Launcher");
			Main.projFrames[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;

			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 180;
		}

		private Deck<Projectile> Darts = new Deck<Projectile>();
		private bool runOnce = true;
		private int indexCounter = 0;

		public override void AI()
		{
			if (runOnce)
			{
				for (int d = 0; d < 120; d++)
				{
					Darts.Add(Main.projectile[Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("RhuthiniumBarrageDart"), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(-14, 15), 0f)]);
				}
				runOnce = false;
			}

			Player player = Main.player[projectile.owner];
			player.Center = projectile.Center;

			player.statDefense = 0;
			player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
			projectile.rotation = (QwertysRandomContent.LocalCursor[projectile.owner] - projectile.Center).ToRotation();

			foreach (Projectile dart in Darts)
			{
				if (dart.ai[1] == 0 && dart.type == mod.ProjectileType("RhuthiniumBarrageDart"))
				{
					dart.Center = projectile.Center + QwertyMethods.PolarVector(25, projectile.rotation) + QwertyMethods.PolarVector(dart.ai[0], projectile.rotation + (float)Math.PI / 2);
					dart.rotation = projectile.rotation;
				}
			}
			if (projectile.timeLeft < 150)
			{
				if (indexCounter < Darts.Count)
				{
					Darts[indexCounter].ai[1] = 1f;
					indexCounter++;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D drawDart = mod.GetTexture("Items/Weapons/Rhuthinium/RhuthiniumBarrageDart");
			foreach (Projectile dart in Darts)
			{
				if (dart != null && dart.active && dart.type == mod.ProjectileType("RhuthiniumBarrageDart"))
				{
					spriteBatch.Draw(drawDart, dart.Center - Main.screenPosition,
					   drawDart.Frame(), Lighting.GetColor((int)dart.Center.X / 16, (int)dart.Center.Y / 16), dart.rotation,
					   new Vector2(drawDart.Width, drawDart.Height * .5f), 1f, 0, 0f);
				}
			}
			Texture2D Launcher = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(Launcher, projectile.Center - Main.screenPosition,
					   Launcher.Frame(), lightColor, projectile.rotation,
					   new Vector2(0, Launcher.Height * .5f), 1f, 0, 0f);
			return false;
		}
	}

	public class RhuthiniumBarrageDart : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Barrage");
			Main.projFrames[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;

			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 300;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void AI()
		{
			if (projectile.ai[1] == 1f)
			{
				projectile.friendly = true;
				projectile.tileCollide = true;
				projectile.extraUpdates = 2;
				projectile.velocity = QwertyMethods.PolarVector(8, projectile.rotation);
			}
		}
	}
}