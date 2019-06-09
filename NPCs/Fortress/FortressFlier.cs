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
    public class FortressFlier : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortress Harpy");
            Main.npcFrameCount[npc.type] = 4;//number of frames, frames will be cut from your nps's png evenly vertically
        }

        public override void SetDefaults()
        {
            npc.width = 56; //should be the same as your npc's frame width
            npc.height = 48;//should be the same as your npc's frame height
            npc.aiStyle = -1; // -1 is blank (we will write our own)
            npc.damage = 50; // damage the enemy does on contact automaticly doubled in expert
            npc.defense = 28; // defense of enemy
            npc.lifeMax = 330; //maximum life doubled automaticly in expert
            npc.value = 100; // how much $$ it drops
            npc.HitSound = SoundID.NPCHit1; //sfx when hit
            npc.DeathSound = SoundID.NPCDeath1; // sfx when killed
            npc.knockBackResist = 0f; //knockback reducion 0 means it takes no knockback
            npc.noGravity = true; // recommended for flying enemies
            banner = npc.type;
            bannerItem = mod.ItemType("FortressFlierBanner");


        }
        
        public override void HitEffect(int hitDirection, double damage)//run whenever enemy is hit should be used for visuals like gore
        {
            

        }
        public override void NPCLoot() //drops
        {
            
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FortressHarpyBeak"), 1);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) //changes spawn rates must return a float
        {
            if (spawnInfo.player.GetModPlayer<FortressBiome>(mod).TheFortress && Main.hardMode) // checks if it's hardmode and player is in the fortress biome
            {
                return 100f;
            }
            return 0f;

        }
        //here I set variables that the AI uses
        float flySpeed = 6f;
        bool clinged = false;
        int timer;
        float playerDistance;
        bool runOnce = true;
        int faceDirection = 1;
        int frame;
        int frameTimer;
        int damage = 24;
        float verticalSpeed = 3;
        float verticalFlightTimer;
        int attackTimer;
        public override void AI() //this will run every frame
        {
            npc.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;
            if (runOnce)
            {
                //I put stuff here I want to only run once
                runOnce = false;
            }
            Player player = Main.player[npc.target]; // sets the variable player needed to locate the player
            npc.TargetClosest(true); // give the npc a target

            playerDistance = (player.Center - npc.Center).Length(); // finds the distance between this enemy and player

            timer++;
            if (clinged) //run when the enemy is clinged to the wall
            {
                
                npc.velocity.X = 0; // set velocity to 0 (no movement)
                npc.velocity.Y = 0;// set velocity to 0 (no movement)
                if (timer > 10 && playerDistance < 200 && Collision.CanHit(npc.Center, 0, 0, player.Center, 0, 0)) // this checks if the player is too close and not behind tiles, the timer is so it doesn't immediatly stick to a wall it jumps off
                {
                    clinged = false; // stop sticking to the wall
                    timer = 0; //reset timer
                }
                else if(Collision.CanHit(npc.Center, 0, 0, player.Center, 0, 0) && playerDistance <600) // this checks if the player is close but not too close and not behind tiles
                {
                    attackTimer++; // this timer is used so the attack isn't every frame
                    if (attackTimer >= 60) //this will be true when the timer is above 60 frames (1 second)
                    {
                        float shootDirection = (player.Center - npc.Center).ToRotation(); // find the direction the player is in
                        for(int p=-1; p <2; p++) //this will repeat 3 times for 3 projectiles
                        {
                            Projectile.NewProjectile(npc.Center, QwertyMethods.PolarVector(6, shootDirection + ((float)Math.PI / 8 * p)), mod.ProjectileType("FortressHarpyProjectile"), damage, player.whoAmI); // shoots a projectile
                        }
                        attackTimer = 0; // resets attackTimer needer for the once per second effect
                    }
                    else if(attackTimer > 45)  //this will be true when the timer is above 45 frames (.75 seconds)
                    {
                        frame = 3; // change the frame to signal it's about to attack
                    }
                    else
                    {
                        frame = 2; // change the frame to normal cling
                    }

                }
                else // player too far away or can't be seen
                {
                    attackTimer = 0;
                    frame = 2;
                }
            }
            else
            {
                //this is for flying animaion it cycles through 2 frames
                frameTimer++;
                if (frameTimer %10 ==0) // true every 10th frame the % gives the remainder of the division problem frameTimer/10
                {
                    if (frame == 0)
                    {
                        frame = 1;
                    }
                    else
                    {
                        frame = 0;
                    }
                }
                //////////////

                verticalFlightTimer += (float)Math.PI / 60; //add this amount to the flight timer every frame, it is used as a radian value so this means it will go 180 degrees every second
                npc.velocity.Y = (float)Math.Cos(verticalFlightTimer) * verticalSpeed; //the up and down flying motion uses a cosine function,
                //It is based on a sine wave on a graph as x continually increases Y goes from 1 - -1
                //Cosine is the derivitive of Sine the harpy flies in a sine wave pattern
                //Vertical speed increases the speed it flies up and down othersie it'll just range from 1 - -1
               npc.velocity.X = flySpeed * faceDirection; // much simpler than the vertical movement this simply moves in the direction of face direction at the flySpeed
                if(npc.collideX && timer >10)
                {
                    faceDirection *= -1; //flips the direction it faces
                    clinged = true; //start clinging to the wall
                    timer = 0; // reset pattern
                }
            }
            
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit) // this is run whenever the npc is hit by a projectile
        {
            if(playerDistance>600) //this checks the distance, it will make it fly away if it's getting 'sniped
            {
                clinged = false; 
                timer = 0;
            }
            else if(Main.rand.Next(5)==0) // if the player is in 'valid' range it will randomly fly away
            {
                clinged = false;
                timer = 0;
            }
        }
        public override void FindFrame(int frameHeight) // this part takes care of animations
        {
            npc.spriteDirection = faceDirection;
            npc.frame.Y = frameHeight * frame;
        }

    }
    public class FortressHarpyProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortress Harpy Projectile");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
            projectile.tileCollide = true;


        }

        public int dustTimer;

        public override void AI()
        {
            
        }

    }

}
