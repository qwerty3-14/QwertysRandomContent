using Microsoft.Xna.Framework;
using QwertysRandomContent.NPCs.Fortress;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.FortressBoss
{
    [AutoloadBossHead]
    public class FortressBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Divine Light");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 156;
            npc.height = 128;
            npc.damage = 24;
            npc.defense = 12;
            npc.boss = true;
            npc.HitSound = SoundID.NPCHit1;

            npc.value = 100000f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            //aiType = 10;
            animationType = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/HigherBeing");
            npc.lifeMax = 4200;
            bossBag = mod.ItemType("FortressBossBag");
            npc.buffImmune[20] = true;
            npc.npcSlots = 200;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.LesserManaPotion;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(6000 * bossLifeScale);
            npc.damage = (int)(42);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        private bool startFight;

        public override void HitEffect(int hitDirection, double damage)
        {
            if (!startFight)
            {
                string key = "Mods.QwertysRandomContent.DivineRage2";
                Color messageColor = Color.Orange;
                if (Main.netMode == 2) // Server
                {
                    NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText(Language.GetTextValue(key), messageColor);
                }
            }
            startFight = true;
        }

        public bool playerDied;

        public override bool CheckActive()
        {
            if (playerDied)
            {
                return true;
            }
            return false;
        }

        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CaeliteBar"), Main.rand.Next(12, 21));
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CaeliteCore"), Main.rand.Next(6, 11));
                if (Main.rand.Next(7) == 0)
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DivineLightMask"));
                if (Main.rand.Next(20) < 3)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Lightling"));
                }
                if (Main.rand.Next(20) < 3)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SkywardHilt"));
                }
            }

            int trophyChance = Main.rand.Next(0, 10);
            if (trophyChance == 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FortressBossTrophy"));
            }
            if (!QwertyWorld.downedFortressBoss)
            {
                QwertyWorld.downedFortressBoss = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state
                }
            }
        }

        private float upperLimit = 60 * 16;
        private float lowerLimit;
        private int YDirection = -1;
        private int XDirection = 1;
        private float speedX;
        private float speedY = .4f;
        private int Center;
        private int maxDistanceFromCenter;
        private bool runOnce = true;
        private bool start = true;
        private int aggressionTimer;
        private int attackTimer;
        private int attackDelay = 120;
        private Player player;
        private bool pickAttack = true;
        private float directionOfPlayer;
        private int damage;
        private int impatient;
        private int quitCount;
        private int dpsCheckCounter;
        private int[] previousHealth = new int[5];
        private float dps = 0;

        public override void AI()
        {
            npc.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;
            Lighting.AddLight(npc.Center, new Vector3(1.2f, 1.2f, 1.2f));
            if (npc.life > npc.lifeMax)
            {
                npc.life = npc.lifeMax;
            }
            if (Main.expertMode)
            {
                damage = 7;
            }
            else
            {
                damage = 9;
            }

            if (start)
            {
                if (QwertyWorld.hasSummonedFortressBoss)
                {
                    startFight = true;
                }
                start = false;
                for (int i = 0; i < previousHealth.Length; i++)
                {
                    previousHealth[i] = npc.lifeMax;
                }
            }
            dpsCheckCounter++;
            if (dpsCheckCounter % 60 == 0)
            {
                for (int i = previousHealth.Length - 1; i > 0; i--)
                {
                    previousHealth[i] = previousHealth[i - 1];
                }
                previousHealth[0] = npc.life;

                dps = (((float)previousHealth[previousHealth.Length - 1] - (float)previousHealth[0]) / (float)previousHealth.Length);

                //Main.NewText("dps: " + dps);
            }
            if (Main.dungeonX < Main.maxTilesX * .5f)
            {
                Center = (int)((double)Main.maxTilesX * 0.8) * 16;
            }
            else
            {
                Center = (int)((double)Main.maxTilesX * 0.2) * 16;
            }
            player = Main.player[npc.target];
            npc.TargetClosest(true);
            if (!player.active || player.dead)
            {
                quitCount++;
                if (quitCount >= 30)
                {
                    npc.position.Y += 100000f;
                    playerDied = true;
                }
            }
            else
            {
                quitCount = 0;
            }
            directionOfPlayer = (player.Center - npc.Center).ToRotation();
            if (Main.maxTilesX > 8000)
            {
                lowerLimit = 280 * 16;
                maxDistanceFromCenter = 750 * 16;
            }
            else if (Main.maxTilesX > 6000)
            {
                lowerLimit = 230 * 16;
                maxDistanceFromCenter = 550 * 16;
            }
            else
            {
                lowerLimit = 130 * 16;
                maxDistanceFromCenter = 320 * 16;
            }
            if (npc.Center.Y < upperLimit)
            {
                YDirection = 1;
            }
            if (npc.Center.Y > lowerLimit)
            {
                YDirection = -1;
            }
            attackDelay = 120;
            float Xdistance = Math.Abs(player.Center.X - npc.Center.X);
            npc.dontTakeDamage = false;
            if (Xdistance > 1800)
            {
                speedX = 0;
                npc.dontTakeDamage = true;
                impatient++;
                attackDelay = 10;
            }
            else if (Xdistance > 900)
            {
                speedX = 2;
                attackDelay = 60;
                impatient = 0;
            }
            else if (Xdistance > 400)
            {
                speedX = 2;
                impatient = 0;
            }
            else
            {
                if (Xdistance < 200)
                {
                    attackDelay = 600;
                }
                speedX = (400f - Xdistance) / 80f + 2;
                impatient = 0;
            }
            if (startFight)
            {
                if (runOnce)
                {
                    runOnce = false;
                }
                attackTimer++;
                if (attackTimer > attackDelay)
                {
                    Main.PlaySound(SoundID.Item43, npc.Center);
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (pickAttack)
                    {
                        if (Main.netMode != 1)
                        {
                            npc.ai[0] = Main.rand.Next(6);
                            npc.netUpdate = true;
                        }
                        pickAttack = false;
                    }
                    if (npc.ai[0] == 4 && (NPC.AnyNPCs(mod.NPCType("HealingBarrier")) || (int)(dps / 12f) < 2))
                    {
                        npc.ai[0] = 0;
                        npc.netUpdate = true;
                    }
                    if (npc.ai[0] == 5)
                    {
                        int numberOfProjectiles = 5;
                        if (Main.expertMode)
                        {
                            numberOfProjectiles += 4;
                        }
                        float spread = 2 * (float)Math.PI;

                        for (int p = 0; p < numberOfProjectiles; p++)
                        {
                            float shiftedAim = (directionOfPlayer - (spread / 2)) + (spread * ((float)p / (float)numberOfProjectiles));
                            float speed = 1;
                            Projectile.NewProjectile(npc.Center + QwertyMethods.PolarVector(speed, shiftedAim), QwertyMethods.PolarVector(speed, shiftedAim), mod.ProjectileType("CaeliteSaw"), damage, 0, player.whoAmI, npc.whoAmI);
                        }
                        attackTimer = 0;
                    }
                    else if (npc.ai[0] == 4)
                    {
                        int numberOfProjectiles = (int)(dps / 12f);

                        for (int i = 0; i < numberOfProjectiles; i++)
                        {
                            NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("HealingBarrier"), 0, (float)i / (float)numberOfProjectiles * 2 * (float)Math.PI, npc.whoAmI * -npc.direction);
                        }
                        attackTimer = 0;
                    }
                    else if (npc.ai[0] == 3)
                    {
                        int numberOfProjectiles = 7;
                        float spread = (float)Math.PI / 2;

                        for (int p = 0; p < numberOfProjectiles; p++)
                        {
                            float shiftedAim = (directionOfPlayer - (spread / 2)) + Main.rand.NextFloat(spread);
                            float speed = Main.rand.Next(7, 10);
                            Projectile.NewProjectile(npc.Center, QwertyMethods.PolarVector(speed, shiftedAim), mod.ProjectileType("Deflect"), (int)(damage * 1.2f), 0, player.whoAmI, npc.whoAmI);
                        }
                        attackTimer = 0;
                    }
                    else
                    {
                        int numberOfProjectiles = 3;
                        float spread = (float)Math.PI / 8;

                        for (int p = 0; p < numberOfProjectiles; p++)
                        {
                            float shiftedAim = (directionOfPlayer - (spread / 2)) + (spread * ((float)p / (float)numberOfProjectiles));
                            float speed = 5;
                            Projectile.NewProjectile(npc.Center, QwertyMethods.PolarVector(Xdistance > 1800 ? speed * 3 : speed, shiftedAim), mod.ProjectileType("BarrierSpread"), Xdistance > 1800 ? damage * 3 : damage, 0, player.whoAmI, npc.whoAmI, Xdistance > 1800 ? 1 : 0);
                        }
                        attackTimer = 0;
                    }
                }
                else
                {
                    pickAttack = true;
                }
                if (npc.Center.X < player.Center.X && npc.Center.X < Center - maxDistanceFromCenter && Main.netMode != 1)
                {
                    npc.Center = new Vector2(player.Center.X + Xdistance, npc.Center.Y);
                    npc.netUpdate = true;
                }
                if (npc.Center.X > player.Center.X && npc.Center.X > Center + maxDistanceFromCenter && Main.netMode != 1)
                {
                    npc.Center = new Vector2(player.Center.X - Xdistance, npc.Center.Y);
                    npc.netUpdate = true;
                }
                npc.spriteDirection = npc.direction;
                XDirection = -npc.direction;
                npc.velocity = new Vector2(speedX * XDirection, speedY * YDirection);
                //Main.NewText(Xdistance);
                //Main.NewText(player.GetModPlayer<FortressBiome>().TheFortress);
            }
            else
            {
                //Main.NewText(player.GetModPlayer<FortressBiome>().TheFortress);
                QwertyWorld.hasSummonedFortressBoss = true;
                if (Main.netMode != 2)
                {
                    aggressionTimer++;
                    if (aggressionTimer > 20 * 60)
                    {
                        string key = "Mods.QwertysRandomContent.DivineStart";
                        Color messageColor = Color.Orange;
                        if (Main.netMode == 2) // Server
                        {
                            NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText(Language.GetTextValue(key), messageColor);
                        }
                        startFight = true;
                        npc.netUpdate = true;
                    }
                    else if (!player.GetModPlayer<FortressBiome>().TheFortress && aggressionTimer > 10)
                    {
                        string key = "Mods.QwertysRandomContent.DivineLeave";
                        Color messageColor = Color.Orange;
                        if (Main.netMode == 2) // Server
                        {
                            NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText(Language.GetTextValue(key), messageColor);
                        }
                        npc.active = false;
                    }
                }
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(startFight);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            startFight = reader.ReadBoolean();
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;

            if (npc.frameCounter < 10)
            {
                npc.frame.Y = 0 * frameHeight;
            }
            else if (npc.frameCounter < 20)
            {
                npc.frame.Y = 1 * frameHeight;
            }
            else if (npc.frameCounter < 30)
            {
                npc.frame.Y = 2 * frameHeight;
            }
            else if (npc.frameCounter < 40)
            {
                npc.frame.Y = 3 * frameHeight;
            }
            else
            {
                npc.frameCounter = 0;
            }
        }
    }

    public class BarrierSpread : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Barrier Spread");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            //aiType = ProjectileID.Bullet;
            projectile.width = 40;
            projectile.height = 40;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 1200;
            projectile.tileCollide = false;
        }

        public int dustTimer;
        private Projectile clearCheck;

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            Dust dust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"))];
            dust2.scale = .5f;
            if (projectile.frameCounter % 10 == 0)
            {
                projectile.frame++;
                if (projectile.frame > 1)
                {
                    projectile.frame = 0;
                }
            }
            for (int p = 0; p < 1000; p++)
            {
                clearCheck = Main.projectile[p];
                if (clearCheck.friendly && !clearCheck.sentry && clearCheck.minionSlots <= 0 && Collision.CheckAABBvAABBCollision(projectile.position, projectile.Size, clearCheck.position, clearCheck.Size))
                {
                    clearCheck.Kill();
                }
            }
            if (!Main.npc[(int)projectile.ai[0]].active || Main.npc[(int)projectile.ai[0]].type != mod.NPCType("FortressBoss"))
            {
                projectile.Kill();
            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            if (Main.expertMode)
            {
                int estimatedDamage = (4 * damage) - (int)((float)target.statDefense / 4f * 3f);

                if (estimatedDamage < 18f)
                {
                    damage = (int)((((float)target.statDefense / 4f * 3f) + 18f) / 4f);
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (target.HasBuff(BuffID.PotionSickness))
            {
                target.buffTime[target.FindBuffIndex(BuffID.PotionSickness)] += 600;
            }
            else
            {
                target.AddBuff(BuffID.PotionSickness, 600);
            }
        }
    }

    public class Deflect : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deflect");

            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            //aiType = ProjectileID.Bullet;
            projectile.width = 64;
            projectile.height = 64;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.light = .6f;
            projectile.tileCollide = false;
            projectile.timeLeft = 1200;
        }

        public int dustTimer;
        private Projectile clearCheck;
        private int counter = 60;

        public override void AI()
        {
            counter--;
            projectile.rotation += (float)Math.PI / 30;
            Dust dust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"))];
            dust2.scale = .5f;
            projectile.frameCounter++;
            if (projectile.frameCounter % 10 == 0)
            {
                projectile.frame++;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
            projectile.velocity *= .96f;
            if (counter > 0)
            {
                projectile.alpha = 200;
            }
            else
            {
                projectile.alpha = 0;
                for (int p = 0; p < 1000; p++)
                {
                    clearCheck = Main.projectile[p];
                    if (clearCheck.friendly && !clearCheck.sentry && clearCheck.velocity != Vector2.Zero && clearCheck.damage > 0 && clearCheck.minionSlots <= 0 && Collision.CheckAABBvAABBCollision(projectile.position, projectile.Size, clearCheck.position, clearCheck.Size))
                    {
                        clearCheck.Kill();
                        Projectile d = Main.projectile[Projectile.NewProjectile(clearCheck.Center, -clearCheck.velocity.SafeNormalize(-Vector2.UnitY) * 10f, mod.ProjectileType("Deflected"), clearCheck.damage / 2, clearCheck.knockBack, clearCheck.owner)];
                        if (d.damage > 50)
                        {
                            d.damage = 50;
                        }
                    }
                }
            }
            if (!Main.npc[(int)projectile.ai[0]].active || Main.npc[(int)projectile.ai[0]].type != mod.NPCType("FortressBoss"))
            {
                projectile.Kill();
            }
        }
    }

    public class Deflected : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deflected");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            //aiType = ProjectileID.Bullet;
            projectile.width = 22;
            projectile.height = 22;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.light = .6f;
            projectile.tileCollide = false;
            projectile.timeLeft = 600;
        }

        public override void AI()
        {
            if (Main.rand.Next(2) == 0)
            {
                Dust dust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"))];
                dust2.scale = .5f;
            }
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.frameCounter++;
            if (projectile.frameCounter % 10 == 0)
            {
                projectile.frame++;
                if (projectile.frame > 1)
                {
                    projectile.frame = 0;
                }
            }
        }
    }

    public class HealingBarrier : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Healing Barrier");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.width = 50;
            npc.height = 50;
            npc.damage = 10;
            npc.defense = 0;

            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = 0f;
            //npc.aiStyle = 10;
            //aiType = 10;
            npc.aiStyle = -1;
            animationType = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = -1;
            npc.lifeMax = Main.expertMode ? 22 : 44;
            //npc.timeLeft = 5 * 60;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.lifeMax = 44;
            if (npc.life > npc.lifeMax)
            {
                npc.life = npc.lifeMax;
            }
            npc.frameCounter++;

            if (npc.frameCounter < 10)
            {
                npc.frame.Y = 0 * frameHeight;
            }
            else if (npc.frameCounter < 20)
            {
                npc.frame.Y = 1 * frameHeight;
            }
            else
            {
                npc.frameCounter = 0;
            }
        }

        private int spinDirection = 1;
        private bool runOnce = true;
        private NPC parent;
        private float radius = 160;
        private int timer;

        public override void AI()
        {
            Lighting.AddLight(npc.Center, new Vector3(1f, 1f, 1f));
            timer++;
            if (runOnce)
            {
                if (npc.ai[1] < 0)
                {
                    spinDirection = -1;
                    npc.ai[1] = Math.Abs(npc.ai[1]);
                }
                runOnce = false;
            }
            npc.ai[1] = Math.Abs(npc.ai[1]);
            parent = Main.npc[(int)npc.ai[1]];
            npc.position.X = parent.Center.X - (int)(Math.Cos(npc.ai[0]) * radius) - npc.width / 2;
            npc.position.Y = parent.Center.Y - (int)(Math.Sin(npc.ai[0]) * radius) - npc.height / 2;
            npc.ai[0] += (float)Math.PI / 200 * spinDirection;

            if (!parent.active || parent.type != mod.NPCType("FortressBoss"))
            {
                npc.active = false;
            }
            else if ((timer > 720 && Main.expertMode) || timer > 900)
            {
                parent.life += npc.life * 2;
                parent.HealEffect(npc.life * 2, true);
                float distance = (parent.Center - npc.Center).Length();
                for (int d = 0; d < distance; d += 2)
                {
                    Dust dust = Dust.NewDustPerfect(npc.Center + QwertyMethods.PolarVector(d, (parent.Center - npc.Center).ToRotation()), mod.DustType("CaeliteDust"));
                    dust.frame.Y = 0;
                }
                npc.active = false;
            }
        }
    }

    public class CaeliteSaw : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Saw");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            //aiType = ProjectileID.Bullet;
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = 3;
            projectile.light = .6f;
            projectile.tileCollide = true;
            projectile.timeLeft = 600;
        }

        private int timer;

        public override void AI()
        {
            timer++;
            if (timer == 60)
            {
                projectile.velocity = projectile.velocity.SafeNormalize(-Vector2.UnitY) * 11;
            }
            projectile.rotation += (float)Math.PI / 7.5f;
            if (!Main.npc[(int)projectile.ai[0]].active || Main.npc[(int)projectile.ai[0]].type != mod.NPCType("FortressBoss"))
            {
                projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"));
            }
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            projectile.penetrate--;
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

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            if (Main.expertMode)
            {
                int estimatedDamage = (4 * damage) - (int)((float)target.statDefense / 4f * 3f);

                if (estimatedDamage < 18f)
                {
                    damage = (int)((((float)target.statDefense / 4f * 3f) + 18f) / 4f);
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("PowerDown"), 600);
        }
    }
}