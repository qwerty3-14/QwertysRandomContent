using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.HydraBoss
{
	internal class HydraHead : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Head");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 72;
			npc.height = 72;

			npc.damage = 50;
			npc.defense = 18;
			npc.boss = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 60f;
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			//aiType = 10;
			animationType = -1;
			npc.noGravity = true;
			npc.dontTakeDamage = false;
			npc.noTileCollide = true;
			npc.rotation = (float)Math.PI / 2;
			npc.lifeMax = 2000;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/BeastOfThreeHeads");
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = 1000;
			npc.damage = (int)(npc.damage * .7f);
		}

		private NPC Body = null;
		private float headSpread = 3f * (float)Math.PI / 4f;
		private bool runOnce = true;
		private float rotateTo;
		private Vector2 flyTo;
		private int attackTimer = 0;
		private bool attacking = false;
		private int projDamge;
		private bool beamAttack;
		private Projectile laser;
		private int shotWarming = 60;
		private int beamTime = 300;

		public override void AI()
		{
			if (Main.expertMode)
			{
				projDamge = npc.damage / 4;
			}
			else
			{
				projDamge = npc.damage / 2;
			}
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			if (runOnce)
			{
				if (npc.ai[0] == 0)
				{
					npc.ai[0] = -1;
				}

				runOnce = false;
			}

			if (npc.ai[0] == -1)
			{
				for (int n = 0; n < 200; n++)
				{
					if (Main.npc[n].type == mod.NPCType("Hydra") && Main.npc[n].active)
					{
						npc.ai[0] = n;
						break;
					}
				}
			}

			if (npc.ai[0] != -1 || Main.npc[(int)npc.ai[0]].type != mod.NPCType("Hydra"))
			{
				Body = Main.npc[(int)npc.ai[0]];
				int headCount = 0;
				int whichHeadAmI = 0;
				for (int n = 0; n < 200; n++)
				{
					if (Main.npc[n].type == mod.NPCType("HydraHead") && Main.npc[n].active && Main.npc[n].ai[0] == npc.ai[0])
					{
						if (Main.npc[n].Center.X < npc.Center.X || (Main.npc[n].Center.X == npc.Center.X && n < npc.whoAmI))
						{
							whichHeadAmI++;
						}
						headCount++;
					}
				}

				float rotationOffset = (headSpread * (((float)whichHeadAmI + 1) / ((float)headCount + 1))) - headSpread / 2f;
				Vector2 offSet = QwertyMethods.PolarVector(400, -(float)Math.PI / 2 + rotationOffset);
				offSet.X *= 1.5f;

				flyTo = Body.Center + offSet;
				npc.velocity = (flyTo - npc.Center) * .1f;

				if (attacking && attackTimer == 0)
				{
					if (beamAttack)
					{
						laser.Kill();
						laser = null;
					}
					if (Main.netMode != 1)
					{
						if (!beamAttack)
						{
							Projectile.NewProjectile(npc.Center + QwertyMethods.PolarVector(50, npc.rotation), QwertyMethods.PolarVector(5, npc.rotation), mod.ProjectileType("HydraBreath"), projDamge, 0f, Main.myPlayer);
						}
					}
					attacking = false;
					beamAttack = false;
				}
				if (attackTimer < 60)
				{
					attackTimer++;
				}
				else
				{
					if (Main.rand.Next(20) == 0 && Main.netMode != 1)
					{
						attacking = true;
						if (Main.rand.NextFloat(10) < Math.Abs(player.velocity.X) && ((npc.Center.X > player.Center.X && player.velocity.X > 0) || (npc.Center.X < player.Center.X && player.velocity.X < 0)))
						{
							beamAttack = true;
							attackTimer = -beamTime;
							rotateTo = (float)Math.PI / 2;
						}
						else
						{
							rotateTo = (player.Center - npc.Center).ToRotation() + (Main.rand.NextFloat(1, -1) * (float)Math.PI / 8);
							attackTimer = -shotWarming;
						}
						npc.netUpdate = true;
					}
				}
				if (beamAttack)
				{
					if (attackTimer < -beamTime / 2)
					{
						float dir = npc.rotation + Main.rand.NextFloat(-1, 1) * (float)Math.PI / 4;
						Dust.NewDustPerfect(npc.Center + QwertyMethods.PolarVector(50, npc.rotation), mod.DustType("HydraBeamGlow"), QwertyMethods.PolarVector(6, dir));
					}
					else if (attackTimer == -beamTime / 2)
					{
						laser = Main.projectile[Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("HydraBeamT"), (int)(projDamge * 1.5f), 3f, Main.myPlayer, npc.whoAmI, 420)];
					}
					else
					{
						npc.velocity = Vector2.Zero;
					}
				}
				if (!attacking)
				{
					rotateTo = (player.Center - npc.Center).ToRotation();
					if (laser != null)
					{
						if (laser.active)
						{
							laser.Kill();
							laser = null;
						}
					}
				}

				npc.rotation = QwertyMethods.SlowRotation(npc.rotation, rotateTo, 4);
				if (!Body.active || Body.type != mod.NPCType("Hydra"))
				{
					npc.life = 0;
					npc.checkDead();
				}
				if (Body.dontTakeDamage)
				{
					attacking = false;
					beamAttack = false;
				}
			}
			else
			{
				npc.life = 0;
				npc.checkDead();
			}
		}

		public override void FindFrame(int frameHeight)
		{
			if (attacking)
			{
				npc.frame.Y = (int)npc.ai[1] * 2 * frameHeight + frameHeight;
			}
			else
			{
				npc.frame.Y = (int)npc.ai[1] * 2 * frameHeight;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.ai[0] != -1)
			{
				Body = Main.npc[(int)npc.ai[0]];
				int headCount = 0;
				int whichHeadAmI = 0;
				for (int n = 0; n < 200; n++)
				{
					if (Main.npc[n].type == mod.NPCType("HydraHead") && Main.npc[n].active && Main.npc[n].ai[0] == npc.ai[0])
					{
						if (n < npc.whoAmI)
						{
							whichHeadAmI++;
						}
						headCount++;
					}
				}
				if (whichHeadAmI == headCount - 1 && npc.ai[0] != -1)
				{
					Body = Main.npc[(int)npc.ai[0]];
					spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/Hydra"), Body.position - Main.screenPosition,
							Body.frame, Lighting.GetColor((int)Body.Center.X / 16, (int)Body.Center.Y / 16), Body.rotation,
							new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
					spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/Hydra_Glow"), Body.position - Main.screenPosition,
								Body.frame, Color.White, Body.rotation,
								new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
				}
				Vector2 neckOrigin = new Vector2(Body.Center.X, Body.Center.Y - 50);
				Vector2 center = npc.Center;
				Vector2 distToProj = neckOrigin - npc.Center;
				float projRotation = distToProj.ToRotation() - 1.57f;
				float distance = distToProj.Length();
				while (distance > 30f && !float.IsNaN(distance))
				{
					distToProj.Normalize();                 //get unit vector
					distToProj *= 30f;                      //speed = 30
					center += distToProj;                   //update draw position
					distToProj = neckOrigin - center;    //update distance
					distance = distToProj.Length();

					//Draw chain
					spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/HydraNeck"), center - Main.screenPosition,
						new Rectangle(0, 0, 52, 30), Lighting.GetColor((int)center.X / 16, (int)center.Y / 16), projRotation,
						new Vector2(52 * 0.5f, 30 * 0.5f), 1f, SpriteEffects.None, 0f);
				}
				spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/HydraNeckBase"), neckOrigin - Main.screenPosition,
							new Rectangle(0, 0, 52, 30), Lighting.GetColor((int)neckOrigin.X / 16, (int)neckOrigin.Y / 16), projRotation,
							new Vector2(52 * 0.5f, 30 * 0.5f), 1f, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition,
						npc.frame, drawColor, npc.rotation,
						new Vector2(72 * 0.5f, 72 * 0.5f), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/HydraHead_Glow"), npc.Center - Main.screenPosition,
						npc.frame, Color.White, npc.rotation,
						new Vector2(72 * 0.5f, 72 * 0.5f), 1f, SpriteEffects.None, 0f);
		}

		public override void BossHeadRotation(ref float rotation)
		{
			rotation = npc.rotation;
		}

		public override bool PreNPCLoot()
		{
			if (laser != null)
			{
				if (laser.active)
				{
					laser.Kill();
					laser = null;
				}
			}
			if (npc.ai[0] != -1)
			{
				Body = Main.npc[(int)npc.ai[0]];
				if (Body.life > 1)
				{
					Body.life--;
				}
				else
				{
					//Body.dontTakeDamage = true;
					Body.ai[3]++;
				}

				for (int h = 0; h < 2; h++)
				{
					if (Main.netMode != 1 && Body.active && Body.type == mod.NPCType("Hydra"))
					{
						NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("HydraHead"), ai0: Body.whoAmI, ai1: npc.ai[1]);
					}
				}
			}
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(attacking);
			writer.Write(rotateTo);
			writer.Write(beamAttack);
			writer.Write(attackTimer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			attacking = reader.ReadBoolean();
			rotateTo = reader.ReadSingle();
			beamAttack = reader.ReadBoolean();
			attackTimer = reader.ReadInt32();
		}

		public override void BossHeadSlot(ref int index)
		{
			switch ((int)npc.ai[1])
			{
				case 0:
					index = NPCHeadLoader.GetBossHeadSlot(QwertysRandomContent.HydraHead1);
					break;

				case 1:
					index = NPCHeadLoader.GetBossHeadSlot(QwertysRandomContent.HydraHead2);
					break;

				case 2:
					index = NPCHeadLoader.GetBossHeadSlot(QwertysRandomContent.HydraHead3);
					break;
			}
		}
	}
}