using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.TundraBoss
{
    public class SlidingPenguin : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sliding Penguin");
            //Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 42;
            npc.height = 18;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            //npc.value = 6000f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.lifeMax = 25;
            npc.defense = 4;
            npc.damage = 20;

            npc.noGravity = false;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = 30;
            npc.lifeMax = 25;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void NPCLoot()
        {
        }

        private int timer;
        float speed = 5;
        public override void AI()
        {
            if(npc.ai[1] == 1)
            {
                npc.TargetClosest(false);
                Player player = Main.player[npc.target];
                npc.rotation = (player.Center - npc.Center).ToRotation() + (float)Math.PI;
                npc.velocity = QwertyMethods.PolarVector(5, npc.rotation - (float)Math.PI);
                if(npc.velocity.X > 0)
                {
                    npc.rotation += (float)Math.PI;
                }
                npc.ai[1] = 2;
                npc.noTileCollide = true;
                npc.noGravity = true;
                npc.spriteDirection = (int)npc.ai[0];
            }
            else if (npc.ai[1] == 2)
            {
                npc.TargetClosest(false);
                Player player = Main.player[npc.target];
                npc.noTileCollide = true;
                npc.noGravity = true;
                if (npc.Center.Y < player.Center.Y)
                {
                    npc.ai[1] = 0;
                }
            }
            else
            {
                npc.noGravity = false;
                npc.rotation = 0;
                npc.noTileCollide = false;
                npc.velocity.X = speed * npc.ai[0];
                timer++;
                if (npc.collideX && timer > 5)
                {
                    npc.ai[0] *= -1;
                    timer = 0;
                }
                npc.spriteDirection = (int)npc.ai[0];
                if (timer > 180)
                {
                    speed -= 5f / 180f;
                }
                if (speed <= 0)
                {
                    NPC Penguin = Main.npc[NPC.NewNPC((int)npc.Top.X, (int)npc.Top.Y, NPCID.Penguin)];
                    npc.active = false;
                }
            }
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            npc.ai[0] *= -1;
            npc.netUpdate = true;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if(npc.life <=0)
            {
                Gore.NewGore(npc.position, npc.velocity, 160);
                Gore.NewGore(new Vector2(npc.position.X, npc.position.Y), npc.velocity, 161);
            }
        }
    }
}