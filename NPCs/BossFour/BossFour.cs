using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;
using System.Collections.Generic;
using System.IO;

namespace QwertysRandomContent.NPCs.BossFour
{
    public class BossFour : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Old Lord");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.width = 1500;
            npc.height = 300;
            npc.damage = 70;
            npc.defense = 60;
            //npc.boss = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 60f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;

            animationType = -1;
            npc.noGravity = true;
            npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/EnergisedPlanetaryIncinerationClimax");
            npc.lifeMax = 1;
            //bossBag = mod.ItemType("HydraBag");
            npc.netAlways = true;

        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * 1f);
        }
        public bool playerDied;
        public override bool CheckActive()
        {
            if(playerDied)
            {
                return true;
            }
            return false;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            return 0f;

        }
        public int regenTimer;
        public int timer;
        public Projectile laser;
        public Projectile laser2;
        public NPC turret;
        public NPC turret2;
        public NPC turret3;
        public NPC turret4;
        public NPC weakPoint;
        public NPC background;
        public Vector2 turretPosition;
        public float moveDirection;
        //public Vector2 moveTo;
        public float moveSpeed;
        public bool runOnce = true;
        public int attack = 1;
        public int nextAttack=1;
        public int attackTimer;
        public Vector2 weakPointPosition;
        public Vector2 backgroundPosition;
        public bool startDeathSequence;
        public int deathTmier;
        public int weakTimer;
        public bool below70;
        public bool did70Attack;
        public bool summoned70;
        public bool below40;
        public bool did40Attack;
        public bool summoned40;
        public bool below10;
        public bool did10Attack;
        public bool summoned10;
        public bool notOffscreen = true;
        public NPC miniboss1;
        public NPC miniboss2;
        public NPC miniboss3;
        public NPC miniboss4;
        public int shotDamage;
        public int offtimer;
        public int count;
        public int quitCount;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }
        
        public override void AI()
        {
            timer++;
            if (Main.netMode != 1 && timer ==300)
            {
                
                npc.netUpdate = true;
            }
            if (Main.expertMode)
            {
                shotDamage = (int)(npc.damage / 4 * 1.6f);
            }
            else
            {
                shotDamage = npc.damage / 2;
            }
            Player player = Main.player[npc.target];
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

            if (runOnce)
            {

                turret = Main.npc[NPC.NewNPC((int)(npc.position.X), (int)(npc.position.Y), mod.NPCType("Turret"), 0)];
                turret2 = Main.npc[NPC.NewNPC((int)(npc.position.X), (int)(npc.position.Y), mod.NPCType("Turret"), 0)];
                turret3 = Main.npc[NPC.NewNPC((int)(npc.position.X), (int)(npc.position.Y), mod.NPCType("Turret"), 0)];
                turret4 = Main.npc[NPC.NewNPC((int)(npc.position.X), (int)(npc.position.Y), mod.NPCType("Turret"), 0)];


                weakPoint = Main.npc[NPC.NewNPC((int)(npc.position.X), (int)(npc.position.Y), mod.NPCType("WeakPoint"))];

                background = Main.npc[NPC.NewNPC((int)(npc.position.X), (int)(npc.position.Y), mod.NPCType("BackGround"), 0)];




                npc.netUpdate = true;
                runOnce = false;
            }
            if (!startDeathSequence)
            {
                
                
                    if (.7f > (float)weakPoint.life / (float)weakPoint.lifeMax)
                    {
                        below70 = true;
                    }
                    if (.4f > (float)weakPoint.life / (float)weakPoint.lifeMax)
                    {
                        below40 = true;
                    }
                    if (.1f > (float)weakPoint.life / (float)weakPoint.lifeMax)
                    {
                        below10 = true;
                    }
                
                if (notOffscreen)
                {
                    #region
                    if (Main.netMode == 0)
                    {
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 1 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2);
                        turret.position = turretPosition;
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 2 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2);
                        turret2.position = turretPosition;
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 4 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2);
                        turret3.position = turretPosition;
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 5 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2);
                        turret4.position = turretPosition;
                        weakPointPosition = new Vector2(npc.Center.X - weakPoint.width / 2, npc.position.Y + npc.height);
                        weakPoint.position = weakPointPosition;
                        backgroundPosition = new Vector2(npc.Center.X - background.width / 2, npc.Center.Y - background.height / 2  );
                        background.position = backgroundPosition;
                    }
                    else 
                    {
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 1 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2);
                        turret.ai[0]= turretPosition.X;
                        turret.ai[1] = turretPosition.Y;
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 2 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2);
                        turret2.ai[0] = turretPosition.X;
                        turret2.ai[1] = turretPosition.Y;
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 4 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2);
                        turret3.ai[0] = turretPosition.X;
                        turret3.ai[1] = turretPosition.Y;
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 5 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2);
                        turret4.ai[0] = turretPosition.X;
                        turret4.ai[1] = turretPosition.Y;
                        weakPointPosition = new Vector2(npc.Center.X - weakPoint.width / 2, npc.position.Y + npc.height);
                        weakPoint.ai[0] = weakPointPosition.X;
                        weakPoint.ai[1] = weakPointPosition.Y;
                        backgroundPosition = new Vector2(npc.Center.X - background.width / 2, npc.Center.Y - background.height / 2 );
                        background.ai[0] = backgroundPosition.X;
                        background.ai[1] = backgroundPosition.Y;
                    }
                    #endregion
                }
                else 
                {
                    #region
                    if (Main.netMode==0)
                    {
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 1 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2 - 3000);
                        turret.position = turretPosition;
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 2 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2 - 3000);
                        turret2.position = turretPosition;
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 4 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2 - 3000);
                        turret3.position = turretPosition;
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 5 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2 - 3000);
                        turret4.position = turretPosition;
                        weakPointPosition = new Vector2(npc.Center.X - weakPoint.width / 2, npc.position.Y + npc.height - 3000);
                        weakPoint.position = weakPointPosition;
                        backgroundPosition = new Vector2(npc.Center.X - background.width / 2, npc.Center.Y - background.height / 2 - 3000);
                        background.position = backgroundPosition;
                    }
                    else 
                    {
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 1 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2 - 3000);
                        turret.ai[0] = turretPosition.X;
                        turret.ai[1] = turretPosition.Y;
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 2 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2 - 3000);
                        turret2.ai[0] = turretPosition.X;
                        turret2.ai[1] = turretPosition.Y;
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 4 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2 - 3000);
                        turret3.ai[0] = turretPosition.X;
                        turret3.ai[1] = turretPosition.Y;
                        turretPosition = new Vector2(npc.position.X - turret.width / 2 + 5 * (npc.width / 6), npc.position.Y + npc.height - turret.height / 2 - 3000);
                        turret4.ai[0] = turretPosition.X;
                        turret4.ai[1] = turretPosition.Y;
                        weakPointPosition = new Vector2(npc.Center.X - weakPoint.width / 2, npc.position.Y + npc.height - 3000);
                        weakPoint.ai[0] = weakPointPosition.X;
                        weakPoint.ai[1] = weakPointPosition.Y;
                        backgroundPosition = new Vector2(npc.Center.X - background.width / 2, npc.Center.Y - background.height / 2 - 3000);
                        background.ai[0] = backgroundPosition.X;
                        background.ai[1] = backgroundPosition.Y;
                    }
                    #endregion
                }



               
                    
               

                if (timer == 300 && Main.netMode != 1)
                {
                    float laserDistanceFromCenter = 800;
                    laser = Main.projectile[Projectile.NewProjectile(npc.Center.X + laserDistanceFromCenter, npc.Center.Y + npc.height/2 +10, 0, 14f, mod.ProjectileType("SideLaser"), (int)(shotDamage*1.5f), 3f, Main.myPlayer, npc.whoAmI, laserDistanceFromCenter)];
                    laser2 = Main.projectile[Projectile.NewProjectile(npc.Center.X + laserDistanceFromCenter, npc.Center.Y + npc.height / 2 +10, 0, 14f, mod.ProjectileType("SideLaser"), (int)(shotDamage * 1.5f), 3f, Main.myPlayer, npc.whoAmI, -laserDistanceFromCenter)];

                }

                if (timer > 420 )
                {
                    if (true)
                    {

                        if (attack == 1)
                        {
                            #region
                            attackTimer++;
                            if (attackTimer > 1500)
                            {

                                attack=0;
                                attackTimer = 0;
                                weakPoint.defense = 1;
                            }
                            else if (attackTimer > 1020)
                            {
                                turret.defense = 3;
                                turret2.defense = 3;
                                turret3.defense = 3;
                                turret4.defense = 3;
                                weakPoint.defense = 2;
                            }
                            else if (attackTimer > 900)
                            {
                                turret.defense = 3;
                                turret2.defense = 3;
                                turret3.defense = 3;
                                turret4.defense = 3;
                                weakPoint.defense = 1;
                            }
                            else if (attackTimer > 600)
                            {
                                turret.defense = 2;
                                turret2.defense = 3;
                                turret3.defense = 3;
                                turret4.defense = 2;
                                weakPoint.defense = 1;
                            }
                            else if (attackTimer > 300)
                            {
                                turret.defense = 2;
                                turret2.defense = 3;
                                turret3.defense = 2;
                                turret4.defense = 2;
                                weakPoint.defense = 1;
                            }
                            else
                            {


                                turret.defense = 2;
                                turret2.defense = 2;
                                turret3.defense = 2;
                                turret4.defense = 2;
                                weakPoint.defense = 1;
                            }
                            #endregion

                        }
                        else if(attack==2)
                        {
                            #region
                            attackTimer++;
                            if (attackTimer > 1111)
                            {
                                turret.defense = 1;
                                turret2.defense = 1;
                                turret3.defense = 1;
                                turret4.defense = 1;
                                weakPoint.defense = 1;
                                attack=0;
                                attackTimer = 1;
                            }
                            else if (attackTimer > 690)
                            {
                                turret.defense = 2;
                                turret2.defense = 2;
                                turret3.defense = 2;
                                turret4.defense = 2;
                                weakPoint.defense = 3;
                            }
                            else if (attackTimer>510)
                            {
                                turret.defense = 2;
                                turret2.defense = 5;
                                turret3.defense = 4;
                                turret4.defense = 2;
                                weakPoint.defense = 3;
                            }
                            else
                            {
                                turret.defense = 2;
                                turret2.defense = 5;
                                turret3.defense = 4;
                                turret4.defense = 2;
                                weakPoint.defense = 1;
                            }
                            #endregion
                        }
                        else if (attack == 3)
                        {
                            #region
                            attackTimer++;
                            if(weakTimer>=3)
                            {
                                weakPoint.defense = 4;
                                if(weakTimer >= 7)
                                {
                                    weakTimer = 0;
                                    attack = 0;
                                }
                            }
                            if (attackTimer == 180)
                            {
                                turret.defense = 7;
                                turret2.defense = 6;
                                turret3.defense = 7;
                                turret4.defense = 7;
                                
                                weakTimer++;
                                attackTimer = 0;
                            }
                            else if (attackTimer == 150)
                            {
                                turret.defense = 7;
                                turret2.defense = 7;
                                turret3.defense = 6;
                                turret4.defense = 7;
                                
                            }
                            else if (attackTimer == 120)
                            {
                                turret.defense = 7;
                                turret2.defense = 7;
                                turret3.defense = 7;
                                turret4.defense = 6;
                                
                            }
                            else if (attackTimer == 90)
                            {
                                turret.defense = 7;
                                turret2.defense = 7;
                                turret3.defense = 6;
                                turret4.defense = 7;
                                
                            }
                            else if(attackTimer==60)
                            {
                                turret.defense = 7;
                                turret2.defense = 6;
                                turret3.defense = 7;
                                turret4.defense = 7;
                                
                            }
                            else if (attackTimer==30)
                            {
                                turret.defense = 6;
                                turret2.defense = 7;
                                turret3.defense = 7;
                                turret4.defense = 7;
                                
                            }
                            else if(attackTimer == 0)
                            {
                                turret.defense = 7;
                                turret2.defense = 7;
                                turret3.defense = 7;
                                turret4.defense = 7;
                                weakPoint.defense = 1;
                            }
                            else
                            {
                                turret.defense = 7;
                                turret2.defense = 7;
                                turret3.defense = 7;
                                turret4.defense = 7;
                            }
                            #endregion
                        }
                        else if (attack == 4)
                        {
                            #region
                            attackTimer++;
                            if (attackTimer>1500)
                            {
                                attack = 0;
                                //attackTimer = 0;
                            }
                            else if (attackTimer > 1200)
                            {
                                turret.defense = 1;
                                turret2.defense = 1;
                                turret3.defense = 1;
                                turret4.defense = 1;
                                weakPoint.defense = 5;
                            }
                            else if(attackTimer > 600)
                            {
                                turret.defense = 8;
                                turret2.defense = 9;
                                turret3.defense = 9;
                                turret4.defense = 8;
                            }
                            else if (attackTimer > 540)
                            {
                                turret.defense = 8;
                                turret2.defense = 1;
                                turret3.defense = 9;
                                turret4.defense = 8;
                            }
                            else if (attackTimer>120)
                            {
                                turret.defense = 8;
                                turret2.defense = 1;
                                turret3.defense = 1;
                                turret4.defense = 8;
                            }
                            else
                            {
                                turret.defense = 8;
                                turret2.defense = 1;
                                turret3.defense = 1;
                                turret4.defense =7;
                            }
                            #endregion
                        }
                        else
                        {
                            
                            if(!did70Attack && below70)
                            {
                                #region
                                notOffscreen = false;
                                
                                Vector2 target = new Vector2(player.Center.X, player.Center.Y - 500);
                                Vector2 moveTo = new Vector2(target.X, target.Y) - npc.Center;
                                npc.velocity = (moveTo) * .1f;
                                if (Main.netMode != 1)
                                {
                                    offtimer++;
                                    if (offtimer > 24)
                                    {
                                        
                                            float shotSpeed = 3f;
                                            float theta = MathHelper.ToRadians(Main.rand.Next(0, 360));
                                            float startDistance = 1000f;
                                            Projectile.NewProjectile(player.Center.X + (float)Math.Cos(theta) * startDistance, player.Center.Y + (float)Math.Sin(theta) * startDistance, -(float)Math.Cos(theta) * shotSpeed, -(float)Math.Sin(theta) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                                        
                                        offtimer = 0;
                                    }



                                    if (!summoned70)
                                    {
                                        miniboss1 = Main.npc[NPC.NewNPC((int)player.Center.X - 2000, (int)player.Center.Y, mod.NPCType("Minion"))];

                                        laser.Kill();
                                        laser2.Kill();
                                        summoned70 = true;
                                    }
                                    else if (!miniboss1.active)
                                    {
                                        did70Attack = true;
                                        notOffscreen = true;
                                        timer = 0;
                                        

                                            npc.netUpdate = true;
                                        
                                    }
                                }
                                #endregion
                            }
                            else if (!did40Attack && below40)
                            {
                                #region
                                notOffscreen = false;
                                Vector2 target = new Vector2(player.Center.X, player.Center.Y - 500);
                                Vector2 moveTo = new Vector2(target.X, target.Y) - npc.Center;
                                npc.velocity = (moveTo) * .1f;
                                if (Main.netMode != 1)
                                {
                                    offtimer++;
                                    if (offtimer % 18 == 0)
                                    {
                                        float shotSpeed = 3f;
                                        float theta = MathHelper.ToRadians(Main.rand.Next(0, 360));
                                        float startDistance = 1000f;
                                        Projectile.NewProjectile(player.Center.X + (float)Math.Cos(theta) * startDistance, player.Center.Y + (float)Math.Sin(theta) * startDistance, -(float)Math.Cos(theta) * shotSpeed, -(float)Math.Sin(theta) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);

                                    }
                                    if (offtimer > 120)
                                    {
                                        float shotSpeed = 3f;
                                        float theta = MathHelper.ToRadians(Main.rand.Next(0, 360));
                                        float startDistance = 1000f;
                                        Projectile.NewProjectile(player.Center.X + (float)Math.Cos(theta) * startDistance, player.Center.Y + (float)Math.Sin(theta) * startDistance, -(float)Math.Cos(theta) * shotSpeed * 1.5f, -(float)Math.Sin(theta) * shotSpeed * 1.5f, mod.ProjectileType("BurstShot2"), shotDamage, 0, Main.myPlayer);
                                        offtimer = 0;
                                    }
                                    if (!summoned40)
                                    {
                                        miniboss1 = Main.npc[NPC.NewNPC((int)player.Center.X - 2000, (int)player.Center.Y, mod.NPCType("Minion"))];

                                        laser.Kill();
                                        laser2.Kill();
                                        summoned40 = true;
                                    }
                                    else if (!miniboss1.active)
                                    {
                                        did40Attack = true;
                                        notOffscreen = true;
                                        timer = 0;
                                        

                                            npc.netUpdate = true;
                                        
                                    }
                                }
                                #endregion
                            }
                            else if (!did10Attack && below10)
                            {
                                #region
                                notOffscreen = false;
                                Vector2 target = new Vector2(player.Center.X, player.Center.Y - 500);
                                Vector2 moveTo = new Vector2(target.X, target.Y) - npc.Center;
                                npc.velocity = (moveTo) * .1f;
                                if (Main.netMode != 1)
                                {
                                    offtimer++;
                                    if (offtimer % 12 == 0)
                                    {
                                        float shotSpeed = 3f;
                                        float theta = MathHelper.ToRadians(Main.rand.Next(0, 360));
                                        float startDistance = 1000f;
                                        Projectile.NewProjectile(player.Center.X + (float)Math.Cos(theta) * startDistance, player.Center.Y + (float)Math.Sin(theta) * startDistance, -(float)Math.Cos(theta) * shotSpeed, -(float)Math.Sin(theta) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);

                                    }
                                    if (offtimer > 120)
                                    {

                                        count++;
                                        if (count > 5)
                                        {
                                            float shotSpeed = 3f;
                                            float theta = MathHelper.ToRadians(Main.rand.Next(0, 360));
                                            float startDistance = 1000f;
                                            Projectile.NewProjectile(player.Center.X + (float)Math.Cos(theta) * startDistance, player.Center.Y + (float)Math.Sin(theta) * startDistance, -(float)Math.Cos(theta) * shotSpeed * 1.5f, -(float)Math.Sin(theta) * shotSpeed * 1.5f, mod.ProjectileType("MegaBurst"), shotDamage, 0, Main.myPlayer);
                                            count = 0;
                                        }
                                        else
                                        {
                                            float shotSpeed = 3f;
                                            float theta = MathHelper.ToRadians(Main.rand.Next(0, 360));
                                            float startDistance = 1000f;
                                            Projectile.NewProjectile(player.Center.X + (float)Math.Cos(theta) * startDistance, player.Center.Y + (float)Math.Sin(theta) * startDistance, -(float)Math.Cos(theta) * shotSpeed * 1.5f, -(float)Math.Sin(theta) * shotSpeed * 1.5f, mod.ProjectileType("BurstShot2"), shotDamage, 0, Main.myPlayer);
                                        }

                                        offtimer = 0;
                                    }
                                    if (!summoned10)
                                    {
                                        miniboss1 = Main.npc[NPC.NewNPC((int)player.Center.X - 2000, (int)player.Center.Y, mod.NPCType("Minion"))];

                                        laser.Kill();
                                        laser2.Kill();
                                        summoned10 = true;
                                    }
                                    else if (!miniboss1.active)
                                    {
                                        did10Attack = true;
                                        notOffscreen = true;
                                        timer = 0;
                                        if (Main.netMode != 1)
                                        {

                                            npc.netUpdate = true;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                notOffscreen = true;
                                attackTimer++;
                                if (attackTimer > 180)
                                {
                                    nextAttack++;
                                    if (nextAttack > 4)
                                    {
                                        nextAttack = 1;
                                    }
                                    attack = nextAttack;
                                    attackTimer = 0;

                                }
                            }
                            turret.defense = 1;
                            turret2.defense = 1;
                            turret3.defense = 1;
                            turret4.defense = 1;
                            weakPoint.defense = 1;
                            
                        }

                    }
                    if (Math.Abs(player.Center.X - npc.Center.X) > npc.width / 2 && notOffscreen)
                    {
                        timer = 0;
                        if (Main.netMode != 1)
                        {

                            npc.netUpdate = true;
                        }
                        attackTimer = 0;
                        laser.Kill();
                        laser2.Kill();


                    }
                    if (player.Center.Y < weakPoint.Center.Y)
                    {
                        timer = 0;
                        if (Main.netMode != 1 )
                        {

                            npc.netUpdate = true;
                        }
                        attackTimer = 0;
                        laser.Kill();
                        laser2.Kill();
                    }

                }
                else 
                {
                    turret.defense = 1;
                    turret2.defense = 1;
                    turret3.defense = 1;
                    turret4.defense = 1;
                    weakPoint.defense = 1;
                }

                if (timer > 300 && notOffscreen)
                {
                    npc.velocity = new Vector2(0, 0);

                }
                else if(notOffscreen)
                {

                    Vector2 target = new Vector2(player.Center.X, player.Center.Y - 500);
                    Vector2 moveTo = new Vector2(target.X, target.Y) - npc.Center;
                    

                    npc.velocity = (moveTo) * .1f;
                    for (int p = 0; p < 200; p++)
                    {
                        if (Main.projectile[p].type == mod.ProjectileType("SuperLaser") && Main.projectile[p].active)
                        {
                            Main.projectile[p].Kill();
                        }
                    }
                    player.AddBuff(mod.BuffType("HealingHalt"), 60);
                }
                if (player.Center.Y - weakPoint.Center.Y > 1000)
                {
                    npc.velocity.Y += 3;
                }
                
            }
            else
            {
                #region
                laser.Kill();
                laser2.Kill();
                turret.defense = 1;
                turret2.defense = 1;
                turret3.defense = 1;
                turret4.defense = 1;
                weakPoint.defense = 1;
                deathTmier++;
                QwertyWorld.downedB4 = true;
                int fallSpeed = 400;
                if (deathTmier > 30)
                {
                    turret.velocity = new Vector2(0, fallSpeed);
                    turret.noGravity = false;
                    if (deathTmier > 60)
                    {
                        turret2.velocity = new Vector2(0, fallSpeed);
                        turret2.noGravity = false;
                        if (deathTmier > 90)
                        {
                            turret3.velocity = new Vector2(0, fallSpeed);
                            turret3.noGravity = false;
                           
                            if (deathTmier > 120)
                            {
                                turret4.velocity = new Vector2(0, fallSpeed);
                                turret4.noGravity = false;
                                turret.life = 0;
                                turret.checkDead();
                                if (deathTmier > 150)
                                {
                                    turret2.life = 0;
                                turret2.checkDead();
                                    background.velocity = new Vector2(0, fallSpeed);
                                    background.noGravity = false;
                                    
                                    if (deathTmier > 180)
                                    {
                                        turret3.life = 0;
                                        turret3.checkDead();

                                        if (deathTmier > 210)
                                        {
                                            turret4.life = 0;
                                            turret4.checkDead();
                                            if (deathTmier > 270)
                                            {
                                                background.life = 0;
                                                background.checkDead();
                                                npc.life = 0;
                                                npc.checkDead();
                                                laser.Kill();
                                                laser2.Kill();

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            if (true)
            {
                if(!weakPoint.active)
                {
                    startDeathSequence = true;

                    




                }
            }
            /*
            if (Main.netMode == 1)
            {
                Main.NewText("client: " + did70Attack);
            }


            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + did70Attack), Color.White);
            }
            */


        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(timer);
            writer.Write(did70Attack);
            writer.Write(did40Attack);
            writer.Write(did10Attack);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            timer = reader.ReadInt32();
            did70Attack = reader.ReadBoolean();
            did40Attack = reader.ReadBoolean();
            did10Attack = reader.ReadBoolean();
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <=0)
            {
                laser.Kill();
                laser2.Kill();
            }

        }
        

    }
    
    // The following laser shows a channeled ability, after charging up the laser will be fired
    // Using custom drawing, dust effects, and custom collision checks for tiles
    public class SideLaser : ModProjectile
    {
        // The maximum charge value
        private const float MaxChargeValue = 50f;
        //The distance charge particle from the player center
        private const float MoveDistance = 60f;

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
            
            projectile.position = new Vector2(shooter.Center.X + projectile.ai[1], shooter.Center.Y+ downFromCenter) + projectile.velocity * MoveDistance;
            projectile.timeLeft = 2;
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
            unit *= -1;
            for (Distance = MoveDistance; Distance <= 2200f; Distance += 5f)
            {
                start = new Vector2(shooter.Center.X + projectile.ai[1], shooter.Center.Y + downFromCenter) + projectile.velocity * Distance;
                /*
                if (!Collision.CanHit(new Vector2(shooter.Center.X + projectile.ai[1], shooter.Center.Y + downFromCenter), 1, 1, start, 1, 1))
                {
                    Distance -= 5f;
                    break;
                }
                */
            }

            

            //Add lights
            /*
            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (Distance - MoveDistance), 26,
                DelegateMethods.CastLight);
                */

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            /*
            if (AtMaxCharge)
            {
                DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], new Vector2(shooter.Center.X + projectile.ai[1], shooter.Center.Y + downFromCenter),
                    projectile.velocity, 188, projectile.damage, -1.57f, 1f, 4000f, lightColor, (int)MoveDistance);
            }
            */
            return false;
        }

        // The core function of drawing a laser
        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 4000f, Color color = default(Color), int transDist = 50)
        {
            Vector2 origin = start;
            float r = unit.ToRotation() + rotation;

            #region Draw laser body
            if(projectile.ai[1] > 0)
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
}
