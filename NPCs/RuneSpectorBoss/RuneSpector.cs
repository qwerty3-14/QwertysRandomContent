using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.RuneSpectorBoss
{
    [AutoloadBossHead]
    class RuneSpector : ModNPC
    {
        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Rune Ghost");
            Main.npcFrameCount[npc.type] = 12;

        }
        public override void SetDefaults()
        {

            npc.width = 82;
            npc.height = 90;
            npc.damage = 0;
            npc.defense = 42;
            npc.knockBackResist = 0f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 60f;

            npc.aiStyle = -1;
            npc.boss = true;
            animationType = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.alpha = 0;
            npc.lifeMax = 60000;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/TheConjurer");
            bossBag = mod.ItemType("RuneGhostBag");

        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }
        /*
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.666666f * bossLifeScale);
            npc.damage = (int)(npc.damage * .4f);
        }
        */
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                QwertyWorld.downedRuneGhost = true;

            }
            
        }
        
        public void newRune(float posX, float posY)
        {
            if (Main.netMode != 1)
            {
                int damage; //for hostile projectiles damage is doubled in normal and quadrupled in expert
                if (Main.expertMode)
                {
                    damage = 28;
                }
                else
                {
                    damage = 40;
                }
                int selectRune = Main.rand.Next(1, 5);
                if (selectRune == 1)
                {
                    Projectile.NewProjectile(posX - 0, posY - 0, 0, 0, mod.ProjectileType("RedRune"), 200, 3f, Main.myPlayer); //The red rune never summons a hostile rune so it's damage is decided in the projectile itself
                }
                else if (selectRune == 2)
                {
                    Projectile.NewProjectile(posX - 0, posY - 0, 0, 0, mod.ProjectileType("GreenRune"), damage, 3f, Main.myPlayer);
                }
                else if (selectRune == 3)
                {
                    Projectile.NewProjectile(posX - 0, posY - 0, 0, 0, mod.ProjectileType("PurpleRune"), damage, 3f, Main.myPlayer);
                }
                else if (selectRune == 4)
                {
                    Projectile.NewProjectile(posX - 0, posY - 0, 0, 0, mod.ProjectileType("CyanRune"), (int)(damage*1.5f), 3f, Main.myPlayer);
                }
                /*
                else if (selectRune == 5)
                {
                    Projectile.NewProjectile(posX - 0, posY - 0, 0, 0, mod.ProjectileType("GrayRune"), damage, 3f, Main.myPlayer);
                }
                else if (selectRune == 6)
                {
                    Projectile.NewProjectile(posX - 0, posY - 0, 0, 0, mod.ProjectileType("OrangeRune"), damage*3, 3f, Main.myPlayer);
                }
                */
            }
        }
        public float flyDirection;
        public float flySpeed = 5;
        public int flyMode;
        //0 = flying
        //1 = transition
        //2 = rune draw
        public Vector2 flyTo;
        public int transitionTime;
        public bool runOnce = true;
        public int flyTime;
        
        public int Yshift = 250;
        public int runeMode = 1;
        // 1 = summons 1 rune above head
        // 2 = summons 2 runes from the arms
        // 3 = summons 3 runes 
        public bool quitOnce = true;
        public int quitCount;
        public bool isAngry = false;
        public bool isFurious = false;
        public int multiRunNumber = 1;
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
                    npc.alpha = 0;
                    if (quitOnce)
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("QuitRune"), 0, 3f, Main.myPlayer);
                        quitOnce = false;
                    }
                    flyMode = 2;
                    npc.velocity = new Vector2(0f, 0f);
                    quitCount++;
                    if(quitCount >=120)
                    {
                        npc.position.Y += 100000f;
                    }

                }

            }
            else
            {
                quitCount = 0;
                
                quitOnce = true;

                if (npc.life > npc.lifeMax)
                {
                    npc.life = npc.lifeMax;
                }
                if (runOnce)
                {
                    npc.ai[1] = 0;
                    runOnce = false;
                    if (Main.expertMode)
                    {
                        npc.lifeMax = 75000;
                        npc.life = npc.lifeMax;
                    }
                }

                if (Main.expertMode)
                {
                    if ((float)npc.life / npc.lifeMax < .2f)
                    {
                        runeMode = 3;
                        if(!isFurious)
                        {
                            string key = "Mods.QwertysRandomContent.GhostFurious";
                            Color messageColor = Color.Red;
                            if (Main.netMode == 2) // Server
                            {
                                NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                            }
                            else if (Main.netMode == 0) // Single Player
                            {
                                Main.NewText(Language.GetTextValue(key), messageColor);
                            }
                            isFurious = true;
                        }
                    }
                    else if ((float)npc.life / npc.lifeMax < .6f)
                    {
                        runeMode = 2;
                        if (!isAngry)
                        {
                            string key = "Mods.QwertysRandomContent.GhostAngry";
                            Color messageColor = Color.Red;
                            if (Main.netMode == 2) // Server
                            {
                                NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                            }
                            else if (Main.netMode == 0) // Single Player
                            {
                                Main.NewText(Language.GetTextValue(key), messageColor);
                            }
                            isAngry = true;
                        }
                    }
                    else
                    {
                        runeMode = 1;
                    }
                }
                else
                {
                    if ((float)npc.life / npc.lifeMax < .4f)
                    {
                        runeMode = 2;
                        if (!isFurious)
                        {
                            string key = "Mods.QwertysRandomContent.GhostFurious";
                            Color messageColor = Color.Red;
                            if (Main.netMode == 2) // Server
                            {
                                NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                            }
                            else if (Main.netMode == 0) // Single Player
                            {
                                Main.NewText(Language.GetTextValue(key), messageColor);
                            }
                            isFurious = true;
                        }
                    }
                    else
                    {
                        runeMode = 1;
                    }
                }



                flyDirection = (flyTo - npc.Center).ToRotation();
                if ((flyTo - npc.Center).Length() < 10)
                {

                    flyTime = 0;
                    flySpeed = 5;
                    npc.velocity = new Vector2(0, 0);
                    transitionTime++;
                    if (transitionTime >= 187)
                    {
                        if (Main.netMode != 1)
                        {
                            npc.ai[0] = Main.rand.Next(1, 4);
                            npc.netUpdate = true;
                        }
                        if (npc.ai[0] == 1)
                        {
                            if (Main.netMode != 1)
                            {
                                npc.ai[1] = 300;
                                npc.netUpdate = true;
                            }
                        }
                        else if (npc.ai[0] == 2)
                        {
                            if (Main.netMode != 1)
                            {
                                npc.ai[1] = 0;
                                npc.netUpdate = true;
                            }
                        }
                        else if (npc.ai[0] == 3)
                        {
                            if (Main.netMode != 1)
                            {
                                npc.ai[1] = -300;
                                npc.netUpdate = true;
                            }
                        }
                        Yshift *= -1;
                        
                        if (Main.netMode != 1)
                        {
                            npc.ai[2] = player.Center.X + npc.ai[1];
                            npc.ai[3] = player.Center.Y - Yshift;
                            npc.netUpdate = true;
                        }

                        flyTo = new Vector2(npc.ai[2], npc.ai[3]);

                    }
                    else if (transitionTime >= 157)
                    {
                        flyTo = npc.Center;
                        if (Main.netMode != 1)
                        {
                            npc.ai[2] = npc.Center.X;
                            npc.ai[3] = npc.Center.Y;
                            npc.netUpdate = true;
                        }

                        flyTo = new Vector2(npc.ai[2], npc.ai[3]);
                        flyMode = 3;
                    }
                    else if (transitionTime > 30)
                    {
                        flyTo = npc.Center;
                        if (Main.netMode != 1)
                        {
                            npc.ai[2] = npc.Center.X;
                            npc.ai[3] = npc.Center.Y;
                            npc.netUpdate = true;
                        }

                        flyTo = new Vector2(npc.ai[2], npc.ai[3]);
                        flyMode = 2;
                    }
                    else if (transitionTime == 30)
                    {
                        if (runeMode == 1)
                        {
                            npc.netUpdate = true;
                            Main.PlaySound(SoundID.Item43, npc.Center);
                            newRune(npc.Center.X, npc.Center.Y - 120);
                        }
                        else if (runeMode == 2)
                        {
                            npc.netUpdate = true;
                            Main.PlaySound(SoundID.Item43, npc.Center);
                            newRune(npc.Center.X - 120, npc.Center.Y);
                            newRune(npc.Center.X + 120, npc.Center.Y);
                        }
                        else if (runeMode == 3)
                        {
                            npc.netUpdate = true;
                            Main.PlaySound(SoundID.Item43, npc.Center);
                            newRune(npc.Center.X - 103.923f, npc.Center.Y + 60);
                            newRune(npc.Center.X + 103.923f, npc.Center.Y + 60);
                            newRune(npc.Center.X, npc.Center.Y - 120);
                        }
                        flyTo = npc.Center;
                        if (Main.netMode != 1)
                        {
                            npc.ai[2] = npc.Center.X;
                            npc.ai[3] = npc.Center.Y;
                            npc.netUpdate = true;
                        }

                         flyTo = new Vector2(npc.ai[2], npc.ai[3]); ;
                        flyMode = 2;
                    }
                    else
                    {
                        flyTo = npc.Center;
                        if (Main.netMode != 1)
                        {
                            npc.ai[2] = npc.Center.X;
                            npc.ai[3] = npc.Center.Y;
                            npc.netUpdate = true;
                        }

                        flyTo = new Vector2(npc.ai[2], npc.ai[3]);
                        flyMode = 1;
                    }

                }
                else if ((flyTo - npc.Center).Length() < 128)
                {

                    
                   
                    if (Main.netMode != 1)
                    {
                        npc.ai[2] = player.Center.X + npc.ai[1];
                        npc.ai[3] = player.Center.Y - Yshift;
                        npc.netUpdate = true;
                    }

                    flyTo = new Vector2(npc.ai[2], npc.ai[3]);
                    npc.velocity = new Vector2((float)Math.Cos(flyDirection) * flySpeed, (float)Math.Sin(flyDirection) * flySpeed);
                    flyMode = 0;
                    npc.alpha = (int)(flyTo - npc.Center).Length() * 2;
                    if (npc.alpha <= 0)
                        npc.alpha = 0;
                    transitionTime = 0;
                    flyTime++;
                    if (flyTime >= 300)
                    {
                        flySpeed = 20;
                    }
                }
                else
                {

                    if (Main.netMode != 1)
                    {
                        npc.ai[2] = player.Center.X + npc.ai[1];
                        npc.ai[3] = player.Center.Y - Yshift;
                        npc.netUpdate = true;
                    }

                    flyTo = new Vector2(npc.ai[2], npc.ai[3]);
                    npc.velocity = new Vector2((float)Math.Cos(flyDirection) * flySpeed, (float)Math.Sin(flyDirection) * flySpeed);
                    flyMode = 0;
                    if (npc.alpha >= 255)
                        npc.alpha = 255;
                    else
                        npc.alpha += 10;

                    transitionTime = 0;
                    flyTime++;
                    if (flyTime >= 300)
                    {
                        flySpeed = 20;
                    }


                }




            }
            if (npc.alpha == 255)
            {
                npc.dontTakeDamage = true;
            }
            else
            {
                npc.dontTakeDamage = false;
            }

            /*
            if (Main.netMode == 1)
            {
                Main.NewText("client: " + flyTo);
            }
            
            
            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + flyTo), Color.White);
            }
            */
        }
        public int blinkTime = 360;
        public int glow;
        int flymode1Timer;
        public override void FindFrame(int frameHeight)
        {
            if (flyMode == 0)
            {
                flymode1Timer = 0;
                if (npc.velocity.X > 0)
                {
                    npc.spriteDirection = 1;
                }
                else
                {
                    npc.spriteDirection = -1;
                }
                
                npc.frameCounter++;

                if (npc.frameCounter < blinkTime-30)
                {
                    npc.frame.Y = 0 * frameHeight;
                    glow = 0;
                }
                else if (npc.frameCounter < blinkTime-20)
                {
                    npc.frame.Y = 1 * frameHeight;
                    glow = 1;
                }
                else if (npc.frameCounter < blinkTime-10)
                {
                    npc.frame.Y = 2 * frameHeight;
                    glow = 2;
                }
                else if (npc.frameCounter < blinkTime)
                {
                    npc.frame.Y = 3 * frameHeight;
                    glow = 1;


                }
                else
                {
                    
                    npc.frameCounter = 0;
                    
                }
            }
            else if (flyMode == 1)
            {
                flymode1Timer++;
                if(flymode1Timer > 21)
                {
                    npc.frame.Y = 7 * frameHeight;
                }
                else if(flymode1Timer > 14)
                {
                    npc.frame.Y = 6 * frameHeight;
                }
                else if(flymode1Timer > 7)
                {
                    npc.frame.Y = 5 * frameHeight;
                }
                else
                {
                    npc.frame.Y = 4 * frameHeight;
                }
                
                
            }
            else if (flyMode == 3)
            {
                flymode1Timer--;
                if (flymode1Timer > 21)
                {
                    npc.frame.Y = 7 * frameHeight;
                }
                else if (flymode1Timer > 14)
                {
                    npc.frame.Y = 6 * frameHeight;
                }
                else if (flymode1Timer > 7)
                {
                    npc.frame.Y = 5 * frameHeight;
                }
                else
                {
                    npc.frame.Y = 4 * frameHeight;
                }


            }
            else if (flyMode == 2)
            {
                
                if (npc.velocity.X > 0)
                {
                    npc.spriteDirection = 1;
                    
                }
                else
                {
                    npc.spriteDirection = -1;
                }
                npc.frameCounter++;

                if (npc.frameCounter < 7)
                {
                    npc.frame.Y = 8 * frameHeight;
                    
                }
                else if (npc.frameCounter < 14)
                {
                    npc.frame.Y = 9 * frameHeight;
                   
                }
                else if (npc.frameCounter < 21)
                {
                    npc.frame.Y = 10 * frameHeight;
                    
                }
                else if (npc.frameCounter < 28)
                {
                    npc.frame.Y = 11 * frameHeight;
                    

                }
                else
                {

                    npc.frameCounter = 0;
                   
                }
            }
        }
        
        /*
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (npc.direction == 1)
            {

                
                spriteBatch.Draw(mod.GetTexture("NPCs/RuneSpectorBoss/RuneSpector_Glow"), new Vector2(npc.Center.X - Main.screenPosition.X, npc.Center.Y - Main.screenPosition.Y+4),
                       npc.frame, Color.Lerp(new Color(255, 255, 255, 255), new Color(0, 0, 0, 0), (float)npc.alpha / 255f), npc.rotation,
                       new Vector2(50 * 0.5f, 44 * 0.5f), 1f, SpriteEffects.None, 0f);

            }
            else
            {
               
                spriteBatch.Draw(mod.GetTexture("NPCs/RuneSpectorBoss/RuneSpector_Glow"), new Vector2(npc.Center.X - Main.screenPosition.X, npc.Center.Y - Main.screenPosition.Y+4),
                       npc.frame, new Color(255, 255, 255, npc.alpha), npc.rotation,
                       new Vector2(50 * 0.5f, 44 * 0.5f), 1f, SpriteEffects.FlipHorizontally, 0f);
            }
                
           
        }
        */
        
        public override void NPCLoot()
        {
            if (Main.netMode != 1)
            {
                int centerX = (int)(npc.position.X + npc.width / 2) / 16;
                int centerY = (int)(npc.position.Y + npc.height / 2) / 16;
                int halfLength = npc.width / 2 / 16 + 1;

                int trophyChance = Main.rand.Next(0, 10);

                if (Main.expertMode)
                {
                    npc.DropBossBags();
                }
                else
                {

                    int runeCount = Main.rand.Next(20, 31);
                    int selectScroll = Main.rand.Next(1, 5);
                    if (Main.rand.Next(7) == 0)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("RunicRobe"));
                    if (Main.rand.Next(7) == 0)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("RuneGhostMask"));


                    if (selectScroll == 1)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IceScroll"));
                    }
                    if (selectScroll == 2)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PursuitScroll"));
                    }
                    if (selectScroll == 3)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("LeechScroll"));
                    }
                    if (selectScroll == 4)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AggroScroll"));
                    }


                    
                    
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 73, 30);
                    
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CraftingRune"), runeCount);

                }
                if (trophyChance == 1)
                {
                    //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraTrophy"));
                }

            }
        }
    }
}
