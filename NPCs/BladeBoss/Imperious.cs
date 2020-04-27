using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.BladeBoss
{
    [AutoloadBossHead]
    public class Imperious : ModNPC
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicImperious ? base.Texture + "_Old" : base.Texture;
        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Imperious");
            Main.npcFrameCount[npc.type] = 1;

        }

        public override void SetDefaults()
        {

            npc.width = 10;
            npc.height = 10;
            npc.damage = 65;
            npc.defense = 42;
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
            npc.chaseable = false;
            npc.immortal = true;
            npc.alpha = 255;
            npc.behindTiles = true;
            npc.buffImmune[BuffID.Ichor] = true;
        }
        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return true;
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
            for (int h = 0; h < hitBoxSegmentIds.Length; h++)
            {
                if (hitBoxSegmentIds[h] != -1)
                {
                    Main.npc[hitBoxSegmentIds[h]].life = 0;
                    Main.npc[hitBoxSegmentIds[h]].HitEffect(0, 10.0);
                    Main.npc[hitBoxSegmentIds[h]].active = false;
                }
            }
            ClearPhantoms();
            QwertyWorld.downedBlade = true;
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            }
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                switch (Main.rand.Next(8))
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
                    case 6:
                        Item.NewItem(npc.Center, Vector2.Zero, mod.ItemType("Imperium"));
                        break;
                    case 7:
                        Item.NewItem(npc.Center, Vector2.Zero, mod.ItemType("Swordquake"));
                        break;
                }
            }
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem(npc.Center, Vector2.Zero, mod.ItemType("BladeBossTrophy"));
            }
        }
        List<Vector3> nextPositions = new List<Vector3>();
        float maxSpeed = 30f;
        float maxSwingSpeed = (float)Math.PI / 80;
        int[] hitBoxSegmentIds = { -1, -1, -1, -1, -1, -1, -1, -1 };
        int totalLength = 398;
        int bladeLength = 308;
        int bladeWidth = 82;
        bool debug = false;
        float swingTargetDistance = 200;
        int SpecialAttack
        {
            get => (int)npc.ai[0];
            set => npc.ai[0] = value;
        }
        const int none = 0;
        const int swordquake = 1;
        const int starRage = 2;
        const int phantomCircle = 3; //this attack is disabled in multiplayer since it would probably feel cheap for the player not being targeted
        int SpecialAttackTimer
        {
            get => (int)npc.ai[1];
            set => npc.ai[1] = value;
        }
        int swordlagmiteCount = 24;
        int swordlagmiteStart = 120;
        int followupSwordlagmiteDelay = 10;
        int followupSwordlagmiteDelay2 = 120;
        bool secondPhase = false;
        List<int> PhantomBladeIds = new List<int>();
        Vector3[] trailingEffect = new Vector3[10];
        void CreatePhantom()
        {
             
            if (secondPhase)
            {
                Main.PlaySound(SoundID.Item8, npc.Center);
                if (Main.netMode != 1)
                {
                    PhantomBladeIds.Add(Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("PhantomBlade"), (int)(npc.damage * (Main.expertMode ? .25f : .5f)), 0, ai0: npc.rotation));
                    Main.projectile[PhantomBladeIds[PhantomBladeIds.Count - 1]].rotation = Main.projectile[PhantomBladeIds[PhantomBladeIds.Count - 1]].ai[0] = npc.rotation;
                    Main.projectile[PhantomBladeIds[PhantomBladeIds.Count - 1]].netUpdate = true;
                    npc.netUpdate = true;
                }
            }
        }
        void ClearPhantoms()
        {
            foreach(int p in PhantomBladeIds)
            {
                Main.projectile[p].Kill();
            }
            PhantomBladeIds.Clear();
        }
        void FindMaxSpeed()
        {
            if (Main.netMode != 1)
            {
                Vector2 diff = nextPositions[0].to2() - npc.Center;
                maxSpeed = diff.Length() / (QwertyMethods.AngularDifference(npc.rotation, nextPositions[0].Z) / maxSwingSpeed);
                if (maxSpeed > 24f)
                {                          
                    maxSpeed = 24f;
                }
                npc.netUpdate = true;
            }
            
        }
        void AddPosition(Vector3 orientation)
        {
            if (Main.netMode != 1)
            {
                nextPositions.Add(orientation);
                if (nextPositions.Count == 1)
                {
                    FindMaxSpeed();
                }
                npc.netUpdate = true;
            }
        }
        
        public override void AI()
        {
            followupSwordlagmiteDelay2 = secondPhase ? 90 : 120;
            npc.frameCounter++;
            if(npc.frameCounter % 2 ==0)
            {
                for (int v = trailingEffect.Length-1; v >0; v--)
                {
                    trailingEffect[v] = trailingEffect[v - 1];
                }
            }
            
            trailingEffect[0] = new Vector3(npc.Center.X, npc.Center.Y, npc.rotation);
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0f, -30f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                }
            }
            else
            {
                if (!secondPhase && Main.expertMode && (float)npc.life / npc.lifeMax < .5f)
                {
                    npc.dontTakeDamage = true;
                    npc.ai[2]++;
                    npc.velocity = Vector2.Zero;
                    SpecialAttack = none;
                    SpecialAttackTimer = 0;
                    if (npc.ai[2] > 120)
                    {
                        npc.dontTakeDamage = false;
                        secondPhase = true;
                    }
                }
                else
                {

                    //debug area
                    if (debug)
                    {
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            AddPosition(new Vector3(Main.MouseWorld.X, Main.MouseWorld.Y, (Main.MouseWorld - player.Center).ToRotation()));
                        }
                    }

                    //move to planned location
                    if (nextPositions.Count > 0)
                    {
                        Vector2 diff = nextPositions[0].to2() - npc.Center;
                        npc.velocity = diff;
                        if (npc.velocity.Length() > maxSpeed)
                        {
                            npc.velocity = npc.velocity.SafeNormalize(Vector2.UnitY) * maxSpeed;
                        }
                        npc.rotation.SlowRotation(nextPositions[0].Z, maxSwingSpeed);
                        if (diff.Length() < .01f && QwertyMethods.AngularDifference(nextPositions[0].Z, npc.rotation) < .01f)
                        {
                            if (Main.netMode != 1)
                            {
                                nextPositions.RemoveAt(0);
                                if (nextPositions.Count > 0)
                                {
                                    FindMaxSpeed();
                                }
                                npc.netUpdate = true;
                            }

                        }
                    }
                    else
                    {
                        npc.velocity = Vector2.Zero;
                    }

                    //plan where to go if there is no where planned
                    if (nextPositions.Count == 0)
                    {
                        switch ((int)SpecialAttack)
                        {
                            case none:
                                CreatePhantom();
                                SpecialAttackTimer++;
                                float towardMe = (npc.Center - player.Center).ToRotation();
                                if (((player.Center - npc.Center).Length() < totalLength && QwertyMethods.AngularDifference(towardMe + (float)Math.PI, npc.rotation) < (float)Math.PI / 2) || SpecialAttackTimer >= 10)
                                {
                                    Vector2 pos = player.Center + QwertyMethods.PolarVector(500, towardMe);
                                    AddPosition(new Vector3(pos.X, pos.Y, towardMe));
                                    if (SpecialAttackTimer >= 10)
                                    {
                                        ClearPhantoms();
                                        SpecialAttackTimer = 0;
                                        npc.netUpdate = true;
                                        if (Main.netMode != 1)
                                        {
                                            switch (Main.rand.Next((secondPhase && Main.netMode == 0) ? 3 : 2))
                                            {
                                                case 0:
                                                    AddPosition(new Vector3(pos.X, pos.Y, (float)Math.PI / 2));
                                                    AddPosition(new Vector3(pos.X, pos.Y - 40, (float)Math.PI / 2));
                                                    SpecialAttack = swordquake;
                                                    break;
                                                case 1:
                                                    AddPosition(new Vector3(pos.X, pos.Y, -(float)Math.PI / 2));
                                                    SpecialAttack = starRage;
                                                    break;
                                                case 2:
                                                    AddPosition(new Vector3(pos.X, pos.Y, -(float)Math.PI / 2));
                                                    AddPosition(new Vector3(pos.X, pos.Y + 40, -(float)Math.PI / 2));
                                                    AddPosition(new Vector3(pos.X, pos.Y - 400, -(float)Math.PI / 2));
                                                    SpecialAttack = phantomCircle;
                                                    break;
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    Vector2 pos = player.Center + QwertyMethods.PolarVector(swingTargetDistance, towardMe);
                                    AddPosition(new Vector3(pos.X, pos.Y, towardMe + (float)Math.PI));
                                }
                                break;
                            case swordquake:
                                if (Collision.CanHit(npc.Center, 0, 0, npc.Center + Vector2.UnitY * bladeLength, 0, 0))
                                {
                                    npc.velocity = Vector2.UnitY * 24f;
                                }
                                else
                                {
                                    npc.velocity = Vector2.Zero;
                                    SpecialAttackTimer++;
                                    Vector2 spawnPos = npc.Center + Vector2.UnitY * bladeLength;
                                    if (SpecialAttackTimer >= swordlagmiteStart + followupSwordlagmiteDelay2 + swordlagmiteCount * followupSwordlagmiteDelay)
                                    {
                                        SpecialAttackTimer = 0;
                                        SpecialAttack = none;
                                    }
                                    else
                                    {
                                        if (SpecialAttackTimer >= swordlagmiteStart + followupSwordlagmiteDelay2 && SpecialAttackTimer < swordlagmiteStart + followupSwordlagmiteDelay2 + swordlagmiteCount * followupSwordlagmiteDelay)
                                        {
                                            if (SpecialAttackTimer % followupSwordlagmiteDelay == 0)
                                            {
                                                int wave = (SpecialAttackTimer - (swordlagmiteStart + followupSwordlagmiteDelay2)) / followupSwordlagmiteDelay;
                                                if (Main.netMode != 1)
                                                {
                                                    SpawnSwordlagmite(spawnPos + Vector2.UnitX * (2 * wave * bladeWidth));

                                                    if (SpecialAttackTimer != 300)
                                                    {
                                                        SpawnSwordlagmite(spawnPos + Vector2.UnitX * (-2 * wave * bladeWidth));
                                                    }
                                                }
                                            }
                                        }
                                        if (SpecialAttackTimer >= swordlagmiteStart && SpecialAttackTimer < swordlagmiteStart + swordlagmiteCount * followupSwordlagmiteDelay)
                                        {
                                            if (SpecialAttackTimer % followupSwordlagmiteDelay == 0)
                                            {
                                                int wave = (SpecialAttackTimer - swordlagmiteStart) / followupSwordlagmiteDelay;
                                                if (Main.netMode != 1)
                                                {
                                                    SpawnSwordlagmite(spawnPos + Vector2.UnitX * (2 * wave * bladeWidth + bladeWidth));
                                                    SpawnSwordlagmite(spawnPos + Vector2.UnitX * (-2 * wave * bladeWidth - bladeWidth));
                                                }
                                            }
                                        }
                                    }

                                }
                                break;
                            case starRage:
                                SpecialAttackTimer++;
                                if (SpecialAttackTimer > 600)
                                {
                                    SpecialAttackTimer = 0;
                                    SpecialAttack = none;
                                }
                                else if (SpecialAttackTimer > 60)
                                {
                                    if (SpecialAttackTimer > 480 && QwertyMethods.AngularDifference(npc.rotation, (float)-Math.PI / 2) < (float)Math.PI / 30)
                                    {
                                        npc.rotation.SlowRotation((float)-Math.PI / 2, maxSwingSpeed);
                                    }
                                    else
                                    {


                                        if (SpecialAttackTimer % 16 == 0)
                                        {
                                            Main.PlaySound(SoundID.Item105, npc.Center);
                                        }
                                        int starDirection = (player.Center.X - npc.Center.X > 0 ? 1 : -1);
                                        npc.rotation += (float)Math.PI / 30 * starDirection;
                                        npc.velocity = (player.Center - npc.Center).SafeNormalize(-Vector2.UnitY) * (secondPhase ? 4f : 2f);
                                        int impactZoneWidth = 1000;
                                        int startAwayAmount = 800;
                                        float shootSpeed = 10f;
                                        if (SpecialAttackTimer % 16 == 0)
                                        {
                                            if (Main.netMode != 1)
                                            {
                                                Vector2 fallToHere = new Vector2((player.Center.X + (player.velocity.X * startAwayAmount / shootSpeed)) - impactZoneWidth / 2 + Main.rand.Next(impactZoneWidth), player.Bottom.Y);
                                                Vector2 positionOffset = QwertyMethods.PolarVector(startAwayAmount, (float)Math.PI + (float)Math.PI / 2 - (float)Math.PI / 4 + Main.rand.NextFloat() * (float)Math.PI / 8);
                                                positionOffset.X *= starDirection;
                                                Projectile.NewProjectile(fallToHere + positionOffset, QwertyMethods.PolarVector(shootSpeed, (positionOffset).ToRotation() + (float)Math.PI), mod.ProjectileType("Swordpocalypse"), (int)(npc.damage * (Main.expertMode ? .2f : .4f)), 0);
                                            }
                                        }
                                    }
                                }
                                break;
                            case phantomCircle:

                                npc.rotation.SlowRotation((float)Math.PI / 2, maxSwingSpeed);
                                SpecialAttackTimer++;
                                int circleDelay = 90;
                                int circleSpeed = 14;
                                int cicrleRange = 1400;
                                float swordCount = 7f;
                                int waveCount = 7;
                                if (SpecialAttackTimer >= circleDelay * waveCount + (cicrleRange - bladeLength + 18) / circleSpeed)
                                {
                                    SpecialAttackTimer = 0;
                                    SpecialAttack = none;
                                }
                                else
                                {

                                    if (SpecialAttackTimer <= circleDelay * waveCount && SpecialAttackTimer % circleDelay == 0)
                                    {
                                        Main.PlaySound(SoundID.Item8, player.Center);
                                        if (Main.netMode != 1)
                                        {
                                            float offset = Main.rand.NextFloat() * 2 * (float)Math.PI;
                                            if (SpecialAttackTimer == circleDelay * waveCount)
                                            {
                                                offset = -(float)Math.PI / 2;
                                            }
                                            for (int i = (SpecialAttackTimer == circleDelay * waveCount ? 1 : 0); i < swordCount; i++)
                                            {
                                                Projectile p = Main.projectile[Projectile.NewProjectile(player.Center + QwertyMethods.PolarVector(cicrleRange, (i / swordCount) * 2 * (float)Math.PI + offset), QwertyMethods.PolarVector(-circleSpeed, (i / swordCount) * 2 * (float)Math.PI + offset), mod.ProjectileType("PhantomBlade"), (int)(npc.damage * (Main.expertMode ? .25f : .5f)), 0, ai0: npc.rotation)];
                                                p.timeLeft = (cicrleRange - bladeLength + 18) / circleSpeed;
                                                p.rotation = p.ai[0] = (i / swordCount) * 2 * (float)Math.PI + offset + (float)Math.PI;
                                                p.netUpdate = true;
                                            }
                                        }
                                    }
                                    if (SpecialAttackTimer >= circleDelay * waveCount)
                                    {
                                        npc.velocity = Vector2.UnitY * circleSpeed;
                                    }
                                    else
                                    {
                                        Vector2 goTo = player.Center - Vector2.UnitY * cicrleRange;
                                        if ((goTo - npc.Center).Length() < 30f)
                                        {
                                            npc.Center = goTo;
                                            npc.velocity = Vector2.Zero;
                                        }
                                        else
                                        {
                                            npc.velocity = (goTo - npc.Center).SafeNormalize(-Vector2.UnitY) * 30f;
                                        }
                                    }
                                }


                                break;
                        }

                    }


                }
            }
            //position hitbox segments
            npc.realLife = npc.whoAmI;
            for (int h = 0; h < hitBoxSegmentIds.Length; h++)
            {
                Vector2 spot = npc.Center + npc.velocity + QwertyMethods.PolarVector((totalLength - bladeLength - 18) + h * (bladeLength / (hitBoxSegmentIds.Length + 1)) + bladeWidth / 2, npc.rotation);
                if (hitBoxSegmentIds[h] == -1)
                {
                    if (Main.netMode != 1)
                    {
                        hitBoxSegmentIds[h] = NPC.NewNPC((int)spot.X, (int)spot.Y, mod.NPCType("BladeHitbox"));
                        Main.npc[hitBoxSegmentIds[h]].realLife = npc.realLife;
                        npc.netUpdate = true;
                    }

                }
                else
                {
                    Main.npc[hitBoxSegmentIds[h]].Center = spot;
                    Main.npc[hitBoxSegmentIds[h]].timeLeft = 10;
                    Lighting.AddLight(spot, 1f, 1f, 1f);
                    Lighting.AddLight(spot + QwertyMethods.PolarVector(bladeWidth / 2, npc.rotation), 1f, 1f, 1f);
                }

            }
        }
        void SpawnSwordlagmite(Vector2 pos)
        {
            while(Collision.CanHit(pos, 0, 0, pos + Vector2.UnitY * 4, 0, 0))
            {
                pos.Y++;
            }
            pos.Y += bladeLength;
            Projectile.NewProjectile(pos, Vector2.Zero, mod.ProjectileType("Swordlacmite"), (int)(npc.damage * (Main.expertMode ? .25f : .5f)), 0f);
        }
        public override void DrawEffects(ref Color drawColor)
        {

            drawColor.R = (byte)(drawColor.R * (1f - .4f * npc.ai[2]/120f));
            drawColor.G = (byte)(drawColor.G * (1f - .4f * npc.ai[2] / 120f));
            drawColor.B = (byte)(drawColor.B * (1f - .4f * npc.ai[2] / 120f));


        }
        int phsaeChangeAnimationSpeed = 40;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if(secondPhase)
            {
                GameShaders.Armor.Apply(1050, npc);
            }
            if (debug)
            {
                foreach (Vector3 pos in nextPositions)
                {
                    Texture2D texture2 = mod.GetTexture("NPCs/BladeBoss/DebugBlade");
                    spriteBatch.Draw(texture2, pos.to2() - Main.screenPosition, null, drawColor, pos.Z, new Vector2(18, texture2.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);
                }
            }

            Texture2D texture = Main.npcTexture[npc.type];
            
            if (!secondPhase && Main.expertMode && (float)npc.life / npc.lifeMax < .5f)
            {
                int alpha = (int)(120f *(((int)npc.ai[2] % phsaeChangeAnimationSpeed) / (float)phsaeChangeAnimationSpeed));
                spriteBatch.Draw(texture, npc.Center + QwertyMethods.PolarVector(totalLength/2, npc.rotation) - Main.screenPosition, null, new Color(alpha, alpha, alpha, alpha), npc.rotation, new Vector2(texture.Width/2f, texture.Height / 2f), new Vector2(1f, 1f)  * (1 + (1 * (phsaeChangeAnimationSpeed - (int)npc.ai[2] % phsaeChangeAnimationSpeed) / (float)phsaeChangeAnimationSpeed)), SpriteEffects.None, 0f);
            }
            if(secondPhase)
            {
                for(int v =0; v< trailingEffect.Length; v++)
                {
                    if (trailingEffect[v] != null)
                    {
                        spriteBatch.Draw(texture, trailingEffect[v].to2() - Main.screenPosition, null, new Color(v*3, v* 3, v* 3, v*3), trailingEffect[v].Z, new Vector2(18, texture.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);
                    }
                }
            }
            spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(18, texture.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);



            if (debug)
            {
                texture = mod.GetTexture("NPCs/BladeBoss/DebugBladeOutline");
                spriteBatch.Draw(texture, Main.MouseWorld - Main.screenPosition, null, drawColor, (Main.MouseWorld - Main.LocalPlayer.Center).ToRotation(), new Vector2(18, texture.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);
                
            }
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(nextPositions.Count);
            for(int i =0; i < nextPositions.Count; i++)
            {
                writer.WriteVector2(nextPositions[i].to2());
                writer.Write(nextPositions[i].Z);
            }
            writer.Write(maxSpeed);
            writer.WriteVector2(npc.position);
            for (int i = 0; i < hitBoxSegmentIds.Length; i++)
            {
                writer.Write(hitBoxSegmentIds[i]);
            }
            writer.Write(PhantomBladeIds.Count);
            for(int i =0; i < PhantomBladeIds.Count; i++)
            {
                writer.Write(PhantomBladeIds[i]);
            }
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            nextPositions.Clear();
            int count = reader.ReadInt32();
            for(int i =0; i < count; i++)
            {
                Vector2 pos = reader.ReadVector2();
                float rot = reader.ReadSingle();
                nextPositions.Add(new Vector3(pos.X, pos.Y, rot));
            }
            maxSpeed = reader.ReadSingle();
            npc.position = reader.ReadVector2();
            for (int i = 0; i < hitBoxSegmentIds.Length; i++)
            {
                hitBoxSegmentIds[i] = reader.ReadInt32();
            }
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                PhantomBladeIds[i] = reader.ReadInt32();
            }
        }
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = npc.rotation;
        }
    }
    public class BladeHitbox : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Imperious");
        }
        public override void SetDefaults()
        {

            npc.width = 82;
            npc.height = 82;
            npc.damage = 65;
            npc.defense = 42;
            npc.boss = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 0f;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.lifeMax = 25000;
            npc.buffImmune[BuffID.Ichor] = true;

        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {

            npc.lifeMax = (int)(32000 * bossLifeScale);
            npc.damage = 100;

        }
        public override void AI()
        {
            
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            List<int> hitboxIds = new List<int>();
            for(int i =0; i < Main.npc.Length; i++)
            {
                if(Main.npc[i].type == mod.NPCType("BladeHitbox"))
                {
                    hitboxIds.Add(i);
                }
            }
            if(projectile.usesLocalNPCImmunity || projectile.localNPCImmunity[npc.whoAmI] != 0)
            {
                foreach(int who in hitboxIds)
                {
                    projectile.localNPCImmunity[who] = projectile.localNPCImmunity[npc.whoAmI];
                    Main.npc[who].immune[projectile.owner] = npc.immune[projectile.owner];
                }
            }
            else
            {
                foreach (int who in hitboxIds)
                {
                    Main.npc[who].immune[projectile.owner] = npc.immune[projectile.owner];
                }
            }
        }
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            List<int> hitboxIds = new List<int>();
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].type == mod.NPCType("BladeHitbox"))
                {
                    hitboxIds.Add(i);
                }
            }
            foreach (int who in hitboxIds)
            {
                Main.npc[who].immune[player.whoAmI] = npc.immune[player.whoAmI];
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            for(int b =0; b < npc.buffType.Length; b++)
            {
                if(npc.buffType[b] != 0)
                {
                    target.AddBuff(npc.buffType[b], npc.buffTime[b]);
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false ;
        }
        

    }
    public class SwordQuakeShake : ModPlayer
    {
        public bool shake = false;
        int repoX = 0;
        int repoY = 0;
        int time = 0;
        public override void ResetEffects()
        {
            shake = false;
        }
        public override void PreUpdate()
        {
            
        }
        public override void ModifyScreenPosition()
        {
            if (NPC.AnyNPCs(mod.NPCType("Imperious")))
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    if (Main.npc[i].type == mod.NPCType("Imperious"))
                    {
                        if (Main.npc[i].ai[0] == 1 && Main.npc[i].ai[1] > 0)
                        {
                            shake = true;
                            time = (int)Main.npc[i].ai[1];
                        }
                    }
                }
            }
            
            if (shake)
            {
                if (time % 3==0)
                {
                    repoX = Main.rand.Next(-10, 11);
                    repoY = Main.rand.Next(-10, 11);
                }
            }
            else
            {
                repoX = repoY = 0;
            }
            Main.screenPosition.X += repoX;
            Main.screenPosition.Y += repoY;
            
        }
    }
    public class Swordlacmite : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordlagmite");
            ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true; // projectiles with hide but without this will draw in the lighting values of the owner player.
        }
        public override void SetDefaults()
        {
            projectile.width = 82;
            projectile.height = 2;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 190;
            projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindNPCsAndTiles.Add(index);
        }
        
        const int lingerTime = 60;
        const int extendSpeed = 30;
        public override void AI()
        {
            if(projectile.timeLeft == lingerTime)
            {
                projectile.height += extendSpeed;
                projectile.position.Y -= extendSpeed;

                Player player = null;
                float dist = -1f;
                for(int i =0; i < Main.player.Length; i++)
                {
                    if(Main.player[i].active && (Math.Abs(Main.player[i].Center.X- projectile.Center.X)< dist || dist == -1f))
                    {
                        player = Main.player[i];
                        dist = Math.Abs(Main.player[i].Center.X - projectile.Center.X);
                    }
                }
                if (player != null)
                {
                    if (!(projectile.position.Y < player.Center.Y - 100))
                    {
                        projectile.timeLeft++;
                    }
                }
                
                
            }
            if (projectile.timeLeft == lingerTime - 1)
            {
                Main.PlaySound(SoundID.Item69, projectile.Center);
            }
            if (projectile.timeLeft == 1)
            {
                projectile.height -= extendSpeed;
                projectile.position.Y += extendSpeed;
                if(projectile.height> extendSpeed)
                {
                    projectile.timeLeft++;
                }
            }
        }
        
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if(projectile.timeLeft > 130)
            {
                Texture2D line = mod.GetTexture("NPCs/BladeBoss/WarningLaser");
                spriteBatch.Draw(line, new Vector2(projectile.Center.X - Main.screenPosition.X, Main.screenHeight), null, ((projectile.timeLeft % 10 == 0) ? Color.White : Color.Red), 0f, new Vector2(1, 6), new Vector2(1f, Main.screenHeight / 6), 0, 0);
            }
            int tipHeight = 60;
            int segmentHeight = 40;
            Texture2D texture = Main.projectileTexture[projectile.type];
            int k = 0;
            spriteBatch.Draw(texture, projectile.position + Vector2.UnitY * (k * segmentHeight) - Main.screenPosition, new Rectangle(0, 0, projectile.width, 82), Lighting.GetColor((int)projectile.Center.X / 16, (int)(projectile.position.Y + tipHeight/2) / 16), 0, Vector2.Zero, 1f, 0, 0);
            
            for (; k < ((projectile.height-tipHeight)/ segmentHeight)-1; k++)
            {
                spriteBatch.Draw(texture, projectile.position + Vector2.UnitY *(k * segmentHeight + tipHeight) - Main.screenPosition, new Rectangle(0, tipHeight, projectile.width, segmentHeight), Lighting.GetColor((int)projectile.Center.X / 16, (int)(projectile.position.Y + (k * segmentHeight) + tipHeight + segmentHeight/2) / 16), 0, Vector2.Zero, 1f, 0, 0);
            }
            spriteBatch.Draw(texture, projectile.position + Vector2.UnitY * (k * segmentHeight + tipHeight) - Main.screenPosition, new Rectangle(0, tipHeight, projectile.width, (projectile.height-tipHeight) % segmentHeight), Lighting.GetColor((int)projectile.Center.X / 16, (int)(projectile.position.Y + (k * segmentHeight) + tipHeight + ((projectile.height - tipHeight) % segmentHeight) / 2) / 16), 0, Vector2.Zero, 1f, 0, 0);
            return false;
        }
    }
    public class Swordpocalypse : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordpocalypse");
        }
        public override void SetDefaults()
        {
            projectile.hostile = true;
            projectile.width = projectile.height = 34;
            projectile.penetrate = 2;
            projectile.tileCollide = false;
        }
        int bladeLength = 124;
        int bladeStart = 56;
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
        }
        public override void Kill(int timeLeft)
        {
            for(int d =0; d < 40; d++)
            {
                int lengthOffset =  (projectile.width/2 - bladeLength) + Main.rand.Next(bladeLength);
                int widthOffset =  + Main.rand.Next(projectile.width) - projectile.width/2;
                Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(lengthOffset, projectile.rotation) + QwertyMethods.PolarVector(widthOffset, projectile.width + (float)Math.PI / 2), 15);
                dust.noGravity = true;
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float CP = 0;
            Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(-bladeLength + projectile.width / 2 + bladeStart, projectile.rotation), 15);
            Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(projectile.width / 2, projectile.rotation), 15);
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center + QwertyMethods.PolarVector(-bladeLength + projectile.width / 2 + bladeStart, projectile.rotation), projectile.Center + QwertyMethods.PolarVector(projectile.width / 2, projectile.rotation), projectile.width, ref CP);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, projectile.Center-Main.screenPosition, null, lightColor, projectile.rotation, new Vector2(texture.Width - projectile.width / 2, texture.Height / 2), 1f, 0, 0);
            return false;
        }
       
    }
}
