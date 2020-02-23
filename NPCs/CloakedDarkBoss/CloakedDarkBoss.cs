using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.CloakedDarkBoss
{
    public class CloakedDarkBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Noehtnap");
            Main.npcFrameCount[npc.type] = 5;

        }
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? base.Texture + "_Old" : base.Texture;
        public override void SetDefaults()
        {

            npc.width = 166;
            npc.height = 128;
            npc.damage = 80;
            npc.defense = 12;
            npc.boss = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 60f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            animationType = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/TheGodsBleed");
            npc.lifeMax = 5000;
            bossBag = mod.ItemType("NoehtnapBag");



        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }
        bool canDespawn = false;
        public override bool CheckActive()
        {
            return canDespawn;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return npc.chaseable;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {

            npc.lifeMax = (int)(7000 * bossLifeScale);
            npc.damage = 120;

        }
        static float randomRotation()
        {
            return Main.rand.NextFloat(-1, 1) * (float)Math.PI;
        }

        public Projectile cloak;
        int timer = -360;
        float playerviewRadius = 80;
        float orbitalVelocity = 7f;
        float orbitDistance = 350;
        float retreatApproachSpeed = 4f;
        float pupilDirection = 0f;
        float greaterPupilRadius = 18;
        float lesserPupilRadius = 6;
        float pupilStareOutAmount = 1f;
        float blinkCounter = 60;
        int frame = 0;
        float pulseCounter = 0f;
        int attackType = -1;
        Projectile myWall = null;
        Vector2 lastMoved = Vector2.UnitX;
        float defaultOrbitalSpeed()
        {
            if(orbitalVelocity == 0)
            {
                npc.netUpdate = true;
                return 7f * (Main.rand.Next(2) == 0 ? 1f : -1f);
            }
            return 7f * (orbitalVelocity > 0 ? 1f : -1f);
        }
        public override void AI()
        {
            //QwertyMethods.ServerClientCheck(timer);
            
            Player player = Main.player[npc.target];
            npc.chaseable = false;
            if ( npc.ai[3] > 10)
            {
                npc.chaseable = true;
            }
            else
            {
                for (int i = 0; i < Main.player.Length; i++)
                {
                    if ((Main.player[i].active && (QwertysRandomContent.LocalCursor[i] - npc.Center).Length() < 180) || (Main.player[i].Center - npc.Center).Length() < orbitDistance )
                    {
                        npc.chaseable = true;
                        break;
                    }
                }
            }
            npc.TargetClosest(false);
            pupilDirection = (player.Center - npc.Center).ToRotation();
            pulseCounter += (float)Math.PI / 30;
            npc.scale = 1f + .05f * (float)Math.Sin(pulseCounter);
            pupilStareOutAmount = (player.Center - npc.Center).Length() / 300f;
            if (pupilStareOutAmount > 1f)
            {
                pupilStareOutAmount = 1f;
            }

            blinkCounter--;
            if (blinkCounter < 0 && attackType != 1)
            {
                if (blinkCounter % 10 == 0)
                {
                    if (frame == 7)
                    {
                        blinkCounter = Main.rand.Next(180, 240);
                    }
                    else
                    {
                        frame++;
                    }
                }
            }
            else
            {
                
                frame = 0;
            }
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    canDespawn = true;
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
                
                canDespawn = false;
                
                if ((cloak == null || cloak.type != mod.ProjectileType("Cloak") || !cloak.active) && Main.netMode != 1)
                {

                    cloak = Main.projectile[Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("Cloak"), 0, 0, Main.myPlayer, npc.whoAmI)];
                }
                
                if (playerviewRadius > 80)
                {
                    playerviewRadius -= 10;
                }
                if (Main.netMode != 1)
                {
                    cloak.ai[1] = playerviewRadius;
                    cloak.timeLeft = 2;
                }
                
                
                if(myWall != null)
                {
                    myWall.timeLeft = 2;
                }
                
                if(npc.ai[3]>0)
                {
                    npc.ai[3]--;
                }
                if(attackType != 0)
                {
                    timer++;
                    if(player.velocity.Length() > 0f)
                    {
                        lastMoved = player.velocity;
                    }
                    
                }
                switch (attackType)
                {
                    case -1:
                        
                        if (timer > 120 *  (Main.expertMode ? .2f + .8f*((float)npc.life/npc.lifeMax) : 1f) && (player.Center - npc.Center).Length() < 1000f)
                        {
                            if (Main.netMode != 1)
                            {
                                attackType = Main.rand.Next(7);
                                switch (attackType)
                                {
                                    default:
                                        orbitalVelocity = 14f;
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            orbitalVelocity *= -1;
                                        }
                                        break;
                                    case 0:
                                        orbitalVelocity = defaultOrbitalSpeed()*4f;
                                        break;
                                    case 1:
                                        orbitalVelocity = 0;
                                        Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("Warning"), 0, 0f, Main.myPlayer, 0, 0);
                                        if(!Main.dedServ)
                                        {
                                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Notice").WithVolume(3f).WithPitchVariance(.5f), npc.Center);
                                        }
                                        npc.velocity = Vector2.Zero;
                                        break;
                                }
                                
                                npc.netUpdate = true;
                            }
                            timer = 0;
                        }
                        break;
                    default:
                        if (timer>=60)
                        {
                            orbitalVelocity = defaultOrbitalSpeed();
                            timer = 0;
                            attackType = -1;
                        }
                        else if(timer % 15==0 && Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("EtimsicCannon"), Main.expertMode ? 18 : 24, 0f, Main.myPlayer, (player.Center - npc.Center).ToRotation());
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("Warning"), 0, 0f, Main.myPlayer, 1, (player.Center - npc.Center).ToRotation());
                            if (!Main.dedServ)
                            {
                                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Notice").WithVolume(3f).WithPitchVariance(.5f), npc.Center);
                            }
                                
                        }
                        break;
                    case 0:
                        if (timer == 0 && QwertyMethods.AngularDifference(lastMoved.ToRotation(), (npc.Center - player.Center).ToRotation()) < (float)Math.PI / 30)
                        {
                            if (Main.netMode != 1)
                            {
                                myWall = Main.projectile[Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("EtimsicWall"), Main.expertMode ? 24 : 36, 0f, Main.myPlayer, (player.Center - npc.Center).ToRotation() + (float)Math.PI / 2)];
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("Warning"), 0, 0f, Main.myPlayer, 2, (player.Center - npc.Center).ToRotation() + (float)Math.PI / 2);
                            }
                            
                            if (!Main.dedServ)
                            {
                                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Notice").WithVolume(3f).WithPitchVariance(.5f), npc.Center);
                            }
                            
                            timer = 1;
                            npc.netUpdate = true;
                        }
                        if(timer >0)
                        {
                            timer++;
                        }
                        if(timer >=60)
                        {
                            orbitalVelocity = defaultOrbitalSpeed();
                            npc.netUpdate = true;
                            timer = 0;
                            attackType = -1;
                        }
                        break;
                    case 1:
                        if (timer > 60)
                        {
                            npc.ai[3] = 80;
                            if (timer < 180 && timer % 15 == 0)
                            {
                                npc.velocity = Vector2.Zero;
                                if (Main.netMode != 1)
                                {
                                    Projectile.NewProjectile(npc.Center + new Vector2((float)Math.Cos(pupilDirection) * greaterPupilRadius * pupilStareOutAmount, (float)Math.Sin(pupilDirection) * lesserPupilRadius) * npc.scale, QwertyMethods.PolarVector(10, pupilDirection), mod.ProjectileType("EtimsicRay"), Main.expertMode ? 18 : 24, 0f, Main.myPlayer);
                                }
                                if (!Main.dedServ)
                                {
                                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/PewPew").WithVolume(3f).WithPitchVariance(.5f), npc.Center);
                                }
                                   
                            }
                            if(timer == 179 && Main.netMode != 1)
                            {
                                npc.netUpdate = true;

                            }
                            if(timer == 180)
                            {
                                npc.velocity = QwertyMethods.PolarVector(15, (player.Center - npc.Center).ToRotation());
                                npc.netUpdate = true;
                            }
                            if(timer > 240)
                            {
                                npc.netUpdate = true;
                                orbitalVelocity = defaultOrbitalSpeed();
                                timer = 0;
                                attackType = -1;

                            }
                        }
                        break;
                }
                
                
                

                //movement
                if (attackType == 1)
                {

                }
                else
                {
                    npc.velocity = QwertyMethods.PolarVector(orbitalVelocity, (player.Center - npc.Center).ToRotation() + (float)Math.PI / 2);
                   
                    if ((player.Center - npc.Center).Length() < orbitDistance - 50)
                    {
                        npc.velocity += QwertyMethods.PolarVector(-retreatApproachSpeed, (player.Center - npc.Center).ToRotation());
                    }
                    else if ((player.Center - npc.Center).Length() > orbitDistance + 50)
                    {
                        npc.velocity += QwertyMethods.PolarVector(retreatApproachSpeed, (player.Center - npc.Center).ToRotation());
                    }
                }


            }


        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(timer);
            writer.Write(orbitalVelocity);
            writer.Write(attackType);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            timer = reader.ReadInt32();
            orbitalVelocity = reader.ReadSingle();
            attackType = reader.ReadInt32();
        }
        public override void FindFrame(int frameHeight)
        {
            if (frame > 4)
            {
                npc.frame.Y = (8 - frame) * frameHeight;
            }
            else
            {
                npc.frame.Y = frame * frameHeight;
            }


        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            spriteBatch.Draw(texture, npc.Center - Main.screenPosition,
                       npc.frame, drawColor, npc.rotation,
                       new Vector2(npc.width * 0.5f, npc.height * 0.5f), npc.scale, SpriteEffects.None, 0f);
            Texture2D Pupil = mod.GetTexture("NPCs/CloakedDarkBoss/Pupil" + (ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? "_Old" : ""));
            spriteBatch.Draw(Pupil, npc.Center - Main.screenPosition + new Vector2((float)Math.Cos(pupilDirection) * greaterPupilRadius * pupilStareOutAmount, (float)Math.Sin(pupilDirection) * lesserPupilRadius) * npc.scale,
                       Pupil.Frame(), drawColor, npc.rotation,
                       Pupil.Size() * .5f, npc.scale, SpriteEffects.None, 0f);
            Texture2D Eyelid = mod.GetTexture("NPCs/CloakedDarkBoss/Eyelid" + (ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? "_Old" : ""));
            spriteBatch.Draw(Eyelid, npc.Center - Main.screenPosition,
                       npc.frame, drawColor, npc.rotation,
                       new Vector2(npc.width * 0.5f, npc.height * 0.5f), npc.scale, SpriteEffects.None, 0f);
            return false;
        }
       
        public override void NPCLoot()
        {
            QwertyWorld.downedNoetnap = true;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                Item.NewItem(npc.getRect(), mod.ItemType("EtimsMaterial"), 12 + Main.rand.Next(13));
                if(Main.rand.Next(20)<3)
                {
                    Item.NewItem(npc.getRect(), mod.ItemType("EyeOfDarkness"));
                }
                if (Main.rand.Next(20) < 3)
                {
                    Item.NewItem(npc.getRect(), mod.ItemType("NoScope"));
                }
            }
        }

    }

    public class Cloak : ModProjectile
    {
        public bool cloak;
        public override void SetStaticDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.width = 0;
            projectile.height = 0;
            projectile.timeLeft = 2;
            projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsOverWiresUI.Add(index);
        }
        List<Vector3> lightSpots = new List<Vector3>();
        public override void AI()
        {
            projectile.Center = Main.npc[(int)projectile.ai[0]].Center;
            // QwertyMethods.ServerClientCheck((int)projectile.ai[1]);
            if (Main.netMode != 1)
            {
                projectile.netUpdate = true;
            }
        }
        int screenWidthOld = 0;
        int screenHeightOld = 0;
        int height=0;
        int width=0;
        Color[] defaultdataColors = null;
        Color trans = new Color(0, 0, 0, 0);
        float scale = 4;
        void lightsUpdate(Color color)
        {
            foreach (Vector3 spot in lightSpots)
            {
                Vector2 spotCoords = new Vector2(spot.X, spot.Y);
                for (int localX = 0; localX < (int)spot.Z / scale; localX++)
                {
                    for (int localY = 0; localY < (int)spot.Z / scale; localY++)
                    {
                        if (new Vector2(localX, localY).Length() < spot.Z / scale)
                        {

                            int x = (int)spotCoords.X + localX;
                            int y = (int)spotCoords.Y + localY;
                            int loc = x + y * width;
                            if (loc < defaultdataColors.Length && x < width && x > 0 && loc>=0)
                            {
                                defaultdataColors[loc] = color;
                            }
                            x = (int)spotCoords.X - localX;
                            y = (int)spotCoords.Y + localY;
                            loc = x + y * width;
                            if (loc < defaultdataColors.Length && x < width && x > 0 && loc >= 0)
                            {
                                defaultdataColors[loc] = color;
                            }
                            x = (int)spotCoords.X - localX;
                            y = (int)spotCoords.Y - localY;
                            loc = x + y * width;
                            if (loc < defaultdataColors.Length && x < width && x > 0 && loc >= 0)
                            {
                                defaultdataColors[loc] = color;
                            }
                            x = (int)spotCoords.X + localX;
                            y = (int)spotCoords.Y - localY;
                            loc = x + y * width;
                            if (loc < defaultdataColors.Length && x < width && x > 0 && loc >= 0)
                            {
                                defaultdataColors[loc] = color;
                            }
                        }
                    }
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            Player drawPlayer = Main.LocalPlayer;
            if(drawPlayer.GetModPlayer<ShapeShifterPlayer>().drawGodOfBlasphemy)
            {
                return false;
            }
            
            if (Main.screenWidth != screenWidthOld || Main.screenHeight != screenHeightOld)
            {
                height = (int)(Main.screenHeight / scale);
                width = (int)(Main.screenWidth / scale);
                height++;
                Texture2D defaultShadow = new Texture2D(Main.graphics.GraphicsDevice, width, height);
                defaultdataColors = new Color[width * height];
                for(int x =0; x < width; x++)
                {
                    for(int y =0; y < height; y++)
                    {
                        defaultdataColors[x + y * width] = Color.Black;
                    }
                }
            }

            lightsUpdate(Color.Black); //reset color

            lightSpots = new List<Vector3>();
            lightSpots.Add(new Vector3((drawPlayer.Center.X-Main.screenPosition.X)/scale, (drawPlayer.Center.Y-Main.screenPosition.Y)/scale, projectile.ai[1]));
            if (!Main.gamePaused)
            {
                lightSpots.Add(new Vector3(Main.mouseX/scale, Main.mouseY/scale, 80));
            }

            NPC master = Main.npc[(int)projectile.ai[0]];
            if (master.ai[3] > 0)
            {
                lightSpots.Add(new Vector3((master.Center.X - Main.screenPosition.X)/scale, (master.Center.Y-Main.screenPosition.Y)/scale, master.ai[3]));
            }
            for(int i =0; i < Main.projectile.Length; i++)
            {
                if(Main.projectile[i].active && (Main.projectile[i].type == mod.ProjectileType("EtimsicCannon") || Main.projectile[i].type == mod.ProjectileType("EtimsicWall")) && Main.projectile[i].ai[1]==1)
                {
                    lightSpots.Add(new Vector3((Main.projectile[i].Center.X - Main.screenPosition.X) / scale, (Main.projectile[i].Center.Y - Main.screenPosition.Y) / scale, 40));
                }
            }

            lightsUpdate(trans); //now that we have lights make them transparent
            Texture2D TheShadow = new Texture2D(Main.graphics.GraphicsDevice, width, height);
            TheShadow.SetData(0, null, defaultdataColors, 0, width * height);
            spriteBatch.Draw(TheShadow, Vector2.Zero, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, .2f);
            
            screenWidthOld = Main.screenWidth;
            screenHeightOld = Main.screenHeight;
            return false;
        }



    }
    

}
