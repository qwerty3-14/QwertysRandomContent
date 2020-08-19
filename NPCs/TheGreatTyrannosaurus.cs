using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs
{
    [AutoloadBossHead]
    public class TheGreatTyrannosaurus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Great Tyrannosaurus");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 166;
            npc.height = 154;
            npc.damage = 100;
            npc.defense = 22;
            npc.lifeMax = 28000;

            npc.boss = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 60f;
            npc.knockBackResist = 0f;
            npc.aiStyle = 3;

            aiType = 27;
            animationType = -1;
            npc.noGravity = false;
            npc.noTileCollide = false;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/OldDinosNewGuns");

            bossBag = mod.ItemType("TRexBag");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * .6f);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (QwertyWorld.DinoEvent && !NPC.AnyNPCs(mod.NPCType("Velocichopper")) && !NPC.AnyNPCs(mod.NPCType("TheGreatTyrannosaurus")))
            {
                if (QwertyWorld.DinoKillCount >= 140)
                {
                    return 50f;
                }
                return 3f;
            }
            return 0f;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override bool CheckActive()
        {
            Player player = Main.player[npc.target];
            float playerDistance = (float)Math.Sqrt((npc.Center.X - player.Center.X) * (npc.Center.X - player.Center.X) + (npc.Center.Y - player.Center.Y) * (npc.Center.Y - player.Center.Y));
            if (playerDistance > 2000f)
            {
                return true;
            }
            return false;
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

        private const int moveFrameType = 0;
        private const int attackFrameType = 1;
        private const int launchFrameType = 4;
        public int timer = 0;
        public int Pos = 1;
        public int damage = 35;
        public int walkTime = 300;
        public int moveCount = 0;
        public int fireCount = 0;
        public int frameType = 0;
        public int attack = 0;
        public bool meteorsLaunched = false;
        public int multiplayerAttackCycle = 1;
        private int[] attackreloadTimes = new int[] { 10, 4, 30 };
        private Vector2 gunOffset = new Vector2(78, 76);
        private float gunRot = 0f;
        private int meteorTime;
        private int gunFrame = 0;

        public override void AI()
        {
            if (Main.expertMode)
            {
                damage = 25;
            }

            Player player = Main.player[npc.target];
            npc.TargetClosest(true);
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0f, -100f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }

            timer++;
            npc.frameCounter++;
            gunOffset = npc.spriteDirection == 1 ? new Vector2(78, 76) : new Vector2(166 - 78, 76);
            gunFrame = 0;
            if (timer > walkTime)
            {
                npc.aiStyle = -1;
                npc.directionY = -Math.Sign(player.Center.X - npc.Center.X);
                meteorsLaunched = attack == 2;
                npc.velocity.X = 0;
                npc.velocity.Y += 4.3f;

                if (attack == 2)
                {
                    gunRot = npc.spriteDirection == 1 ? 0f : (float)Math.PI;
                }
                else
                {
                    gunRot = (player.Center - (npc.position + gunOffset)).ToRotation();
                    if (npc.frameCounter % 4 > 1)
                    {
                        gunFrame = 1;
                    }
                }
                if ((timer - walkTime) % attackreloadTimes[attack] == 0)
                {
                    Main.PlaySound(16, npc.position + gunOffset, 0);
                    if (Main.netMode != 1)
                    {
                        float spread = MathHelper.ToRadians(Main.rand.Next(-15, 15));

                        switch (attack)
                        {
                            case 0:
                                Projectile.NewProjectile(npc.position + gunOffset + QwertyMethods.PolarVector(56, gunRot), QwertyMethods.PolarVector(10f, gunRot + spread), mod.ProjectileType("SnowFlake"), damage, 3f, Main.myPlayer);
                                break;

                            case 1:
                                NPC.NewNPC((int)(npc.position + gunOffset + QwertyMethods.PolarVector(56, gunRot)).X, (int)(npc.position + gunOffset + QwertyMethods.PolarVector(56, gunRot)).Y, mod.NPCType("Mosquitto"), 0, spread, npc.direction);
                                break;

                            case 2:
                                Projectile.NewProjectile(npc.Center + new Vector2(-24 * npc.direction, -74f), Vector2.UnitY * -40f, mod.ProjectileType("MeteorLaunch"), damage, 3f, Main.myPlayer);

                                break;
                        }
                    }
                }
                if (timer >= walkTime * 2)
                {
                    timer = 0;
                    attack = Main.rand.Next(3);
                }
            }
            else
            {
                if (npc.frameCounter >= 10)
                {
                    gunOffset.Y += 2;
                }
                npc.aiStyle = 3;
                gunRot = npc.spriteDirection == 1 ? 0f : (float)Math.PI;
                if (meteorsLaunched)
                {
                    meteorTime++;
                    if (meteorTime > 10)
                    {
                        if (Main.netMode != 1)
                        {
                            int Xvar = Main.rand.Next(-750, 750);
                            Projectile.NewProjectile(player.Center.X + Xvar * 1.0f, player.Center.Y - 800f, 0f, 10f, mod.ProjectileType("MeteorFall"), damage, 3f, Main.myPlayer);
                        }
                        meteorTime = 0;
                    }
                }
            }
        }

        public override void NPCLoot()
        {
            QwertyWorld.downedTyrant = true;
            QwertyWorld.DinoKillCount += 10;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            if (Main.netMode != 1)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DinoBone"));
                        break;

                    case 1:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DinoFlail"));
                        break;

                    case 2:
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TheTyrantsExtinctionGun"));
                        break;
                }

                if (Main.rand.Next(4) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("WornPrehistoricBow"));
                }
            }
        }

        public int moveFrame = 0;
        public int moveFrame2 = 1;

        public int launchFrame = 3;
        public int launchFrame2 = 4;

        public override void FindFrame(int frameHeight)
        {
            // This makes the sprite flip horizontally in conjunction with the npc.direction.
            npc.spriteDirection = npc.direction;

            if (frameType == moveFrameType)
            {
                if (npc.frameCounter < 10)
                {
                    npc.frame.Y = (moveFrame * frameHeight);
                }
                else if (npc.frameCounter < 20)
                {
                    npc.frame.Y = (moveFrame2 * frameHeight);
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }
            if (frameType == attackFrameType)
            {
                npc.frame.Y = (moveFrame * frameHeight);
            }
            if (frameType == launchFrameType)
            {
                if (npc.frameCounter < 10)
                {
                    npc.frame.Y = (moveFrame * frameHeight);
                }
                else if (npc.frameCounter < 20)
                {
                    npc.frame.Y = (launchFrame * frameHeight);
                }
                else if (npc.frameCounter < 30)
                {
                    npc.frame.Y = (launchFrame2 * frameHeight);
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            int frameHeight = 28;
            bool flip = npc.spriteDirection == 1;
            Texture2D gun = mod.GetTexture("NPCs/TheTyrantsExtinctionGun");
            spriteBatch.Draw(gun, npc.position + gunOffset - Main.screenPosition,
                       new Rectangle(0, frameHeight * gunFrame, 80, frameHeight), drawColor, gunRot + (float)Math.PI,
                       new Vector2(70, 14), 1f, flip ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
        }
    }

    public class SnowFlake : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SnowFlake");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 0;

            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            projectile.rotation += 1.5f;
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.velocity.X = -velocityChange.X;
            }
            if (projectile.velocity.Y != velocityChange.Y)
            {
                projectile.velocity.Y = -velocityChange.Y;
            }
            return false;
        }
    }

    public class MeteorLaunch : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 0;

            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
            projectile.tileCollide = true;
        }

        public bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                Main.PlaySound(SoundID.Item62, projectile.position);
                for (int i = 0; i < 50; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1.4f;
                }
                // Fire Dust spawn
                for (int i = 0; i < 80; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 5f;
                    dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 3f;
                }
                runOnce = false;
            }
            projectile.rotation += 1.5f;
        }
    }

    public class MeteorFall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 0;

            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 1200;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            projectile.rotation += 1.5f;
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (Main.netMode != 1)
            {
                Main.PlaySound(SoundID.Item62, projectile.position);
                for (int i = 0; i < 50; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1.4f;
                }
                // Fire Dust spawn
                for (int i = 0; i < 80; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 5f;
                    dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 3f;
                }
            }
            return true;
        }
    }
}