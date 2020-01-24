using Microsoft.Xna.Framework;
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
            npc.lifeMax = 2100;
            npc.defense = 8;
            npc.damage = 40;
            npc.boss = true;
            npc.noGravity = false;
            music = MusicID.Boss5;
            bossBag = mod.ItemType("TundraBossBag");

        }
        const int IdleFrame = 4;
        const int JumpFrame = 1;
        const int AboutToJumpFrame = 0;
        const int ShootSliderFrame = 3;
        const int ShootFlierFrame = 2;
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = 60;
            npc.lifeMax = (int)(2400 * bossLifeScale);

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }
        public override void NPCLoot()
        {
            QwertyWorld.downedBear = true;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
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

        }
        int timer;
        int attackDelay = 60;
        int resetAttacks = 360;
        int attackCounter;
        bool landed;
        int frame = 4;
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
                timer++;
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
                    if (Main.netMode != 1)
                    {
                        npc.ai[0] = Main.rand.Next(2);
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
                        if (timer % 60 == 0 && attackCounter < 3)
                        {
                            attackCounter++;
                            if (Main.netMode != 1)
                            {

                                NPC.NewNPC((int)npc.Center.X + 30 * npc.direction, (int)npc.Center.Y + 14, mod.NPCType("SlidingPenguin"), ai0: npc.direction);
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
                        if (timer == attackDelay + 60)
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
            timer = reader.ReadInt32();
        }
    }
}
