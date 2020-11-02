using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using QwertysRandomContent.NPCs.RuneGhost.RuneBuilder;
using System.IO;
using Terraria.Localization;

namespace QwertysRandomContent.NPCs.RuneGhost
{
    public class RuneGhost : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rune Ghost");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            npc.width = 50;
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
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(75000 * bossLifeScale);
        }
        public override bool CheckActive()
        {
            return despawn;
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }
        bool casting = false;
        float castingSpeed = 1f;
        int frame = 0;
        float castTime = 0f;
        bool drawRune = true;
        Vector2 goTo;
        bool runOnce = true;
        float goToYOffset = -150;
        float flightTime = 0;
        float flightTimeMax = 60;
        int phase = 0;
        bool despawn = false;
        int lastRune = 5;
        void StartMoving()
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            
            phase = 0;
            if (Main.expertMode)
            {
                castingSpeed = 1f + 1f - ((float)npc.life / (float)npc.lifeMax);
                if ( (((float)npc.life / npc.lifeMax) < .5f))
                {
                    phase++;
                }
            }
            if (Main.netMode != 1)
            {
                goTo = player.Center + Vector2.UnitY * goToYOffset + Vector2.UnitX * Main.rand.NextFloat(-400, 400);
                npc.velocity = castingSpeed * ((goTo - npc.Center) / flightTimeMax);
                npc.netUpdate = true;
            }
            
            flightTime = flightTimeMax;
            goToYOffset *= -1;
            
        }
        public override void AI()
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            if (!player.active || player.dead || despawn)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                
                if (!player.active || player.dead || despawn)
                {
                    despawn = true;
                    casting = false;
                    npc.velocity = Vector2.UnitX * 10f;
                    if (flightTime > 90)
                    {
                        flightTime--;
                    }
                    else
                    {
                        npc.position = new Vector2(100000, 0);
                    }
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }
            else
            {
                if (runOnce)
                {
                    StartMoving();
                    runOnce = false;
                }
                npc.dontTakeDamage = !casting;
                if (casting)
                {
                    npc.velocity = Vector2.Zero;
                    castTime += castingSpeed;
                    #region calculate frame
                    frame = 0;
                    if (castTime >= 15)
                    {
                        frame++;
                    }
                    if (castTime >= 30)
                    {
                        frame++;
                    }
                    if (castTime >= 45)
                    {
                        frame++;
                    }
                    if (castTime >= 60)
                    {
                        frame++;

                        frame += (((int)castTime - 60) / 15) % 4;
                    }
                    #endregion

                    if (castTime >= 60 && drawRune)
                    {
                        drawRune = false;
                        if (Main.netMode != 1)
                        {
                            for (int i = 0; i < phase + 1; i++)
                            {
                                Projectile rune = Main.projectile[Projectile.NewProjectile(npc.Top + QwertyMethods.PolarVector(-120, (float)Math.PI * ((float)(i + 1) / (phase + 2))), Vector2.Zero, mod.ProjectileType("BigRune"), Main.expertMode ? 30 : 40, 0, Main.myPlayer)];
                                
                                int newRune = lastRune == 5 ? Main.rand.Next(4) : Main.rand.Next(3);
                                if(newRune >= lastRune)
                                {
                                    newRune++;
                                }
                                lastRune = newRune;
                                rune.ai[0] = lastRune;
                                rune.ai[1] = castingSpeed;
                                rune.netUpdate = true;
                            }
                        }

                    }
                    if (castTime >= 300)
                    {
                        casting = false;
                        StartMoving();
                        frame = 0;
                        castTime = 0;
                    }
                }
                else
                {
                    flightTime -= castingSpeed;
                    if (flightTime < 0)
                    {

                        drawRune = true;
                        casting = true;
                        castTime = 0;
                        if (Main.netMode != 1)
                        {
                            npc.netUpdate = true;
                        }
                    }
                }
            }
            
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if(casting)
            {
                Texture2D texture = Main.npcTexture[npc.type];
                spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle(0, 90 * frame, 82, 90), Color.White, npc.rotation, new Vector2(41, 45), Vector2.One, 0, 0);
            }
            else
            {
                Texture2D texture = RuneSprites.runeGhostMoving;
                int phaseTime = (int)(flightTimeMax / 3f);
                int phaseFrame =  (int)((float)RuneSprites.runeGhostPhaseIn.Length * (float)((int)flightTime % phaseTime) / phaseTime);
                if (phaseFrame > RuneSprites.runeGhostPhaseIn.Length - 1)
                {
                    phaseFrame = RuneSprites.runeGhostPhaseIn.Length - 1;
                }
                if (flightTime < phaseTime)
                {
                    float c = 1f - (flightTime / (float)phaseTime);
                    spriteBatch.Draw(RuneSprites.runeGhostPhaseIn[19-phaseFrame], npc.Center - Main.screenPosition, null, new Color(c, c, c, c), npc.rotation, texture.Size() * .5f, Vector2.One * 2, npc.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
                if (flightTime > flightTimeMax - phaseTime)
                {
                    float c = (flightTime / (float)phaseTime);
                    spriteBatch.Draw(RuneSprites.runeGhostPhaseIn[phaseFrame], npc.Center - Main.screenPosition, null, new Color(c, c, c, c), npc.rotation, texture.Size() * .5f, Vector2.One * 2, npc.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            }
            return false;
        }
        public override void NPCLoot()
        {
            if (!QwertyWorld.downedRuneGhost)
            {
                string key = "Mods.QwertysRandomContent.ArmsDealerStock";
                Color messageColor = Color.Lime;
                if (Main.netMode == 2) // Server
                {
                    NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText(Language.GetTextValue(key), messageColor);
                }
                QwertyWorld.downedRuneGhost = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state
                }
            }

            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                int runeCount = Main.rand.Next(20, 31);
                if (Main.rand.Next(7) == 0)
                    Item.NewItem(npc.getRect(), mod.ItemType("RunicRobe"));
                if (Main.rand.Next(7) == 0)
                    Item.NewItem(npc.getRect(), mod.ItemType("RuneGhostMask"));

                string scrollName = "";
                switch(Main.rand.Next(4))
                {
                    case 0:
                        scrollName = "AggroScroll";
                        break;
                    case 1:
                        scrollName = "LeechScroll";
                        break;
                    case 2:
                        scrollName = "IceScroll";
                        break;
                    case 3:
                        scrollName = "PursuitScroll";
                        break;
                }
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(scrollName));

                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 73, 30);
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CraftingRune"), runeCount);
            }

        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(npc.velocity);
            writer.WriteVector2(npc.position);
            writer.Write(flightTime);
            writer.Write(castTime);
            writer.Write(castingSpeed);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            
            npc.velocity = reader.ReadVector2();
            npc.position = reader.ReadVector2();
            flightTime = reader.ReadSingle();
            castTime = reader.ReadSingle();
            castingSpeed = reader.ReadSingle();
        }
    }
}
