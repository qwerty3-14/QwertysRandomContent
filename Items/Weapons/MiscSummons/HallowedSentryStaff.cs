using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSummons
{
	public class HallowedSentryStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Sentry Staff");
			Tooltip.SetDefault("Summons a hallowed sentry that casts blades around your enemies!");
		}

		public override void SetDefaults()
		{
			item.damage = 25;
			item.mana = 20;
			item.width = 32;
			item.height = 32;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 1;
			item.noMelee = true;
			item.knockBack = .01f;
			item.value = 424000;
			item.rare = 4;
			item.UseSound = SoundID.Item44;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("HallowedSentry");
			item.summon = true;
			item.sentry = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
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

	public class HallowedSentry : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Sentry");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			Main.projFrames[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.sentry = true;
			projectile.width = 38;
			projectile.height = 38;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.ignoreWater = true;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.knockBack = 10f;
			projectile.penetrate = -1;
			projectile.tileCollide = true;
			projectile.sentry = true;
			projectile.minion = true;
			projectile.usesLocalNPCImmunity = true;
			//projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
		}

		private NPC target;

		private float maxDistance = 1000f;
		private float distance;
		private int timer;
		private float bladeStartDist = 100;

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			player.UpdateMaxTurrets();
			timer++;

			if (QwertyMethods.ClosestNPC(ref target, maxDistance, projectile.Center, false, player.MinionAttackTargetNPC))
			{
				projectile.rotation = (target.Center - projectile.Center).ToRotation();
				if (timer % 10 == 0)
				{
					Vector2 offset = QwertyMethods.PolarVector(bladeStartDist, ((float)(timer % 80) / 80f) * (float)Math.PI * 2);
					if (offset.X > 0)
					{
						offset.X += target.width / 2;
					}
					else if (offset.X < 0)
					{
						offset.X -= target.width / 2;
					}
					if (offset.Y > 0)
					{
						offset.Y += target.height / 2;
					}
					else if (offset.Y < 0)
					{
						offset.Y -= target.height / 2;
					}
					Projectile.NewProjectile(target.Center + offset, -offset.SafeNormalize(-Vector2.UnitY) * 8, mod.ProjectileType("Excalimini"), projectile.damage, projectile.knockBack, player.whoAmI);
					if (timer % 20 == 0)
					{
						Projectile.NewProjectile(projectile.Center, QwertyMethods.PolarVector(8, projectile.rotation + (float)Math.PI / 2), mod.ProjectileType("ExcaliminiDraw"), projectile.damage, projectile.knockBack, player.whoAmI);
					}
					else
					{
						Projectile.NewProjectile(projectile.Center, QwertyMethods.PolarVector(8, projectile.rotation - (float)Math.PI / 2), mod.ProjectileType("ExcaliminiDraw"), projectile.damage, projectile.knockBack, player.whoAmI);
					}
				}
			}
		}
	}

	public class Excalimini : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("RhuthiniumJavelin");
		}

		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.penetrate = 1;
			projectile.GetGlobalProjectile<ImplaingProjectile>().CanImpale = true;
			projectile.GetGlobalProjectile<ImplaingProjectile>().damagePerImpaler = 1;
			projectile.alpha = 255;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Items/Weapons/MiscSummons/Excalimini");
			spriteBatch.Draw(texture, new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y + 2),
						new Rectangle(0, 0, texture.Width, texture.Height), Color.Lerp(lightColor, new Color(0, 0, 0, 0), (float)projectile.alpha / 255f), projectile.rotation,
						new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
			return false;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			// For going through platforms and such, javelins use a tad smaller size
			width = height = 4; // notice we set the width to the height, the height to 10. so both are 10
			return true;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// Inflate some target hitboxes if they are beyond 8,8 size
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			// Return if the hitboxes intersects, which means the javelin collides or not
			return projHitbox.Intersects(targetHitbox);
		}

		// Here's an example on how you could make your AI even more readable, by giving AI fields more descriptive names
		// These are not used in AI, but it is good practice to apply some form like this to keep things organized

		// Are we sticking to a target?
		public bool isStickingToTarget
		{
			get { return projectile.ai[0] == 1f; }
			set { projectile.ai[0] = value ? 1f : 0f; }
		}

		// WhoAmI of the current target
		public float targetWhoAmI
		{
			get { return projectile.ai[1]; }
			set { projectile.ai[1] = value; }
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit,
			ref int hitDirection)
		{
			// If you'd use the example above, you'd do: isStickingToTarget = 1f;
			// and: targetWhoAmI = (float)target.whoAmI;
			isStickingToTarget = true; // we are sticking to a target
			targetWhoAmI = (float)target.whoAmI; // Set the target whoAmI
			projectile.velocity =
				(target.Center - projectile.Center) *
				0.75f; // Change velocity based on delta center of targets (difference between entity centers)
			projectile.netUpdate = true; // netUpdate this javelin
			target.AddBuff(mod.BuffType("Impaled"), 900); // Adds the Impaled debuff
			projectile.penetrate = -1;
			projectile.damage = 0; // Makes sure the sticking javelins do not deal damage anymore

			// The following code handles the javelin sticking to the enemy hit.
			int maxStickingJavelins = 75; // This is the max. amount of javelins being able to attach
			Point[] stickingJavelins = new Point[maxStickingJavelins]; // The point array holding for sticking javelins
			int javelinIndex = 0; // The javelin index
			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (i != projectile.whoAmI // Make sure the looped projectile is not the current javelin
					&& currentProjectile.active // Make sure the projectile is active
					&& currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
					&& currentProjectile.type == projectile.type // Make sure the projectile is of the same type as this javelin
					&& currentProjectile.ai[0] == 1f // Make sure ai0 state is set to 1f (set earlier in ModifyHitNPC)
					&& currentProjectile.ai[1] == (float)target.whoAmI
				) // Make sure ai1 is set to the target whoAmI (set earlier in ModifyHitNPC)
				{
					stickingJavelins[javelinIndex++] =
						new Point(i, currentProjectile.timeLeft); // Add the current projectile's index and timeleft to the point array
					if (javelinIndex >= stickingJavelins.Length
					) // If the javelin's index is bigger than or equal to the point array's length, break
					{
						break;
					}
				}
			}
			// Here we loop the other javelins if new javelin needs to take an older javelin's place.
			if (javelinIndex >= stickingJavelins.Length)
			{
				int oldJavelinIndex = 0;
				// Loop our point array
				for (int i = 1; i < stickingJavelins.Length; i++)
				{
					// Remove the already existing javelin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javelin's timeLeft
					if (stickingJavelins[i].Y < stickingJavelins[oldJavelinIndex].Y)
					{
						oldJavelinIndex = i; // Remember the index of the removed javelin
					}
				}
				// Remember that the X value in our point array was equal to the index of that javelin, so it's used here to kill it.
				Main.projectile[stickingJavelins[oldJavelinIndex].X].Kill();
			}
		}

		// Added these 2 constant to showcase how you could make AI code cleaner by doing this
		// Change this number if you want to alter how long the javelin can travel at a constant speed
		private const float maxTicks = 45f;

		// Change this number if you want to alter how the alpha changes
		private const int alphaReduction = 20;

		public override void AI()
		{
			//projectile.alpha -= 1;
			// Slowly remove alpha as it is present
			if (projectile.alpha > 0)
			{
				projectile.alpha -= alphaReduction;
			}
			// If alpha gets lower than 0, set it to 0
			if (projectile.alpha < 0)
			{
				projectile.alpha = 0;
			}
			// If ai0 is 0f, run this code. This is the 'movement' code for the javelin as long as it isn't sticking to a target
			if (!isStickingToTarget)
			{
				targetWhoAmI += 1f;
				// For a little while, the javelin will travel with the same speed, but after this, the javelin drops velocity very quickly.
				if (targetWhoAmI >= maxTicks)
				{
					// Change these multiplication factors to alter the javelin's movement change after reaching maxTicks
					float velXmult = 0.98f; // x velocity factor, every AI update the x velocity will be 98% of the original speed
					float
						velYmult = 0.35f; // y velocity factor, every AI update the y velocity will be be 0.35f bigger of the original speed, causing the javelin to drop to the ground
					targetWhoAmI = maxTicks; // set ai1 to maxTicks continuously
					projectile.velocity.X = projectile.velocity.X * velXmult;
					projectile.velocity.Y = projectile.velocity.Y + velYmult;
				}
				// Make sure to set the rotation accordingly to the velocity, and add some to work around the sprite's rotation
				projectile.rotation =
					projectile.velocity.ToRotation() +
					MathHelper.ToRadians(
						135f); // Please notice the MathHelper usage, offset the rotation by 135 degrees (to radians because rotation uses radians) because the sprite's rotation is not aligned!
			}
			// This code is ran when the javelin is sticking to a target
			if (isStickingToTarget)
			{
				// These 2 could probably be moved to the ModifyNPCHit hook, but in vanilla they are present in the AI
				projectile.ignoreWater = true; // Make sure the projectile ignores water
				projectile.tileCollide = false; // Make sure the projectile doesn't collide with tiles anymore
				int aiFactor = 15; // Change this factor to change the 'lifetime' of this sticking javelin
				bool killProj = false; // if true, kill projectile at the end
				bool hitEffect = false; // if true, perform a hit effect
				projectile.localAI[0] += 1f;
				// Every 30 ticks, the javelin will perform a hit effect
				hitEffect = projectile.localAI[0] % 30f == 0f;
				int projTargetIndex = (int)targetWhoAmI;
				if (projectile.localAI[0] >= (float)(60 * aiFactor)// If it's time for this javelin to die, kill it
					|| (projTargetIndex < 0 || projTargetIndex >= 200)) // If the index is past its limits, kill it
				{
					killProj = true;
				}
				else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage) // If the target is active and can take damage
				{
					// Set the projectile's position relative to the target's center
					projectile.Center = Main.npc[projTargetIndex].Center - projectile.velocity * 2f;
					projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
					if (hitEffect) // Perform a hit effect here
					{
						Main.npc[projTargetIndex].HitEffect(0, 1.0);
					}
				}
				else // Otherwise, kill the projectile
				{
					killProj = true;
				}

				if (killProj) // Kill the projectile
				{
					projectile.Kill();
				}
			}
		}
	}

	public class ExcaliminiDraw : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("RhuthiniumJavelin");
		}

		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			//projectile.thrown = true;
			projectile.penetrate = 1;
			projectile.GetGlobalProjectile<ImplaingProjectile>().CanImpale = true;
			projectile.GetGlobalProjectile<ImplaingProjectile>().damagePerImpaler = 1;
			projectile.alpha = 0;
			projectile.tileCollide = false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Items/Weapons/MiscSummons/Excalimini");
			spriteBatch.Draw(texture, new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y + 2),
						new Rectangle(0, 0, texture.Width, texture.Height), Color.Lerp(lightColor, new Color(0, 0, 0, 0), (float)projectile.alpha / 255f), projectile.rotation,
						new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
			return false;
		}

		private const int alphaIncrease = 20;

		public override void AI()
		{
			if (projectile.alpha < 255)
			{
				projectile.alpha += alphaIncrease;
			}

			if (projectile.alpha > 255)
			{
				projectile.alpha = 255;
			}

			// Make sure to set the rotation accordingly to the velocity, and add some to work around the sprite's rotation
			projectile.rotation =
				projectile.velocity.ToRotation() +
				MathHelper.ToRadians(
					135f); // Please notice the MathHelper usage, offset the rotation by 135 degrees (to radians because rotation uses radians) because the sprite's rotation is not aligned!
		}
	}
}