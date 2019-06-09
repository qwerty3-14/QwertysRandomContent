using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.World.Generation;

namespace QwertysRandomContent.NPCs.Fortress
{
    public class Caster : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("High Preist");
            Main.npcFrameCount[npc.type] = 9;
        }

        public override void SetDefaults()
        {
            npc.width = 58;
            npc.height = 64;
            npc.aiStyle = -1;
            npc.damage = 20;
            npc.defense = 6;
            npc.lifeMax = 75;
            npc.value = 500;
            //npc.alpha = 100;
            npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            //npc.dontTakeDamage = true;
            //npc.scale = 1.2f;
            //npc.buffImmune[mod.BuffType("PowerDown")] = true;
            npc.buffImmune[BuffID.Confused] = false;
            banner = npc.type;
            bannerItem = mod.ItemType("CasterBanner");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<FortressBiome>(mod).TheFortress && !NPC.AnyNPCs(mod.NPCType("Caster")) && !NPC.AnyNPCs(mod.NPCType("FortressBoss")))
            {
                return 10f;
            }
            return 0f;

        }
        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FortressBossSummon"), 1);
            }
            else if (Main.rand.Next(3)<=1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FortressBossSummon"), 1);
            }
        }
        int timer;
        int GenerateRingTime=30;
        int throwRingTime = 150;
        Projectile ring;
        float ringSpeed = 6;
        int ringProjectileCount;
        bool castingFrames;
        public override void AI()
        {
            npc.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;
            timer++;
            npc.TargetClosest(true);
            npc.spriteDirection = npc.direction;
            Player player = Main.player[npc.target];
            ringProjectileCount = 2 - (int)((float)npc.life / (float)npc.lifeMax * 2) + 4;
            if (timer== GenerateRingTime)
            {
                ring = Main.projectile[Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("RingCenter"), 11, 0, player.whoAmI, ringProjectileCount, npc.direction)];
                castingFrames = true;
            }
            if(timer > GenerateRingTime && timer < GenerateRingTime+throwRingTime)
            {
                ring.Center = npc.Center;

            }
            if(timer == GenerateRingTime + throwRingTime)
            {
                castingFrames = false;
                ring.velocity = ((player.Center - npc.Center).SafeNormalize(-Vector2.UnitY)* ringSpeed) * (npc.confused ? -1: 1);
                timer = 0;
            }
            npc.velocity.X = npc.velocity.X * 0.93f;
            if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
            {
                npc.velocity.X = 0f;
            }
            /*
            if (npc.ai[0] == 0f)
            {
                npc.ai[0] = 500f;
            }
            */
            if (npc.ai[2] != 0f && npc.ai[3] != 0f)
            {

                Main.PlaySound(SoundID.Item8, npc.position);
                for (int num67 = 0; num67 < 50; num67++)
                {



                    int num75 = Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CaeliteDust"), 0f, 0f, 100, default(Color), 2.5f);
                    Main.dust[num75].velocity *= 3f;
                    Main.dust[num75].noGravity = true;

                }
                npc.position.X = npc.ai[2] * 16f - (float)(npc.width / 2) + 8f;
                npc.position.Y = npc.ai[3] * 16f - (float)npc.height;
                npc.velocity.X = 0f;
                npc.velocity.Y = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                Main.PlaySound(SoundID.Item8, npc.position);
                for (int num76 = 0; num76 < 50; num76++)
                {


                    int num84 = Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CaeliteDust"), 0f, 0f, 100, default(Color), 2.5f);
                    Main.dust[num84].velocity *= 3f;
                    Main.dust[num84].noGravity = true;

                }
            }
            //npc.ai[0] += 1f;

            if (Math.Abs(npc.position.X - Main.player[npc.target].position.X) + Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
            {
                npc.ai[0] = 650f;
            }
            if (npc.ai[0] >= 650f && Main.netMode != 1)
            {
                npc.ai[0] = 1f;
                int playerTilePositionX = (int)Main.player[npc.target].position.X / 16;
                int playerTilePositionY = (int)Main.player[npc.target].position.Y / 16;
                int npcTilePositionX = (int)npc.position.X / 16;
                int npcTilePositionY = (int)npc.position.Y / 16;
                int playerTargetShift = 40;
                int num90 = 0;
                
                
                for (int s =0; s<100; s++)
                {
                    num90++;
                    int nearPlayerX = Main.rand.Next(playerTilePositionX - playerTargetShift, playerTilePositionX + playerTargetShift);
                    int nearPlayerY = Main.rand.Next(playerTilePositionY - playerTargetShift, playerTilePositionY + playerTargetShift);
                    for (int num93 = nearPlayerY; num93 < playerTilePositionY + playerTargetShift; num93++)
                    {
                        if ((nearPlayerX < playerTilePositionX - 12 || nearPlayerX > playerTilePositionX + 12) && (num93 < npcTilePositionY - 1 || num93 > npcTilePositionY + 1 || nearPlayerX < npcTilePositionX - 1 || nearPlayerX > npcTilePositionX + 1) && Main.tile[nearPlayerX, num93].nactive())
                        {
                            bool flag5 = true;
                            if (Main.tile[nearPlayerX, num93 - 1].lava())
                            {
                                flag5 = false;
                            }
                            if (flag5 && Main.tileSolid[(int)Main.tile[nearPlayerX, num93].type] && !Collision.SolidTiles(nearPlayerX - 1, nearPlayerX + 1, num93 - 4, num93 - 1))
                            {
                                npc.ai[1] = 20f;
                                npc.ai[2] = (float)nearPlayerX;
                                npc.ai[3] = (float)num93 -1;
                                
                                break;
                            }
                        }
                    }
                }
                npc.netUpdate = true;
            }
            if (npc.ai[1] > 0f)
            {
                npc.ai[1] -= 1f;

            }




            




        }
        public override void HitEffect(int hitDirection, double damage)
        {
            //timer = 0;
            npc.ai[0] = 650f;
            npc.TargetClosest(true);
        }
        int frameCounter;
        int frame;
        public override void FindFrame(int frameHeight)
        {
            frameCounter++;
            frame = 0;
            if(frameCounter>50)
            {
                frameCounter = 0;
            }
            if(frameCounter >40)
            {
                frame = 3;
            }
            else if(frameCounter>30)
            {
                frame = 2;
            }
            else if(frameCounter >20)
            {
                frame = 1;
            }
            if(castingFrames)
            {
                frame += 5;
            }
            npc.frame.Y = frameHeight * frame;
        }

    }
    public class RingCenter : ModProjectile
    {
        public override void SetStaticDefaults()
        {
           
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 8 * 60;
        }
        bool runOnce = true;
        int projectilesInRing = 6;
        public override void AI()
        {
            projectilesInRing = (int)projectile.ai[0];

            if (runOnce)
            {
                for (int i = 0; i < projectilesInRing; i++)
                {
                    //Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("RingOuter"), projectile.damage, projectile.knockBack, projectile.owner, (float)i / (float)projectilesInRing * 2 * (float)Math.PI, projectile.whoAmI);
                    
                    if (projectile.ai[1] == 1)
                    {
                        Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("RingOuter"), projectile.damage, projectile.knockBack, projectile.owner, (float)i / (float)projectilesInRing * 2 * (float)Math.PI, projectile.whoAmI);
                    }
                    else
                    {
                        Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("RingOuter"), projectile.damage, projectile.knockBack, projectile.owner, (float)i / (float)projectilesInRing * 2 * (float)Math.PI, -projectile.whoAmI);
                    }
                    
                        
                }
                runOnce = false;
            }
        }
    }
    public class RingOuter : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Sphere");
            Main.projFrames[projectile.type] = 4;
        }
        
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
        }
        bool runOnce = true;
        int projectilesInRing = 4;
        Projectile parent;
        float radius =60;
        Projectile clearCheck;
        int spinDirection = 1;
        int frameTimer;
        public override void AI()
        {
            Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"))];
            dust.scale = .5f;
            if (runOnce)
            {
                if(projectile.ai[1] < 0)
                {
                    spinDirection = -1;
                    projectile.ai[1] = Math.Abs(projectile.ai[1]);
                }
                runOnce = false;
            }
            projectile.ai[1] = Math.Abs(projectile.ai[1]);
            parent = Main.projectile[(int)projectile.ai[1]];
            projectile.position.X = parent.Center.X - (int)(Math.Cos(projectile.ai[0]) * radius) - projectile.width / 2;
            projectile.position.Y = parent.Center.Y - (int)(Math.Sin(projectile.ai[0]) * radius) - projectile.height / 2;
            projectile.ai[0] += (float)Math.PI / 120 * spinDirection;
            for(int p =0; p <1000; p++)
            {
                clearCheck = Main.projectile[p];
                if (clearCheck.friendly && clearCheck.type != mod.ProjectileType("PunnishFist") && !clearCheck.sentry && clearCheck.minionSlots <=0 && Collision.CheckAABBvAABBCollision(projectile.position, projectile.Size, clearCheck.position, clearCheck.Size))
                {
                    clearCheck.Kill();
                }
            }
            if(!parent.active || parent.type != mod.ProjectileType("RingCenter"))
            {
                projectile.Kill();
            }
            frameTimer++;
            if(frameTimer>10)
            {
                projectile.frame++;
                if(projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
                frameTimer = 0;
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                
                Dust dust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, mod.DustType("CaeliteDust"))];
                dust.velocity *= 3;
                
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(3) == 0)
            {
                target.AddBuff(mod.BuffType("PowerDown"), 600);
            }
        }


    }
}
