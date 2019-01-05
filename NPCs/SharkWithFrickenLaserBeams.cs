using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

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
            npc.damage = 40;
            npc.defense = 2;
            npc.lifeMax = 300;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 400f;
            npc.knockBackResist = 0.7f;
            banner = NPCID.Shark;
            bannerItem = ItemID.SharkBanner;

        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			
				return SpawnCondition.Ocean.Chance* .005f;
			
		}
        public float laserDistance=500f;
        public int laserTimer;
        public float headShift = -20f;
        public override void AI()
        {
            Player player = Main.player[npc.target];
            float distance = (player.Center - npc.Center).Length();
            if (distance <= laserDistance)
            {
                
                
                if (player.Center.X < npc.Center.X)
                {
                    npc.direction = -1;
                    npc.rotation = (player.Center - npc.Center).ToRotation()+ (float)Math.PI;
                }
                else
                {
                    npc.direction = 1;
                    npc.rotation = (player.Center - npc.Center).ToRotation();
                }
                npc.aiStyle = -1;
                npc.velocity = new Vector2(0, 0);
                laserTimer++;
                if(laserTimer>10)
                {
                    if(Main.netMode !=1)
                    {
                        Projectile.NewProjectile(npc.Center.X+ ((float)Math.Sin(npc.rotation) * headShift), npc.Center.Y + ((float)Math.Cos(npc.rotation)*headShift), (float)Math.Cos(npc.rotation) * 10* npc.direction, (float)Math.Sin(npc.rotation)*10* npc.direction, ProjectileID.PinkLaser, 40, 3f, Main.myPlayer);
                    }
                    laserTimer = 0;
                }


            }
            else
            {
                npc.aiStyle = 16;
                if (npc.direction == 0)
                {
                    npc.TargetClosest(true);
                }
                if (npc.wet)
                {

                    {
                        bool flag14 = false;

                        if (!flag14)
                        {
                            if (npc.collideX)
                            {
                                npc.velocity.X = npc.velocity.X * -1f;
                                npc.direction *= -1;
                                npc.netUpdate = true;
                            }
                            if (npc.collideY)
                            {
                                npc.netUpdate = true;
                                if (npc.velocity.Y > 0f)
                                {
                                    npc.velocity.Y = Math.Abs(npc.velocity.Y) * -1f;
                                    npc.directionY = -1;
                                    npc.ai[0] = -1f;
                                }
                                else if (npc.velocity.Y < 0f)
                                {
                                    npc.velocity.Y = Math.Abs(npc.velocity.Y);
                                    npc.directionY = 1;
                                    npc.ai[0] = 1f;
                                }
                            }
                        }

                        if (flag14)
                        {
                            npc.TargetClosest(true);


                            {
                                npc.velocity.X = npc.velocity.X + (float)npc.direction * 0.15f;
                                npc.velocity.Y = npc.velocity.Y + (float)npc.directionY * 0.15f;
                                if (npc.velocity.X > 5f)
                                {
                                    npc.velocity.X = 5f;
                                }
                                if (npc.velocity.X < -5f)
                                {
                                    npc.velocity.X = -5f;
                                }
                                if (npc.velocity.Y > 3f)
                                {
                                    npc.velocity.Y = 3f;
                                }
                                if (npc.velocity.Y < -3f)
                                {
                                    npc.velocity.Y = -3f;
                                }
                            }

                        }
                        else
                        {

                            {
                                npc.velocity.X = npc.velocity.X + (float)npc.direction * 0.1f;
                                if (npc.velocity.X < -1f || npc.velocity.X > 1f)
                                {
                                    npc.velocity.X = npc.velocity.X * 0.95f;
                                }
                                if (npc.ai[0] == -1f)
                                {
                                    npc.velocity.Y = npc.velocity.Y - 0.01f;
                                    if ((double)npc.velocity.Y < -0.3)
                                    {
                                        npc.ai[0] = 1f;
                                    }
                                }
                                else
                                {
                                    npc.velocity.Y = npc.velocity.Y + 0.01f;
                                    if ((double)npc.velocity.Y > 0.3)
                                    {
                                        npc.ai[0] = -1f;
                                    }
                                }
                            }
                            int num254 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
                            int num255 = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;
                            if (Main.tile[num254, num255 - 1] == null)
                            {
                                Main.tile[num254, num255 - 1] = new Tile();
                            }
                            if (Main.tile[num254, num255 + 1] == null)
                            {
                                Main.tile[num254, num255 + 1] = new Tile();
                            }
                            if (Main.tile[num254, num255 + 2] == null)
                            {
                                Main.tile[num254, num255 + 2] = new Tile();
                            }
                            if (Main.tile[num254, num255 - 1].liquid > 128)
                            {
                                if (Main.tile[num254, num255 + 1].active())
                                {
                                    npc.ai[0] = -1f;
                                }
                                else if (Main.tile[num254, num255 + 2].active())
                                {
                                    npc.ai[0] = -1f;
                                }
                            }
                            if (((double)npc.velocity.Y > 0.4 || (double)npc.velocity.Y < -0.4))
                            {
                                npc.velocity.Y = npc.velocity.Y * 0.95f;
                            }
                        }
                    }
                }
                else
                {
                    if (npc.velocity.Y == 0f)
                    {

                        {
                            npc.velocity.X = npc.velocity.X * 0.94f;
                            if ((double)npc.velocity.X > -0.2 && (double)npc.velocity.X < 0.2)
                            {
                                npc.velocity.X = 0f;
                            }
                        }

                    }
                    npc.velocity.Y = npc.velocity.Y + 0.3f;
                    if (npc.velocity.Y > 10f)
                    {
                        npc.velocity.Y = 10f;
                    }
                    npc.ai[0] = 1f;
                }
                npc.rotation = npc.velocity.Y * (float)npc.direction * 0.1f;
                if ((double)npc.rotation < -0.2)
                {
                    npc.rotation = -0.2f;
                }
                if ((double)npc.rotation > 0.2)
                {
                    npc.rotation = 0.2f;
                    return;
                }
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
            
               Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 268, 1, false, 0, false, false);
            
            
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 319, 1, false, 0, false, false);
            
        }






    }
}
