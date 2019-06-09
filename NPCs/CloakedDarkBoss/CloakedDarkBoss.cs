using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using System.IO;

namespace QwertysRandomContent.NPCs.CloakedDarkBoss
{
    public class CloakedDarkBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Noehtnap");
            Main.npcFrameCount[npc.type] = 5;

        }

        public override void SetDefaults()
        {

            npc.width = 166;
            npc.height = 128;
            npc.damage = 58;
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
            npc.lifeMax = 8000;
            bossBag = mod.ItemType("NoehtnapBag");



        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {

            npc.lifeMax = (int)(10000 * bossLifeScale);
            npc.damage = 65;

        }
        static float randomRotation()
        {
            return Main.rand.NextFloat(-1, 1) * (float)Math.PI;
        }

        public Projectile cloak;
        public Deck<Vector2> attacks = new Deck<Vector2>();
        int timeBetweenAttacks = 90;
        int startAttacksDelay = 180;
        int forecastTime = 60;
        int foreCastTimePerAttack = 30;
        int BoxDrawTime = 60;
        int retreatTime = 240;
        int timer = 0;
        float playerviewRadius = 1200;
        float orbitalVelocity = 7f;
        float orbitDistance = 400;
        float retreatApproachSpeed = 4f;
        int chargeTime = 0;
        float pupilDirection = 0f;
        float greaterPupilRadius = 18;
        float lesserPupilRadius = 6;
        float pupilStareOutAmount = 1f;
        float blinkCounter = 60;
        int frame = 0;
        float pulseCounter = 0f;
        bool rage = false;
        int cloakID;
        public override void AI()
        {
            //QwertyMethods.ServerClientCheck(timer);
            if (Main.expertMode)
            {
                timeBetweenAttacks = 60;
            }
            Player player = Main.player[npc.target];
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
            if (blinkCounter < 0)
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
            
            if ((cloak == null || cloak.type != mod.ProjectileType("Cloak") || !cloak.active )&& Main.netMode != 1)
            {
                
                cloak = Main.projectile[ cloakID = Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("Cloak"), 0, 0, Main.myPlayer, npc.whoAmI)];
            }
            if (attacks.Count > 0)
            {
               
                if (!rage && (float)npc.life / (float)npc.lifeMax < .2f)
                {
                    rage = true;
                    attacks.Clear();
                }
                timer++;
                if (timer > timeBetweenAttacks + startAttacksDelay + forecastTime)
                {
                    switch ((int)attacks[0].X)
                    {
                        case 0:
                            if(Main.netMode != 1)
                            {
                                for (int t = 0; t < 4; t++)
                                {

                                    float r = attacks[0].Y + t * (float)Math.PI / 2;
                                    Projectile turret = Main.projectile[Projectile.NewProjectile(player.Center + QwertyMethods.PolarVector(orbitDistance, r), Vector2.Zero, mod.ProjectileType("Cannon"), Main.expertMode ? 22 : 35, 0f, Main.myPlayer)];
                                    turret.rotation = r + (float)Math.PI;
                                    turret.netUpdate = true;
                                }
                            }
                            break;
                        case 1:
                            chargeTime = 60;
                            npc.Center = player.Center + QwertyMethods.PolarVector(-600, attacks[0].Y);
                            npc.velocity = QwertyMethods.PolarVector(20, attacks[0].Y);
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Warp"), npc.Center);
                            break;
                        case 2:
                            if(Main.netMode != 1)
                            {
                                Projectile catalyst = Main.projectile[Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("BloodforceCatalyst"), Main.expertMode ? 20 : 34, 0f, Main.myPlayer)];
                                catalyst.rotation = attacks[0].Y - (float)Math.PI / 4;
                                catalyst.netUpdate = true;
                            }
                           
                            break;
                        case 3:
                            if (Main.netMode != 1)
                            {
                                Projectile tripwire = Main.projectile[Projectile.NewProjectile(player.Center + QwertyMethods.PolarVector(300, attacks[0].Y), Vector2.Zero, mod.ProjectileType("Tripwire"), Main.expertMode ? 30 : 45, 0f, Main.myPlayer)];
                                tripwire.rotation = attacks[0].Y + (float)Math.PI / 2;
                                tripwire.timeLeft = timeBetweenAttacks * attacks.Count + 60;
                                tripwire.netUpdate = true;
                            }
                            
                            break;
                    }
                    attacks.RemoveAt(0);
                    timer = startAttacksDelay + forecastTime;
                }
                else if (timer > startAttacksDelay + forecastTime)
                {

                }
                else if (timer < forecastTime)
                {

                }

            }
            else
            {

                timer = -retreatTime;
                if (Main.netMode != 1)
                {


                    float ratio = (float)npc.life / (float)npc.lifeMax;
                    if (ratio > .9f)
                    {
                        attacks.Add(new Vector2(0, randomRotation()));
                        attacks.Add(new Vector2(1, randomRotation()));
                        attacks.Add(new Vector2(0, randomRotation()));
                    }
                    else if (ratio > .8f)
                    {

                        attacks.Add(new Vector2(0, randomRotation()));
                        attacks.Add(new Vector2(1, randomRotation()));
                        attacks.Add(new Vector2(1, randomRotation()));
                        attacks.Add(new Vector2(2, randomRotation()));
                        attacks.Shuffle();
                    }
                    else if (ratio > .7f)
                    {

                        attacks.Add(new Vector2(1, randomRotation()));
                        attacks.Add(new Vector2(1, randomRotation()));
                        attacks.Add(new Vector2(1, randomRotation()));
                        attacks.Add(new Vector2(2, randomRotation()));
                        attacks.Add(new Vector2(2, randomRotation()));
                        attacks.Shuffle();
                    }
                    else if (ratio > .6f)
                    {
                        attacks.Add(new Vector2(3, randomRotation()));
                        attacks.Add(new Vector2(3, randomRotation()));
                        attacks.Add(new Vector2(1, randomRotation()));
                        attacks.Add(new Vector2(1, randomRotation()));
                        attacks.Add(new Vector2(1, randomRotation()));
                    }
                    else if (ratio > .5f)
                    {

                        attacks.Add(new Vector2(2, randomRotation()));
                        attacks.Add(new Vector2(0, randomRotation()));
                        attacks.Add(new Vector2(0, randomRotation()));
                        attacks.Add(new Vector2(0, randomRotation()));
                        attacks.Add(new Vector2(0, randomRotation()));
                        attacks.Shuffle();
                        attacks.Insert(0, new Vector2(3, randomRotation()));
                    }
                    else if (ratio > .4f)
                    {

                        attacks.Add(new Vector2(3, randomRotation()));
                        attacks.Add(new Vector2(3, randomRotation()));
                        attacks.Add(new Vector2(2, randomRotation()));
                        attacks.Add(new Vector2(2, randomRotation()));
                        attacks.Add(new Vector2(2, randomRotation()));
                    }
                    else if (ratio > .3f)
                    {

                        attacks.Add(new Vector2(2, randomRotation()));
                        attacks.Add(new Vector2(2, randomRotation()));
                        attacks.Add(new Vector2(1, randomRotation()));
                        attacks.Add(new Vector2(3, randomRotation()));
                        attacks.Shuffle();
                        attacks.Insert(0, new Vector2(3, randomRotation()));
                        attacks.Add(new Vector2(1, randomRotation()));

                    }
                    else if (ratio > .2f)
                    {

                        attacks.Add(new Vector2(3, randomRotation()));
                        attacks.Add(new Vector2(3, randomRotation()));
                        attacks.Add(new Vector2(3, randomRotation()));
                        attacks.Add(new Vector2(2, randomRotation()));
                        attacks.Add(new Vector2(1, randomRotation()));
                        attacks.Add(new Vector2(1, randomRotation()));
                        attacks.Add(new Vector2(1, randomRotation()));

                    }
                    else if (ratio > .1f)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            attacks.Add(new Vector2(2, randomRotation()));
                        }
                    }
                    else
                    {
                        attacks.Add(new Vector2(3, randomRotation()));
                        attacks.Add(new Vector2(3, randomRotation()));
                        for (int i = 0; i < 10; i++)
                        {
                            attacks.Add(new Vector2(2, randomRotation()));
                        }
                    }
                    forecastTime = foreCastTimePerAttack * attacks.Count + BoxDrawTime;
                    npc.netUpdate = true;
                }
            }
            if (playerviewRadius > 80 && timer > forecastTime)
            {
                playerviewRadius -= 10;
            }
            else if (playerviewRadius < 1200 && timer < forecastTime && timer > -120)
            {
                playerviewRadius += 10;
            }
           
           if(Main.netMode != 1)
            {
                cloak.ai[1] = playerviewRadius;
                cloak.timeLeft = 2;
            }
                
            
           
            
            npc.dontTakeDamage = false;
            if (playerviewRadius > 500)
            {
                orbitDistance = 1200;
                retreatApproachSpeed = 8f;
                if ((player.Center - npc.Center).Length() > 1000)
                {
                    npc.dontTakeDamage = true;
                }

            }
            else
            {
                retreatApproachSpeed = 4f;
                orbitDistance = 500;
            }
            npc.ai[3] = 0;
            if (chargeTime > 0)
            {
                chargeTime--;
                if (chargeTime > 45)
                {
                    npc.ai[3] = (60 - chargeTime) * 7;
                }
                else if (chargeTime > 30)
                {
                    npc.ai[3] = 210 - (60 - chargeTime) * 7;
                }

            }
            else
            {

                chargeTime = 0;
                npc.velocity = QwertyMethods.PolarVector(orbitalVelocity, (player.Center - npc.Center).ToRotation() + (float)Math.PI / 2);
                if (Main.netMode != 1 && Main.rand.Next(200) == 0)
                {
                    orbitalVelocity *= -1;
                    npc.netUpdate = true;
                }
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
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(timer);
            writer.Write(orbitalVelocity);
            writer.Write(forecastTime);
            writer.Write(attacks.Count);
            foreach(Vector2 attack in attacks)
            {
                writer.WriteVector2(attack);
            }
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            timer = reader.ReadInt32();
            orbitalVelocity = reader.ReadSingle();
            forecastTime = reader.ReadInt32();
            int attackCount = reader.ReadInt32();
            attacks.Clear();
            for(int i = 0; i < attackCount; i++)
            {
                attacks.Add(reader.ReadVector2());
            }

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
            Texture2D Pupil = mod.GetTexture("NPCs/CloakedDarkBoss/Pupil");
            spriteBatch.Draw(Pupil, npc.Center - Main.screenPosition + new Vector2((float)Math.Cos(pupilDirection) * greaterPupilRadius * pupilStareOutAmount, (float)Math.Sin(pupilDirection) * lesserPupilRadius) * npc.scale,
                       Pupil.Frame(), drawColor, npc.rotation,
                       Pupil.Size() * .5f, npc.scale, SpriteEffects.None, 0f);
            Texture2D Eyelid = mod.GetTexture("NPCs/CloakedDarkBoss/Eyelid");
            spriteBatch.Draw(Eyelid, npc.Center - Main.screenPosition,
                       npc.frame, drawColor, npc.rotation,
                       new Vector2(npc.width * 0.5f, npc.height * 0.5f), npc.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (timer < forecastTime && timer > 0)
            {
                float Spacing = 150;
                Texture2D texture = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
                var dataColors = new Color[] { Color.Black };
                texture.SetData(0, null, dataColors, 0, 1);
                float scl = (float)timer / BoxDrawTime;
                if (scl > 1f)
                {
                    scl = 1f;
                }
                spriteBatch.Draw(texture,
                new Vector2(Main.screenWidth / 2, Main.screenHeight / 4),
                null,
                Color.White,
                0f,
                new Vector2(1, 1) * .5f,
                new Vector2(attacks.Count * Spacing, Spacing) * scl,
                SpriteEffects.None,
                0f
                );
                texture = mod.GetTexture("NPCs/CloakedDarkBoss/AttackForecast");

                for (int i = 0; i < attacks.Count; i++)
                {
                    if (timer - BoxDrawTime > foreCastTimePerAttack * i)
                    {
                        spriteBatch.Draw(texture,
                    new Vector2(Main.screenWidth / 2, Main.screenHeight / 4) + new Vector2(Spacing * i - ((attacks.Count - 1f) / 2f * Spacing), 0),
                    new Rectangle(0, 98 * (int)attacks[i].X, 98, 98),
                    Color.White,
                    attacks[i].Y,
                    new Vector2(98, 98) * .5f,
                    1f,
                    SpriteEffects.None,
                    0f
                    );
                    }

                }

            }
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
        public override void AI()
        {
            projectile.Center = Main.npc[(int)projectile.ai[0]].Center;
           // QwertyMethods.ServerClientCheck((int)projectile.ai[1]);
            if(Main.netMode !=1)
            {
                projectile.netUpdate = true;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            Player drawPlayer = Main.LocalPlayer;
            float scale = 32;
            if (projectile.ai[1] > 300)
            {
                scale = 12;
            }
            


            int height = (int)(Main.screenHeight / scale);
            int width = (int)(Main.screenWidth / scale);
            height++;
            Texture2D TheShadow = new Texture2D(Main.graphics.GraphicsDevice, width, height);
            var dataColors = new Color[width * height];
            List<Vector3> lightSpots = new List<Vector3>();
            lightSpots.Add(new Vector3(drawPlayer.Center.X, drawPlayer.Center.Y, projectile.ai[1]));
            if(!Main.gamePaused)
            {
                lightSpots.Add(new Vector3(Main.MouseWorld.X, Main.MouseWorld.Y, 80));
            }
            
            NPC master = Main.npc[(int)projectile.ai[0]];
            if (master.ai[3]>0)
            {
                lightSpots.Add(new Vector3(master.Center.X, master.Center.Y, master.ai[3]));
            }
            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].active && Main.projectile[p].type == mod.ProjectileType("Cannon") && Main.projectile[p].timeLeft > 30)
                {
                    //Main.NewText("HI");
                    if (Main.projectile[p].timeLeft > 30)
                    {
                        float r = 0;
                        if (Main.projectile[p].timeLeft > 60)
                        {
                            r = 90 - Main.projectile[p].timeLeft;
                            r *= 2;
                        }
                        else
                        {
                            r = Main.projectile[p].timeLeft - 30;
                            r *= 2;
                        }
                        lightSpots.Add(new Vector3(Main.projectile[p].Center.X, Main.projectile[p].Center.Y, r));
                    }

                }

                if (Main.projectile[p].active && Main.projectile[p].type == mod.ProjectileType("BloodforceCatalyst") )
                {
                    if(Main.projectile[p].timeLeft>20)
                    {
                        int f = 40;
                        if(Main.projectile[p].timeLeft >40)
                        {
                            f = 60 - Main.projectile[p].timeLeft;
                            f *= 2;
                        }
                        for(int i = 0; i <4; i++)
                        {
                            Vector2 offset = Main.projectile[p].Center + QwertyMethods.PolarVector(Main.projectile[p].ai[1], Main.projectile[p].rotation + (float)Math.PI * i / 2);
                            lightSpots.Add(new Vector3(offset.X, offset.Y, f));
                        }
                        
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Vector2 offset = Main.projectile[p].Center + QwertyMethods.PolarVector(Main.projectile[p].ai[1], Main.projectile[p].rotation + (float)Math.PI *  i / 2);
                            lightSpots.Add(new Vector3(offset.X, offset.Y, Main.projectile[p].timeLeft *2));
                        }
                    }
                }
                if (Main.projectile[p].active && Main.projectile[p].type == mod.ProjectileType("Tripwire"))
                {
                    if(Main.projectile[p].ai[1] <60)
                    {
                        for(int d =0; d< 2; d++)
                        {
                            Vector2 Cen = Main.projectile[p].Center + QwertyMethods.PolarVector(Main.projectile[p].ai[1] * 7, Main.projectile[p].rotation + (float)Math.PI * d);
                            float r = 60;
                            if (Main.projectile[p].ai[1] > 50)
                            {
                                r = 60 - (Main.projectile[p].ai[1]-50) * 6;
                            }
                            else if (Main.projectile[p].ai[1]<10)
                            {
                                r = (Main.projectile[p].ai[1]) * 6;
                            }
                           
                            lightSpots.Add(new Vector3(Cen.X, Cen.Y, r));
                        }
                    }
                }
            }
            for(int i =0; i < lightSpots.Count; i++)
            {
                lightSpots[i]= new Vector3(lightSpots[i].X-4, lightSpots[i].Y-4, lightSpots[i].Z);
                
            }
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool check = false;
                    foreach (Vector3 spot in lightSpots)
                    {
                        if (((new Vector2(spot.X, spot.Y) - Main.screenPosition) / scale - (new Vector2(x, y))).Length() < (spot.Z+scale) / scale )
                        {

                            check = true;
                            break;
                        }
                    }
                    if (!check)
                    {
                        dataColors[x + y * width] = Color.Black;
                    }



                }
            }
            TheShadow.SetData(0, null, dataColors, 0, width * height);
            //Main.NewText("Big: " + dataColors.Length);
            //Precise darkening, Save the smaller pixels for when closer to a thing you need to reveal for better performance
            spriteBatch.Draw(TheShadow, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), scale, 0, 0);
            if(scale >12)
            {
                foreach (Vector3 hr in lightSpots)
                {
                    scale = 12f;
                    width = (int)((hr.Z + scale * 8) * 2f / scale);
                    height = width;
                    if (width < Main.screenWidth / scale || height < Main.screenHeight / scale)
                    {
                        TheShadow = new Texture2D(Main.graphics.GraphicsDevice, width, height);
                        dataColors = new Color[width * height];
                        Vector2 pos = new Vector2(hr.X, hr.Y) - Main.screenPosition - new Vector2(width, height) * scale / 2f;
                        //pos /= scale;
                        for (int x = 0; x < width; x++)
                        {
                            for (int y = 0; y < height; y++)
                            {
                                bool check = false;
                                foreach (Vector3 spot in lightSpots)
                                {
                                    if (((new Vector2(spot.X, spot.Y) - Main.screenPosition) / scale - (new Vector2(x, y) + pos / scale)).Length() < spot.Z / scale)
                                    {

                                        check = true;
                                        break;
                                    }
                                }
                                if (!check)
                                {
                                    dataColors[x + y * width] = Color.Black;
                                }



                            }
                        }
                        TheShadow.SetData(0, null, dataColors, 0, width * height);
                        // Main.NewText("Precise: " + dataColors.Length)
                        spriteBatch.Draw(TheShadow, pos+new Vector2(scale, scale)*.5f, null, Color.White, 0f, new Vector2(0, 0), scale, 0, 0);
                    }

                }
            }
            
            return false;
        }



    }
    public class Cannon : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.light = .8f;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.width = 34;
            projectile.height = 34;
            projectile.timeLeft = 90;
            projectile.tileCollide = false;
        }
        public override void AI()
        {
            if(Main.netMode != 1)
            {
                projectile.netUpdate = true;
            }
            
            if (projectile.timeLeft % 10 == 0 && projectile.timeLeft < 30)
            {
                Main.PlaySound(SoundID.Item62, projectile.position);
                if(Main.netMode != 1)
                {
                    Projectile.NewProjectile(projectile.Center + QwertyMethods.PolarVector(17, projectile.rotation), QwertyMethods.PolarVector(10, projectile.rotation), mod.ProjectileType("CannonProjectile"), projectile.damage, 0f, Main.myPlayer);
                }
                
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(projectile.rotation);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            projectile.rotation = reader.ReadSingle();
        }
    }
    public class CannonProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heavan Raider Cannon");
        }
        public override void SetDefaults()
        {
            projectile.light = .8f;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.width = 10;
            projectile.height = 10;
            projectile.tileCollide = false;
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.extraUpdates = 1;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AI()
        {
            Dust.NewDustPerfect(projectile.Center, mod.DustType("BloodforceDust"), Vector2.Zero);
        }
    }
    public class BloodforceCatalyst : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.light = .8f;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.width = 10;
            projectile.height = 10;
            projectile.timeLeft = 60;
            projectile.tileCollide = false;
        }
        bool runOnce = true;
        public override void AI()
        {
            if (Main.netMode != 1)
            {
                projectile.netUpdate = true;
            }
            if (runOnce)
            {
                projectile.ai[1] = 100;
                runOnce = false;
            }
            if(projectile.timeLeft <=20)
            {
                projectile.ai[1] -= (100f / 20f);
            }
            projectile.rotation += (9 * (float)Math.PI / 4) / 60 ;
        }
        public override void Kill(int timeLeft)
        {
            if(Main.netMode != 1)
            {
                for (int r = 0; r < 4; r++)
                {
                    Projectile p = Main.projectile[Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("Bloodforce"), projectile.damage, 0f, Main.myPlayer)];
                    p.rotation = projectile.rotation + (float)Math.PI * r / 2;
                }
            }
            
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Zap"), projectile.Center);
            for (int r =0; r <4; r++)
            {
                spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + QwertyMethods.PolarVector(projectile.ai[1], projectile.rotation + (float)Math.PI * r/2),
                       texture.Frame(), lightColor, projectile.rotation * (float)Math.PI * 2 * r / 4,
                       texture.Size() * .5f, projectile.scale, SpriteEffects.None, 0f);
            }
            
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(projectile.rotation);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            projectile.rotation = reader.ReadSingle();
        }
    }
    public class Bloodforce : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloodforce");
        }
        public override void SetDefaults()
        {
            projectile.light = .8f;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.width = 26;
            projectile.height = 26;
            projectile.tileCollide = false;
            projectile.timeLeft = 10;
            
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float p = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + QwertyMethods.PolarVector(800, projectile.rotation), 26, ref p);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int width = 400;
            int height = 13;
            Texture2D ray = new Texture2D(Main.graphics.GraphicsDevice, width, height);
            var dataColors = new Color[width * height];
            
            for (int w = 0; w < 2; w++)
            {
                for (int x = 0; x < width; x++)
                {
                   
                    int output = (int)( 6 * (float)Math.Sin(x * Math.PI / 10f + w *Math.PI));
                    output += 6;
                    //Main.NewText(output);


                    dataColors[x + output * width] = new Color(122, 24, 24);



                }
            }
            
            ray.SetData(0, null, dataColors, 0, width * height);
            spriteBatch.Draw(ray, projectile.Center - Main.screenPosition,
                   ray.Frame(), Color.White, projectile.rotation ,
                   new Vector2(0, height / 2f), 2f, SpriteEffects.None, 0f);
            
            return false;
        }
        public override void AI()
        {
            if (Main.netMode != 1)
            {
                projectile.netUpdate = true;
            }

        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(projectile.rotation);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            projectile.rotation = reader.ReadSingle();
        }
    }
    public class Tripwire : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.light = .8f;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.width = 10;
            projectile.height = 10;
            projectile.timeLeft = 600;
            projectile.tileCollide = false;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Emitsic Beam");
        }
        bool runOnce = true;
        float lengthMultiplier = 7;
        int startWire = 60;
        public override void AI()
        {
            if (Main.netMode != 1)
            {
                projectile.netUpdate = true;
            }
            if (runOnce)
            {
                
                runOnce = false;
            }
            if(projectile.ai[1] < startWire)
            {
                projectile.ai[1]++;
            }
            
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float p = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center + QwertyMethods.PolarVector(projectile.ai[1] * lengthMultiplier, projectile.rotation + (float)Math.PI), projectile.Center + QwertyMethods.PolarVector(projectile.ai[1] * lengthMultiplier, projectile.rotation), 20, ref p) && projectile.ai[1]== startWire;
        }
        public override void Kill(int timeLeft)
        {
            
        }
        float trigCounter = 0f;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if(projectile.ai[1] == startWire-1)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Tripwire"), projectile.Center);
            }
            if (projectile.ai[1] == startWire)
            {
                trigCounter += (float)Math.PI / 8f;
                int width = (int)(projectile.ai[1] * lengthMultiplier);
                int height = 21;
                Texture2D ray = new Texture2D(Main.graphics.GraphicsDevice, width, height);
                var dataColors = new Color[width * height];
                trigCounter += (float)Math.PI / 30;
                for (int w = 0; w < 3; w++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        float Modifier = trigCounter / 10f + w * (float)Math.PI * 2 / 3;
                        int output = (int)(Math.Sin(Modifier) * 10 * (float)Math.Sin(x * Math.PI / 15f + trigCounter));
                        output += 10;

                        

                        dataColors[x + output * width] = new Color(122, 24, 24);



                    }
                }
                ray.SetData(0, null, dataColors, 0, width * height);
                spriteBatch.Draw(ray, projectile.Center - Main.screenPosition,
                       ray.Frame(), Color.White, projectile.rotation,
                       ray.Size() * .5f, 2f, SpriteEffects.None, 0f);
                

            }
            Texture2D texture = Main.projectileTexture[projectile.type];
            for(int i =0; i<2; i++)
            {
                spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + QwertyMethods.PolarVector(projectile.ai[1]* lengthMultiplier, projectile.rotation + (float)Math.PI * i),
                       texture.Frame(), lightColor, projectile.rotation + (float)Math.PI  * i,
                       texture.Size() * .5f, projectile.scale, SpriteEffects.None, 0f);
            }
            
           
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(projectile.rotation);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            projectile.rotation = reader.ReadSingle();
        }
    }

}
