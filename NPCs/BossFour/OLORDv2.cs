using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.BossFour
{
	[AutoloadBossHead]
	public class OLORDv2 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oversized Laser-emitting Obliteration Radiation-emitting Destroyer");
			Main.npcFrameCount[npc.type] = 2;
		}

		public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicOLORD ? base.Texture + "_Old" : base.Texture;

		public override void SetDefaults()
		{
			npc.width = 320;
			npc.height = 60;
			npc.damage = 70;
			npc.defense = 50;
			npc.boss = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 1;
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;

			animationType = -1;
			npc.noGravity = true;
			npc.dontTakeDamage = true;
			npc.noTileCollide = true;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/EnergisedPlanetaryIncinerationClimax");
			npc.netAlways = true;

			npc.scale = 1f;
			npc.lifeMax = 100000;
			bossBag = mod.ItemType("B4Bag");
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Vector2 pos = npc.Center + new Vector2(-591, -612);
				Gore gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/OGore1"), 1f)];
				pos = pos = npc.Center + new Vector2(-591, 612);
				gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/OGore2"), 1f)];
				pos = pos = npc.Center + new Vector2(0, -30); ;
				gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/OGore3"), 1f)];
			}
		}

		private int frame = 0;
		private const int guideWidth = 750;
		private const int turretVerticalShift = -30;
		private Vector2[] turret = new Vector2[4] { new Vector2(0, 2), new Vector2(0, 2), new Vector2(0, 2), new Vector2(0, 2) }; //Y is the frame, X is the angle
		private Vector2[] turretPos = new Vector2[4] { new Vector2(-2 * (guideWidth / 3), turretVerticalShift), new Vector2(-1 * (guideWidth / 3), turretVerticalShift), new Vector2(1 * (guideWidth / 3), turretVerticalShift), new Vector2(2 * (guideWidth / 3), turretVerticalShift) };
		private int shotDamage = 35;
		private int quitCount;
		private bool playerDied = false;
		public Projectile[] wall = new Projectile[2];
		private bool activeWalls = false;
		private bool[] shootLaser = new bool[] { false, false, false, false };
		private Projectile[] tLaser = new Projectile[4];
		private Projectile superLaser;
		private bool activeSuperLaser = false;

		/// ///////////
		private int timer;

		private int attack = 0;
		private bool runOnce = true;
		private Vector2 GoTo;
		private float laserDistanceFromCenter = 800;
		private bool didHalfDeadSequence = false;
		private bool halfDeadSqequence = false;

		public override void AI()
		{
			Player player = Main.player[npc.target];
			if (Main.expertMode)
			{
				shotDamage = (int)(npc.damage / 4 * 1.6f);
				if (npc.life < (int)(npc.lifeMax * .3f))
				{
					npc.ai[3] = 1;
					/*
                    if(!didHalfDeadSequence)
                    {
                        string key = "O.L.O.R.D. is frusturated (attack speed doubled!)";
                        Main.PlaySound(SoundID.Roar, player.position, 0);
                        Color messageColor = Color.Orange;
                        if (Main.netMode == 2) // Server
                        {
                            NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText(Language.GetTextValue(key), messageColor);
                        }
                        didHalfDeadSequence = true;
                    }
                    */
				}
				else
				{
					npc.ai[3] = 1;
				}
			}
			else
			{
				shotDamage = npc.damage / 2;
				npc.ai[3] = 1;
			}

			#region
			//////////////////
			npc.width = (int)(320 * npc.scale);
			npc.height = (int)(60 * npc.scale);
			// npc.hide = !(npc.scale == 1f);
			//npc.behindTiles = npc.hide;
			if (runOnce)
			{
				if (Main.netMode != 1)
				{
					for (int t = 0; t < tLaser.Length; t++)
					{
						tLaser[t] = Main.projectile[Projectile.NewProjectile((npc.Center + turretPos[t] * npc.scale), QwertyMethods.PolarVector(14f, turret[t].X + (float)Math.PI / 2), mod.ProjectileType("TurretLaser2"), (int)(1.5f * shotDamage), 3f, Main.myPlayer, (npc.Center + turretPos[t] * npc.scale).X, (npc.Center + turretPos[t] * npc.scale).Y)];
					}

					wall[0] = Main.projectile[Projectile.NewProjectile(npc.Center.X + laserDistanceFromCenter, npc.position.Y, 0, 14f, mod.ProjectileType("SideLaser"), (int)(shotDamage * 1.5f), 3f, Main.myPlayer, npc.whoAmI, laserDistanceFromCenter)];
					wall[1] = Main.projectile[Projectile.NewProjectile(npc.Center.X + laserDistanceFromCenter, npc.position.Y, 0, 14f, mod.ProjectileType("SideLaser"), (int)(shotDamage * 1.5f), 3f, Main.myPlayer, npc.whoAmI, -laserDistanceFromCenter)];
					superLaser = Main.projectile[Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 14f, mod.ProjectileType("SuperLaser2"), 5 * shotDamage, 3f, Main.myPlayer, npc.whoAmI, npc.rotation + (float)Math.PI / 2)];
				}
				if (Main.netMode != 1)
				{
					attack = Main.rand.Next(4);
					npc.netUpdate = true;
				}
				runOnce = false;
			}
			if (Main.netMode != 1)
			{
				if (superLaser.type != mod.ProjectileType("SuperLaser2"))
				{
					superLaser = Main.projectile[Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 14f, mod.ProjectileType("SuperLaser2"), 5 * shotDamage, 3f, Main.myPlayer, npc.whoAmI, npc.rotation + (float)Math.PI / 2)];
				}

				//superLaser.timeLeft = 10;
				for (int t = 0; t < tLaser.Length; t++)
				{
					if (tLaser[t].type != mod.ProjectileType("TurretLaser2"))
					{
						tLaser[t] = Main.projectile[Projectile.NewProjectile((npc.Center + turretPos[t] * npc.scale), QwertyMethods.PolarVector(14f, turret[t].X + (float)Math.PI / 2), mod.ProjectileType("TurretLaser2"), (int)(1.5f * shotDamage), 3f, Main.myPlayer, (npc.Center + turretPos[t] * npc.scale).X, (npc.Center + turretPos[t] * npc.scale).Y)];
					}
					tLaser[t].rotation = turret[t].X;
					tLaser[t].Center = (npc.Center + turretPos[t] * npc.scale);
					tLaser[t].ai[0] = (npc.Center + turretPos[t] * npc.scale).X;
					tLaser[t].ai[1] = (npc.Center + turretPos[t] * npc.scale).Y;
					tLaser[t].timeLeft = 10;

					if (!shootLaser[t])
					{
						tLaser[t].localAI[0] = 0;
					}
					else
					{
						tLaser[t].localAI[0] += (npc.ai[3] - 1);
					}
					tLaser[t].netUpdate = true;
				}
				if (!activeWalls)
				{
					for (int w = 0; w < 2; w++)
					{
						if (wall[w].type != mod.ProjectileType("SideLaser") && Main.netMode != 1)
						{
							wall[w] = Main.projectile[Projectile.NewProjectile(npc.Center.X + laserDistanceFromCenter, npc.position.Y, 0, 14f, mod.ProjectileType("SideLaser"), (int)(shotDamage * 1.5f), 3f, Main.myPlayer, npc.whoAmI, laserDistanceFromCenter * w == 1 ? -1 : 1)];
						}
						wall[w].localAI[0] = 0;
						wall[w].netUpdate = true;
					}
				}
				if (!activeSuperLaser)
				{
					superLaser.localAI[0] = 0;
					superLaser.netUpdate = true;
				}
				else
				{
					superLaser.localAI[0] += (npc.ai[3] - 1);
				}
			}
			for (int q = 0; q < (int)npc.ai[3]; q++)
			{
				activeSuperLaser = false;
				for (int t = 0; t < turret.Length; t++)
				{
					turret[t].Y = 2;
					shootLaser[t] = false;
				}

				if (frame == 0)
				{
					npc.dontTakeDamage = true;
				}
				else
				{
					npc.dontTakeDamage = false;
				}
				player = Main.player[npc.target];
				npc.TargetClosest(true);
				if (!player.active || player.dead)
				{
					quitCount++;
					if (quitCount >= 120)
					{
						npc.position.Y += 100000f;
						playerDied = true;
					}
				}
				else
				{
					quitCount = 0;
				}

				//////////////////
				#endregion

				timer++;
				frame = 0;
				int startAttacks = 420;

				if ((Math.Abs(player.Center.X - npc.Center.X) > guideWidth || player.Center.Y < npc.Center.Y + 50))
				{
					timer = 0;
					if (Main.netMode != 1)
					{
						npc.netUpdate = true;
					}
				}
				if (timer > 300)
				{
					player.GetModPlayer<OLORDScreenLock>().screenLock = npc.whoAmI;

					activeWalls = true;
					npc.velocity = Vector2.Zero;
					if (player.Center.Y - npc.Center.Y > 1000)
					{
						npc.velocity.Y += 3;
					}
				}
				else
				{
					activeWalls = false;
					player.AddBuff(mod.BuffType("HealingHalt"), 60);
					GoTo = new Vector2(player.Center.X, player.Center.Y - 350);
					npc.velocity = (GoTo - npc.Center) * .1f;
					if (npc.velocity.Length() > 30)
					{
						npc.velocity = npc.velocity.SafeNormalize(-Vector2.UnitY) * 30;
					}
				}

				int attackDuration = 1500;
				if (timer > startAttacks)
				{
					int startShooting = 60;

					switch (attack)
					{
						case 0:
							#region pew pew laser pew pew attack

							int[] laserSwitch = new int[] { 300, 600, 900 };
							if (timer > attackDuration + startAttacks)
							{
								timer = startAttacks;
								if (Main.netMode != 1)
								{
									attack = Main.rand.Next(1, 4);
									npc.netUpdate = true;
								}
							}
							else if (timer > startAttacks + laserSwitch[2] + 60)
							{
								for (int t = 0; t < turret.Length; t++)
								{
									turret[t].Y = 1;
									shootLaser[t] = true;
								}
								frame = 1;
								if (timer % 30 == 0)
								{
									if (Main.netMode != 1)
									{
										if (timer % 60 == 0)
										{
											for (int p = -5; p < 7; p += 2)
											{
												Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y + npc.height / 2), QwertyMethods.PolarVector(3 * npc.ai[3], p * (float)Math.PI / 12 + (float)Math.PI / 2), mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
											}
										}
										else
										{
											for (int p = -3; p < 4; p++)
											{
												Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y + npc.height / 2), QwertyMethods.PolarVector(3 * npc.ai[3], p * (float)Math.PI / 6 + (float)Math.PI / 2), mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
											}
										}
									}
								}
							}
							else if (timer > startShooting + startAttacks)
							{
								if (timer > startAttacks + laserSwitch[2])
								{
									shootLaser[0] = true;
									shootLaser[3] = true;
								}
								if (timer > startAttacks + laserSwitch[1])
								{
									shootLaser[2] = true;
								}
								if (timer > startAttacks + laserSwitch[0])
								{
									shootLaser[1] = true;
								}
								for (int t = 0; t < turret.Length; t++)
								{
									if (shootLaser[t])
									{
										turret[t].Y = 1;
									}
									else
									{
										turret[t].X = QwertyMethods.SlowRotation(turret[t].X, (player.Center - (npc.Center + turretPos[t] * npc.scale)).ToRotation() - (float)Math.PI / 2, 4);
										turret[t].Y = 0;
										if (timer % 120 == 0)
										{
											if (Main.netMode != 1)
											{
												for (int r = -1; r < 2; r++)
												{
													Projectile p = Main.projectile[Projectile.NewProjectile((npc.Center + turretPos[t] * npc.scale), QwertyMethods.PolarVector(3 * npc.ai[3], turret[t].X + (float)Math.PI / 2 + r * (float)Math.PI / 8), mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer)];
													p.scale = npc.scale;
												}
												Vector2 center = (npc.Center + turretPos[t] * npc.scale);
												for (int i = 0; i < 30; i++)
												{
													float theta = turret[t].X + (float)Math.PI / 2 + Main.rand.NextFloat(-(float)Math.PI / 3, (float)Math.PI / 3);
													float dist = Main.rand.NextFloat(60f, 100f);
													if (Main.netMode != 1)
													{
														Dust.NewDustPerfect(center, mod.DustType("B4PDust"), QwertyMethods.PolarVector(dist / 10, theta));
													}
												}
											}
										}
									}
								}
							}
							else if (timer > startShooting / 2 + startAttacks)
							{
								for (int t = 0; t < turret.Length; t++)
								{
									turret[t].Y = 1;
								}
							}

							#endregion
							break;

						case 1:
							#region Super Laser
							if (timer > attackDuration + startAttacks)
							{
								timer = startAttacks;
								if (Main.netMode != 1)
								{
									attack = Main.rand.Next(3);
									if (attack >= 1)
									{
										attack++;
									}
									npc.netUpdate = true;
								}
							}
							else if (timer > startShooting + startAttacks)
							{
								float laserProgress = 1f - (((float)timer - (float)startAttacks) / 960f);
								//Main.NewText(laserProgress);
								if (timer < startAttacks + 960 + 120)
								{
									shootLaser[1] = true;
									shootLaser[2] = true;
								}
								if (timer > startAttacks + 960)
								{
									activeSuperLaser = true;
									laserProgress = 0f;
									frame = 1;
								}
								for (int t = 0; t < turret.Length; t++)
								{
									if (shootLaser[t])
									{
										turret[t].Y = 1;
										if (t == 1)
										{
											turret[t].X = QwertyMethods.SlowRotation(turret[t].X, (float)Math.PI / 2 * laserProgress, 4);
										}
										else if (t == 2)
										{
											turret[t].X = QwertyMethods.SlowRotation(turret[t].X, -(float)Math.PI / 2 * laserProgress, 4);
										}
									}
									else
									{
										turret[t].X = QwertyMethods.SlowRotation(turret[t].X, (player.Center - (npc.Center + turretPos[t] * npc.scale)).ToRotation() - (float)Math.PI / 2, 4);
										turret[t].Y = 1;
										if (timer % 120 < 60 && timer % 10 == 0)
										{
											if (Main.netMode != 1)
											{
												Projectile p = Main.projectile[Projectile.NewProjectile((npc.Center + turretPos[t] * npc.scale), QwertyMethods.PolarVector(3 * npc.ai[3], turret[t].X + (float)Math.PI / 2), mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer)];
												p.scale = npc.scale;

												Vector2 center = (npc.Center + turretPos[t] * npc.scale);
												for (int i = 0; i < 10; i++)
												{
													float theta = turret[t].X + (float)Math.PI / 2 + Main.rand.NextFloat(-(float)Math.PI / 4, (float)Math.PI / 4);
													float dist = Main.rand.NextFloat(60f, 100f);
													if (Main.netMode != 1)
													{
														Dust.NewDustPerfect(center, mod.DustType("B4PDust"), QwertyMethods.PolarVector(dist / 10, theta));
													}
												}
											}
										}
									}
								}
							}
							else if (timer > startShooting / 2 + startAttacks)
							{
								for (int t = 0; t < turret.Length; t++)
								{
									turret[t].Y = 1;
								}
							}

							#endregion
							break;

						case 2:
							#region gravity attack
							if (timer > attackDuration + startAttacks)
							{
								timer = startAttacks;
								if (Main.netMode != 1)
								{
									attack = Main.rand.Next(3);
									if (attack >= 2)
									{
										attack++;
									}
									npc.netUpdate = true;
								}
							}
							else if (timer > startShooting + startAttacks)
							{
								if (timer > startAttacks + 960)
								{
									frame = 1;
								}
								if (timer == attackDuration + startAttacks - 480 && Main.netMode != 1)
								{
									Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 14f, mod.ProjectileType("BlackHoleSeed"), (int)(2.5f * shotDamage), 3f, Main.myPlayer, npc.ai[3]);
								}
								for (int t = 0; t < turret.Length; t++)
								{
									turret[t].Y = 0;
									turret[t].X = QwertyMethods.SlowRotation(turret[t].X, 0, 4);
									if (timer % 120 == t * 30)
									{
										if (Main.netMode != 1)
										{
											Projectile.NewProjectile((npc.Center + turretPos[t] * npc.scale), QwertyMethods.PolarVector(3 * npc.ai[3], turret[t].X + (float)Math.PI / 2), mod.ProjectileType("TurretGrav"), shotDamage, 0, Main.myPlayer);
										}
										Vector2 center = (npc.Center + turretPos[t] * npc.scale);
										for (int i = 0; i < 30; i++)
										{
											float theta = turret[t].X + (float)Math.PI / 2 + Main.rand.NextFloat(-(float)Math.PI / 3, (float)Math.PI / 3);
											float dist = Main.rand.NextFloat(60f, 100f);
											if (Main.netMode != 1)
											{
												Dust.NewDustPerfect(center, mod.DustType("B4PDust"), QwertyMethods.PolarVector(dist / 10, theta));
											}
										}
									}
								}
							}
							else if (timer > startShooting / 2 + startAttacks)
							{
								for (int t = 0; t < turret.Length; t++)
								{
									turret[t].Y = 1;
								}
							}
							#endregion
							break;

						case 3:
							#region Large bursts
							if (timer > attackDuration + startAttacks)
							{
								timer = startAttacks;
								if (Main.netMode != 1)
								{
									attack = Main.rand.Next(3);
									npc.netUpdate = true;
								}
							}
							else if (timer > startAttacks + 960)
							{
								frame = 1;
								if (timer == startAttacks + 1320 && Main.netMode != 1)
								{
									Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 7f * npc.ai[3], mod.ProjectileType("MegaBurst"), shotDamage, 3f, Main.myPlayer, npc.ai[3]);
								}
								for (int t = 0; t < turret.Length; t++)
								{
									if (timer % 30 == 0 && turret[t].Y < 2)
									{
										turret[t].Y++;
									}
								}
							}
							else if (timer > startShooting + startAttacks)
							{
								for (int t = 0; t < turret.Length; t++)
								{
									if (t == 0 || t == 3)
									{
										turret[t].Y = 0;
									}
									else
									{
										turret[t].Y = 1;
									}
									turret[t].X = QwertyMethods.SlowRotation(turret[t].X, (player.Center - (npc.Center + turretPos[t] * npc.scale)).ToRotation() - (float)Math.PI / 2, 4);
									if (timer % 90 == 0)
									{
										if (Main.netMode != 1)
										{
											if ((timer % 180 == 0 && t == 0) || (timer % 180 != 0 && t == 3))
											{
												Projectile p = Main.projectile[Projectile.NewProjectile((npc.Center + turretPos[t] * npc.scale), QwertyMethods.PolarVector(4.5f * npc.ai[3], turret[t].X + (float)Math.PI / 2), mod.ProjectileType("BurstShot"), shotDamage, 0, Main.myPlayer)];
												p.scale = npc.scale;

												Vector2 center = (npc.Center + turretPos[t] * npc.scale);
												for (int i = 0; i < 30; i++)
												{
													float theta = turret[t].X + (float)Math.PI / 2 + Main.rand.NextFloat(-(float)Math.PI / 3, (float)Math.PI / 3);
													float dist = Main.rand.NextFloat(60f, 100f);
													if (Main.netMode != 1)
													{
														Dust.NewDustPerfect(center, mod.DustType("B4PDust"), QwertyMethods.PolarVector(dist / 10, theta));
													}
												}
											}
											if (((timer % 180 == 0 && t == 2) || (timer % 180 != 0 && t == 1)))
											{
												Projectile p = Main.projectile[Projectile.NewProjectile((npc.Center + turretPos[t] * npc.scale), QwertyMethods.PolarVector(9, turret[t].X + (float)Math.PI / 2), mod.ProjectileType("MagicMineLayer"), shotDamage, 0, Main.myPlayer, npc.whoAmI, (player.Center - npc.Center).Length())];
												p.scale = npc.scale;

												Vector2 center = (npc.Center + turretPos[t] * npc.scale);
												for (int i = 0; i < 10; i++)
												{
													float theta = turret[t].X + (float)Math.PI / 2 + Main.rand.NextFloat(-(float)Math.PI / 4, (float)Math.PI / 4);
													float dist = Main.rand.NextFloat(60f, 100f);
													if (Main.netMode != 1)
													{
														Dust.NewDustPerfect(center, mod.DustType("B4PDust"), QwertyMethods.PolarVector(dist / 10, theta));
													}
												}
											}
										}
									}
								}
							}
							else if (timer > startShooting / 2 + startAttacks)
							{
								for (int t = 0; t < turret.Length; t++)
								{
									turret[t].Y = 1;
								}
							}
							#endregion
							break;
					}

					/*
                    if (Main.netMode == 1)
                    {
                        Main.NewText("client: " + attack);
                    }

                    if (Main.netMode == 2) // Server
                    {
                        NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + attack), Color.Black);
                    }*/
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (activeWalls)
			{
				Texture2D Walls = mod.GetTexture("NPCs/BossFour/SideLaser" + (ModContent.GetInstance<SpriteSettings>().ClassicOLORD ? "_Old" : ""));
				for (int h = 0; h < 20; h++)
				{
					spriteBatch.Draw(Walls, new Vector2(npc.Center.X + laserDistanceFromCenter, npc.position.Y + 50 + h * 188) - Main.screenPosition,
								  new Rectangle(316, 0, 316, 188), drawColor, npc.rotation,
								   new Vector2(316 * .5f, 188 * .5f), npc.scale, SpriteEffects.None, 0f);
					spriteBatch.Draw(Walls, new Vector2(npc.Center.X - laserDistanceFromCenter, npc.position.Y + 50 + h * 188) - Main.screenPosition,
								  new Rectangle(0, 0, 316, 188), drawColor, npc.rotation,
								   new Vector2(316 * .5f, 188 * .5f), npc.scale, SpriteEffects.None, 0f);
				}
			}
			Texture2D BK = mod.GetTexture("NPCs/BossFour/BackGround" + (ModContent.GetInstance<SpriteSettings>().ClassicOLORD ? "_Old" : ""));
			float backgroundOffset = 100f; //70 for old
			spriteBatch.Draw(BK, new Vector2(npc.Center.X, npc.position.Y - (backgroundOffset * npc.scale)) - Main.screenPosition,
						  BK.Frame(), drawColor, npc.rotation,
						   BK.Size() / 2, npc.scale, SpriteEffects.None, 0f);

			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			for (int t = 0; t < turret.Length; t++)
			{
				spriteBatch.Draw(mod.GetTexture("NPCs/BossFour/Turret" + (ModContent.GetInstance<SpriteSettings>().ClassicOLORD ? "_Old" : "")), npc.Center + turretPos[t] * npc.scale - Main.screenPosition,
						   new Rectangle(0, (int)turret[t].Y * 78, 142, 78), drawColor, turret[t].X,
						   new Vector2(142 * 0.5f, 78 * 0.5f), npc.scale, SpriteEffects.None, 0f);
			}
			/*
            int num33 = (int)((npc.position.X - 8f) / 16f);
            int num34 = (int)((npc.position.X + (float)npc.width + 8f) / 16f);
            int num35 = (int)((npc.position.Y - 8f) / 16f);
            int num36 = (int)((npc.position.Y + (float)npc.height + 8f) / 16f);
            for (int l = num33; l <= num34; l++)
            {
                for (int m = num35; m <= num36; m++)
                {
                    if (Lighting.Brightness(l, m) == 0f)
                    {
                        color9 = Microsoft.Xna.Framework.Color.Black;
                    }
                }
            }*/
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(timer);
			writer.Write(attack);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			timer = reader.ReadInt32();
			attack = reader.ReadInt32();
		}

		/// ////////////////////////////////////////////////////////

		public override void NPCLoot()
		{
			QwertyWorld.downedB4 = true;
			if (Main.netMode == NetmodeID.Server)
				NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.

			int centerX = (int)(npc.position.X + npc.width / 2) / 16;
			int centerY = (int)(npc.position.Y + npc.height / 2) / 16;
			int halfLength = npc.width / 2 / 16 + 1;

			int trophyChance = Main.rand.Next(0, 10);

			if (Main.expertMode)
			{
				npc.DropBossBags();
			}
			else
			{
				int selectWeapon = Main.rand.Next(1, 7);

				if (selectWeapon == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("B4Bow"));
				}
				if (selectWeapon == 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("B4GiantBow"));
				}
				if (selectWeapon == 3)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DreadnoughtStaff"));
				}
				if (selectWeapon == 4)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BlackHoleStaff"));
				}
				if (selectWeapon == 5)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Jabber"));
				}
				if (selectWeapon == 6)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ExplosivePierce"));
				}
				if (Main.rand.Next(100) < 15)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TheDevourer"));
				}

				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 73, 60);
			}
			if (trophyChance == 1)
			{
				//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraTrophy"));
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(120000 * bossLifeScale);
			npc.damage = 70;
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}

		public override bool CheckActive()
		{
			return playerDied;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0f;
		}
	}

	// The following laser shows a channeled ability, after charging up the laser will be fired
	// Using custom drawing, dust effects, and custom collision checks for tiles
	public class TurretLaser2 : ModProjectile
	{
		// The maximum charge value
		private const float MaxChargeValue = 120f;

		//The distance charge particle from the player center
		private const float MoveDistance = 63f;

		// The actual distance is stored in the ai0 field
		// By making a property to handle this it makes our life easier, and the accessibility more readable
		public float Distance;

		// The actual charge value is stored in the localAI0 field
		public float Charge
		{
			get { return projectile.localAI[0]; }
			set { projectile.localAI[0] = value; }
		}

		//public NPC shooter;
		// Are we at max charge? With c#6 you can simply use => which indicates this is a get only property
		public bool AtMaxCharge;

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.hide = false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.localAI[0]);
			writer.Write(projectile.rotation);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.localAI[0] = reader.ReadSingle();
			projectile.rotation = reader.ReadSingle();
		}

		// The AI of the projectile

		public override void AI()
		{
			//Main.NewText(projectile.whoAmI +", "+Distance);
			//shooter = Main.npc[(int)projectile.ai[0]];
			Vector2 mousePos = Main.MouseWorld;
			Player player = Main.player[projectile.owner];
			/*
            if (Main.netMode == 1)
            {
                Main.NewText("client: " + projectile.whoAmI + ", "+ projectile.Center);
            }

            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + projectile.whoAmI + ", " + projectile.Center), Color.Black);
            }
            */
			#region Set projectile position

			//Vector2 diff = new Vector2((float)Math.Cos(shooter.rotation + (float)Math.PI / 2) * 14f, (float)Math.Sin(shooter.rotation + (float)Math.PI / 2) * 14f);

			Vector2 diff = new Vector2((float)Math.Cos(projectile.rotation + (float)Math.PI / 2) * 14f, (float)Math.Sin(projectile.rotation + (float)Math.PI / 2) * 14f);
			diff.Normalize();
			projectile.velocity = diff;
			//projectile.direction = projectile.Center.X > shooter.Center.X ? 1 : -1;
			projectile.netUpdate = true;

			//projectile.position = new Vector2(shooter.Center.X, shooter.Center.Y) + projectile.velocity * MoveDistance;
			projectile.Center += projectile.velocity * MoveDistance;

			int dir = projectile.direction;

			#endregion

			#region Charging process
			// Kill the projectile if the player stops channeling

			// Do we still have enough mana? If not, we kill the projectile because we cannot use it anymore

			Vector2 offset = projectile.velocity;
			offset *= MoveDistance - 20;
			//Vector2 pos = new Vector2(shooter.Center.X, shooter.Center.Y) + offset - new Vector2(10, 10);
			Vector2 pos = new Vector2(projectile.ai[0], projectile.ai[1]) + offset - new Vector2(10, 10);
			if (Charge < MaxChargeValue)
			{
				Charge++;
				Distance = 0;
				AtMaxCharge = false;
			}
			else
			{
				AtMaxCharge = true;
			}

			int chargeFact = (int)(Charge / 20f);

			#endregion
			if (Charge > 10 && !AtMaxCharge)
			{
				Vector2 center = projectile.Center + QwertyMethods.PolarVector(-60, projectile.rotation + (float)Math.PI / 2);
				for (int i = 0; i < 6; i++)
				{
					float theta = projectile.rotation + (float)Math.PI / 2 + Main.rand.NextFloat(-(float)Math.PI / 4, (float)Math.PI / 4);
					float dist = Main.rand.NextFloat(30f, 60f);
					if (Main.netMode != 1)
					{
						Dust.NewDustPerfect(center + QwertyMethods.PolarVector(dist, theta), mod.DustType("TurretLaserDust"), QwertyMethods.PolarVector(-dist / 10, theta));
					}
				}
			}

			if (Charge < MaxChargeValue) return;
			//Vector2 start = new Vector2(shooter.Center.X, shooter.Center.Y);
			Vector2 start = new Vector2(projectile.ai[0], projectile.ai[1]);
			Vector2 unit = projectile.velocity;
			unit *= -1;
			for (Distance = MoveDistance; Distance <= 2200f; Distance += 5f)
			{
				//start = new Vector2(shooter.Center.X, shooter.Center.Y) + projectile.velocity * Distance;
				start = new Vector2(projectile.ai[0], projectile.ai[1]) + projectile.velocity * Distance;
			}

			//Add lights
			DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
			Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (Distance - MoveDistance), 26,
				DelegateMethods.CastLight);
		}

		public int colorCounter;
		public Color lineColor;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (AtMaxCharge)
			{
				DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], new Vector2(projectile.ai[0], projectile.ai[1]),
					projectile.velocity, 48f, projectile.damage, -1.57f, 1f, 4000f, Color.White, (int)MoveDistance);
			}
			else
			{
				Vector2 center = projectile.Center + QwertyMethods.PolarVector(-60, projectile.rotation + (float)Math.PI / 2);

				//float projRotation = shooter.rotation;
				float projRotation = projectile.rotation;
				//update draw position

				float lineLength = 4000f;
				Color drawColor = lightColor;

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
				if (Charge > 10)
				{
					spriteBatch.Draw(mod.GetTexture("Items/Weapons/Rhuthinium/laser"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
						new Rectangle(0, 0, 1, (int)lineLength - 10), lineColor, projRotation,
						new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}

		// The core function of drawing a laser
		private int frame = 0;

		public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 4000f, Color color = default(Color), int transDist = 50)
		{
			projectile.frameCounter++;
			if (projectile.frameCounter % 4 == 0)
			{
				frame++;
				if (frame > 3)
				{
					frame = 0;
				}
			}
			Vector2 origin = start;
			float r = unit.ToRotation() + rotation;

			#region Draw laser body
			for (float i = transDist; i <= Distance; i += step)
			{
				Color c = Color.White;
				origin = start + i * unit;
				spriteBatch.Draw(texture, origin - Main.screenPosition,
					new Rectangle(frame * 50, 50, 50, 48), i < transDist ? Color.Transparent : c, r,
					new Vector2(50 * .5f, 48 * .5f), scale, 0, 0);
			}
			#endregion

			#region Draw laser tail
			spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
				new Rectangle(frame * 50, 0, 50, 48), Color.White, r, new Vector2(50 * .5f, 48 * .5f), scale, 0, 0);
			#endregion
		}

		// Change the way of collision check of the projectile
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// We can only collide if we are at max charge, which is when the laser is actually fired

			Player player = Main.player[projectile.owner];
			Vector2 unit = projectile.velocity;
			float point = 0f;
			// Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
			// It will look for collisions on the given line using AABB
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), new Vector2(projectile.ai[0], projectile.ai[1]),
				new Vector2(projectile.ai[0], projectile.ai[1]) + unit * Distance, 50, ref point);

			return false;
		}

		// Set custom immunity time on hitting an NPC
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 5;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 unit = projectile.velocity;
			Utils.PlotTileLine(projectile.Center, projectile.Center + unit * Distance, (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
		}
	}

	public class SuperLaser2 : ModProjectile
	{
		// The maximum charge value
		private const float MaxChargeValue = 270f;

		//The distance charge particle from the player center
		private const float MoveDistance = 80f;

		// The actual distance is stored in the ai0 field
		// By making a property to handle this it makes our life easier, and the accessibility more readable
		public float Distance;

		// The actual charge value is stored in the localAI0 field
		public float Charge
		{
			get { return projectile.localAI[0]; }
			set { projectile.localAI[0] = value; }
		}

		public NPC shooter;

		// Are we at max charge? With c#6 you can simply use => which indicates this is a get only property
		public bool AtMaxCharge;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Laser");
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.hide = false;
			projectile.timeLeft = 2;
		}

		// The AI of the projectile
		public float downFromCenter = 130;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.localAI[0]);
			writer.Write(projectile.rotation);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.localAI[0] = reader.ReadSingle();
			projectile.rotation = reader.ReadSingle();
		}

		public override void AI()
		{
			shooter = Main.npc[(int)projectile.ai[0]];
			Vector2 mousePos = Main.MouseWorld;
			Player player = Main.player[projectile.owner];
			if (!shooter.active)
			{
				projectile.Kill();
			}
			#region Set projectile position
			/*
            if (Main.netMode == 1)
            {
                Main.NewText("client: "  + projectile.timeLeft);
            }

            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: "  + projectile.timeLeft), Color.Black);
            }
            */
			Vector2 diff = new Vector2(0, 14);
			diff.Normalize();
			projectile.velocity = diff;
			projectile.direction = projectile.Center.X > shooter.Center.X ? 1 : -1;
			projectile.netUpdate = true;
			projectile.timeLeft = 2;
			projectile.Center = new Vector2(shooter.Center.X, shooter.Center.Y) + projectile.velocity * MoveDistance;

			int dir = projectile.direction;
			/*
            player.ChangeDir(dir);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
            */
			#endregion

			#region Charging process
			// Kill the projectile if the player stops channeling

			// Do we still have enough mana? If not, we kill the projectile because we cannot use it anymore

			Vector2 offset = projectile.velocity;
			offset *= MoveDistance - 20;
			Vector2 pos = new Vector2(shooter.Center.X, shooter.Center.Y) + offset - new Vector2(10, 10);

			if (Charge < MaxChargeValue)
			{
				Charge++;
				Distance = 0;
				AtMaxCharge = false;
			}
			else
			{
				AtMaxCharge = true;
			}

			int chargeFact = (int)(Charge / 20f);
			if (Charge > 10 && !AtMaxCharge)
			{
				Vector2 center = projectile.Center + QwertyMethods.PolarVector(-60, projectile.rotation + (float)Math.PI / 2);
				for (int i = 0; i < 15; i++)
				{
					float theta = projectile.rotation + (float)Math.PI / 2 + Main.rand.NextFloat(-5 * (float)Math.PI / 12, 5 * (float)Math.PI / 12);
					float dist = Main.rand.NextFloat(30f, 120f);
					if (Main.netMode != 1)
					{
						Dust.NewDustPerfect(center + QwertyMethods.PolarVector(dist, theta), mod.DustType("TurretLaserDust"), QwertyMethods.PolarVector(-dist / 10, theta));
					}
				}
			}

			#endregion

			if (Charge < MaxChargeValue) return;

			Main.LocalPlayer.GetModPlayer<OLORDScreenLock>().shake = true;
			Vector2 start = new Vector2(shooter.Center.X, shooter.Center.Y);
			Vector2 unit = projectile.velocity;
			unit *= -1;
			for (Distance = MoveDistance; Distance <= 2200f; Distance += 5f)
			{
				start = new Vector2(shooter.Center.X, shooter.Center.Y) + projectile.velocity * Distance;
				/*
                if (!Collision.CanHit(new Vector2(shooter.Center.X, shooter.Center.Y), 1, 1, start, 1, 1))
                {
                    Distance -= 5f;
                    break;
                }
                */
			}

			//Add lights
			DelegateMethods.v3_1 = new Vector3(10f, 10f, 10f);
			Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (Distance - MoveDistance), 26,
				DelegateMethods.CastLight);
		}

		public int colorCounter;
		public Color lineColor;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (AtMaxCharge)
			{
				DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], new Vector2(shooter.Center.X, shooter.Center.Y),
					projectile.velocity, 35, projectile.damage, -1.57f, 1f, 4000f, Color.White, (int)MoveDistance);
			}
			else
			{
				Vector2 center = projectile.Center;

				float projRotation = 0;
				//update draw position

				float lineLength = 4000f;
				Color drawColor = lightColor;

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
				if (Charge > 10)
				{
					spriteBatch.Draw(mod.GetTexture("Items/Weapons/Rhuthinium/laser"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, 1, (int)lineLength - 10), lineColor, projRotation,
					new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
				}
			}

			return false;
		}

		// The core function of drawing a laser
		public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 4000f, Color color = default(Color), int transDist = 50)
		{
			Vector2 origin = start;
			float r = unit.ToRotation() + rotation;

			#region Draw laser body
			for (float i = transDist; i <= Distance; i += step)
			{
				Color c = Color.White;
				origin = start + i * unit;
				origin.Y += 118;
				spriteBatch.Draw(texture, origin - Main.screenPosition,
					new Rectangle(0, 130, 458, 118), i < transDist ? Color.Transparent : c, r,
					new Vector2(458 * .5f, 118 * .5f), scale, 0, 0);
			}
			#endregion

			#region Draw laser tail
			spriteBatch.Draw(texture, start + unit * (transDist) - Main.screenPosition,
				new Rectangle(0, 0, 458, 118), Color.White, r, new Vector2(458 * .5f, 118 * .5f), scale, 0, 0);
			#endregion

			#region Draw laser head
			spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition,
				new Rectangle(0, 260, 458, 124), Color.White, r, new Vector2(458 * .5f, 124 * .5f), scale, 0, 0);
			#endregion
		}

		// Change the way of collision check of the projectile
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// We can only collide if we are at max charge, which is when the laser is actually fired

			Player player = Main.player[projectile.owner];
			Vector2 unit = projectile.velocity;
			float point = 0f;
			// Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
			// It will look for collisions on the given line using AABB
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), new Vector2(shooter.Center.X, shooter.Center.Y),
				new Vector2(shooter.Center.X, shooter.Center.Y) + unit * Distance, 472, ref point);

			return false;
		}

		// Set custom immunity time on hitting an NPC
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 5;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 unit = projectile.velocity;
			Utils.PlotTileLine(projectile.Center, projectile.Center + unit * Distance, (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
		}
	}

	public class OLORDScreenLock : ModPlayer
	{
		public int screenLock = -1;
		public bool shake = false;

		public override void ResetEffects()
		{
			screenLock = -1;
			shake = false;
		}

		public override void ModifyScreenPosition()
		{
			if (screenLock != -1 && player.active && player.statLife > 0)
			{
				NPC OLORD = Main.npc[screenLock];
				if (OLORD.active)
				{
					Main.screenPosition.X = OLORD.Center.X - Main.screenWidth / 2;
					Main.screenPosition.Y = OLORD.position.Y - 100;
				}
			}
			if (shake)
			{
				Main.screenPosition.X += Main.rand.Next(-20, 21);
				Main.screenPosition.Y += Main.rand.Next(-20, 21);
			}
		}
	}

	public class SideLaser : ModProjectile
	{
		private const float MaxChargeValue = 50f;
		private const float MoveDistance = 60f;
		public float Distance;

		// The actual charge value is stored in the localAI0 field
		public float Charge
		{
			get { return projectile.localAI[0]; }
			set { projectile.localAI[0] = value; }
		}

		public NPC shooter;
		public bool AtMaxCharge { get { return Charge == MaxChargeValue; } }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("O.L.O.R.D.'s wall");
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
		}

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			// Add this projectile to the list of projectiles that will be drawn BEFORE tiles and NPC are drawn. This makes the projectile appear to be BEHIND the tiles and NPC.
			drawCacheProjsBehindNPCs.Add(index);
		}

		// The AI of the projectile
		public float downFromCenter = 0;

		public override void AI()
		{
			shooter = Main.npc[(int)projectile.ai[0]];
			Vector2 mousePos = Main.MouseWorld;
			Player player = Main.player[projectile.owner];

			if (!shooter.active)
			{
				projectile.Kill();
			}
			#region Set projectile position

			Vector2 diff = new Vector2(0, 14);
			diff.Normalize();
			projectile.velocity = diff;
			projectile.direction = projectile.Center.X > shooter.Center.X ? 1 : -1;
			projectile.netUpdate = true;

			projectile.position = new Vector2(shooter.Center.X + projectile.ai[1], shooter.Center.Y + downFromCenter) + projectile.velocity * MoveDistance;
			projectile.timeLeft = 2;
			int dir = projectile.direction;

			#endregion

			#region Charging process

			Vector2 offset = projectile.velocity;
			offset *= MoveDistance - 20;
			Vector2 pos = new Vector2(shooter.Center.X + projectile.ai[1], shooter.Center.Y + downFromCenter) + offset - new Vector2(10, 10);

			if (Charge < MaxChargeValue)
			{
				Charge++;
				Distance = 0;
			}

			int chargeFact = (int)(Charge / 20f);

			#endregion

			if (Charge < MaxChargeValue) return;
			Vector2 start = new Vector2(shooter.Center.X + projectile.ai[1], shooter.Center.Y + downFromCenter);
			Vector2 unit = projectile.velocity;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		// The core function of drawing a laser
		public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 4000f, Color color = default(Color), int transDist = 50)
		{
			Vector2 origin = start;
			float r = unit.ToRotation() + rotation;

			#region Draw laser body
			if (projectile.ai[1] > 0)
			{
				for (float i = transDist; i <= Distance; i += step)
				{
					Color c = Color.White;
					origin = start + i * unit;
					spriteBatch.Draw(texture, origin - Main.screenPosition,
						new Rectangle(316, 0, 631, (int)step), color, r,
						new Vector2(316 * .5f, step * .5f), scale, 0, 0);
				}
			}
			else
			{
				for (float i = transDist; i <= Distance; i += step)
				{
					Color c = Color.White;
					origin = start + i * unit;
					spriteBatch.Draw(texture, origin - Main.screenPosition,
						new Rectangle(0, 0, 316, (int)step), color, r,
						new Vector2(316 * .5f, step * .5f), scale, 0, 0);
				}
			}

			#endregion
		}

		// Change the way of collision check of the projectile
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// We can only collide if we are at max charge, which is when the laser is actually fired

			Player player = Main.player[projectile.owner];
			Vector2 unit = projectile.velocity;
			float point = 0f;
			// Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
			// It will look for collisions on the given line using AABB
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), new Vector2(shooter.Center.X + projectile.ai[1], shooter.Center.Y + downFromCenter),
				new Vector2(shooter.Center.X + projectile.ai[1], shooter.Center.Y + downFromCenter) + unit * Distance, 316, ref point);

			return false;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 unit = projectile.velocity;
			Utils.PlotTileLine(projectile.Center, projectile.Center + unit * Distance, (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
		}
	}
}