using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace QwertysRandomContent.NPCs.AncientMachine
{
    public class AncientMachine : ModNPC
    {
        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Ancient Machine");
            Main.npcFrameCount[npc.type] = 4;

        }

        public override void SetDefaults()
        {

            npc.width = 392;
            npc.height = 348;
            npc.damage = 50;
            npc.defense = 18;
            npc.boss = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 60f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            //aiType = 10;
            animationType = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/BuiltToDestroy");
            npc.lifeMax = 7500;
            bossBag = mod.ItemType("AncientMachineBag");
            npc.buffImmune[20] = true;
            

        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {

            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * .6f);

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
                return 0f;
        }
        void ADI(int amount, Vector2 position)
        {
            for (int i = 0; i < amount; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(position, mod.DustType("AncientGlow"), QwertyMethods.PolarVector(Main.rand.Next(amount/200, amount/20), theta));
                dust.noGravity = true;
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if(npc.life <=0)
            {
               
                Vector2 pos = npc.Center + QwertyMethods.PolarVector(98, npc.rotation) + QwertyMethods.PolarVector(120, npc.rotation + (float)Math.PI / 2);
                Gore gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/Debris_1"), 1f)];
                gore.rotation = npc.rotation;

                pos = npc.Center + QwertyMethods.PolarVector(98, npc.rotation) + QwertyMethods.PolarVector(120, npc.rotation + (float)Math.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/Debris_2"), 1f)];
                gore.rotation = npc.rotation;

                pos = npc.Center + QwertyMethods.PolarVector(144, npc.rotation) + QwertyMethods.PolarVector(67, npc.rotation + (float)Math.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/Debris_3"), 1f)];
                gore.rotation = npc.rotation;
                pos = npc.Center + QwertyMethods.PolarVector(144, npc.rotation) + QwertyMethods.PolarVector(-67, npc.rotation + (float)Math.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/Debris_4"), 1f)];
                gore.rotation = npc.rotation;

                pos = npc.Center + QwertyMethods.PolarVector(-15, npc.rotation) + QwertyMethods.PolarVector(102, npc.rotation + (float)Math.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/Debris_5"), 1f)];
                gore.rotation = npc.rotation;
                pos = npc.Center + QwertyMethods.PolarVector(-15, npc.rotation) + QwertyMethods.PolarVector(-102, npc.rotation + (float)Math.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/Debris_6"), 1f)];
                gore.rotation = npc.rotation;

                pos = npc.Center + QwertyMethods.PolarVector(-15, npc.rotation) + QwertyMethods.PolarVector(0, npc.rotation + (float)Math.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/Debris_7"), 1f)];
                gore.rotation = npc.rotation;
                pos = npc.Center + QwertyMethods.PolarVector(-15, npc.rotation) + QwertyMethods.PolarVector(0, npc.rotation + (float)Math.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/Debris_8"), 1f)];
                gore.rotation = npc.rotation;
                pos = npc.Center + QwertyMethods.PolarVector(-154, npc.rotation) + QwertyMethods.PolarVector(0, npc.rotation + (float)Math.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/Debris_9"), 1f)];
                gore.rotation = npc.rotation;
                pos = npc.Center + QwertyMethods.PolarVector(77, npc.rotation) + QwertyMethods.PolarVector(0, npc.rotation + (float)Math.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/Debris_10"), 1f)];
                gore.rotation = npc.rotation;
                pos = npc.Center + QwertyMethods.PolarVector(166, npc.rotation) + QwertyMethods.PolarVector(0, npc.rotation + (float)Math.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/Debris_11"), 1f)];
                gore.rotation = npc.rotation;

                pos = npc.Center + QwertyMethods.PolarVector(-65, npc.rotation) + QwertyMethods.PolarVector(79, npc.rotation + (float)Math.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/Debris_12"), 1f)];
                gore.rotation = npc.rotation;
                pos = npc.Center + QwertyMethods.PolarVector(-65, npc.rotation) + QwertyMethods.PolarVector(-79, npc.rotation + (float)Math.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/Debris_13"), 1f)];
                gore.rotation = npc.rotation;

            }
            

        }
        Vector2 MissileOffset = new Vector2();
        int frame = 0;
        const int defaultFrameX = 22;
        const int defaultFrameY = 148;
        public override void FindFrame(int frameHeight)
        {
            

            npc.frameCounter++;

            if (npc.frameCounter > 10)
            {
                frame++;
                if (frame >= 7)
                {
                    frame = 0;
                }
                npc.frameCounter = 0;
            }
            if (frame > 3)
            {
                npc.frame.Y = (frame - 4) * frameHeight;
                npc.frame.X = npc.width;
                npc.frame.Width = npc.width;
            }
            else
            {
                npc.frame.Y = frame * frameHeight;
                npc.frame.X = 0;
                npc.frame.Width = npc.width;
            }
            switch(frame)
            {
                case 0:
                    MissileOffset = new Vector2(defaultFrameX, defaultFrameY);
                    break;
                case 1:
                    MissileOffset = new Vector2(defaultFrameX-2, defaultFrameY+2);
                    break;
                case 2:
                    MissileOffset = new Vector2(defaultFrameX-2, defaultFrameY+6);
                    break;
                case 3:
                    MissileOffset = new Vector2(defaultFrameX-2, defaultFrameY+8);
                    break;
                case 4:
                    MissileOffset = new Vector2(defaultFrameX-4, defaultFrameY+10);
                    break;
                case 5:
                    MissileOffset = new Vector2(defaultFrameX+2, defaultFrameY+6);
                    break;
                case 6:
                    MissileOffset = new Vector2(defaultFrameX, defaultFrameY);
                    break;
            }


        }
        public const int RingRadius = 300;
        public const int RingDustQty = 400;
        public int damage = 30;
        public int switchTime = 150;
        public int moveCount = -1;
        public int fireCount = 0;
        public int attackType = 1;
        public int AI_Timer = 0;
        public int AI_Timer2 = 0;
        public bool runOnce = true;
        Vector2 moveTo;
        float orbSpeed=12;
        bool angry;
        bool justTeleported;
        int missileReloadCounter;
        int missileFrame=0;
        int missileFlashCounter;
        int missileGlowFrame = 0;
        float angle = (float)Math.PI / 6;
        public override void AI()
        {
            missileFlashCounter++;
            if(missileFlashCounter>60)
            {
                missileFlashCounter = 0;
                
            }
            else if(missileFlashCounter>30)
            {
                missileGlowFrame = 1;
            }
            else
            {
                missileGlowFrame = 0;
            }
            if(missileReloadCounter>0)
            {
                missileReloadCounter--;
            }
            
            //Main.NewText(npc.Size);
            //Main.NewText(npc.scale);
            if (npc.life < npc.lifeMax/2 && Main.expertMode)
            {
                angry = true;
            }
            switchTime = (int)(((float)npc.life / (float)npc.lifeMax) * 60) + 90;
            Player player = Main.player[npc.target];
            npc.TargetClosest(true);
            if (runOnce)
            {

                if (Main.netMode != 1)
                {
                    npc.ai[0] = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                    npc.netUpdate = true;
                   
                }
                runOnce = false;
                 moveTo = new Vector2(player.Center.X + (float)Math.Cos(npc.ai[0]) * 700, player.Center.Y + (float)Math.Sin(npc.ai[0]) * 400);
            }
            AI_Timer++;
            AI_Timer2++;
            
            if (Main.expertMode)
            {
                #region exerpt aggression
                damage = 20;
                
                #endregion
            }
            
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0f, 10f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }
            float targetAngle = new Vector2(player.Center.X - npc.Center.X, player.Center.Y - npc.Center.Y).ToRotation();


            npc.rotation = targetAngle;




            


            /*
            if( AI_Timer<6)
            {
            Vector2 teleTo = new Vector2(player.Center.X + 400f, player.Center.Y + -400f );
            npc.position = (teleTo);
            }
            */







            if (AI_Timer > switchTime)
            {
                moveCount++;
                //Main.NewText(moveCount);
                for (int i = 0; i < RingDustQty; i++)
                {
                    float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);

                    Dust dust = Dust.NewDustPerfect(npc.Center + QwertyMethods.PolarVector(RingRadius, theta), mod.DustType("AncientGlow"), QwertyMethods.PolarVector(-RingRadius / 10, theta));
                    dust.noGravity = true;
                }
                if (Main.netMode != 1)
                {
                   


                    npc.ai[0] = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                    npc.netUpdate = true;
                }
                moveTo = new Vector2(player.Center.X + (float)Math.Cos(npc.ai[0]) * 700, player.Center.Y + (float)Math.Sin(npc.ai[0]) * 400);
                if(Main.netMode !=1)
                {
                    npc.ai[2] = moveTo.X;
                    npc.ai[3] = moveTo.Y;
                    npc.netUpdate = true;
                }
                justTeleported = true;
                AI_Timer = 0;
                AI_Timer2 = 0;
                
            }
            if (moveCount >= 3)
            {
                #region special attacks
                npc.velocity = new Vector2(0, 0);


                if (AI_Timer == switchTime / 2)
                {
                    if (Main.netMode != 1)
                    {

                        npc.ai[1] = Main.rand.Next(3);
                        npc.netUpdate = true;
                        /*
                        NPC.NewNPC((int)(player.Center.X + 565.7f), (int)npc.Center.Y, mod.NPCType("AncientMinion"));
                        NPC.NewNPC((int)(player.Center.X + -565.7f), (int)npc.Center.Y, mod.NPCType("AncientMinion"));

                        if (Main.expertMode)
                        {
                            NPC.NewNPC((int)npc.Center.X, (int)(player.Center.Y + -565.7f), mod.NPCType("AncientMinion"));
                            NPC.NewNPC((int)npc.Center.X, (int)(player.Center.Y + 565.7f), mod.NPCType("AncientMinion"));
                        }
                        */
                    }

                    if (npc.ai[1] == 0)
                    {
                        Main.PlaySound(25, npc.position, 0);
                        for (int r = 0; r < 5; r++)
                        {
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos((npc.rotation + r * (float)Math.PI / 8) - (float)Math.PI / 4) * orbSpeed, (float)Math.Sin((npc.rotation + r * (float)Math.PI / 8) - (float)Math.PI / 4) * orbSpeed, mod.ProjectileType("AncientEnergy"), damage, 3f, Main.myPlayer);
                            }
                        }
                    }
                    if (npc.ai[1] == 1)
                    {
                        Main.PlaySound(25, npc.position, 0);
                        missileReloadCounter = 60;
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center + QwertyMethods.PolarVector(MissileOffset.X, npc.rotation) +QwertyMethods.PolarVector(MissileOffset.Y, npc.rotation +(float)Math.PI/2), QwertyMethods.PolarVector(orbSpeed, npc.rotation+ angle), mod.ProjectileType("AncientMissile"), damage, 3f, Main.myPlayer);
                            Projectile.NewProjectile(npc.Center + QwertyMethods.PolarVector(MissileOffset.X, npc.rotation) + QwertyMethods.PolarVector(-MissileOffset.Y, npc.rotation + (float)Math.PI / 2), QwertyMethods.PolarVector(orbSpeed, npc.rotation- angle), mod.ProjectileType("AncientMissile"), damage, 3f, Main.myPlayer);

                        }

                    }
                    if (npc.ai[1] == 2)
                    {
                        

                        if (Main.netMode != 1)
                        {
                            float d = new Vector2(player.Center.X - npc.Center.X, player.Center.Y - npc.Center.Y).ToRotation();
                            Vector2 pos = npc.Center + QwertyMethods.PolarVector(200, npc.rotation) + QwertyMethods.PolarVector(100, npc.rotation + (float)Math.PI / 2);
                            NPC.NewNPC((int)pos.X, (int)pos.Y, mod.NPCType("AncientMinion"), 0 , npc.whoAmI);
                            pos = npc.Center + QwertyMethods.PolarVector(200, npc.rotation) + QwertyMethods.PolarVector(-100, npc.rotation + (float)Math.PI / 2);
                            NPC.NewNPC((int)pos.X, (int)pos.Y, mod.NPCType("AncientMinion"), 0, npc.whoAmI);
                            if (angry)
                            {
                                pos = npc.Center + QwertyMethods.PolarVector(100, npc.rotation) + QwertyMethods.PolarVector(-200, npc.rotation + (float)Math.PI / 2);
                                NPC.NewNPC((int)pos.X, (int)pos.Y, mod.NPCType("AncientMinion"), 0, npc.whoAmI);
                                pos = npc.Center + QwertyMethods.PolarVector(100, npc.rotation) + QwertyMethods.PolarVector(200, npc.rotation + (float)Math.PI / 2);
                                NPC.NewNPC((int)pos.X, (int)pos.Y, mod.NPCType("AncientMinion"), 0, npc.whoAmI);
                            }
                        }

                    }


                }
                if (AI_Timer == 3 * switchTime / 4)
                {
                    if (angry)
                    {
                        if (npc.ai[1] == 0)
                        {
                            Main.PlaySound(25, npc.position, 0);
                            for (int r = 0; r < 4; r++)
                            {
                                if (Main.netMode != 1)
                                {
                                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos((npc.rotation + r * (float)Math.PI / 6) - (float)Math.PI / 4) * orbSpeed, (float)Math.Sin((npc.rotation + r * (float)Math.PI / 6) - (float)Math.PI / 4) * orbSpeed, mod.ProjectileType("AncientEnergy"), damage, 3f, Main.myPlayer);
                                }
                            }
                        }
                        if (npc.ai[1] == 1)
                        {
                            Main.PlaySound(25, npc.position, 0);
                            missileReloadCounter = 60;
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.Center + QwertyMethods.PolarVector(MissileOffset.X, npc.rotation) + QwertyMethods.PolarVector(MissileOffset.Y, npc.rotation + (float)Math.PI / 2), QwertyMethods.PolarVector(orbSpeed, npc.rotation+ angle), mod.ProjectileType("AncientMissile"), damage, 3f, Main.myPlayer);
                                Projectile.NewProjectile(npc.Center + QwertyMethods.PolarVector(MissileOffset.X, npc.rotation) + QwertyMethods.PolarVector(-MissileOffset.Y, npc.rotation + (float)Math.PI / 2), QwertyMethods.PolarVector(orbSpeed, npc.rotation- angle), mod.ProjectileType("AncientMissile"), damage, 3f, Main.myPlayer);

                            }

                        }
                    }
                    moveCount = -1;
                }


                #endregion

            }
            else
            {
                if (AI_Timer == switchTime / 2)
                {
                    Main.PlaySound(25, npc.position, 0);
                    if (Main.netMode != 1)
                    {
                        
                        Projectile.NewProjectile(npc.Center, new Vector2((float)Math.Cos((npc.rotation)), (float)Math.Sin(npc.rotation)) * orbSpeed, mod.ProjectileType("AncientEnergy"), damage, 3f, Main.myPlayer);
                    }
                }
                if (AI_Timer == 3 * switchTime / 4 && angry)
                {
                    Main.PlaySound(25, npc.position, 0);
                    if (Main.netMode != 1)
                    {
                        
                        Projectile.NewProjectile(npc.Center, new Vector2((float)Math.Cos((npc.rotation)), (float)Math.Sin(npc.rotation)) * orbSpeed, mod.ProjectileType("AncientEnergy"), damage, 3f, Main.myPlayer);
                    }
                }
            }
            //npc.velocity = (moveTo - npc.Center) * .02f;
            npc.Center = new Vector2(npc.ai[2], npc.ai[3]);








            if(justTeleported)
            {
                Main.PlaySound(SoundID.Item8, npc.position);
                for (int i = 0; i < RingDustQty; i++)
                {
                    float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                    Dust dust = Dust.NewDustPerfect(npc.Center, mod.DustType("AncientGlow"), QwertyMethods.PolarVector(RingRadius / 10, theta));
                    dust.noGravity = true;
                }
                justTeleported = false;
            }









        }
        
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Rectangle mF = new Rectangle(0, missileGlowFrame * 36, 20, 36);
            
            /*
            spriteBatch.Draw(mod.GetTexture("NPCs/AncientMachine/AncientMachineEquipedMissile"), new Vector2(npc.Center.X - Main.screenPosition.X, npc.Center.Y - Main.screenPosition.Y),
                        new Rectangle(missileGlowFrame * 392, missileFrame*380, 392, 380), drawColor, npc.rotation,
                        new Vector2(npc.width * 0.5f, npc.height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("NPCs/AncientMachine/AncientMachineEquipedMissile_Glow"), new Vector2(npc.Center.X - Main.screenPosition.X, npc.Center.Y - Main.screenPosition.Y),
                        new Rectangle(missileGlowFrame * 392, missileFrame * 380, 392, 380), Color.White, npc.rotation,
                        new Vector2(npc.width * 0.5f, npc.height * 0.5f), 1f, SpriteEffects.None, 0f);*/
            spriteBatch.Draw(mod.GetTexture("NPCs/AncientMachine/AncientMissile"), npc.Center - Main.screenPosition + QwertyMethods.PolarVector(MissileOffset.X, npc.rotation) + QwertyMethods.PolarVector(MissileOffset.Y, npc.rotation + (float)Math.PI / 2) + QwertyMethods.PolarVector(-missileReloadCounter/2, npc.rotation+angle),
                        mF, drawColor, npc.rotation + (float)Math.PI / 2 + angle,
                        new Vector2(mF.Width * 0.5f, mF.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("NPCs/AncientMachine/AncientMissile"), npc.Center - Main.screenPosition + QwertyMethods.PolarVector(MissileOffset.X, npc.rotation) + QwertyMethods.PolarVector(-MissileOffset.Y, npc.rotation + (float)Math.PI / 2) + QwertyMethods.PolarVector(-missileReloadCounter/2, npc.rotation - angle),
                        mF, drawColor, npc.rotation + (float)Math.PI / 2 - angle,
                        new Vector2(mF.Width * 0.5f, mF.Height * 0.5f), 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(mod.GetTexture("NPCs/AncientMachine/AncientMissile_Glow"), npc.Center - Main.screenPosition + QwertyMethods.PolarVector(MissileOffset.X, npc.rotation) + QwertyMethods.PolarVector(MissileOffset.Y, npc.rotation + (float)Math.PI / 2) + QwertyMethods.PolarVector(-missileReloadCounter / 2, npc.rotation + angle),
                        mF, Color.White, npc.rotation + (float)Math.PI / 2 + angle,
                        new Vector2(mF.Width * 0.5f, mF.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("NPCs/AncientMachine/AncientMissile_Glow"), npc.Center - Main.screenPosition + QwertyMethods.PolarVector(MissileOffset.X, npc.rotation) + QwertyMethods.PolarVector(-MissileOffset.Y, npc.rotation + (float)Math.PI / 2) + QwertyMethods.PolarVector(-missileReloadCounter / 2, npc.rotation - angle),
                        mF, Color.White, npc.rotation + (float)Math.PI / 2 - angle,
                        new Vector2(mF.Width * 0.5f, mF.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("NPCs/AncientMachine/AncientMachine"), npc.Center - Main.screenPosition,
                        npc.frame, drawColor, npc.rotation,
                        new Vector2(npc.width * 0.5f, npc.height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("NPCs/AncientMachine/AncientMachine_Glow"), npc.Center - Main.screenPosition,
                        npc.frame, Color.White, npc.rotation,
                        new Vector2(npc.width * 0.5f, npc.height * 0.5f), 1f, SpriteEffects.None, 0f);

            return false;
        }

        public override void NPCLoot()
        {
            QwertyWorld.downedAncient = true;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state

            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
               
                string[] loot = QwertysRandomContent.AMLoot.Draw(2);

                foreach (string item in loot)
                {
                    Item.NewItem(npc.getRect(), mod.ItemType(item));
                }
                  
               

                if (Main.rand.Next(100) < 15)
                {
                    Item.NewItem(npc.getRect(), mod.ItemType("AncientMiner"));
                }
                Item.NewItem(npc.getRect(), 73, 8);
            }
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("AncientMachineTrophy"));
            }


        }
        public override void BossHeadSlot(ref int index)
        {

            index = NPCHeadLoader.GetBossHeadSlot(QwertysRandomContent.AncientMachineHead);

        }
       
    }

    public class AncientEnergy : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Energy");
            Main.projFrames[projectile.type] = 1;

        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 62;
            projectile.height = 62;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
            projectile.tileCollide = false;
            projectile.alpha = 255;

        }
        
        public int dustTimer;

        public override void AI()
        {
            if (projectile.alpha > 0)
            {
                //projectile.alpha -= (int)(255f / 180f);
                projectile.alpha -= 2;
            }
            else
            {
                projectile.alpha = 0;
            }
            projectile.scale = .5f + (.5f * 1 - (projectile.alpha / 255f));
            for(int d =0; d < projectile.alpha/10; d++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(70, theta), mod.DustType("AncientGlow"), QwertyMethods.PolarVector(-10, theta) + projectile.velocity);
                
                dust.alpha = 255;
            }
            //Main.NewText(projectile.alpha);
            dustTimer++;
            if (dustTimer > 2)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("AncientGlow"), 0, 0, 0, default(Color), .4f);
                
                dustTimer = 0;
            }
            projectile.frameCounter++;
            
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            
            spriteBatch.Draw(mod.GetTexture("NPCs/AncientMachine/AncientEnergy"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), Color.Lerp(new Color(1f, 1f, 1f, 1f), new Color(0, 0, 0, 0), (float)projectile.alpha/255f), projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
    public class AncientMissile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Missile");

            Main.projFrames[projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 20;
            projectile.height = projectile.width;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 240;
            projectile.tileCollide = false;


        }
        
        public int dustTimer;
        float direction;
        float missileAcceleration = .5f;
        float topSpeed=10f;
        int timer;
        float closest= 10000;
        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter % 30 == 0)
            {
                projectile.frame++;
                if (projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
            }
            timer++;
            if (timer > 30)
            {
                //Player player = Main.player[projectile.owner];
                if (Main.netMode != 1)
                {
                    for (int i = 0; i < 255; i++)
                    {
                        if(Main.player[i].active && (projectile.Center - Main.player[i].Center).Length()<closest)
                        {
                            closest = (projectile.Center - Main.player[i].Center).Length();
                            projectile.ai[0] = (Main.player[i].Center - projectile.Center).ToRotation();
                            projectile.netUpdate = true;
                        }
                        
                    }
                }
                projectile.velocity += new Vector2((float)Math.Cos(projectile.ai[0]) * missileAcceleration, (float)Math.Sin(projectile.ai[0]) * missileAcceleration);
                if (projectile.velocity.Length() > topSpeed)
                {
                    projectile.velocity = projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10;
                }

            }
            //int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("AncientGlow"), 0, 0, 0, default(Color), .4f);
            Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(26, projectile.rotation+(float)Math.PI/2)  + QwertyMethods.PolarVector(Main.rand.Next(-6, 6), projectile.rotation), mod.DustType("AncientGlow"));
            closest = 10000;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("AncientBlast"), projectile.damage, projectile.knockBack, player.whoAmI);

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(mod.GetTexture("NPCs/AncientMachine/AncientMissile"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, projectile.frame * 36, projectile.width, 36), drawColor, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("NPCs/AncientMachine/AncientMissile_Glow"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, projectile.frame * 36, projectile.width, 36), Color.White, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
    public class AncientBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Blast");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 150;
            projectile.height = 150;
            projectile.friendly = true;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 2;
            projectile.usesLocalNPCImmunity = true;


        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.width = 150;
            projectile.height = 150;


            
            Main.PlaySound(SoundID.Item62, projectile.position);


            for (int i = 0; i < 400; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(projectile.Center, mod.DustType("AncientGlow"), QwertyMethods.PolarVector(Main.rand.Next(2, 20), theta));
                dust.noGravity = true;
                

            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

    }
}