using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscBows
{
	public class DeathString : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death String");
			Tooltip.SetDefault("Death's blow... I mean bow" + "\nWhile shooting two skulls will fly around bashing at enemies");
		}

		public override void SetDefaults()
		{
			item.damage = 32;
			item.ranged = true;

			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.knockBack = 2;
			item.value = 54000;
			item.rare = 3;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.width = 24;
			item.height = 62;

			item.shoot = mod.ProjectileType("DeathSkull");
			item.useAmmo = 40;
			item.shootSpeed = 8;
			item.noMelee = true;
			item.channel = true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, -0);
		}

		public float alt = 1;

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float angle = (new Vector2(speedX, speedY)).ToRotation();
			float trueSpeed = (new Vector2(speedX, speedY)).Length();
			position = player.Center;
			for (int l = 0; l < Main.projectile.Length; l++)
			{                                                                  //this make so you can only spawn one skull at the time,
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == item.shoot && proj.owner == player.whoAmI)
				{
					Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(10 * alt)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(10 * alt)) * trueSpeed, type, damage, knockBack, Main.myPlayer, 0f, 0f);
					alt *= -1;
					return true;
				}
			}
			Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(30)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(30)) * trueSpeed, mod.ProjectileType("DeathSkull"), damage, knockBack, Main.myPlayer);
			Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(-30)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(-30)) * trueSpeed, mod.ProjectileType("DeathSkull"), damage, knockBack, Main.myPlayer);

			Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(10 * alt)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(10 * alt)) * trueSpeed, type, damage, knockBack, Main.myPlayer, 0f, 0f);
			alt *= -1;
			return true;
		}

		//Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("Skull"), 0, 0, Main.myPlayer);
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);

			recipe.AddRecipeGroup("QwertysrandomContent:EvilBows");
			recipe.AddIngredient(ItemID.MoltenFury);
			recipe.AddIngredient(mod.ItemType("RhuthiniumBow"));
			recipe.AddIngredient(mod.ItemType("FossilBow"));
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class DeathSkull : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skull");
		}

		public override void SetDefaults()
		{
			projectile.width = 16; //Set the hitbox width
			projectile.height = 18;   //Set the hitbox height
			projectile.hostile = false;    //tells the game if is hostile or not.
			projectile.friendly = true;   //Tells the game whether it is friendly to players/friendly npcs or not
			projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
			projectile.aiStyle = 1;
			projectile.ranged = true;
			projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
			projectile.tileCollide = true; //Tells the game whether or not it can collide with tiles/ terrain

			projectile.timeLeft = 2000;
		}

		public bool runOnce = true;
		public float direction;
		public float speed = 1;
		public float maxSpeed = 10;
		public int timer;
		public bool foundTarget;
		public NPC target;
		public NPC confirm;
		public float maxDistance = 1000;
		public float distance;

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (runOnce)
			{
				runOnce = false;
			}

			timer++;
			if (timer > 5)
			{
				for (int k = 0; k < 200; k++)
				{
					target = Main.npc[k];
					distance = (Main.MouseWorld - target.Center).Length();
					if (distance < maxDistance && target.active && !target.dontTakeDamage && !target.friendly && target.lifeMax > 5 && !target.immortal)
					{
						confirm = Main.npc[k];
						foundTarget = true;
						direction = (confirm.Center - projectile.Center).ToRotation();
						maxDistance = (Main.MouseWorld - target.Center).Length();
					}
				}
				if (!foundTarget)
				{
					direction = (player.Center - projectile.Center).ToRotation();
				}
				projectile.velocity.X += (float)Math.Cos(direction) * speed;
				projectile.velocity.Y += (float)Math.Sin(direction) * speed;
				if (projectile.velocity.X > (float)Math.Cos(direction) * maxSpeed)
				{
					projectile.velocity.X = (float)Math.Cos(direction) * maxSpeed;
				}
				if (projectile.velocity.Y > (float)Math.Sin(direction) * maxSpeed)
				{
					projectile.velocity.Y = (float)Math.Sin(direction) * maxSpeed;
				}
				projectile.rotation = direction;
			}
			if (player.channel)
			{
				projectile.timeLeft = 2;
			}
			else
			{
				projectile.Kill();
			}

			foundTarget = false;
			maxDistance = 1000;
		}

		public override bool OnTileCollide(Vector2 velocityChange)
		{
			if (projectile.velocity.X != velocityChange.X)
			{
				projectile.velocity.X = -2 * velocityChange.X;
			}
			if (projectile.velocity.Y != velocityChange.Y)
			{
				projectile.velocity.Y = -2 * velocityChange.Y;
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -projectile.velocity.X, -projectile.velocity.Y, mod.ProjectileType("BouncyArrowP"), projectile.damage, projectile.knockBack, Main.myPlayer);
			projectile.velocity.X = -2 * projectile.velocity.X;
			projectile.velocity.Y = -2 * projectile.velocity.Y;
		}
	}
}