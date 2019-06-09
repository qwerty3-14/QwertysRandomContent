using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;
using System.IO;
using System.Collections.Generic;

namespace QwertysRandomContent.NPCs.BossFour
{
    public class Turret : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("OLD lord turret");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.width = 142;
            npc.height = 78;
            npc.damage = 70;
            npc.defense = 1;
            //npc.boss = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 60f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;

            animationType = -1;
            npc.noGravity = true;
            npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            //music = MusicID.Boss5;
            npc.lifeMax = 1;
            //bossBag = mod.ItemType("HydraBag");
            npc.noGravity = true;
            npc.netAlways=true;

        }
        
        public override bool CheckActive()
        {
            
            return false;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            return 0f;

        }
        public int regenTimer;
        
        public Projectile laser;
        public Projectile laser2;
        public float moveDirection;
        //public Vector2 moveTo;
        public float moveSpeed;
        public bool runOnce = true;
        public int timer;
        public int f=1;
        public float targetdirection = -(float)Math.PI / 2;
        public float shotSpeed = 3;
        public int shotDamage;
        public bool fireLaser = true;
        public NPC b4;
        public int frame;
        public int frameTimer;
        public int laserTimer;
        public float rotateSpeed = 3;
        public float R;
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
            /*
            b4 = Main.npc[(int)npc.ai[0]];
            npc.position = new Vector2(b4.position.X - npc.width / 2 + 1 * (b4.width / 6), npc.position.Y + b4.height - npc.height / 2);
            */
            if (Main.netMode != 0)
            {
                Vector2 target = new Vector2(npc.ai[0], npc.ai[1]);
                Vector2 moveTo = new Vector2(target.X, target.Y) - npc.position;


                npc.velocity = (moveTo) * 1f;
            }
            if (Main.expertMode)
            {
                shotDamage = (int)(npc.damage / 4 * 1.6f);
            }
            else
            {
                shotDamage = npc.damage / 2;
            }
            Player player = Main.player[npc.target];
         
            npc.TargetClosest(true);
            if (npc.defense == 2)
            {
                #region 3 shot attack
                rotateSpeed = 3;
                frameTimer++;
                if (frameTimer > 30)
                {
                    frame = 0;
                }
                else
                {
                    frame = 1;
                }

                fireLaser = true;
                timer++;
                targetdirection = (player.Center - npc.Center).ToRotation() - (float)Math.PI / 2;
                if (timer > 120)
                {
                    if (Main.netMode != 1)
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(npc.rotation + (float)Math.PI / 2) * shotSpeed, (float)Math.Sin(npc.rotation + (float)Math.PI / 2) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(npc.rotation + (float)Math.PI / 2 + (float)Math.PI / 8) * shotSpeed, (float)Math.Sin(npc.rotation + (float)Math.PI / 2 + (float)Math.PI / 8) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(npc.rotation + (float)Math.PI / 2 - (float)Math.PI / 8) * shotSpeed, (float)Math.Sin(npc.rotation + (float)Math.PI / 2 - (float)Math.PI / 8) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                    }
                    timer = 0;
                }

                if (targetdirection > -(float)Math.PI / 2 && targetdirection < (float)Math.PI / 2)
                {
                }
                else
                {
                    targetdirection = 0;
                }
                #endregion
            }
            else if (npc.defense == 3)
            {
                #region freeze and fire laser!
                frameTimer = 0;
                frame = 1;
                timer = 0;
                if (fireLaser)
                {
                    if (Main.netMode != 1)
                    {
                        laser = Main.projectile[Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(npc.rotation + (float)Math.PI / 2) * 14f, (float)Math.Sin(npc.rotation + (float)Math.PI / 2) * 14f, mod.ProjectileType("TurretLaser"), 2 * shotDamage, 3f, Main.myPlayer, npc.whoAmI, npc.rotation + (float)Math.PI / 2)];
                    }
                    fireLaser = false;
                }
                #endregion
            }
            else if (npc.defense == 4 || npc.defense == 5)
            {
                #region sweeping laser
                frameTimer = 0;
                frame = 1;
                timer++;

                if (timer > 30)
                {
                    rotateSpeed = .25f;
                    if (fireLaser )
                    {
                        if (Main.netMode != 1)
                        {
                            laser = Main.projectile[Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(npc.rotation + (float)Math.PI / 2) * 14f, (float)Math.Sin(npc.rotation + (float)Math.PI / 2) * 14f, mod.ProjectileType("TurretLaser"), 2 * shotDamage, 3f, Main.myPlayer, npc.whoAmI, npc.rotation + (float)Math.PI / 2)];
                        }
                        fireLaser = false;
                    }
                    if (timer > 150)
                    {
                        targetdirection = 0;
                    }
                }
                else
                {
                    rotateSpeed = 3;
                    if (npc.defense == 4)
                    {
                        targetdirection = -(float)Math.PI / 2;
                    }
                    else
                    {
                        targetdirection = (float)Math.PI / 2;
                    }
                }
                #endregion

            }
            else if (npc.defense == 6)
            {
                #region fire grav shots
                rotateSpeed = 3;
                frameTimer++;
                if (frameTimer > 30)
                {
                    frame = 0;
                }
                else
                {
                    frame = 1;
                }

                fireLaser = true;
                timer++;
                targetdirection = 0;

                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(npc.rotation + (float)Math.PI / 2) * shotSpeed, (float)Math.Sin(npc.rotation + (float)Math.PI / 2) * shotSpeed, mod.ProjectileType("TurretGrav"), shotDamage, 0, Main.myPlayer);

                }
                timer = 0;


                if (targetdirection > -(float)Math.PI / 2 && targetdirection < (float)Math.PI / 2)
                {
                }
                else
                {
                    targetdirection = 0;
                }
                npc.defense = 7;
                #endregion
            }
            else if (npc.defense == 7)
            {
                #region open idle
                rotateSpeed = 3;
                frameTimer++;
                if (frameTimer > 30)
                {
                    frame = 0;
                }
                else
                {
                    frame = 1;
                }

                fireLaser = true;
                timer++;
                targetdirection = 0;
                timer = 0;


                if (targetdirection > -(float)Math.PI / 2 && targetdirection < (float)Math.PI / 2)
                {
                }
                else
                {
                    targetdirection = 0;
                }
                #endregion
            }
            else if (npc.defense == 8)
            {
                #region fire BIG shot
                rotateSpeed = 3;
                frameTimer++;
                if (frameTimer > 30)
                {
                    frame = 0;
                }
                else
                {
                    frame = 1;
                }

                fireLaser = true;
                timer++;
                targetdirection = (player.Center - npc.Center).ToRotation() - (float)Math.PI / 2;

                if ( timer > 240)
                {
                    if (Main.netMode != 1)
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(npc.rotation + (float)Math.PI / 2) * shotSpeed * 1.5f, (float)Math.Sin(npc.rotation + (float)Math.PI / 2) * shotSpeed * 1.5f, mod.ProjectileType("BurstShot"), shotDamage, 0, Main.myPlayer);
                    }
                    timer = 0;
                }



                if (targetdirection > -(float)Math.PI / 2 && targetdirection < (float)Math.PI / 2)
                {
                }
                else
                {
                    targetdirection = 0;
                }
                #endregion

            }
            else if (npc.defense == 9)
            {
                #region fire mines
                rotateSpeed = 3;
                frame = 1;

                fireLaser = true;
                timer++;
                targetdirection = (player.Center - npc.Center).ToRotation() - (float)Math.PI / 2;

                if ( timer > 120)
                {
                    if (Main.netMode != 1)
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(npc.rotation + (float)Math.PI / 2) * shotSpeed * 3f, (float)Math.Sin(npc.rotation + (float)Math.PI / 2) * shotSpeed * 3f, mod.ProjectileType("MagicMineLayer"), shotDamage, 0, Main.myPlayer, npc.whoAmI, (player.Center - npc.Center).Length());
                    }
                    timer = 0;
                }



                if (targetdirection > -(float)Math.PI / 2 && targetdirection < (float)Math.PI / 2)
                {
                }
                else
                {
                    targetdirection = 0;
                }
                #endregion

            }
            else
            {
                rotateSpeed = 3;
                frameTimer = 0;
                fireLaser = true;
                timer = 0;
                targetdirection = 0;
                frame = 2;
            }





           
                R = QwertyMethods.SlowRotation(R, targetdirection, rotateSpeed);
                
            
            npc.rotation = R;
            /*
            if (Main.netMode == 1)
            {
                Main.NewText("client: " + npc.whoAmI + ", "+ npc.ai[2] + ", "  +npc.ai[3]);
            }
            
            
            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + npc.whoAmI + ", " + npc.ai[2] + ", " + npc.ai[3]), Color.White);
            }
            */
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frameHeight * frame;
        }
        


    }
    public class TurretShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pew Pew");


        }
        public override void SetDefaults()
        {
            
            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 360;
            projectile.tileCollide = false;
            //projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.

        }
        /*
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsOverWiresUI.Add(index);
        }*/
        public override bool PreAI()
        {
           
            return base.PreAI();
        }
        public override void AI()
        {
            
            projectile.rotation += (float)Math.PI/30;
            for (int i = 0; i < 1; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("B4PDust"));
            }
        }



    }
    public class BurstShot : ModProjectile
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
            projectile.timeLeft = 360;
            projectile.tileCollide = false;


        }
        public float distance;
        float closest = 250;
        Player player;
        public override void AI()
        {
            if (Main.netMode != 1)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && (projectile.Center - Main.player[i].Center).Length() < closest)
                    {
                        projectile.Kill();
                    }

                }
            }
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
    public class TurretGrav : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pew Pew");


        }
        public override void SetDefaults()
        {

            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 360;
            projectile.tileCollide = false;


        }
        public float horiSpeed;
        public float horiAccCon = .075f;
        public float vertSpeed;
        public float vertAccCon = .075f;
        public float direction;
        public float maxSpeed = 12f;
        float closest = 10000;
        public override void AI()
        {
            if (Main.netMode != 1)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && (projectile.Center - Main.player[i].Center).Length() < closest)
                    {
                        closest = (projectile.Center - Main.player[i].Center).Length();
                        projectile.ai[0] = (Main.player[i].Center - projectile.Center).ToRotation();
                        projectile.netUpdate = true;
                    }

                }
            }


            
            horiSpeed += (float)Math.Cos(projectile.ai[0]) *horiAccCon;
            vertSpeed += (float)Math.Sin(projectile.ai[0]) * vertAccCon;
            projectile.velocity = new Vector2(horiSpeed, vertSpeed);

            if(projectile.velocity.Length() > maxSpeed)
            {
                projectile.velocity = projectile.velocity.SafeNormalize(-Vector2.UnitY) * maxSpeed;
            }
            projectile.rotation += (float)Math.PI / 30;
            for (int i = 0; i < 1; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("B4PDust"));
            }
            closest = 10000;

        }



    }
    public class MagicMineLayer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Mine");
            

        }
        public override void SetDefaults()
        {

            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 300;
            projectile.tileCollide = false;


        }
        public float horiSpeed;
        public float vertSpeed;
        public float direction;
        public float pullSpeed = .5f;
        public float dustSpeed = 20f;
        public NPC origin;
        
        public int frameTimer;
        public float distance;
        public override void AI()
        {
            origin = Main.npc[(int)projectile.ai[0]];

            Player player = Main.player[projectile.owner];

            if((origin.Center-projectile.Center).Length()>projectile.ai[1])
            {
                projectile.Kill();
            }





            
        }
        public override void Kill(int timeLeft)
        {
            if (Main.netMode != 1)
            {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MagicMine"), projectile.damage, 0, Main.myPlayer);
            }
        }


    }
    public class MagicMine : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Mine");
            Main.projFrames[projectile.type] = 5;

        }
        public override void SetDefaults()
        {

            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
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
        public float distance;
        public override void AI()
        {
            projectile.velocity = new Vector2(0, 0);

            Player player = Main.player[projectile.owner];








            frameTimer++;
            if (frameTimer % 5 == 0)
            {
                projectile.frame++;
                if (projectile.frame > 4)
                {
                    projectile.frame = 0;
                }
            }




        }



    }

    // The following laser shows a channeled ability, after charging up the laser will be fired
    // Using custom drawing, dust effects, and custom collision checks for tiles
    public class TurretLaser : ModProjectile
    {
        // The maximum charge value
        private const float MaxChargeValue = 120f;
        //The distance charge particle from the player center
        private const float MoveDistance = 60f;

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
        
        public override void AI()
        {

            shooter = Main.npc[(int)projectile.ai[0]];
            Vector2 mousePos = Main.MouseWorld;
            Player player = Main.player[projectile.owner];
            if (!shooter.active || (shooter.defense !=3 && shooter.defense != 4 && shooter.defense != 5))
            {
                projectile.Kill();
            }
            
            #region Set projectile position


            Vector2 diff = new Vector2((float)Math.Cos(shooter.rotation + (float)Math.PI / 2) * 14f, (float)Math.Sin(shooter.rotation + (float)Math.PI / 2) * 14f);
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
            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
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
                    projectile.velocity, 10, projectile.damage, -1.57f, 1f, 4000f, Color.White, (int)MoveDistance);
            }
            else
            {
                Vector2 center = projectile.Center;
                
                float projRotation = shooter.rotation;
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
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 26, 28, 26), i < transDist ? Color.Transparent : c, r,
                    new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            }
            #endregion

            #region Draw laser tail
            spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(0, 0, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            #endregion

            #region Draw laser head
            spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 52, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
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
                new Vector2(shooter.Center.X, shooter.Center.Y) + unit * Distance, 22, ref point);

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

