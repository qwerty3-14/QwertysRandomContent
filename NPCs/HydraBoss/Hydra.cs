using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QwertysRandomContent.NPCs.HydraBoss
{
    public class Hydra : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            npc.width = 560;
            npc.height = 250;
            npc.damage = 0;
            npc.defense = 18;
            npc.boss = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 60f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;

            animationType = -1;
            npc.noGravity = true;
            npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/BeastOfThreeHeads");
            npc.lifeMax = 999999;
            bossBag = mod.ItemType("HydraBag");

        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * 1.2f);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            return 0f;

        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {


            }
            for (int i = 0; i < 10; i++)
            {
                int dustType = 148;
                int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }




        public int damage = 30;
        public NPC Head1;
        public NPC Head2;
        public NPC Head3;
        public NPC Head4;
        public NPC Head5;
        public NPC Head6;
        public NPC Head7;
        public NPC Head8;
        public NPC Head9;
        public bool Head1JustDied = true;
        public bool Head2JustDied = true;
        public bool Head3JustDied = true;
        public int headCheck = 0;
        public bool runOnce = true;
        public int head1wait;
        public int head2wait;
        public int head3wait;
        public override void AI()
        {

            if (runOnce)
            {
                if (Main.netMode != 1)
                {
                    Head1 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 100, mod.NPCType("Head1"), 0, npc.whoAmI)];
                    Head2 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 100, mod.NPCType("Head2"), 0, npc.whoAmI)];
                    Head3 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 100, mod.NPCType("Head3"), 0, npc.whoAmI)];
                }
                Head4 = new NPC();
                Head5 = new NPC();
                Head6 = new NPC();
                Head7 = new NPC();
                Head8 = new NPC();
                Head9 = new NPC();
                

                //
                //
                runOnce = false;
            }




            if (Main.netMode != 1)
            {
                if (!Head1.active && Head1JustDied)
                {
                    head1wait++;
                    if (head1wait > 2)
                    {
                        Head4 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 100, mod.NPCType("Head4"), 0, npc.whoAmI)];
                        Head5 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 100, mod.NPCType("Head5"), 0, npc.whoAmI)];

                        Head1JustDied = false;
                    }
                }



                if (!Head2.active && Head2JustDied)
                {
                    head2wait++;
                    if (head2wait > 2)
                    {
                        Head6 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 100, mod.NPCType("Head6"), 0, npc.whoAmI)];
                        Head7 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 100, mod.NPCType("Head7"), 0, npc.whoAmI)];


                        Head2JustDied = false;
                    }
                }

                if (!Head3.active && Head3JustDied)
                {
                    head3wait++;
                    if (head3wait > 2)
                    {
                        Head8 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 100, mod.NPCType("Head8"), 0, npc.whoAmI)];
                        Head9 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 100, mod.NPCType("Head9"), 0, npc.whoAmI)];

                        Head3JustDied = false;
                    }
                }

                if (head3wait > 2 && head2wait > 2 && head1wait > 2)
                {
                    if (Head1.active || Head2.active || Head3.active || Head4.active || Head5.active || Head6.active || Head7.active || Head8.active || Head9.active)
                    {
                        headCheck = 0;
                    }
                    else
                    {
                        headCheck++;
                        if (headCheck > 60)
                        {

                            npc.life = 0;
                            //QwertyWorld.downedhydra = true;
                            npc.checkDead();
                        }
                    }
                }
            }

            Player player = Main.player[npc.target];
            if (Main.netMode != 1)
            {

                player = Main.player[npc.target];
                npc.TargetClosest(true);
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

            Vector2 target = new Vector2(player.Center.X, player.Center.Y);
            Vector2 moveTo = new Vector2(target.X, target.Y) - npc.Center;


            npc.velocity = (moveTo) * .04f;














        }
        public override void FindFrame(int frameHeight)
        {

            npc.frameCounter++;
            if (npc.frameCounter < 5)
            {
                npc.frame.Y = 0 * frameHeight;
            }
            else if (npc.frameCounter < 10)
            {
                npc.frame.Y = 1 * frameHeight;
            }
            else if (npc.frameCounter < 15)
            {
                npc.frame.Y = 2 * frameHeight;
            }
            else if (npc.frameCounter < 20)
            {
                npc.frame.Y = 3 * frameHeight;
            }
            else if (npc.frameCounter < 25)
            {
                npc.frame.Y = 4 * frameHeight;
            }
            else
            {
                npc.frameCounter = 0;
            }
        }
        public void DrawHead(SpriteBatch spriteBatch, string headTexture, string glowMaskTexture, NPC head, Color drawColor)
        {
            if (head.active)
            {
                Vector2 neckOrigin = new Vector2(npc.Center.X, npc.Center.Y - 50);
                Vector2 center = head.Center;
                Vector2 distToProj = neckOrigin - head.Center;
                float projRotation = distToProj.ToRotation() - 1.57f;
                float distance = distToProj.Length();
                spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/HydraNeckBase"), neckOrigin - Main.screenPosition,
                            new Rectangle(0, 0, 52, 30), drawColor, projRotation,
                            new Vector2(52 * 0.5f, 30 * 0.5f), 1f, SpriteEffects.None, 0f);
                while (distance > 30f && !float.IsNaN(distance))
                {
                    distToProj.Normalize();                 //get unit vector
                    distToProj *= 30f;                      //speed = 30
                    center += distToProj;                   //update draw position
                    distToProj = neckOrigin - center;    //update distance
                    distance = distToProj.Length();


                    //Draw chain
                    spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/HydraNeck"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 52, 30), drawColor, projRotation,
                        new Vector2(52 * 0.5f, 30 * 0.5f), 1f, SpriteEffects.None, 0f);

                }
                spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/HydraNeckBase"), neckOrigin - Main.screenPosition,
                            new Rectangle(0, 0, 52, 30), drawColor, projRotation,
                            new Vector2(52 * 0.5f, 30 * 0.5f), 1f, SpriteEffects.None, 0f);

                spriteBatch.Draw(mod.GetTexture(headTexture), new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y),
                            head.frame, drawColor, head.rotation,
                            new Vector2(106 * 0.5f, 72 * 0.5f), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(mod.GetTexture(glowMaskTexture), new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y),
                        head.frame, Color.White, head.rotation,
                        new Vector2(106 * 0.5f, 72 * 0.5f), 1f, SpriteEffects.None, 0f);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
           
            spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/Hydra"), new Vector2(npc.position.X - Main.screenPosition.X, npc.position.Y - Main.screenPosition.Y),
                        npc.frame, drawColor, npc.rotation,
                        new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/Hydra_Glow"), new Vector2(npc.position.X - Main.screenPosition.X, npc.position.Y - Main.screenPosition.Y),
                        npc.frame, Color.White, npc.rotation,
                        new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            if (Main.netMode == 0)
            {
                DrawHead(spriteBatch, "NPCs/HydraBoss/Head1", "NPCs/HydraBoss/Head1_Glow", Head1, drawColor);
                DrawHead(spriteBatch, "NPCs/HydraBoss/Head2", "NPCs/HydraBoss/Head2_Glow", Head2, drawColor);
                DrawHead(spriteBatch, "NPCs/HydraBoss/Head3", "NPCs/HydraBoss/Head3_Glow", Head3, drawColor);
                DrawHead(spriteBatch, "NPCs/HydraBoss/Head1", "NPCs/HydraBoss/Head1_Glow", Head4, drawColor);
                DrawHead(spriteBatch, "NPCs/HydraBoss/Head1", "NPCs/HydraBoss/Head1_Glow", Head5, drawColor);
                DrawHead(spriteBatch, "NPCs/HydraBoss/Head2", "NPCs/HydraBoss/Head2_Glow", Head6, drawColor);
                DrawHead(spriteBatch, "NPCs/HydraBoss/Head2", "NPCs/HydraBoss/Head2_Glow", Head7, drawColor);
                DrawHead(spriteBatch, "NPCs/HydraBoss/Head3", "NPCs/HydraBoss/Head3_Glow", Head8, drawColor);
                DrawHead(spriteBatch, "NPCs/HydraBoss/Head3", "NPCs/HydraBoss/Head3_Glow", Head9, drawColor);

              
                
                
            }
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            
        }
        
        public override void NPCLoot()
        {
            QwertyWorld.downedhydra = true;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            if (Main.netMode != 1)
            {
                int centerX = (int)(npc.position.X + npc.width / 2) / 16;
                int centerY = (int)(npc.position.Y + npc.height / 2) / 16;
                int halfLength = npc.width / 2 / 16 + 1;

                int trophyChance = Main.rand.Next(0, 10);

                if (Main.expertMode)
                {
                    //npc.DropBossBags();
                    
                    if (Main.netMode == 0)
                    {
                        npc.DropBossBags();
                    }
                    else
                    {
                    npc.value = 0f;
                        for (int i = 0; i < 200; i++)
                        {
                            if(Main.player[i].active)
                            {
                                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraBag"));
                            }
                        }

                    }
                    
                    /*
                        if (Main.netMode == 2)
                        {
                            int num = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraBag"));
                            Main.itemLockoutTime[num] = 54000;
                            for (int i = 0; i < 255; i++)
                            {
                                if (Main.player[i].active)
                                {
                                    NetMessage.SendData(90, i, -1, null, num, 0f, 0f, 0f, 0, 0, 0);
                                }
                            }
                            Main.item[num].active = false;
                        }
                        else if (Main.netMode == 0)
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraBag"));
                        }
                        */






                }
                else
                {

                    int scaleCount = Main.rand.Next(20, 31);
                    int arrowCount = Main.rand.Next(80, 161);
                    int weaponLoot = Main.rand.Next(1, 4);
                    int getHook = Main.rand.Next(0, 100);
                    int getHydrator = Main.rand.Next(0, 100);

                    if (weaponLoot == 1)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Hydrent"));
                    }
                    if (weaponLoot == 2)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraBeam"));
                    }
                    if (weaponLoot == 3)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraCannon"));
                    }
                    if (getHook < 10)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraHook"));
                    }
                    if (getHydrator < 10)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Hydrator"));
                    }


                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 73, 12);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraHeadStaff"));
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraScale"), scaleCount);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraArrow"), arrowCount);

                }
                if (trophyChance == 1)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraTrophy"));
                }

            }
        }

    }
}
