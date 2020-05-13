using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Rhuthinium       ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
	public class RhuthiniumGuardianStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Guardian Staff");
			Tooltip.SetDefault("Summons an extremely slow guardian to shoot at your enemies");
			//Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(38, 2));
		}

		public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium ? base.Texture + "_Old" : base.Texture;

		public override void SetDefaults()
		{
			item.damage = 200;  //The damage stat for the Weapon.
			item.mana = 20;      //this defines how many mana this weapon use
			item.width = 38;    //The size of the width of the hitbox in pixels.
			item.height = 38;     //The size of the height of the hitbox in pixels.
			item.useTime = 25;   //How fast the Weapon is used.
			item.useAnimation = 25;    //How long the Weapon is used for.
			item.useStyle = 1;  //The way your Weapon will be used, 1 is the regular sword swing for example
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 10f;  //The knockback stat of your Weapon.
			item.value = 25000;
			item.rare = 3;
			item.UseSound = SoundID.Item44;   //The sound played when using your Weapon
			item.autoReuse = true;   //Weather your Weapon will be used again after use while holding down, if false you will need to click again after use to use it again.
			item.shoot = mod.ProjectileType("RhuthiniumGuardian");   //This defines what type of projectile this weapon will shot
			item.summon = true;    //This defines if it does Summon damage and if its effected by Summon increasing Armor/Accessories.
			item.sentry = true; //tells the game that this is a sentry
		}

		public Projectile projC;

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);   //this make so the projectile will spawn at the mouse cursor position

			//projC = Main.projectile[Projectile.NewProjectile(position.X, position.Y, 0, 0, mod.ProjectileType("RhuthiniumGuardian"), damage, knockBack, Main.myPlayer, 0f, 0f)];

			return true;
		}

		public override bool CanUseItem(Player player)
		{
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

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class RhuthiniumGuardian : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Guardian");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
		}

		public override void SetDefaults()
		{
			projectile.width = 30; //Set the hitbox width
			projectile.height = 30;   //Set the hitbox heinght
			projectile.hostile = false;    //tells the game if is hostile or not.
			projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly npcs or not
			projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
			Main.projFrames[projectile.type] = 1;  //this is where you add how many frames u'r projectile has to make the animation
			projectile.knockBack = 10f;
			projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
			projectile.tileCollide = true; //Tells the game whether or not it can collide with tiles/ terrain
			projectile.sentry = true; //tells the game that this is a sentry
			projectile.timeLeft = Projectile.SentryLifeTime;
		}

		private NPC confirmTarget;
		private int timer;
		private Color lineColor;
		private bool drawLine;
		private bool alternateColor = false;
		private int colorCounter;

		private bool startCountdown;
		private int countdownTimer;
		private float Aim;
		private float shardVelocity = 30f;
		private float lineLength = 0;

		public override void AI()
		{
			Main.player[projectile.owner].UpdateMaxTurrets();
			Player player = Main.player[projectile.owner];

			projectile.rotation += (float)Math.PI / 60;   //this make the projctile to rotate

			if (QwertyMethods.ClosestNPC(ref confirmTarget, 100000, projectile.Center, false, player.MinionAttackTargetNPC))
			{
				drawLine = true;
				lineLength = (confirmTarget.Center - projectile.Center).Length();
				Aim = (confirmTarget.Center - projectile.Center).ToRotation();
				timer++;
				if (timer == 420)
				{
					startCountdown = true;
				}
				if (timer >= 600)
				{
					if (Main.netMode != 1)
						Projectile.NewProjectile(projectile.Center, QwertyMethods.PolarVector(shardVelocity, Aim), mod.ProjectileType("RhuthiniumShard"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
					timer = 0;
				}
			}
			if (startCountdown)
			{
				alternateColor = true;
				countdownTimer++;
				if (countdownTimer == 180)
				{
					startCountdown = false;
				}
			}
			else
			{
				alternateColor = false;
				countdownTimer = 0;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.Draw(mod.GetTexture("Items/Weapons/Rhuthinium/RhuthiniumGuardianLower" + (ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium ? "_Old" : "")), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
					new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), lightColor, -projectile.rotation,
					new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);

			if (alternateColor)
			{
				colorCounter++;

				if (colorCounter >= 20)
				{
					colorCounter = 0;
				}
				else if (colorCounter >= 10)
				{
					lineColor = Color.White;
				}
				else
				{
					lineColor = Color.Red;
				}
			}
			else
			{
				lineColor = Color.Red;
			}
			//Draw chain
			if (drawLine)
			{
				Vector2 center = projectile.Center;
				Vector2 distToProj = confirmTarget.Center - center;
				float projRotation = distToProj.ToRotation() - 1.57f;
				distToProj.Normalize();                 //get unit vector
				distToProj *= 12f;                      //speed = 12
				center += distToProj;                   //update draw position
				distToProj = confirmTarget.Center - center;    //update distance
				Color drawColor = lightColor;

				spriteBatch.Draw(mod.GetTexture("Items/Weapons/Rhuthinium/laser"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, 1, (int)lineLength - 10), lineColor, projRotation,
					new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(mod.GetTexture("Items/Weapons/Rhuthinium/RhuthiniumGuardian" + (ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium ? "_Old" : "")), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
					new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), lightColor, projectile.rotation,
					new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
			drawLine = false;

			return false;
		}
	}

	public class RhuthiniumShard : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Shard");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.minion = true;
			projectile.knockBack = 10f;
			projectile.extraUpdates = 3;
		}

		public override void AI()
		{
		}

		public override void Kill(int timeLeft)
		{
		}
	}
}