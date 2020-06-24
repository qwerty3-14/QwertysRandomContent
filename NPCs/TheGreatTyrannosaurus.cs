using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs
{
    [AutoloadBossHead]
    public class TheGreatTyrannosaurus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Great Tyrannosaurus");
            Main.npcFrameCount[npc.type] = 10;
        }

        public override void SetDefaults()
        {
            npc.width = 170;
            npc.height = 148;
            npc.damage = 100;
            npc.defense = 22;
            npc.lifeMax = 28000;

            npc.boss = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 60f;
            npc.knockBackResist = 0f;
            npc.aiStyle = 3;

            aiType = 27;
            animationType = -1;
            npc.noGravity = false;
            npc.noTileCollide = false;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/OldDinosNewGuns");

            bossBag = mod.ItemType("TRexBag");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * .6f);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (QwertyWorld.DinoEvent && !NPC.AnyNPCs(mod.NPCType("Velocichopper")) && !NPC.AnyNPCs(mod.NPCType("TheGreatTyrannosaurus")))
            {
                if (QwertyWorld.DinoKillCount >= 140)
                {
                    return 50f;
                }
                return 3f;
            }
            return 0f;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override bool CheckActive()
        {
            Player player = Main.player[npc.target];
            float playerDistance = (float)Math.Sqrt((npc.Center.X - player.Center.X) * (npc.Center.X - player.Center.X) + (npc.Center.Y - player.Center.Y) * (npc.Center.Y - player.Center.Y));
            if (playerDistance > 2000f)
            {
                return true;
            }
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
            }
            for (int i = 0; i < 10; i++)
            {
                int dustType = 148;
                int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }

        private const int moveFrameType = 0;
        private const int attackFrameType = 1;
        private const int attackFrameUpType = 2;
        private const int attackFrameAngleType = 3;
        private const int launchFrameType = 4;
        public int AI_Timer = 0;
        public int Pos = 1;
        public int damage = 35;
        public int walkTime = 300;
        public int moveCount = 0;
        public int fireCount = 0;
        public int frameType = 0;
        public int climateReloadTime = 10;
        public int diseaseReloadTime = 4;
        public int climateTime = 0;
        public int diseaseTime = 0;
        public int attack;
        public int runOnce = 0;
        public int meteorTime = 0;
        public int meteorReloadTime = 30;
        public bool meteorsLaunched = false;
        public int multiplayerAttackCycle = 1;

        public override void AI()
        {
            if (runOnce == 0)
            {
                runOnce = 1;
            }
            AI_Timer++;
            if (Main.expertMode)
            {
                damage = 25;
            }

            Player player = Main.player[npc.target];
            npc.TargetClosest(true);
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0f, -100f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }

            if (AI_Timer > walkTime)
            {
                if (attack == 1)
                {
                    meteorsLaunched = false;
                    climateTime++;
                    if (climateTime > climateReloadTime)
                    {
                        Main.PlaySound(16, npc.position, 0);
                        if (Main.netMode != 1)
                        {
                            if (frameType == attackFrameType)
                            {
                                int spread = Main.rand.Next(-15, 15);
                                Projectile.NewProjectile(npc.Center.X + (55f * npc.direction), npc.Center.Y, (float)Math.Cos(MathHelper.ToRadians(spread)) * 10f * npc.direction, -10f * (float)Math.Sin(MathHelper.ToRadians(spread)), mod.ProjectileType("SnowFlake"), damage, 3f, Main.myPlayer);
                            }
                            else if (frameType == attackFrameUpType)
                            {
                                int spread = Main.rand.Next(-15, 15);
                                Projectile.NewProjectile(npc.Center.X + (30f * npc.direction), npc.Center.Y - 30f, (float)Math.Cos(Math.PI / 2 + MathHelper.ToRadians(spread)) * 10f, -10f * (float)Math.Sin(Math.PI / 2 + MathHelper.ToRadians(spread)), mod.ProjectileType("SnowFlake"), damage, 3f, Main.myPlayer);
                            }
                            else if (frameType == attackFrameAngleType)
                            {
                                int spread = Main.rand.Next(-15, 15);
                                Projectile.NewProjectile(npc.Center.X + (41f * npc.direction), npc.Center.Y - 24f, (float)Math.Cos(Math.PI / 4 + MathHelper.ToRadians(spread)) * 10f * npc.direction, -10f * (float)Math.Sin(Math.PI / 4 + MathHelper.ToRadians(spread)), mod.ProjectileType("SnowFlake"), damage, 3f, Main.myPlayer);
                            }
                        }
                        climateTime = 0;
                    }

                    float playerPositionSummery = (player.Center.X - npc.Center.X) * npc.direction + (player.Center.Y - npc.Center.Y);
                    if (playerPositionSummery > 200f)
                    {
                        frameType = attackFrameType;
                    }
                    else if (playerPositionSummery < -200f)
                    {
                        frameType = attackFrameUpType;
                    }
                    else
                    {
                        frameType = attackFrameAngleType;
                    }

                    npc.velocity.X = (0);
                    npc.velocity.Y = (0);
                    if (AI_Timer > walkTime * 2)
                    {
                        multiplayerAttackCycle = 2;
                        AI_Timer = 0;
                    }
                }

                if (attack == 2)
                {
                    meteorsLaunched = false;
                    diseaseTime++;
                    if (diseaseTime > diseaseReloadTime)
                    {
                        Main.PlaySound(16, npc.position, 0);
                        if (Main.netMode != 1)
                        {
                            if (frameType == attackFrameType)
                            {
                                int spread = Main.rand.Next(-15, 15);

                                NPC.NewNPC((int)npc.Center.X + 55 * npc.direction, (int)npc.Center.Y, mod.NPCType("Mosquitto"), 0, (float)MathHelper.ToRadians(spread), npc.direction);
                            }
                            else if (frameType == attackFrameUpType)
                            {
                                int spread = Main.rand.Next(-15, 15);

                                NPC.NewNPC((int)npc.Center.X + 30 * npc.direction, (int)npc.Center.Y - 30, mod.NPCType("Mosquitto"), 0, (float)Math.PI / 2 + MathHelper.ToRadians(spread), npc.direction);
                            }
                            else if (frameType == attackFrameAngleType)
                            {
                                int spread = Main.rand.Next(-15, 15);

                                NPC.NewNPC((int)npc.Center.X + 31 * npc.direction, (int)npc.Center.Y - 24, mod.NPCType("Mosquitto"), 0, (float)Math.PI / 4 + MathHelper.ToRadians(spread), npc.direction);
                                //NPC.NewNPC(int X, int Y, int Type, [int start = 0], [float ai0 = 0], [float ai1 = 0], [float ai2 = 0], [float ai3 = 0], [int target = 255])
                            }
                        }
                        diseaseTime = 0;
                    }

                    float playerPositionSummery = (player.Center.X - npc.Center.X) * npc.direction + (player.Center.Y - npc.Center.Y);
                    if (playerPositionSummery > 200f)
                    {
                        frameType = attackFrameType;
                    }
                    else if (playerPositionSummery < -200f)
                    {
                        frameType = attackFrameUpType;
                    }
                    else
                    {
                        frameType = attackFrameAngleType;
                    }

                    npc.velocity.X = (0);
                    npc.velocity.Y = (0);
                    if (AI_Timer > walkTime * 2)
                    {
                        multiplayerAttackCycle = 3;
                        AI_Timer = 0;
                    }
                }
                if (attack == 3)
                {
                    meteorsLaunched = false;
                    meteorTime++;
                    if (meteorTime > meteorReloadTime)
                    {
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center.X + (-28f * npc.direction), npc.Center.Y - 74f, 0f, -40f, mod.ProjectileType("MeteorLaunch"), damage, 3f, Main.myPlayer);
                        }
                        meteorTime = 0;
                    }

                    frameType = launchFrameType;

                    npc.velocity.X = (0);
                    npc.velocity.Y = (0);
                    if (AI_Timer > walkTime * 2)
                    {
                        multiplayerAttackCycle = 1;
                        AI_Timer = 0;
                        meteorsLaunched = true;
                    }
                }
            }
            else
            {
                frameType = moveFrameType;
                if (Main.netMode == 0)
                {
                    if (Main.netMode != 1)
                    {
                        npc.ai[1] = Main.rand.Next(1, 4);
                        npc.netUpdate = true;
                    }
                    attack = (int)npc.ai[1];
                }
                else
                {
                    attack = multiplayerAttackCycle;
                }
            }
            if (meteorsLaunched)
            {
                meteorTime++;
                if (meteorTime > 10)
                {
                    if (Main.netMode != 1)
                    {
                        int Xvar = Main.rand.Next(-750, 750);
                        Projectile.NewProjectile(player.Center.X + Xvar * 1.0f, player.Center.Y - 800f, 0f, 10f, mod.ProjectileType("MeteorFall"), damage, 3f, Main.myPlayer);
                    }
                    meteorTime = 0;
                }
            }
        }

        public override void NPCLoot()
        {
            QwertyWorld.downedTyrant = true;
            QwertyWorld.DinoKillCount += 10;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            if (Main.netMode != 1)
            {
                int centerX = (int)(npc.position.X + npc.width / 2) / 16;
                int centerY = (int)(npc.position.Y + npc.height / 2) / 16;
                int halfLength = npc.width / 2 / 16 + 1;
                //int weaponLoot = Main.rand.Next(1, 5);
                //int trophyChance = Main.rand.Next(0, 10);
                switch (Main.rand.Next(3))
                {
                    case 0:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DinoBone"));
                        break;

                    case 1:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DinoFlail"));
                        break;

                    case 2:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TheTyrantsExtinctionGun"));
                        break;
                }

                if (Main.rand.Next(4) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("WornPrehistoricBow"));
                }
                /*
				if (Main.expertMode)
				{
					npc.DropBossBags();
				}
				else
				{
					if (weaponLoot == 1)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(""));
					}
					if (weaponLoot == 2)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(""));
					}
					if (weaponLoot == 3)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(""));
					}
					if (weaponLoot == 4)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(""));
					}
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 73, 8);
				}
					if (trophyChance == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(""));
				}
				*/
            }
        }

        public int moveFrame = 0;
        public int moveFrame2 = 1;

        public int attackFrame = 2;
        public int attackFrame2 = 3;

        public int attackFrameUp = 4;
        public int attackFrameUp2 = 5;

        public int attackFrameAngle = 6;
        public int attackFrameAngle2 = 7;

        public int launchFrame = 8;
        public int launchFrame2 = 9;

        public override void FindFrame(int frameHeight)
        {
            // This makes the sprite flip horizontally in conjunction with the npc.direction.
            npc.spriteDirection = npc.direction;
            npc.frameCounter++;
            if (frameType == moveFrameType)
            {
                if (npc.frameCounter < 10)
                {
                    npc.frame.Y = (moveFrame * frameHeight);
                }
                else if (npc.frameCounter < 20)
                {
                    npc.frame.Y = (moveFrame2 * frameHeight);
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }
            if (frameType == attackFrameType)
            {
                if (npc.frameCounter < 10)
                {
                    npc.frame.Y = (attackFrame * frameHeight);
                }
                else if (npc.frameCounter < 20)
                {
                    npc.frame.Y = (attackFrame2 * frameHeight);
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }
            if (frameType == attackFrameUpType)
            {
                if (npc.frameCounter < 10)
                {
                    npc.frame.Y = (attackFrameUp * frameHeight);
                }
                else if (npc.frameCounter < 20)
                {
                    npc.frame.Y = (attackFrameUp2 * frameHeight);
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }
            if (frameType == attackFrameAngleType)
            {
                if (npc.frameCounter < 10)
                {
                    npc.frame.Y = (attackFrameAngle * frameHeight);
                }
                else if (npc.frameCounter < 20)
                {
                    npc.frame.Y = (attackFrameAngle2 * frameHeight);
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }
            if (frameType == launchFrameType)
            {
                if (npc.frameCounter < 10)
                {
                    npc.frame.Y = (moveFrame * frameHeight);
                }
                else if (npc.frameCounter < 20)
                {
                    npc.frame.Y = (launchFrame * frameHeight);
                }
                else if (npc.frameCounter < 30)
                {
                    npc.frame.Y = (launchFrame2 * frameHeight);
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }
        }
    }

    public class SnowFlake : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SnowFlake");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 0;

            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            projectile.rotation += 1.5f;
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.velocity.X = -velocityChange.X;
            }
            if (projectile.velocity.Y != velocityChange.Y)
            {
                projectile.velocity.Y = -velocityChange.Y;
            }
            return false;
        }
    }

    public class MeteorLaunch : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 0;

            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
            projectile.tileCollide = true;
        }

        public bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                Main.PlaySound(SoundID.Item62, projectile.position);
                for (int i = 0; i < 50; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1.4f;
                }
                // Fire Dust spawn
                for (int i = 0; i < 80; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 5f;
                    dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 3f;
                }
                runOnce = false;
            }
            projectile.rotation += 1.5f;
        }
    }

    public class MeteorFall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 0;

            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 1200;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            projectile.rotation += 1.5f;
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (Main.netMode != 1)
            {
                Main.PlaySound(SoundID.Item62, projectile.position);
                for (int i = 0; i < 50; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1.4f;
                }
                // Fire Dust spawn
                for (int i = 0; i < 80; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 5f;
                    dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 3f;
                }
            }
            return true;
        }
    }
}