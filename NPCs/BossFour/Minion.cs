using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;
using QwertysRandomContent;
namespace QwertysRandomContent.NPCs.BossFour
{
    public class Minion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("O.L.O.R.D. Minion");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.width = 300;
            npc.height = 300;
            npc.damage = 140;
            npc.defense = 50;
            
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 1;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.netAlways = true;
            animationType = -1;
            npc.noGravity = true;
            
            npc.noTileCollide = true;
            
            

            
            npc.lifeMax = 36000;
            //bossBag = mod.ItemType("HydraBag");
            

        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * 1f);
        }
        public override bool CheckActive()
        {

            return false;
        }
       
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            return 0f;

        }
        
        
        public bool runOnce = true;
        public int shotDamage;
        public int f = 1;
        public float TargetDirection;
        public float Speed=5f;
        public float direction;
        public int timer;
        public override void AI()
        {
            
            if (Main.expertMode)
            {
                shotDamage = (int)(npc.damage / 16 * 1.6f);
            }
            else
            {
                shotDamage = npc.damage / 8;
            }
            Player player = Main.player[npc.target];
            npc.TargetClosest(true);
            
            TargetDirection = (player.Center - npc.Center).ToRotation();
            
            direction = QwertyMethods.SlowRotation(direction, TargetDirection, 1);
            npc.velocity = new Vector2((float)(Math.Cos(direction) * Speed), (float)(Math.Sin(direction) * Speed));
            npc.rotation = direction - MathHelper.ToRadians(90);
            timer++;
            if(timer%120==0)
            {
                float shotSpeed=3f;
                float forwardshift = 100;
                float sideShift = 40;
                float distance = (player.Center - npc.Center).Length();
                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(direction + Math.PI/2)* shotSpeed, (float)Math.Sin(direction +  Math.PI/2) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)Math.Cos(direction -  Math.PI/2) * shotSpeed, (float)Math.Sin(direction -  Math.PI/2) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                Projectile.NewProjectile(npc.Center.X+(float)Math.Cos(direction) * forwardshift + (float)Math.Cos(direction + Math.PI / 2) * sideShift, npc.Center.Y + (float)Math.Sin(direction) * forwardshift+(float)Math.Sin(direction + Math.PI / 2) * sideShift, (float)Math.Cos(direction + Math.PI / 2) * shotSpeed, (float)Math.Sin(direction + Math.PI / 2) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                Projectile.NewProjectile(npc.Center.X + (float)Math.Cos(direction) * forwardshift + (float)Math.Cos(direction - Math.PI / 2) * sideShift, npc.Center.Y + (float)Math.Sin(direction) * forwardshift + (float)Math.Sin(direction - Math.PI / 2) * sideShift, (float)Math.Cos(direction - Math.PI / 2) * shotSpeed, (float)Math.Sin(direction - Math.PI / 2) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                

                for (int r = 0; r < 8; r++)
                {
                    Projectile.NewProjectile(npc.Center.X + (float)Math.Cos(TargetDirection) * (distance + 1000), npc.Center.Y + (float)Math.Sin(TargetDirection) * (distance + 1000), (float)Math.Cos(r * (2 * Math.PI / 8)) * shotSpeed, (float)Math.Sin(r * (2 * Math.PI / 8)) * shotSpeed, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                    Projectile.NewProjectile(npc.Center.X + (float)Math.Cos(TargetDirection) * (distance + 1000), npc.Center.Y + (float)Math.Sin(TargetDirection) * (distance + 1000), (float)Math.Cos(r * (2 * Math.PI / 8) + Math.PI / 8) * shotSpeed * 1.5f, (float)Math.Sin(r * (2 * Math.PI / 8) + Math.PI / 8) * shotSpeed * 1.5f, mod.ProjectileType("TurretShot"), shotDamage, 0, Main.myPlayer);
                }
            }
        }
        public int frame=1;
        public int frameTimer;
        public override void FindFrame(int frameHeight)
        {
            frameTimer++;
            if(frameTimer % 10==0)
            {
                

                frame *= -1;
            }
            if(frame==1)
            {
                npc.frame.Y = frameHeight * 0;
            }
            else
            {
                npc.frame.Y = frameHeight * 1;
            }
            
        }
        


    }
    


}
