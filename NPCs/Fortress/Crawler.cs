using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.World.Generation;

namespace QwertysRandomContent.NPCs.Fortress
{
    public class Crawler : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mollusket");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.width = 36 + 24;
            npc.height = 36 + 24;
            npc.aiStyle = -1;
            npc.damage = 20;
            npc.defense = 18;
            npc.lifeMax = 65;
            drawOffsetY = -4;
            npc.value = 100;
            //npc.alpha = 100;
            npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.direction = 1;
            //npc.dontTakeDamage = true;
            //npc.scale = 1.2f;
            npc.buffImmune[mod.BuffType("PowerDown")] = true;
            banner = npc.type;
            bannerItem = mod.ItemType("CrawlerBanner");
            npc.noTileCollide = true;
            //npc.scale = 1.2f;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    int dustType = mod.DustType("FortressDust"); ;
                    int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
                    Dust dust = Main.dust[dustIndex];
                    dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                    dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                    dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                }

            }
            for (int i = 0; i < 4; i++)
            {
                int dustType = mod.DustType("FortressDust"); ;
                int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<FortressBiome>(mod).TheFortress)
            {
                return 60f;
            }
            return 0f;

        }
        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CaeliteBullet"), Main.rand.Next(2, 5));
            if(Main.rand.Next(20)==0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CaeliteFlask"), 1);
            }
        }

        int timer;
        bool drawLine;
        bool alternateColor;
        int startDirection = -1;
        Vector2 shootFrom;
        float gunShift = -5.5f;
        float gunShiftUp = 11.5f;
        float aimDirection;
        public override void AI()
        {

            npc.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;
            if (npc.ai[0] == 0f)
            {
                npc.TargetClosest(true);
                npc.direction = 1;
                Player initPlayer = Main.player[npc.target];
                if(initPlayer.Center.X < npc.Center.X)
                {
                    //startDirection = 1;
                }
                npc.noGravity = true;
                npc.directionY = 1;
                npc.ai[0] = 1f;
                Point origin = npc.Center.ToTileCoordinates();
                Point point;
                
                while (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(1), new GenCondition[]
                {
                                            new Conditions.IsSolid()
                }), out point))
                {
                    npc.position.Y++;
                    origin = npc.Center.ToTileCoordinates();
                }
                
            }
            shootFrom = npc.Center + QwertyMethods.PolarVector(gunShift, npc.rotation) * -startDirection + QwertyMethods.PolarVector(gunShiftUp, npc.rotation - (float)Math.PI/2);
            int speed = 2;
            if (npc.ai[1] == 0f)
            {
                //npc.rotation += (float)(npc.direction * npc.directionY) * 0.13f;
                if (npc.collideY)
                {
                    npc.ai[0] = 2f;
                }
                if (!npc.collideY && npc.ai[0] == 2f)
                {
                    npc.direction = -npc.direction;
                    npc.ai[1] = 1f;
                    npc.ai[0] = 1f;
                }
                if (npc.collideX)
                {
                    npc.directionY = -npc.directionY;
                    npc.ai[1] = 1f;
                }
            }
            else
            {
                //npc.rotation -= (float)(npc.direction * npc.directionY) * 0.13f;
                if (npc.collideX)
                {
                    npc.ai[0] = 2f;
                }
                if (!npc.collideX && npc.ai[0] == 2f)
                {
                    npc.directionY = -npc.directionY;
                    npc.ai[1] = 0f;
                    npc.ai[0] = 1f;
                }
                if (npc.collideY)
                {
                    npc.direction = -npc.direction;
                    npc.ai[1] = 0f;
                }
            }
            //npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            if(Collision.CanHit(shootFrom, 0, 0, player.Center, 0, 0) && (player.Center-npc.Center).Length() <1000)
            {
                npc.velocity.X = 0;
                npc.velocity.Y = 0;
                aimDirection = (player.Center - npc.Center).ToRotation();
                timer++;
                if (timer > 180)
                {
                    
                     
                    float shootSpeed = 24;
                   
                    Projectile.NewProjectile(shootFrom.X, shootFrom.Y, (float)Math.Cos(aimDirection) * shootSpeed, (float)Math.Sin(aimDirection) * shootSpeed, mod.ProjectileType("MollusketSnipe"), 15, 0, player.whoAmI);
                    timer = 0;
                }
                if(timer>60)
                {
                    alternateColor = true;
                }
                else
                {
                    alternateColor = false;
                }
                if(timer>30)
                {
                    drawLine = true;
                }
                else
                {
                    drawLine = false;
                }
                
            }
            else
            {
                
                drawLine = false;
                alternateColor = false;
                timer = 0;
                npc.velocity.X = (float)(speed * npc.direction);
                npc.velocity.Y = (float)(speed * npc.directionY);
                npc.rotation = npc.velocity.ToRotation();
                npc.rotation += (float)Math.PI/4 * startDirection;
                aimDirection = npc.rotation;
                /*
                if(npc.velocity.X >0 && npc.velocity.Y >0)
                {
                    npc.rotation = 0;
                }
                else if(npc.velocity.X < 0 && npc.velocity.Y > 0)
                {
                    npc.rotation = (float)Math.PI;
                }
                else if (npc.velocity.X > 0 && npc.velocity.Y < 0)
                {
                    npc.rotation = (float)Math.PI;
                }
                else if (npc.velocity.X < 0 && npc.velocity.Y < 0)
                {
                    npc.rotation = (float)Math.PI;
                }
                */
                //npc.spriteDirection = npc.direction;

            }
            
            float num281 = (float)(270 - (int)Main.mouseTextColor) / 400f;

            npc.oldVelocity = npc.velocity;
            npc.collideX = false;
            npc.collideY = false;
            Vector2 position = npc.Center;
            int num = 12;
            int num2 = 12;
            position.X -= (float)(num / 2);
            position.Y -= (float)(num2 / 2);
            npc.velocity = Collision.noSlopeCollision(position, npc.velocity, num, num2, true, true);
            if (npc.oldVelocity.X != npc.velocity.X)
            {
                npc.collideX = true;
            }
            if (npc.oldVelocity.Y != npc.velocity.Y)
            {
                npc.collideY = true;
            }
            //Lighting.AddLight((int)(npc.position.X + (float)(npc.width / 2)) / 16, (int)(npc.position.Y + (float)(npc.height / 2)) / 16, 0.9f, 0.3f + num281, 0.2f);
            return;
        }
        int colorCounter;
        Color lineColor;
        float distance;
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/Fortress/Crawler");
            spriteBatch.Draw(mod.GetTexture("NPCs/Fortress/Crawler"), new Vector2(npc.Center.X - Main.screenPosition.X, npc.Center.Y - Main.screenPosition.Y),
                       npc.frame, drawColor, npc.rotation,
                       new Vector2(npc.width * 0.5f, npc.height * 0.5f), 1f, 0, 0f);
            Player player = Main.player[npc.target];


            /*
                if (distance < 10000f && !target.friendly && target.active && !target.immortal && timer >= 480)
                {
                    drawLine = true;
                    alternateColor = true;
                }
                else if (distance < 10000f && !target.friendly && target.active && !target.immortal && timer >= 120)
                {
                    drawLine = true;
                }
                else
                {
                    drawLine = false;
                }
                */


            if (alternateColor)
            {

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
            }
            else
            {
                lineColor = Color.Red;
            }
            //Draw chain
            if (drawLine)
            {
                Vector2 center = shootFrom;
                Vector2 distToProj = player.Center - center;
                float projRotation = distToProj.ToRotation() - 1.57f;
                distToProj.Normalize();                 //get unit vector
                distToProj *= 12f;                      //speed = 12
                center += distToProj;                   //update draw position
                distToProj = player.Center - center;    //update distance
                distance = distToProj.Length();
                //Color drawColor = lightColor;


                spriteBatch.Draw(mod.GetTexture("Items/Weapons/Rhuthinium/laser"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, 1, (int)distance - 10), lineColor, projRotation,
                    new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            }

            drawLine = false;
            spriteBatch.Draw(mod.GetTexture("NPCs/Fortress/Crawler_Turret"), new Vector2(shootFrom.X - Main.screenPosition.X, shootFrom.Y - Main.screenPosition.Y + 2f),
                        new Rectangle(0, 0, 28, 14), drawColor, aimDirection,
                        new Vector2(5, 9), 1f, 0, 0f);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            





            
           
            return false;


        }
    }
    public class MollusketSnipe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mollusket Snipe");


        }
        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = 1;
            projectile.light = 0.5f;
            //projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 600;
            projectile.ranged = true;
            projectile.extraUpdates = 3;




        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void AI()
        {
            for (int d = 0; d < 1; d++)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, mod.DustType("CaeliteDust"), Vector2.Zero);
                dust.frame.Y = 0;
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("PowerDown"), 480);
        }
       

    }

}
