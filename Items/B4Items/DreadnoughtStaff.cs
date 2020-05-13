using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.B4Items       ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
	public class DreadnoughtStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rod of Command");
			Tooltip.SetDefault("Used by Ur-Quan lords to issue commands");
		}

		public override void SetDefaults()
		{
			item.damage = 60;  //The damage stat for the Weapon.
			item.mana = 20;      //this defines how many mana this weapon use
			item.width = 54;    //The size of the width of the hitbox in pixels.
			item.height = 54;     //The size of the height of the hitbox in pixels.
			item.useTime = 25;   //How fast the Weapon is used.
			item.useAnimation = 25;    //How long the Weapon is used for.
			item.useStyle = 1;  //The way your Weapon will be used, 1 is the regular sword swing for example
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 1f;  //The knockback stat of your Weapon.
			item.value = 750000;
			item.rare = 10;
			item.UseSound = SoundID.Item44;   //The sound played when using your Weapon
			item.autoReuse = true;   //Weather your Weapon will be used again after use while holding down, if false you will need to click again after use to use it again.
			item.shoot = mod.ProjectileType("Dreadnought");   //This defines what type of projectile this weapon will shot
			item.summon = true;    //This defines if it does Summon damage and if its effected by Summon increasing Armor/Accessories.
			item.buffType = mod.BuffType("UrQuan"); //The buff added to player after used the item
			item.buffTime = 3600;
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 SPos = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);   //this make so the projectile will spawn at the mouse cursor position
			position = SPos;

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

	public class Dreadnought : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ur-Quan Dreadnought");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
		}

		public override void SetDefaults()
		{
			projectile.width = 88; //Set the hitbox width
			projectile.height = 88;   //Set the hitbox height
			projectile.hostile = false;    //tells the game if is hostile or not.
			projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly npcs or not
			projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
			Main.projFrames[projectile.type] = 1;  //this is where you add how many frames u'r projectile has to make the animation
			projectile.knockBack = 10f;
			projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
			projectile.tileCollide = false; //Tells the game whether or not it can collide with tiles/ terrain
			projectile.minion = true;
			projectile.minionSlots = 1;
			projectile.timeLeft = 2;
		}

		public int varTime;
		public int Yvar = 0;
		public int YvarOld = 0;
		public int Xvar = 0;
		public int XvarOld = 0;
		public int f = 1;
		public float targetAngle = 90;
		public float s = 1;
		public float tarX;
		public float tarY;
		public bool runOnce = true;

		public float swimDirection;
		public float actDirection;
		public float maxDistance = 10000;
		public float distance;
		public NPC possiblePrey;
		public NPC prey;
		public bool foundTarget;
		public float swimSpeed = 5;
		public float distFromPlayer;
		public float shotSpeed = 12f;
		public Vector2 noTargetwander;
		private int targetwanderTimer;
		private float debugTimer;

		//public int fighterCountMax=10;
		public int fighterTimer;

		public bool close;

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			// Main.NewText(player.MinionAttackTargetNPC);
			if (runOnce)
			{
				projectile.ai[0] = 6;
				noTargetwander = new Vector2(Main.rand.Next(-200, 200), Main.rand.Next(-200, 200));
				runOnce = false;
			}
			if ((projectile.Center - player.Center).Length() > 2000)
			{
				projectile.position = player.Center;
			}
			QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
			if (modPlayer.Dreadnought)
			{
				projectile.timeLeft = 2;
			}

			//  Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 24);
			if (player.MinionAttackTargetNPC != -1)
			{
				prey = Main.npc[player.MinionAttackTargetNPC];
				foundTarget = true;
				swimDirection = (prey.Center - projectile.Center).ToRotation();
				maxDistance = (prey.Center - projectile.Center).Length();
			}
			else
			{
				for (int k = 0; k < 200; k++)
				{
					possiblePrey = Main.npc[k];
					distance = (possiblePrey.Center - projectile.Center).Length();
					distFromPlayer = (possiblePrey.Center - player.Center).Length();
					if (distance < maxDistance && possiblePrey.active && !possiblePrey.dontTakeDamage && !possiblePrey.friendly && possiblePrey.lifeMax > 5 && !possiblePrey.immortal && distFromPlayer < 800)
					{
						prey = Main.npc[k];
						foundTarget = true;

						swimDirection = (prey.Center - projectile.Center).ToRotation();
						maxDistance = (prey.Center - projectile.Center).Length();
					}
				}
			}

			if (!foundTarget)
			{
				targetwanderTimer++;
				if (targetwanderTimer % 120 == 0 && Main.netMode != 2 && Main.myPlayer == projectile.owner)
				{
					noTargetwander = new Vector2(Main.rand.Next(-200, 200), Main.rand.Next(-200, 200));
					projectile.netUpdate = true;
				}

				swimDirection = (noTargetwander + player.Center - projectile.Center).ToRotation();
			}
			else
			{
				fighterTimer++;
				if (fighterTimer % 60 == 0)
				{
					if (projectile.ai[0] > 0)
					{
						Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Ur-Quan/UrQuan-Launch").WithVolume(.8f));
						Projectile.NewProjectile(((float)Math.Cos(actDirection + (float)Math.PI) * projectile.width / 2) + projectile.Center.X, ((float)Math.Sin(actDirection + (float)Math.PI) * projectile.height / 2) + projectile.Center.Y, (float)Math.Cos(actDirection + (float)Math.PI + (float)Math.PI / 4) * shotSpeed, (float)Math.Sin(actDirection + (float)Math.PI + (float)Math.PI / 4) * shotSpeed, mod.ProjectileType("Fighter"), projectile.damage / 6, projectile.knockBack, Main.myPlayer, projectile.whoAmI, 0f);
						projectile.ai[0]--;
					}
					if (projectile.ai[0] > 0)
					{
						Projectile.NewProjectile(((float)Math.Cos(actDirection + (float)Math.PI) * projectile.width / 2) + projectile.Center.X, ((float)Math.Sin(actDirection + (float)Math.PI) * projectile.height / 2) + projectile.Center.Y, (float)Math.Cos(actDirection + (float)Math.PI - (float)Math.PI / 4) * shotSpeed, (float)Math.Sin(actDirection + (float)Math.PI - (float)Math.PI / 4) * shotSpeed, mod.ProjectileType("Fighter"), projectile.damage / 6, projectile.knockBack, Main.myPlayer, projectile.whoAmI, 0f);
						projectile.ai[0]--;
					}
				}
			}

			if (Math.Abs(actDirection - swimDirection) > Math.PI)
			{
				f = -1;
			}
			else
			{
				f = 1;
			}

			swimDirection = new Vector2((float)Math.Cos(swimDirection), (float)Math.Sin(swimDirection)).ToRotation();
			if (actDirection <= swimDirection + MathHelper.ToRadians(4) && actDirection >= swimDirection - MathHelper.ToRadians(4))
			{
				actDirection = swimDirection;
			}
			else if (actDirection <= swimDirection)
			{
				actDirection += MathHelper.ToRadians(2) * f;
			}
			else if (actDirection >= swimDirection)
			{
				actDirection -= MathHelper.ToRadians(2) * f;
			}
			actDirection = new Vector2((float)Math.Cos(actDirection), (float)Math.Sin(actDirection)).ToRotation();
			if (foundTarget)
			{
				if (maxDistance < 300)
				{
					varTime++;
					if (varTime >= 45)
					{
						Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Ur-Quan/UrQuan-Fusion").WithVolume(.8f));
						Projectile.NewProjectile(((float)Math.Cos(actDirection) * projectile.width / 2) + projectile.Center.X, ((float)Math.Sin(actDirection) * projectile.height / 2) + projectile.Center.Y, (float)Math.Cos(actDirection) * shotSpeed, (float)Math.Sin(actDirection) * shotSpeed, mod.ProjectileType("Fusion"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
						varTime = 0;
					}
					projectile.velocity.X = 0;
					projectile.velocity.Y = 0;
				}
				else
				{
					projectile.velocity.X = (float)Math.Cos(actDirection) * swimSpeed;
					projectile.velocity.Y = (float)Math.Sin(actDirection) * swimSpeed;
				}
				for (int k = 0; k < 200; k++)
				{
					if (Main.projectile[k].type == mod.ProjectileType("Dreadnought") && k != projectile.whoAmI)
					{
						if (Collision.CheckAABBvAABBCollision(projectile.position + new Vector2(projectile.width / 4, projectile.height / 4), new Vector2(projectile.width / 2, projectile.height / 2), Main.projectile[k].position + new Vector2(Main.projectile[k].width / 4, Main.projectile[k].height / 4), new Vector2(Main.projectile[k].width / 2, Main.projectile[k].height / 2)))
						{
							projectile.velocity += new Vector2((float)Math.Cos((projectile.Center - Main.projectile[k].Center).ToRotation()) * 10, (float)Math.Sin((projectile.Center - Main.projectile[k].Center).ToRotation()) * 10);
						}
					}
				}
			}
			else
			{
				projectile.velocity.X = (float)Math.Cos(actDirection) * swimSpeed;
				projectile.velocity.Y = (float)Math.Sin(actDirection) * swimSpeed;
			}

			projectile.rotation = actDirection;

			foundTarget = false;
			maxDistance = 10000f;
			close = false;
			/*
            debugTimer++;
            if(debugTimer %10==0)
            {
                CombatText.NewText(player.getRect(), new Color(38, 126, 126), (int)MathHelper.ToDegrees(swimDirection), true, false);
                CombatText.NewText(projectile.getRect(), new Color(38, 126, 126), (int)MathHelper.ToDegrees(actDirection), true, false);
            }
            */
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WritePackedVector2(noTargetwander);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			noTargetwander = reader.ReadPackedVector2();
		}
	}

	public class Fighter : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fighter");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
		}

		public override void SetDefaults()
		{
			projectile.width = 2; //Set the hitbox width
			projectile.height = 2;   //Set the hitbox height
			projectile.hostile = false;    //tells the game if is hostile or not.
			projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly npcs or not
			projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
			Main.projFrames[projectile.type] = 1;  //this is where you add how many frames u'r projectile has to make the animation
			projectile.knockBack = 10f;
			projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
			projectile.tileCollide = false; //Tells the game whether or not it can collide with tiles/ terrain
			projectile.minion = true;
			projectile.timeLeft = 900;
		}

		public int varTime;
		public int Yvar = 0;
		public int YvarOld = 0;
		public int Xvar = 0;
		public int XvarOld = 0;
		public int f = 1;
		public float targetAngle = 90;
		public float s = 1;
		public float tarX;
		public float tarY;
		public bool runOnce = true;

		public float swimDirection;
		public float actDirection;
		public float maxDistance = 10000;
		public float distance;
		public NPC possiblePrey;
		public NPC prey;
		public bool foundTarget;
		public float swimSpeed = 9;
		public float distFromPlayer;
		public float shotSpeed = 12f;
		public Vector2 noTargetwander;
		private int targetwanderTimer;
		private float debugTimer;

		//public int fighterCountMax=10;
		public int fighterTimer;

		public int fighterCount = 10;
		public bool close;
		public Projectile parent;
		public bool returnToParent;
		public bool drawLaser;

		public override void AI()
		{
			parent = Main.projectile[(int)projectile.ai[0]];
			Player player = Main.player[projectile.owner];
			if (runOnce)
			{
				projectile.rotation = projectile.velocity.ToRotation();
				runOnce = false;
			}
			QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
			if (!parent.active)
			{
				projectile.Kill();
			}
			fighterTimer++;
			if (returnToParent)
			{
				swimDirection = (parent.Center - projectile.Center).ToRotation();
				if (Collision.CheckAABBvAABBCollision(projectile.position, new Vector2(projectile.width, projectile.height), parent.position, new Vector2(parent.width, parent.height)))
				{
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Ur-Quan/UrQuan-Recover").WithVolume(.8f));
					projectile.Kill();
				}
			}
			else if (fighterTimer > 600)
			{
				returnToParent = true;
			}
			if (fighterTimer > 10)
			{
				//  Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 24);
				if (!returnToParent)
				{
					if (player.MinionAttackTargetNPC != -1)
					{
						prey = Main.npc[player.MinionAttackTargetNPC];
						foundTarget = true;
						swimDirection = (prey.Center - projectile.Center).ToRotation();
						maxDistance = (prey.Center - projectile.Center).Length();
					}
					else
					{
						for (int k = 0; k < 200; k++)
						{
							possiblePrey = Main.npc[k];
							distance = (possiblePrey.Center - projectile.Center).Length();
							distFromPlayer = (possiblePrey.Center - player.Center).Length();
							if (distance < maxDistance && possiblePrey.active && !possiblePrey.dontTakeDamage && !possiblePrey.friendly && possiblePrey.lifeMax > 5 && !possiblePrey.immortal && distFromPlayer < 800)
							{
								prey = Main.npc[k];
								foundTarget = true;

								swimDirection = (prey.Center - projectile.Center).ToRotation();
								maxDistance = (prey.Center - projectile.Center).Length();
								if (maxDistance + prey.width / 2 < 50)
								{
									close = true;
								}
								else
								{
									close = true;
								}
							}
						}
					}
				}

				if (!foundTarget)
				{
					returnToParent = true;
				}
				else
				{
				}

				swimDirection = new Vector2((float)Math.Cos(swimDirection), (float)Math.Sin(swimDirection)).ToRotation();

				actDirection = swimDirection;

				actDirection = new Vector2((float)Math.Cos(actDirection), (float)Math.Sin(actDirection)).ToRotation();
				if (foundTarget)
				{
					if (maxDistance < 50)
					{
						varTime++;
						if (varTime >= 15)
						{
							drawLaser = true;
						}
						else if (varTime >= 5)
						{
							drawLaser = false;
						}
						if (varTime >= 20)
						{
							Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Ur-Quan/UrQuan-Fighter").WithVolume(.2f));
							Projectile.NewProjectile(prey.Center, new Vector2(0, 0), mod.ProjectileType("FighterShot"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);

							varTime = 0;
						}
						projectile.velocity.X = prey.velocity.X;
						projectile.velocity.Y = prey.velocity.Y;
					}
					else
					{
						drawLaser = false;
						projectile.velocity.X = (float)Math.Cos(actDirection) * swimSpeed;
						projectile.velocity.Y = (float)Math.Sin(actDirection) * swimSpeed;
					}
					for (int k = 0; k < 200; k++)
					{
						if (Main.projectile[k].type == mod.ProjectileType("Fighter") && k != projectile.whoAmI)
						{
							if (Collision.CheckAABBvAABBCollision(projectile.position, new Vector2(projectile.width, projectile.height), Main.projectile[k].position, new Vector2(Main.projectile[k].width, Main.projectile[k].height)))
							{
								projectile.velocity += new Vector2((float)Math.Cos((projectile.Center - Main.projectile[k].Center).ToRotation()) * 3, (float)Math.Sin((projectile.Center - Main.projectile[k].Center).ToRotation()) * 3);
							}
						}
					}
				}
				else
				{
					drawLaser = false;

					projectile.velocity.X = (float)Math.Cos(actDirection) * swimSpeed;
					projectile.velocity.Y = (float)Math.Sin(actDirection) * swimSpeed;
				}

				projectile.rotation = actDirection;

				foundTarget = false;
				maxDistance = 10000f;
				/*
                debugTimer++;
                if(debugTimer %10==0)
                {
                    CombatText.NewText(player.getRect(), new Color(38, 126, 126), (int)MathHelper.ToDegrees(swimDirection), true, false);
                    CombatText.NewText(projectile.getRect(), new Color(38, 126, 126), (int)MathHelper.ToDegrees(actDirection), true, false);
                }
                */
				close = false;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (drawLaser)
			{
				Vector2 center = projectile.Center;
				Vector2 distToProj = prey.Center - center;
				float projRotation = distToProj.ToRotation() - 1.57f;
				distToProj.Normalize();                 //get unit vector
				distToProj *= 12f;                      //speed = 12
				center += distToProj;                   //update draw position
				distToProj = prey.Center - center;    //update distance
				distance = distToProj.Length();
				Color drawColor = lightColor;

				spriteBatch.Draw(mod.GetTexture("Items/Weapons/Rhuthinium/laser"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, 1, (int)distance - 10), Color.Orange, projRotation,
					new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			parent = Main.projectile[(int)projectile.ai[0]];
			parent.ai[0]++;
		}
	}

	public class Fusion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fusion Blast");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.minion = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 30;
			projectile.tileCollide = false;
		}

		public override void AI()
		{
		}
	}

	public class FighterShot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fighter Shot");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.minion = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 2;

			projectile.tileCollide = false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void AI()
		{
		}
	}
}