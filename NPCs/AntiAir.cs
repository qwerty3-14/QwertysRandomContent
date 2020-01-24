using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace QwertysRandomContent.NPCs
{
    public class AntiAir : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anti Air");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            npc.width = 94;
            npc.height = 118;
            npc.damage = 20;
            npc.defense = 20;

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
            npc.lifeMax = 1800;
            banner = npc.type;
            bannerItem = mod.ItemType("AntiAirBanner");

        }


        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (QwertyWorld.DinoEvent)
            {
                return 10f;
            }
            else
            {
                return 0f;
            }

        }





        public override void HitEffect(int hitDirection, double damage)
        {

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
        const int moveFrameType = 0;
        const int attackFrameType = 1;

        public int AI_Timer = 0;
        public int Pos = 1;
        public int damage = 40;
        public int walkTime = 300;
        public int moveCount = 0;
        public int fireCount = 0;
        public int frameType = 0;
        public int ReloadTime = 20;
        public int attackTime = 0;
        public bool secondShot = true;
        public override void AI()
        {

            Player player = Main.player[npc.target];
            npc.TargetClosest(true);

            if (frameType == attackFrameType)

            {
                attackTime++;
                npc.velocity.X = (0);
                npc.velocity.Y = (0);

                if (attackTime > ReloadTime)
                {
                    if (secondShot)
                    {
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center.X - (17f * npc.direction), npc.Center.Y - 40f, 0f, -10f, mod.ProjectileType("AntiAirRocket"), damage, 3f, Main.myPlayer);
                        }
                        //Projectile.NewProjectile(npc.Center.X-(17f*npc.direction), npc.Center.Y-40f, 0f, 0f, 102, damage, 3f, Main.myPlayer);
                        secondShot = false;
                        attackTime = 0;
                    }
                    else
                    {
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center.X + (23f * npc.direction), npc.Center.Y - 40f, 0f, -10f, mod.ProjectileType("AntiAirRocket"), damage, 3f, Main.myPlayer);
                        }
                        //Projectile.NewProjectile(npc.Center.X+(23f*npc.direction), npc.Center.Y-40f, 0f, 0f, 102, damage, 3f, Main.myPlayer);
                        secondShot = true;
                        attackTime = 0;
                    }
                }


            }

            float playerPositionSummery = player.Center.Y - npc.Center.Y;
            Point origin = player.Center.ToTileCoordinates();
            Point point;
            if (playerPositionSummery < -200f && !WorldUtils.Find(origin, Searches.Chain(new Searches.Down(12), new GenCondition[]
                                        {
                                            new Conditions.IsSolid()
                                        }), out point))
            {
                frameType = attackFrameType;
            }
            else
            {
                frameType = moveFrameType;
            }







        }
        public override void NPCLoot()
        {
            QwertyWorld.DinoKillCount += 2;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            if (Main.rand.Next(0, 100) == 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DinoTooth"));
            }
            if (Main.rand.Next(0, 100) == 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("WornPrehistoricBow"));
            }
            if (Main.expertMode)
            {
                if (Main.rand.Next(0, 100) <= 15)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AntiAirWrench"));
                }
            }
            else
            {
                if (Main.rand.Next(0, 100) <= 10)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AntiAirWrench"));
                }
            }






        }






        public int moveFrame = 0;
        public int moveFrame2 = 1;

        public int attackFrameLeft = 2;
        public int attackFrameRight = 3;
        public int attackFrameAlternation = 4;




        public override void FindFrame(int frameHeight)
        {
            // This makes the sprite flip horizontally in conjunction with the npc.direction.
            npc.spriteDirection = -npc.direction;
            npc.frameCounter++;
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
                if (npc.frameCounter < 10)
                {
                    npc.frame.Y = (attackFrameRight * frameHeight);
                }
                else if (npc.frameCounter < 20)
                {
                    npc.frame.Y = (attackFrameAlternation * frameHeight);
                }
                else if (npc.frameCounter < 30)
                {
                    npc.frame.Y = (attackFrameLeft * frameHeight);
                }
                else if (npc.frameCounter < 40)
                {
                    npc.frame.Y = (attackFrameAlternation * frameHeight);
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }


        }

    }
    public class AntiAirRocket : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anti Air Rocket");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 10;
            projectile.height = 40;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 240;
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

            Player player = Main.player[0];

            if (projectile.Center.Y < player.Center.Y)
            {
                projectile.timeLeft = 0;
            }

        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[0];

            if (projectile.Center.X - player.Center.X < 0)
            {
                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 10f, 0f, mod.ProjectileType("AntiAirSide"), 40, 3f, Main.myPlayer);
                }

            }
            else
            {
                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -10f, 0f, mod.ProjectileType("AntiAirSide"), 40, 3f, Main.myPlayer);
                }

            }

            //Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, 102, 300, 3f, Main.myPlayer);




        }




    }
    public class AntiAirSide : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anti Air Rocket");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 240;
            projectile.tileCollide = true;


        }

        public override void Kill(int timeLeft)
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






    }

}
