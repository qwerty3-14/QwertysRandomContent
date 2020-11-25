using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs
{
    public class SharkWithFrickenLaserBeams : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shark With Fricken Laser Beams!");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.width = 100;
            npc.height = 24;
            npc.aiStyle = 16;
            aiType = NPCID.Shark;
            npc.damage = 40;
            npc.defense = 2;
            npc.lifeMax = 300;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 400f;
            npc.knockBackResist = 0.7f;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Ocean.Chance * .005f;
        }

        public float laserDistance = 500f;
        public int laserTimer;
        public float headShift = -20f;

        public override void AI()
        {
            Player player = Main.player[npc.target];
            if (npc.wet && (player.Center - npc.Center).Length() < laserDistance && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
            {
                npc.velocity = Vector2.Zero;
                npc.aiStyle = 0;
                aiType = 0;
                npc.rotation = QwertyMethods.SlowRotation(npc.rotation, (player.Center - npc.Center).ToRotation() + (npc.spriteDirection == -1 ? (float)Math.PI : 0), 2);
                if (QwertyMethods.AngularDifference(npc.rotation, (player.Center - npc.Center).ToRotation() + (npc.spriteDirection == -1 ? (float)Math.PI : 0)) < (float)Math.PI / 20)
                {
                    laserTimer++;
                    if (laserTimer > 60)
                    {
                        laserTimer = 0;
                        Projectile.NewProjectile(npc.Center + QwertyMethods.PolarVector(58, npc.rotation + (npc.spriteDirection == -1 ? (float)Math.PI : 0)), QwertyMethods.PolarVector(12f, npc.rotation + (npc.spriteDirection == -1 ? (float)Math.PI : 0)), ProjectileID.PinkLaser, 20, 0f, Main.myPlayer);
                    }
                }
            }
            else
            {
                npc.aiStyle = 16;
                aiType = NPCID.Shark;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
            npc.frameCounter += 1.0;
            if (npc.wet)
            {
                if (npc.frameCounter < 6.0)
                {
                    npc.frame.Y = 0;
                }
                else if (npc.frameCounter < 12.0)
                {
                    npc.frame.Y = frameHeight;
                }
                else if (npc.frameCounter < 18.0)
                {
                    npc.frame.Y = frameHeight * 2;
                }
                else if (npc.frameCounter < 24.0)
                {
                    npc.frame.Y = frameHeight * 3;
                }
                else
                {
                    npc.frameCounter = 0.0;
                }
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("LaserSharkShift"), 1, false, 0, false, false);
        }
    }
}