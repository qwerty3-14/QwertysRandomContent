using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;

namespace QwertysRandomContent.NPCs.BossFour
{
    [AutoloadBossHead]
    public class WeakPoint : ModNPC
    {
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Old Lord");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.width = 320;
            npc.height = 60;
            npc.damage = 70;
            npc.defense = 50;
            npc.boss = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 1;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            
            animationType = -1;
            npc.noGravity = true;
            npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            music = MusicID.Boss5;
            npc.netAlways = true;


            npc.lifeMax = 100000;
            bossBag = mod.ItemType("B4Bag");
            

        }
        public override void HitEffect(int hitDirection, double damage)
        {
            
            
        }
        public override void NPCLoot()
        {
            QwertyWorld.downedB4 = true;
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
                    npc.DropBossBags();
                }
                else
                {

                    
                    int selectWeapon = Main.rand.Next(1, 7);
                    
                    if (selectWeapon == 1)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("B4Bow"));
                    }
                    if (selectWeapon == 2)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("B4GiantBow"));
                    }
                    if (selectWeapon == 3)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DreadnoughtStaff"));
                    }
                    if (selectWeapon == 4)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BlackHoleStaff"));
                    }
                    if (selectWeapon == 5)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Jabber"));
                    }
                    if (selectWeapon == 6)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ExplosivePierce"));
                    }
                    if (Main.rand.Next(100) < 15)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TheDevourer"));
                    }



                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 73, 60);

                    

                }
                if (trophyChance == 1)
                {
                    //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraTrophy"));
                }

            }
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * 1f);
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }
        public override bool CheckActive()
        {

            return false;
        }
       
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            return 0f;

        }
        /*
        public override void BossHeadSlot(ref int index)
        {

            index = NPCHeadLoader.GetBossHeadSlot(QwertysRandomContent.AncientMachineHead);

        }
        */
        public int regenTimer;
        public int timer;
        public Projectile laser;
        public Projectile laser2;
        public NPC turret;
        public NPC turret2;
        public NPC turret3;
        public NPC turret4;
        public Vector2 turretPosition;
        public float moveDirection;
        //public Vector2 moveTo;
        public float moveSpeed;
        public bool runOnce = true;
        public int attack = 1;
        public int attackTimer;
        public int frame;
        public int shotDamage;
        public float shotSpeed = 3;
        public bool fireLaser = true;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (Main.netMode != 2)
            {

                return true;
            }
            return false;
        }
        public override void AI()
        {
            if (Main.netMode != 0)
            {
                Vector2 target = new Vector2(npc.ai[0], npc.ai[1]);
                Vector2 moveTo = new Vector2(target.X, target.Y) - npc.position;


                npc.velocity = (moveTo) * 1f;
            }
            if (Main.expertMode)
            {
                shotDamage = (int)(npc.damage / 8 * 1.6f);
            }
            else
            {
                shotDamage = npc.damage / 4;
            }
            Player player = Main.player[npc.target];
            npc.TargetClosest(true);
            
            if(npc.defense==2)
            {
                fireLaser = true;
                npc.dontTakeDamage = false;
                frame = 1;
                timer++;
                
                if(timer >90)
                {

                    timer = 30;
                    if (Main.netMode != 1)
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + 1 * (float)Math.PI / 12) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +1 * (float)Math.PI / 12) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + 3 * (float)Math.PI / 12) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +3 * (float)Math.PI / 12) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + 5 * (float)Math.PI / 12) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +5 * (float)Math.PI / 12) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        //Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + 7 * (float)Math.PI / 16) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +7 * (float)Math.PI / 16) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + -1 * (float)Math.PI / 12) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +-1 * (float)Math.PI / 12) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + -3 * (float)Math.PI / 12) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +-3 * (float)Math.PI / 12) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + -5 * (float)Math.PI / 12) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +-5 * (float)Math.PI / 12) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        //Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + -7 * (float)Math.PI / 16) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +-7 * (float)Math.PI / 16) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                    }
                }
                else if(timer==60)
                {
                    if (Main.netMode != 1)
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + 0 * (float)Math.PI / 6) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +0 * (float)Math.PI / 6) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + 1 * (float)Math.PI / 6) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +1 * (float)Math.PI / 6) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + 2 * (float)Math.PI / 6) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +2 * (float)Math.PI / 6) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + 3 * (float)Math.PI / 6) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +3 * (float)Math.PI / 6) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        //Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + 4 * (float)Math.PI / 6) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +4 * (float)Math.PI / 6) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + -1 * (float)Math.PI / 6) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +-1 * (float)Math.PI / 6) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + -2 * (float)Math.PI / 6) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +-2 * (float)Math.PI / 6) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + -3 * (float)Math.PI / 6) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +-3 * (float)Math.PI / 6) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        //Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(-(float)Math.PI / 2 + -4 * (float)Math.PI / 6) * shotSpeed, -(float)Math.Sin(-(float)Math.PI / 2 + +-4 * (float)Math.PI / 6) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                    }
                }
                
            }
            else if(npc.defense==3)
            {
                if (fireLaser)
                {
                    laser = Main.projectile[Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 14f, mod.ProjectileType("SuperLaser"), 5 * shotDamage, 3f, Main.myPlayer, npc.whoAmI, npc.rotation + (float)Math.PI / 2)];
                    fireLaser = false;
                }
                npc.dontTakeDamage = false;
                frame = 1;
                timer++;
                if(timer>600)
                {
                    laser.Kill();
                    timer = 0;
                }
            }
            else if (npc.defense == 4)
            {
                timer++;
                if (timer == 120 && Main.netMode !=1)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 14f, mod.ProjectileType("BlackHoleSeed"), 3 * shotDamage, 3f, Main.myPlayer);
                }
                npc.dontTakeDamage = false;
                frame = 1;
            }
            else if (npc.defense == 5 && Main.netMode != 1)
            {
                timer++;
                if (timer == 120)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 7f, mod.ProjectileType("MegaBurst"),  shotDamage, 3f, Main.myPlayer);
                }
                npc.dontTakeDamage = false;
                frame = 1;
            }
            else
            {
                fireLaser = true;
                timer = 0;
                npc.dontTakeDamage = true;
                frame = 0;
            }
            
            if(Math.Abs(player.Center.X- npc.Center.X) > 800 || player.Center.Y-npc.Center.Y > 1000)
            {
                npc.dontTakeDamage = true;
            }
            npc.defense = 50;
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y=frameHeight* frame;
        }
        


    }
    public class BlackHoleSeed : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pew Pew");


        }
        public override void SetDefaults()
        {

            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 30;
            projectile.tileCollide = false;


        }
        public override void AI()
        {
            
        }
        public override void Kill(int timeLeft)
        {
            if (Main.netMode != 1)
            {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("BlackHole"), projectile.damage, 3f, Main.myPlayer, projectile.ai[0]);
            }
        }


    }
    public class BlackHole : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BlackHole");
            Main.projFrames[projectile.type] = 1;

        }
        public override void SetDefaults()
        {

            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 480;
            projectile.tileCollide = false;


        }
        public float horiSpeed;
        public float vertSpeed;
        public float direction;
        public float pullSpeed = .5f;
        public float dustSpeed = 20f;
        public NPC mass;
        public Projectile proj;
        public int frameTimer;
        public Dust dust;
        public Item item;
        int dustCounter;
        public override void AI()
        {
            projectile.velocity = new Vector2(0, 0);
            projectile.timeLeft -= (int)projectile.ai[0] - 1;
            //Player player = Main.player[projectile.owner];

            for (int p = 0; p < 255; p++)
            {
                direction = (projectile.Center - Main.player[p].Center).ToRotation();
                horiSpeed = (float)Math.Cos(direction) * pullSpeed / 2;
                vertSpeed = (float)Math.Sin(direction) * pullSpeed / 2;
                Main.player[p].velocity += new Vector2(horiSpeed, vertSpeed);

                for (int i = 0; i < 1; i++)
                {
                    int dust = Dust.NewDust(Main.player[p].position, Main.player[p].width, Main.player[p].height, mod.DustType("B4PDust"), 0, 0);

                }
            }
            /*
            for (int g = 0; g < 3; g++)
            {
                Dust blackEs = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("B4PDust"), 0, 0)];
                direction = (projectile.Center - blackEs.position).ToRotation();
                horiSpeed = (float)Math.Cos(direction) * pullSpeed * 50;
                vertSpeed = (float)Math.Sin(direction) * pullSpeed * 50;
                blackEs.velocity += new Vector2(horiSpeed, vertSpeed);


            }
            */

            for (int d = 0; d < 80; d++)
            {

                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(Main.rand.NextFloat(10, 200), theta), mod.DustType("BlackHoleMatter"), QwertyMethods.PolarVector(6, theta + (float)Math.PI / 2));
                dust.scale = 1f;

            }
            
            for (int i = 0; i < Main.dust.Length; i++)
            {
                dust = Main.dust[i];
                if (!dust.noGravity)
                {

                    direction = (projectile.Center - dust.position).ToRotation();
                    horiSpeed = (float)Math.Cos(direction) * pullSpeed *5;
                    vertSpeed = (float)Math.Sin(direction) * pullSpeed *5;
                    dust.velocity += new Vector2(horiSpeed, vertSpeed);
                }
                if(dust.type == mod.DustType("BlackHoleMatter"))
                {
                    direction = (projectile.Center - dust.position).ToRotation();
                    dust.velocity += QwertyMethods.PolarVector(.8f, direction);
                    if ((dust.position - projectile.Center).Length() < 10)
                    {
                        dust.scale = 0f;
                    }
                    else
                    {
                        dust.scale = .35f;
                    }
                    
                    
                }
            }
            for (int i=0; i < 200; i++)
            {
                mass = Main.npc[i];
                if(!mass.boss && mass.active && mass.knockBackResist != 0f)
                {

                    direction = (projectile.Center - mass.Center).ToRotation();
                    horiSpeed = (float)Math.Cos(direction) * pullSpeed;
                    vertSpeed = (float)Math.Sin(direction) * pullSpeed;
                    mass.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(mass.position, mass.width, mass.height, mod.DustType("B4PDust"), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }
            for (int i = 0; i < Main.item.Length; i++)
            {
                item = Main.item[i];
                if (item.position != new Vector2(0,0))
                {

                    direction = (projectile.Center - item.Center).ToRotation();
                    horiSpeed = (float)Math.Cos(direction) * pullSpeed;
                    vertSpeed = (float)Math.Sin(direction) * pullSpeed;
                    item.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(item.position, item.width, item.height, mod.DustType("B4PDust"), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                proj = Main.projectile[i];
                if (proj.active && proj.type != mod.ProjectileType("BlackHole") && proj.type != mod.ProjectileType("SideLaser"))
                {

                    direction = (projectile.Center - proj.Center).ToRotation();
                    horiSpeed = (float)Math.Cos(direction) * pullSpeed;
                    vertSpeed = (float)Math.Sin(direction) * pullSpeed;
                    proj.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(proj.position, proj.width, proj.height, mod.DustType("B4PDust"), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }


            



        }



    }
    public class BurstShot2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pew Pew");


        }
        public override void SetDefaults()
        {

            projectile.width = 102;
            projectile.height = 104;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
            projectile.tileCollide = false;


        }
        public float distance;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.rotation += (float)Math.PI / 30;
            for (int i = 0; i < 1; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("B4PDust"));
            }
            
        }

        public float shotSpeed = 3;
        public override void Kill(int timeLeft)
        {
            for (int r = 0; r < 8; r++)
            {
                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(r * (2 * Math.PI / 8)) * shotSpeed, (float)Math.Sin(r * (2 * Math.PI / 8)) * shotSpeed, mod.ProjectileType("TurretShot"), projectile.damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(r * (2 * Math.PI / 8) + Math.PI / 8) * shotSpeed * 1.5f, (float)Math.Sin(r * (2 * Math.PI / 8) + Math.PI / 8) * shotSpeed * 1.5f, mod.ProjectileType("TurretShot"), projectile.damage, 0, Main.myPlayer);
                }
            }
        }


    }
    public class MegaBurst : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pew Pew");


        }
        public override void SetDefaults()
        {

            projectile.width = 300;
            projectile.height = 300;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 60;
            projectile.tileCollide = false;


        }
        public float distance;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.rotation += (float)Math.PI / 30;
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("B4PDust"));
            }
            projectile.timeLeft -= (int)projectile.ai[0] - 1;

        }

        public float shotSpeed = 3;
        public override void Kill(int timeLeft)
        {
            for (int r = 0; r < 6; r++)
            {
                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(r * (2 * Math.PI / 6)) * shotSpeed * 1.5f, (float)Math.Sin(r * (2 * Math.PI / 6)) * shotSpeed * 1.5f, mod.ProjectileType("BurstShot2"), projectile.damage, 0, Main.myPlayer);
                }
            }
        }


    }
    // The following laser shows a channeled ability, after charging up the laser will be fired
    // Using custom drawing, dust effects, and custom collision checks for tiles
    public class SuperLaser : ModProjectile
    {
        // The maximum charge value
        private const float MaxChargeValue = 300f;
        //The distance charge particle from the player center
        private const float MoveDistance = 80f;

        // The actual distance is stored in the ai0 field
        // By making a property to handle this it makes our life easier, and the accessibility more readable
        public float Distance;

        // The actual charge value is stored in the localAI0 field
        public float Charge
        {
            get { return projectile.localAI[0]; }
            set { projectile.localAI[0] = value; }
        }
        public NPC shooter;
        // Are we at max charge? With c#6 you can simply use => which indicates this is a get only property
        public bool AtMaxCharge { get { return Charge == MaxChargeValue; } }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.hostile = true;
            projectile.hide = false;
        }
        // The AI of the projectile
        public float downFromCenter = 110;
        public override void AI()
        {

            shooter = Main.npc[(int)projectile.ai[0]];
            Vector2 mousePos = Main.MouseWorld;
            Player player = Main.player[projectile.owner];
            if (!shooter.active)
            {
                projectile.Kill();
            }
            #region Set projectile position


            Vector2 diff = new Vector2(0, 14);
            diff.Normalize();
            projectile.velocity = diff;
            projectile.direction = projectile.Center.X > shooter.Center.X ? 1 : -1;
            projectile.netUpdate = true;

            projectile.position = new Vector2(shooter.Center.X, shooter.Center.Y) + projectile.velocity * MoveDistance;
            projectile.timeLeft = 2;
            int dir = projectile.direction;
            /*
            player.ChangeDir(dir);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
            */
            #endregion

            #region Charging process
            // Kill the projectile if the player stops channeling


            // Do we still have enough mana? If not, we kill the projectile because we cannot use it anymore

            Vector2 offset = projectile.velocity;
            offset *= MoveDistance - 20;
            Vector2 pos = new Vector2(shooter.Center.X, shooter.Center.Y) + offset - new Vector2(10, 10);

            if (Charge < MaxChargeValue)
            {
                Charge++;
            }

            int chargeFact = (int)(Charge / 20f);
            


            #endregion

            
            if (Charge < MaxChargeValue) return;
            Vector2 start = new Vector2(shooter.Center.X, shooter.Center.Y);
            Vector2 unit = projectile.velocity;
            unit *= -1;
            for (Distance = MoveDistance; Distance <= 2200f; Distance += 5f)
            {
                start = new Vector2(shooter.Center.X, shooter.Center.Y) + projectile.velocity * Distance;
                /*
                if (!Collision.CanHit(new Vector2(shooter.Center.X, shooter.Center.Y), 1, 1, start, 1, 1))
                {
                    Distance -= 5f;
                    break;
                }
                */
            }

            

            //Add lights
            DelegateMethods.v3_1 = new Vector3(10f, 10f, 10f);
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (Distance - MoveDistance), 26,
                DelegateMethods.CastLight);

        }
        public int colorCounter;
        public Color lineColor;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            if (AtMaxCharge)
            {
                DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], new Vector2(shooter.Center.X, shooter.Center.Y),
                    projectile.velocity, 35, projectile.damage, -1.57f, 1f, 4000f, Color.White, (int)MoveDistance);
            }
            else
            {
                Vector2 center = projectile.Center;

                float projRotation = 0;//(float)Math.PI;
                //update draw position

                float lineLength = 4000f;
                Color drawColor = lightColor;

                colorCounter++;

                if (colorCounter >= 20)
                {
                    colorCounter = 0;
                }
                else if (colorCounter >= 10)
                {
                    lineColor = Color.White;
                }
                else
                {
                    lineColor = Color.Red;
                }
                spriteBatch.Draw(mod.GetTexture("Items/Weapons/Rhuthinium/laser"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, 1, (int)lineLength - 10), lineColor, projRotation,
                    new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            }

            return false;
        }

        // The core function of drawing a laser
        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 4000f, Color color = default(Color), int transDist = 50)
        {
            Vector2 origin = start;
            float r = unit.ToRotation() + rotation;

            #region Draw laser body
            for (float i = transDist; i <= Distance; i += step)
            {
                
                Color c = Color.White;
                origin = start + i * unit;
                origin.Y += 118;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 130, 458, 118), i < transDist ? Color.Transparent : c, r,
                    new Vector2(458 * .5f, 118 * .5f), scale, 0, 0);
            }
            #endregion

            #region Draw laser tail
            spriteBatch.Draw(texture, start + unit * (transDist ) - Main.screenPosition,
                new Rectangle(0, 0, 458, 118), Color.White, r, new Vector2(458 * .5f, 118 * .5f), scale, 0, 0);
            #endregion

            #region Draw laser head
            spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 260, 458, 124), Color.White, r, new Vector2(458 * .5f, 124 * .5f), scale, 0, 0);
            #endregion
        }

        // Change the way of collision check of the projectile
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // We can only collide if we are at max charge, which is when the laser is actually fired

            Player player = Main.player[projectile.owner];
            Vector2 unit = projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), new Vector2(shooter.Center.X, shooter.Center.Y),
                new Vector2(shooter.Center.X, shooter.Center.Y) + unit * Distance, 472, ref point);

            return false;
        }

        // Set custom immunity time on hitting an NPC
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 5;
        }



        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = projectile.velocity;
            Utils.PlotTileLine(projectile.Center, projectile.Center + unit * Distance, (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
        }
    }


}
