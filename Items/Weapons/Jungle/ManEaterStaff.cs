using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace QwertysRandomContent.Items.Weapons.Jungle
{
	public class ManEaterStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Man Eater Staff");
			Tooltip.SetDefault("Summons a man eater to munch on nearby enemies \nCounts as a sentry");
		}

		public override void SetDefaults()
		{
			item.width = 52;
			item.height = 52;
			item.mana = 10;
			item.damage = 16;

			item.shoot = mod.ProjectileType("ManEaterP");
			item.shootSpeed = 0f;
			item.useTime = 20;
			item.useAnimation = 20;

			item.useStyle = 1;
			item.knockBack = 2f;
			item.value = Item.sellPrice(silver: 54);
			item.rare = 3;
			item.UseSound = SoundID.Item44;
			item.sentry = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.summon = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;
			Point point;
			Point origin = position.ToTileCoordinates();
			while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
				{
											new Conditions.IsSolid()
				}), out point))
			{
				position.Y++;
				origin = position.ToTileCoordinates();
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);

			recipe.AddIngredient(ItemID.JungleSpores, 8);
			recipe.AddIngredient(ItemID.Stinger, 2);
			recipe.AddIngredient(ItemID.Vine, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class ManEaterP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Man Eater");
		}

		public override void SetDefaults()
		{
			projectile.minion = true;
			projectile.sentry = true;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 30;
		}

		private Vector2 HeadPos = new Vector2(0, 0);
		private Vector2 Goto;
		private int frame = 0;
		private int animationDir = -1;
		private NPC target;
		private float trigCounter;
		private int wanderTimer;
		private float maxSpeed = 3;
		private Vector2 HeadOffPos;

		public override void AI()
		{
			trigCounter += (float)Math.PI / 60;

			Player player = Main.player[projectile.owner];
			player.UpdateMaxTurrets();

			Vector2 DifferenceToGo = Goto - projectile.Center - HeadPos;
			float speed = DifferenceToGo.Length();
			wanderTimer++;
			if (wanderTimer > 60)
			{
				if (Main.netMode == 1 && projectile.owner == Main.myPlayer)
				{
					projectile.ai[1] = Main.rand.NextFloat(2 * (float)Math.PI);

					if (Main.netMode == 1)
					{
						QwertysRandomContent.ProjectileAIUpdate(projectile);
					}

					projectile.netUpdate = true;
				}
				else if (Main.netMode == 0)
				{
					projectile.ai[1] = Main.rand.NextFloat(2 * (float)Math.PI);
				}
				wanderTimer = 0;
			}
			if (QwertyMethods.ClosestNPC(ref target, 700, projectile.Center, true, player.MinionAttackTargetNPC))
			{
				Goto = target.Center;
				maxSpeed = 6;
			}
			else
			{
				Goto = projectile.Center + QwertyMethods.PolarVector(100, projectile.ai[1]);
			}

			if (speed > maxSpeed)
			{
				speed = maxSpeed;
			}
			HeadPos += QwertyMethods.PolarVector(speed, DifferenceToGo.ToRotation());
			projectile.frameCounter++;
			if (projectile.frameCounter % 10 == 0)
			{
				if (frame == 2 || frame == 0)
				{
					animationDir *= -1;
				}
				frame += animationDir;
			}
			HeadOffPos = HeadPos + QwertyMethods.PolarVector(20 * (float)Math.Sin(trigCounter), HeadPos.ToRotation());
			maxSpeed = 3;
			//were used to check hitbox
			//Dust.NewDustPerfect((projectile.Center + HeadOffPos) + QwertyMethods.PolarVector(-13, HeadPos.ToRotation()), DustID.Fire);
			// Dust.NewDustPerfect((projectile.Center + HeadOffPos) + QwertyMethods.PolarVector(28, HeadPos.ToRotation()), DustID.Fire);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D vine = Main.chain4Texture;
			for (int i = 0; i < HeadOffPos.Length(); i++)
			{
				Vector2 p = projectile.Center + QwertyMethods.PolarVector(i, HeadPos.ToRotation());
				spriteBatch.Draw(vine, p - Main.screenPosition,
					   new Rectangle(0, vine.Height - (i % vine.Height), vine.Width, 1), Lighting.GetColor((int)p.X / 16, (int)p.Y / 16), HeadPos.ToRotation() + (float)Math.PI / 2,
					   new Vector2(vine.Width / 2, .5f), 1f, 0, 0f);
			}

			Texture2D Head = Main.projectileTexture[projectile.type];
			Vector2 lp = (projectile.Center + HeadOffPos);
			spriteBatch.Draw(Head, lp - Main.screenPosition,
					   new Rectangle(0, frame * Head.Height / 3, Head.Width, Head.Height / 3), Lighting.GetColor((int)lp.X / 16, (int)lp.Y / 16), HeadPos.ToRotation(),
					   new Vector2(22, Head.Height / 6), 1f, 0, 0f);
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float p = 0;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(),
				targetHitbox.Size(),
				(projectile.Center + HeadOffPos) + QwertyMethods.PolarVector(-13, HeadPos.ToRotation()),
				(projectile.Center + HeadOffPos) + QwertyMethods.PolarVector(28, HeadPos.ToRotation()),
				34,
				ref p
				) || Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(),
				targetHitbox.Size(),
				(projectile.Center + HeadOffPos),
				new Vector2(1, 1));
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
	}
}