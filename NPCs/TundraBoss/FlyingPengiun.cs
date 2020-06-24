using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.TundraBoss
{
    public class FlyingPenguin : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Penguin");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 22;
            npc.height = 40;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            //npc.value = 6000f;
            npc.knockBackResist = 0f;
            npc.aiStyle = 0;
            npc.lifeMax = 10;
            npc.defense = 4;
            npc.damage = 0;
            npc.noTileCollide = true;
            npc.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void NPCLoot()
        {
        }

        private int timer;
        private float flyAboveHeight = 150;
        private float penguinPoliteness = 80;
        private float flySpeed = 10;
        private int frame;

        public override void AI()
        {
            timer++;
            Player player = Main.player[npc.target];

            Vector2 flyTo = new Vector2(player.Center.X + (penguinPoliteness * npc.ai[0]), player.Center.Y - flyAboveHeight);
            if (timer > 180)
            {
                npc.velocity = new Vector2(0, 10);
                npc.noTileCollide = false;
                if (Main.expertMode)
                {
                    npc.damage = 30;
                }
                else
                {
                    npc.damage = 20;
                }
                npc.TargetClosest(false);
                npc.spriteDirection = -npc.direction;
                npc.rotation = (float)Math.PI;
                if (timer % 10 == 0)
                {
                    if (frame == 1)
                    {
                        frame = 0;
                    }
                    else
                    {
                        frame = 1;
                    }
                }
            }
            else if (timer > 120)
            {
                npc.TargetClosest(true);
                npc.spriteDirection = npc.direction;
                if (timer % 10 == 0)
                {
                    if (frame == 3)
                    {
                        frame = 2;
                    }
                    else
                    {
                        frame = 3;
                    }
                }
                npc.velocity = new Vector2(0, 0);
            }
            else
            {
                npc.TargetClosest(true);
                npc.spriteDirection = npc.direction;
                if (timer % 10 == 0)
                {
                    if (frame == 3)
                    {
                        frame = 2;
                    }
                    else
                    {
                        frame = 3;
                    }
                }
                npc.velocity = (flyTo - npc.Center);
                if (npc.velocity.Length() > flySpeed)
                {
                    npc.velocity = npc.velocity.SafeNormalize(-Vector2.UnitY) * flySpeed;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frame * frameHeight;
        }
    }
}