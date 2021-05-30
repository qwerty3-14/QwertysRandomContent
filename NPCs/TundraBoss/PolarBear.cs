using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.TundraBoss
{
    public class PolarBear : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Polar Exterminator");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            npc.width = 68;
            npc.height = 82;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            //npc.value = 6000f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.lifeMax = 1600;
            npc.defense = 8;
            npc.damage = 40;
            npc.boss = true;
            npc.noGravity = false;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/PolarOpposition");
            bossBag = mod.ItemType("TundraBossBag");
        }

        private const int IdleFrame = 4;
        private const int JumpFrame = 1;
        private const int AboutToJumpFrame = 0;
        private const int ShootSliderFrame = 3;
        private const int ShootFlierFrame = 2;

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = 60;
            npc.lifeMax = (int)(2000 * bossLifeScale);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void NPCLoot()
        {
            if (!QwertyWorld.downedBear)
            {
                QwertyWorld.downedBear = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state
                }
            }

            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PolarMask"));
                }

                switch (Main.rand.Next(3))
                {
                    case 0:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PenguinClub"));
                        break;

                    case 1:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PenguinLauncher"));
                        break;

                    case 2:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PenguinWhistle"));
                        break;
                }
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Penguin, Main.rand.Next(35, 61));
            }
            if (Main.rand.Next(0, 10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PolarTrophy"));
            }
        }

        private float timer;
        private int attackDelay = 60;
        private int resetAttacks = 360;
        private int attackCounter;
        private bool landed;
        private int frame = 4;
        int attackCycle = 0;
        int agentCooldown = 0;
        public override void AI()
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];

            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];

                if (!player.active || player.dead)
                {
                    npc.noTileCollide = true;
                    frame = JumpFrame;
                    npc.velocity = new Vector2(0f, 10f);

                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }
            else
            {
                if(Main.expertMode && landed)
                {
                    timer+= 1 + 2 * (1f - (float)npc.life/npc.lifeMax);
                }
                else
                {
                    timer++;
                }
                
                if (timer > resetAttacks)
                {
                    npc.velocity.X = 10 * npc.direction;
                    npc.velocity.Y = -10;
                    landed = false;
                    frame = JumpFrame;
                    if (Main.netMode != 1)
                    {
                        timer = 0;
                        npc.netUpdate = true;
                    }
                    attackCounter = 0;
                    attackCycle++;
                    if(attackCycle == 7)
                    {
                        attackCycle = 0;
                    }
                    if (Main.netMode != 1)
                    {
                        npc.ai[0] = (attackCycle == 0 || attackCycle == 1 || attackCycle == 3 || attackCycle == 4) ? 0 : 1;
                        npc.netUpdate = true;
                    }
                }
                else if (timer > resetAttacks - 60)
                {
                    frame = AboutToJumpFrame;
                }
                else if (timer > attackDelay)
                {
                    if (npc.ai[0] == 0)
                    {
                        frame = ShootSliderFrame;
                        if (timer > (attackCounter + 1) * 90 + attackDelay && attackCounter < 2)
                        {
                            attackCounter++;
                            if (Main.netMode != 1)
                            {
                                NPC.NewNPC((int)npc.Center.X + 30 * npc.direction, (int)npc.Center.Y + 14, mod.NPCType("SlidingPenguin"), ai0: npc.direction, ai1: (player.Bottom.Y < npc.Center.Y + 14) ? 1 : 0);
                            }
                            Main.PlaySound(SoundID.Item11, npc.position);
                            for (int i = 0; i < 8; i++)
                            {
                                Dust.NewDustPerfect(new Vector2((int)npc.Center.X + 30 * npc.direction, (int)npc.Center.Y + 14), DustID.Ice, new Vector2(npc.direction * (2 + Main.rand.NextFloat() * 2f), 0).RotatedByRandom(Math.PI / 8));
                            }
                        }
                    }
                    else if (npc.ai[0] == 1)
                    {
                        frame = ShootFlierFrame;
                        if (timer > attackDelay + 90 && attackCounter == 0)
                        {
                            attackCounter++;
                            for (int i = -2; i < 3; i++)
                            {
                                if (Main.netMode != 1)
                                {
                                    NPC.NewNPC((int)npc.Center.X + 34 * npc.direction, (int)npc.Center.Y, mod.NPCType("FlyingPenguin"), 0, i);
                                }
                                Main.PlaySound(SoundID.Item11, npc.position);
                            }
                            for (int i = 0; i < 8; i++)
                            {
                                Dust.NewDustPerfect(new Vector2((int)npc.Center.X + 34 * npc.direction, (int)npc.Center.Y), DustID.Ice, new Vector2(npc.direction * (2 + Main.rand.NextFloat() * 2f), 0).RotatedByRandom(Math.PI / 8));
                            }
                        }
                    }
                }
                if (npc.collideY && npc.velocity.Y > 0)
                {
                    if (!landed)
                    {
                        frame = IdleFrame;
                        for (int i = 0; i < 60; i++)
                        {
                            Dust.NewDust(npc.BottomLeft, Main.rand.Next(npc.width), 1, DustID.Ice);
                        }
                        landed = true;
                    }
                    npc.velocity.X = 0;
                }
                /*
                if (Main.netMode == 1)
                {
                    Main.NewText("client: " + timer);
                }

                if (Main.netMode == 2) // Server
                {
                    NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + timer), Color.Black);
                }
                */
                //Main.NewText(npc.collideY);
                npc.noTileCollide = false;
                if (npc.velocity.Y < 0 || player.Bottom.Y > npc.Bottom.Y + 32)
                {
                    npc.noTileCollide = true;
                    npc.collideY = false;
                }
                else
                {
                    npc.noTileCollide = true;
                    Point bottomLeft = npc.BottomLeft.ToTileCoordinates();
                    Texture2D texture = Main.extraTexture[2];
                    for (int i = 0; i < (npc.width / 16) + 1; i++)
                    {
                        Vector2 p = bottomLeft.ToVector2() + new Vector2(i, 0);
                        if (Main.tileSolid[Main.tile[(int)p.X, (int)p.Y].type] && !Main.tileSolidTop[Main.tile[(int)p.X, (int)p.Y].type])
                        {
                            npc.noTileCollide = false;
                        }
                    }
                }
                if (Main.netMode != 1)
                {
                    if (Main.expertMode && (float)npc.life / (float)npc.lifeMax < .5f && agentCooldown <= 0)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            float x = Main.rand.NextFloat(7, 24) * (i == 0 ? 1 : -1);
                            int denLength = 101;
                            int denUpperHeight = 40;
                            int ceilingHeight = (int)((float)Math.Sin(((float)(x + (denLength / 2)) / (float)denLength) * (float)Math.PI) * (float)denUpperHeight);
                            Vector2 spawnPos = FrozenDen.BearSpawn + new Vector2(x * 16, ceilingHeight * -16);
                            NPC.NewNPC((int)spawnPos.X, (int)spawnPos.Y, mod.NPCType("AgentPenguin"));
                            
                        }
                        agentCooldown = 600;
                    }
                    if (agentCooldown > 0)
                    {
                        agentCooldown--;
                    }
                }
                
            }
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frame * frameHeight;
            npc.spriteDirection = npc.direction;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(timer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            timer = reader.ReadSingle();
        }
        /*
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Point bottomLeft = npc.BottomLeft.ToTileCoordinates();
            Texture2D texture = Main.extraTexture[2];
            for (int i = 0; i < (npc.width / 16)+1; i++)
            {
                spriteBatch.Draw(texture, (bottomLeft.ToVector2() * 16) + Vector2.UnitX * i * 16 - Main.screenPosition, new Rectangle(0, 0, 16, 16), Color.White, 0, Vector2.Zero, Vector2.One, 0, 0);
            }
            return base.PreDraw(spriteBatch, drawColor);
        }
        */
    }
}