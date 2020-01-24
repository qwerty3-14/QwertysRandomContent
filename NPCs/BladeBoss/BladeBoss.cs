using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.BladeBoss
{
    [AutoloadBossHead]
    public class BladeBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Imperious");
            Main.npcFrameCount[npc.type] = 1;

        }

        public override void SetDefaults()
        {

            npc.width = 698;
            npc.height = 698;
            npc.damage = 65;
            npc.defense = 24;
            npc.boss = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 150000f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            //aiType = 10;
            animationType = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/BladeOfAGod");
            npc.lifeMax = 25000;
            bossBag = mod.ItemType("BladeBossBag");
            npc.buffImmune[BuffID.Poisoned] = true;

            npc.ai[3] = 1;

        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            if (Main.rand.Next(100) == 0)
            {
                potionType = ItemID.CopperShortsword;
            }
            else
            {
                potionType = ItemID.GreaterHealingPotion;
            }
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {

            npc.lifeMax = (int)(32000 * bossLifeScale);
            npc.damage = 100;

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            return 0f;

        }
        bool activeCheck = false;
        public override bool CheckActive()
        {
            return activeCheck;
        }
        public override void NPCLoot()
        {
            QwertyWorld.downedBlade = true;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                switch (Main.rand.Next(6))
                {
                    case 0:
                        Item.NewItem(npc.Center, Vector2.Zero, mod.ItemType("SwordStormStaff"));
                        break;
                    case 1:
                        Item.NewItem(npc.Center, Vector2.Zero, mod.ItemType("ImperiousTheIV"));
                        break;
                    case 2:
                        Item.NewItem(npc.Center, Vector2.Zero, mod.ItemType("FlailSword"));
                        break;
                    case 3:
                        Item.NewItem(npc.Center, Vector2.Zero, mod.ItemType("SwordMinionStaff"));
                        break;
                    case 4:
                        Item.NewItem(npc.Center, Vector2.Zero, mod.ItemType("SwordsmanBadge"));
                        break;
                    case 5:
                        Item.NewItem(npc.Center, Vector2.Zero, mod.ItemType("BladedArrowShaft"));
                        break;
                }
            }
        }
        float bladeWidth = 74;
        float HiltLength = 94;
        float HiltWidth = 84;
        Vector2 BladeStart;
        Vector2 BladeTip;
        float BladeLength = 300;
        int timer;
        float preSwingPlayerDirection;
        float preSwingPlayerDistance;
        Vector2 hitSpot;
        Vector2 hitSpot2;
        Vector2 postSwingSpot;
        float intialBoost = 10;
        float directionOfAcceleration;
        float acceleration = .1f;
        int counter;
        bool finishSwing;
        float maxSpeed = 16;
        bool toHitSpot2;
        bool rocketMode;
        bool thrust;
        int attackCount = 3;
        /////
        int attack2Timer;
        float rotationSpeed;
        int rotationDirection = 1;
        int attack3Timer;
        int attack3Counter;
        public override bool PreAI()
        {
            counter++;
            if (counter < 60)
            {
                return false;
            }
            return true;
        }
        public override void AI()
        {
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];

            BladeStart = npc.Center + QwertyMethods.PolarVector(HiltLength / 2, npc.rotation + (float)Math.PI / 2);
            BladeTip = npc.Center + QwertyMethods.PolarVector((HiltLength / 2) + BladeLength, npc.rotation + (float)Math.PI / 2);
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0f, -40f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    activeCheck = true;
                    return;
                }
            }
            else
            {
                activeCheck = false;
                timer++;
                //Dust.NewDustPerfect(BladeStart + QwertyMethods.PolarVector(BladeLength / 2, npc.rotation + (float)Math.PI / 2), DustID.Fire);
                //npc.rotation = npc.velocity.ToRotation();

                if (npc.ai[3] == 0)
                {
                    if (npc.ai[2] < 6)
                    {
                        if (!rocketMode)
                        {
                            npc.rotation = QwertyMethods.SlowRotation(npc.rotation, npc.velocity.ToRotation(), 4);
                        }
                        if (timer == 1)
                        {
                            finishSwing = false;
                            toHitSpot2 = false;
                            rocketMode = false;
                            thrust = false;
                            preSwingPlayerDirection = (npc.Center - player.Center).ToRotation();
                            preSwingPlayerDistance = (player.Center - npc.Center).Length();
                            directionOfAcceleration = preSwingPlayerDirection + (float)Math.PI / 4;
                            hitSpot = QwertyMethods.PolarVector(BladeLength, preSwingPlayerDirection) + player.Center;
                            hitSpot2 = QwertyMethods.PolarVector(BladeLength, preSwingPlayerDirection + (float)Math.PI / 2) + player.Center;
                            postSwingSpot = QwertyMethods.PolarVector(1200, preSwingPlayerDirection + (float)Math.PI / 2) + player.Center;
                            npc.rotation = preSwingPlayerDirection + (float)Math.PI;
                            //npc.velocity = QwertyMethods.PolarVector(intialBoost, directionOfAcceleration);
                            //npc.velocity -= QwertyMethods.PolarVector(10, directionOfAcceleration + (float)Math.PI / 2);
                        }
                        //Dust.NewDustPerfect(hitSpot, DustID.Fire);
                        //Dust.NewDustPerfect(postSwingSpot, DustID.Fire);
                        //Dust.NewDustPerfect(QwertyMethods.PolarVector(preSwingPlayerDistance, preSwingPlayerDirection) + player.Center, DustID.Fire);

                        if (finishSwing)
                        {
                            npc.velocity = (postSwingSpot - npc.Center) * acceleration;
                            if ((npc.Center - postSwingSpot).Length() < 100 || rocketMode)
                            {
                                if ((npc.Center - player.Center).Length() < 1100)
                                {
                                    rocketMode = true;
                                    float goTo = ((npc.Center - player.Center).ToRotation() + (float)Math.PI / 2);
                                    goTo = QwertyMethods.PolarVector(1, goTo).ToRotation();
                                    npc.rotation = QwertyMethods.PolarVector(1, npc.rotation).ToRotation();
                                    if (Math.Abs(npc.rotation - goTo) < MathHelper.ToRadians(8) || thrust)
                                    {
                                        thrust = true;
                                        npc.velocity = QwertyMethods.PolarVector(25, npc.rotation + (float)Math.PI / 2);
                                    }
                                    else
                                    {
                                        npc.rotation = QwertyMethods.SlowRotation(npc.rotation, goTo, 4);
                                        //Main.NewText(MathHelper.ToDegrees(Math.Abs(npc.rotation - ((npc.Center - player.Center).ToRotation() + (float)Math.PI / 2))));
                                        //Main.NewText(npc.rotation + ", " + goTo);

                                    }

                                }
                                else
                                {
                                    finishSwing = false;
                                    timer = 0;
                                }

                            }
                        }
                        else if (toHitSpot2)
                        {
                            npc.velocity = (hitSpot2 - npc.Center) * acceleration;
                            if ((npc.Center - hitSpot2).Length() < 100)
                            {

                                if (Main.netMode != 1)
                                {
                                    finishSwing = true;
                                    npc.netUpdate = true;
                                }
                                npc.ai[2]++;
                            }
                        }
                        else
                        {
                            npc.velocity = (hitSpot - npc.Center) * acceleration;
                            if ((npc.Center - hitSpot).Length() < 100)
                            {
                                toHitSpot2 = true;
                            }

                        }
                        if (npc.velocity.Length() > maxSpeed && !thrust)
                        {
                            npc.velocity = npc.velocity.SafeNormalize(-Vector2.UnitY) * maxSpeed;
                        }
                    }
                    else
                    {

                        if (Main.netMode != 1)
                        {
                            npc.ai[2] = 0;
                            npc.ai[3] = Main.rand.Next(attackCount - 1);
                            if (npc.ai[3] >= 0)
                            {
                                npc.ai[3]++;
                            }
                            npc.netUpdate = true;
                        }
                    }
                }
                else if (npc.ai[3] == 1)
                {

                    if (attack2Timer == 0)
                    {
                        preSwingPlayerDirection = (npc.Center - player.Center).ToRotation();
                        rotationSpeed = ((float)Math.PI / 1000) * 15f;
                    }
                    hitSpot = QwertyMethods.PolarVector(BladeLength / 2, preSwingPlayerDirection) + player.Center;
                    if (attack2Timer < 60)
                    {
                        npc.rotation = preSwingPlayerDirection - (float)Math.PI / 2;
                        rotationSpeed = ((float)Math.PI / 1000) * 15f;
                    }
                    else
                    {
                        npc.rotation += rotationSpeed * rotationDirection;
                    }
                    if (attack2Timer > 600)
                    {
                        npc.velocity = ((hitSpot + QwertyMethods.PolarVector(1000, -preSwingPlayerDirection)) - npc.Center) * acceleration;
                        if (attack2Timer > 620)
                        {
                            attack2Timer = 0;
                            if (Main.netMode != 1)
                            {
                                npc.ai[3] = Main.rand.Next(attackCount - 1);
                                if (npc.ai[3] >= 1)
                                {
                                    npc.ai[3]++;
                                }
                                npc.netUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        npc.velocity = (hitSpot - npc.Center) * acceleration;
                    }



                    attack2Timer++;
                }
                else if (npc.ai[3] == 2)
                {
                    //Main.NewText(attack3Timer);
                    if (attack3Counter < 8)
                    {
                        if (attack3Timer == 0)
                        {
                            if (Main.netMode != 1)
                            {

                                npc.Center = QwertyMethods.PolarVector(1200, Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI)) + player.Center;
                                preSwingPlayerDirection = (npc.Center - player.Center).ToRotation();
                                npc.rotation = preSwingPlayerDirection + (float)Math.PI / 2;
                                npc.netUpdate = true;

                                if (attack3Counter == 2 || attack3Counter == 5)
                                {
                                    Vector2 Spawn = QwertyMethods.PolarVector(300, npc.rotation - (float)Math.PI / 2) + npc.Center;
                                    Spawn.Y += 168;
                                    NPC.NewNPC((int)Spawn.X, (int)Spawn.Y, mod.NPCType("BladeMinion"), ai0: npc.rotation);
                                }
                            }

                        }
                        else
                        {
                            npc.rotation = preSwingPlayerDirection + (float)Math.PI / 2;
                            npc.velocity = QwertyMethods.PolarVector(20, npc.rotation + (float)Math.PI / 2);
                            /*
                            float goTo = ((npc.Center - player.Center).ToRotation() + (float)Math.PI / 2);
                            goTo = QwertyMethods.PolarVector(1, goTo).ToRotation();
                            npc.rotation = QwertyMethods.PolarVector(1, npc.rotation).ToRotation();
                            Main.NewText(Math.Abs(npc.rotation - goTo) > (float)Math.PI / 2);
                            */
                        }
                        attack3Timer++;
                        if ((npc.Center - player.Center).Length() > 1300)
                        {
                            attack3Timer = 0;
                            attack3Counter++;
                        }
                    }
                    else
                    {
                        if (Main.netMode != 1)
                        {

                            npc.Center = QwertyMethods.PolarVector(1200, Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI)) + player.Center;
                            preSwingPlayerDirection = (npc.Center - player.Center).ToRotation();

                            npc.netUpdate = true;


                        }
                        attack3Counter = 0;
                        if (Main.netMode != 1)
                        {
                            npc.ai[3] = Main.rand.Next(attackCount - 1);
                            if (npc.ai[3] >= 2)
                            {
                                npc.ai[3]++;
                            }
                            npc.netUpdate = true;
                        }
                    }
                }
                /*
                    if (Main.netMode == 1)
                 {
                     Main.NewText("client: " + npc.Center);
                 }


                 if (Main.netMode == 2) // Server
                 {
                     NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + npc.Center), Color.Black);
                 }
                 */
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/BladeBoss/BladeBoss");
            spriteBatch.Draw(texture, new Vector2(npc.Center.X - Main.screenPosition.X, npc.Center.Y - Main.screenPosition.Y),
                        npc.frame, drawColor, npc.rotation,
                        new Vector2(HiltWidth * 0.5f, HiltLength * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            float col = 0;
            if ((attack2Timer < 60 || attack2Timer > 600) && npc.ai[3] == 1)
            {
                return false;
            }
            return Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Hitbox.Size(), BladeStart, BladeTip, bladeWidth, ref col);
        }
        public override bool? CanBeHitByProjectile(Projectile target)
        {
            float col = 0;
            return Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Hitbox.Size(), BladeStart, BladeTip, bladeWidth, ref col);
        }
        public override bool? CanBeHitByItem(Player player, Item target)
        {
            float col = 0;
            return Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Hitbox.Size(), BladeStart, BladeTip, bladeWidth, ref col);
        }
        public override bool? CanHitNPC(NPC target)
        {
            float col = 0;
            return Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Hitbox.Size(), BladeStart, BladeTip, bladeWidth, ref col);
        }
        Vector2 CollisionOffset;

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.netMode == 0)
            {
                npc.width = 2;
                npc.height = 2;
                CollisionOffset = projectile.Center - npc.position;
                npc.position += CollisionOffset;
            }
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            if (Main.netMode == 0)
            {
                npc.width = 698;
                npc.height = 698;
                npc.position -= CollisionOffset;
            }
            Player player = Main.player[npc.target];
            if (npc.ai[3] == 1)
            {
                float c = 0;
                if (Collision.CheckAABBvLineCollision(player.position, player.Size, BladeStart + QwertyMethods.PolarVector(BladeLength / 2, npc.rotation + (float)Math.PI / 2), BladeStart + QwertyMethods.PolarVector(BladeLength / 2, npc.rotation + (float)Math.PI / 2) + -rotationDirection * QwertyMethods.PolarVector(300, npc.rotation), BladeLength, ref c))
                {
                    if (rotationSpeed < (float)Math.PI / 1000f * 35)
                    {
                        rotationSpeed += (float)Math.PI / 1000f * 5;

                    }
                    rotationDirection *= -1;
                }
            }
        }
        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (Main.netMode == 0)
            {
                npc.width = 2;
                npc.height = 2;
                CollisionOffset = item.Center - npc.position;
                npc.position += CollisionOffset;
            }
        }
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            if (Main.netMode == 0)
            {
                npc.width = 698;
                npc.height = 698;
                npc.position -= CollisionOffset;
            }
            if (npc.ai[3] == 1)
            {
                float c = 0;
                if (Collision.CheckAABBvLineCollision(player.position, player.Size, BladeStart + QwertyMethods.PolarVector(BladeLength / 2, npc.rotation + (float)Math.PI / 2), BladeStart + QwertyMethods.PolarVector(BladeLength / 2, npc.rotation + (float)Math.PI / 2) + -rotationDirection * QwertyMethods.PolarVector(300, npc.rotation), BladeLength, ref c))
                {
                    if (rotationSpeed < (float)Math.PI / 1000f * 35)
                    {
                        rotationSpeed += (float)Math.PI / 1000f * 5;

                    }
                    rotationDirection *= -1;
                }
            }
        }
        public override void BossHeadRotation(ref float rotation)
        {

            rotation = npc.rotation;

        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(finishSwing);
            writer.Write(preSwingPlayerDirection);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            finishSwing = reader.ReadBoolean();
            preSwingPlayerDirection = reader.ReadSingle();
        }

    }

}
