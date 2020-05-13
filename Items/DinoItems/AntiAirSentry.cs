using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace QwertysRandomContent.Items.DinoItems      ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
	public class AntiAirWrench : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anti Air Sentry Wrench");
			Tooltip.SetDefault("Summons a stationary anti air sentry");
		}

		public override void SetDefaults()
		{
			item.damage = 100;  //The damage stat for the Weapon.
			item.mana = 20;      //this defines how many mana this weapon use
			item.width = 56;    //The size of the width of the hitbox in pixels.
			item.height = 56;     //The size of the height of the hitbox in pixels.
			item.useTime = 25;   //How fast the Weapon is used.
			item.useAnimation = 25;    //How long the Weapon is used for.
			item.useStyle = 1;  //The way your Weapon will be used, 1 is the regular sword swing for example
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 10f;  //The knockback stat of your Weapon.
			item.value = 25000;
			item.rare = 3;
			item.UseSound = SoundID.Item44;   //The sound played when using your Weapon
			item.autoReuse = true;   //Weather your Weapon will be used again after use while holding down, if false you will need to click again after use to use it again.
			item.shoot = mod.ProjectileType("AntiAirSentry");   //This defines what type of projectile this weapon will shot
			item.summon = true;    //This defines if it does Summon damage and if its effected by Summon increasing Armor/Accessories.
			item.sentry = true; //tells the game that this is a sentry
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;   //this make so the projectile will spawn at the mouse cursor position

			return true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				player.MinionNPCTargetAim();
			}
			return base.UseItem(player);
		}
	}

	public class AntiAirSentry : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anti Air Sentry");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
			Main.projFrames[projectile.type] = 3;  //this is where you add how many frames u'r projectile has to make the animation
		}

		public override void SetDefaults()
		{
			projectile.sentry = true;
			projectile.width = 60; //Set the hitbox width
			projectile.height = 94;   //Set the hitbox heinght
			projectile.hostile = false;    //tells the game if is hostile or not.
			projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly npcs or not
			projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.knockBack = 10f;
			projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
			projectile.tileCollide = true; //Tells the game whether or not it can collide with tiles/ terrain
			projectile.sentry = true; //tells the game that this is a sentry
		}

		public int frameType = 0;
		public int ReloadTime = 20;

		public int secondShot = 1;
		public float minimumHeight = 400;
		public float maxDistanceX = 600f;
		public float distanceX;
		public float distanceY;
		public NPC target;
		public NPC validTarget;
		public bool foundTarget;
		public int timer;
		public bool playAttackFrame;
		public int attackFrameCounter;
		public int attackFrameTime = 5;
		public int rocketDirection = 1;

		public override void AI()
		{
			projectile.frame = 1;
			Main.player[projectile.owner].UpdateMaxTurrets();
			projectile.velocity.Y = 5;
			for (int i = 0; i < 200; i++)
			{
				target = Main.npc[i];
				distanceX = Math.Abs(Main.npc[i].Center.X - projectile.Center.X);
				distanceY = projectile.Center.Y - Main.npc[i].Center.Y;
				Point origin = target.Center.ToTileCoordinates();
				Point point;
				if (distanceX < maxDistanceX && distanceY > minimumHeight && !target.friendly && target.active && !target.immortal && !target.dontTakeDamage && !WorldUtils.Find(origin, Searches.Chain(new Searches.Down(12), new GenCondition[]
										{
											new Conditions.IsSolid()
										}), out point))
				{
					foundTarget = true;
					validTarget = Main.npc[i];
					if (validTarget.Center.X > projectile.Center.X)
					{
						rocketDirection = 1;
					}
					else
					{
						rocketDirection = -1;
					}
				}
			}
			timer++;
			if (foundTarget && timer > ReloadTime)
			{
				playAttackFrame = true;
				if (Main.netMode != 1)
					Projectile.NewProjectile(projectile.Center.X + (16 * secondShot), projectile.Center.Y - 30, 0, 0, mod.ProjectileType("SentryAntiAir"), projectile.damage, projectile.knockBack, Main.myPlayer, validTarget.Center.Y, rocketDirection);

				secondShot *= -1;

				timer = 0;
			}

			if (playAttackFrame)
			{
				attackFrameCounter++;
				if (secondShot == 1)
				{
					projectile.frame = 0;
				}
				else
				{
					projectile.frame = 2;
				}

				if (attackFrameCounter > attackFrameTime)
				{
					playAttackFrame = false;
				}
			}
			else
			{
				attackFrameCounter = 0;
			}

			foundTarget = false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
	}

	public class SentryAntiAir : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anti Air Rocket");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 240;
			projectile.tileCollide = true;
			projectile.minion = true;
			projectile.alpha = 255;
		}

		public int k = 0;
		public bool runOnce = true;
		public float OriY;
		public float minimumHeight = 400f;
		public float maxDistanceX = 600f;
		public NPC target;
		public bool goRight;
		public bool goLeft;
		public int timer;

		public override void AI()
		{
			timer++;
			projectile.velocity.Y = -10;
			if (runOnce)
			{
				Main.PlaySound(SoundID.Item62, projectile.position);
				for (int i = 0; i < 50; i++)
				{
					int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
					Main.dust[dustIndex].velocity *= .6f;
				}
				// Fire Dust spawn
				for (int i = 0; i < 80; i++)
				{
					int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].velocity *= 2f;
					dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
					Main.dust[dustIndex].velocity *= 1f;
				}
				runOnce = false;
			}
			if (goRight)
			{
				projectile.velocity.X = 10;
				projectile.velocity.Y = 0;
			}
			else if (goLeft)
			{
				projectile.velocity.X = -10;
				projectile.velocity.Y = 0;
			}
			else if (timer > 40)
			{
				for (int i = 0; i < 200; i++)
				{
					target = Main.npc[i];
					Point origin = target.Center.ToTileCoordinates();
					Point point;
					if (target.Center.Y + 20 >= projectile.Center.Y && target.Center.Y - 20 <= projectile.Center.Y && !target.friendly && target.active && !target.immortal && !target.dontTakeDamage && !WorldUtils.Find(origin, Searches.Chain(new Searches.Down(12), new GenCondition[]
										{
											new Conditions.IsSolid()
										}), out point))
					{
						if (target.Center.X > projectile.Center.X)
						{
							goRight = true;
						}
						else
						{
							goLeft = true;
						}
					}
				}
			}
			else
			{
			}

			projectile.alpha = 0;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item62, projectile.position);
			for (int i = 0; i < 50; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
				Main.dust[dustIndex].velocity *= .6f;
			}
			// Fire Dust spawn
			for (int i = 0; i < 80; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity *= 2f;
				dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
				Main.dust[dustIndex].velocity *= 1f;
			}
		}
	}

	/*
	public class SentryAntiAirSide : ModProjectile
	{
		public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Anti Air Rocket");
			}
			public override void SetDefaults()
			{
				projectile.aiStyle = 1;
				aiType = ProjectileID.Bullet;
				projectile.width = 10;
				projectile.height = 10;
				projectile.friendly = true;
				projectile.hostile = false;
				projectile.penetrate = 1;
				projectile.timeLeft = 240;
				projectile.tileCollide = true;
				projectile.minion= true;
			}
	}
	*/
}