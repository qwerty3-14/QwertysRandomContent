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
    public class Swarmer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortress Swarmer");
            Main.npcFrameCount[npc.type] = 2;//number of frames, frames will be cut from your nps's png evenly vertically
        }

        public override void SetDefaults()
        {
            npc.width = 28; //should be the same as your npc's frame width
            npc.height = 28;//should be the same as your npc's frame height
            npc.aiStyle = -1; // -1 is blank (we will write our own)
            npc.damage = 20; // damage the enemy does on contact automaticly doubled in expert
            npc.defense = 3; // defense of enemy
            npc.lifeMax = 14; //maximum life doubled automaticly in expert
            npc.value = 3; // how much $$ it drops
            npc.HitSound = SoundID.NPCHit1; //sfx when hit
            npc.DeathSound = SoundID.NPCDeath1; // sfx when killed
            npc.knockBackResist = 0f; //knockback reducion 0 means it takes no knockback
            npc.noGravity = true; // recommended for flying enemies
            npc.npcSlots = 0.05f;
            npc.noTileCollide = true;
            npc.buffImmune[BuffID.Confused] = false;
        }
        float maxSpeed = 4;
        float maxFriendRepelDistance = 50;
        bool foundfriend = false;
        int antiTileChecks = 8;
        bool runOnce = true;
        int freindCount = 0;
        int totalCount;
        public override void AI()
        {
            if(Main.expertMode)
            {
                npc.lifeMax = 20;
                npc.damage = 30;
                if(Main.hardMode)
                {
                    npc.damage = 70;
                }
            }
            else
            {
                npc.lifeMax = 14;
                npc.damage = 15;
                if (Main.hardMode)
                {
                    npc.damage = 35;
                }
            }
            if (npc.life > npc.lifeMax)
            {
                npc.life = npc.lifeMax;
            }
            
            npc.TargetClosest(true);
            freindCount = 0;
            for (int n = 0; n < 200; n++)
            {

                if (n != npc.whoAmI && Main.npc[n].active && Main.npc[n].type == mod.NPCType("Swarmer") && (Main.npc[n].Center - npc.Center).Length() < 200)
                {
                    freindCount++;
                }
            }
            Player player = Main.player[npc.target];
            npc.velocity = Vector2.Zero;
            float towardsPlayer = (player.Center - npc.Center).ToRotation();
            if(freindCount >=4)
            {
                npc.velocity = QwertyMethods.PolarVector(maxSpeed, towardsPlayer);
            }
            else
            {
                totalCount = 0;
                for (int n = 0; n < 200; n++)
                {
                    if (n != npc.whoAmI && Main.npc[n].active && Main.npc[n].type == mod.NPCType("Swarmer"))
                    {
                        totalCount++;

                    }
                }
                if (totalCount >= 4)
                {
                    for (int n = 0; n < 200; n++)
                    {

                        if (n != npc.whoAmI && Main.npc[n].active && Main.npc[n].type == mod.NPCType("Swarmer") && (Main.npc[n].Center - npc.Center).Length() > 150)
                        {
                            totalCount++;
                            npc.velocity += QwertyMethods.PolarVector(4 , (Main.npc[n].Center - npc.Center).ToRotation());
                        }
                    }
                }
                else
                {
                    npc.velocity = QwertyMethods.PolarVector(-maxSpeed, towardsPlayer);
                }
            }
            

            for(int n =0; n <200; n++)
            {
                
                if(n != npc.whoAmI && Main.npc[n].active && Main.npc[n].type == mod.NPCType("Swarmer") && (Main.npc[n].Center-npc.Center).Length()< maxFriendRepelDistance)
                {
                    npc.velocity += QwertyMethods.PolarVector(-4  * (1 - (Main.npc[n].Center - npc.Center).Length() / maxFriendRepelDistance), (Main.npc[n].Center - npc.Center).ToRotation());
                    foundfriend = true;
                }
            }
           
          
            if (npc.velocity.Length() > maxSpeed)
            {
                npc.velocity = npc.velocity.SafeNormalize(-Vector2.UnitY) * maxSpeed;
            }
            
            if (npc.velocity != Collision.TileCollision(npc.position, npc.velocity, npc.width, npc.height, false, false))
            {
                maxSpeed = 1;
            }
            else
            {
                for(int p =0; p <1000; p++)//cycle through every projectile index
                {
                    float CollisionLineLength = Main.projectile[p].velocity.Length() * 60 * (1 + Main.projectile[p].extraUpdates); //this is basicly how far the selected projectile will travel in 1 second
                    float maxProjectileWidth = Main.projectile[p].Size.Length() * 5f; // fly further away from larger projectiles
                    float col=0f;
                    if (Main.projectile[p].friendly&& Main.projectile[p].active && Collision.CheckAABBvLineCollision(npc.position, npc.Size, Main.projectile[p].Center + QwertyMethods.PolarVector(maxProjectileWidth / 4f, Main.projectile[p].velocity.ToRotation() + (float)Math.PI / 2), Main.projectile[p].Center + QwertyMethods.PolarVector(maxProjectileWidth / 4f, Main.projectile[p].velocity.ToRotation() + (float)Math.PI / 2) + QwertyMethods.PolarVector(CollisionLineLength, Main.projectile[p].velocity.ToRotation()), maxProjectileWidth/2f, ref col))
                    {
                        npc.velocity += QwertyMethods.PolarVector(maxSpeed, Main.projectile[p].velocity.ToRotation()+(float)Math.PI/2);
                    }
                    else if (Main.projectile[p].friendly && Main.projectile[p].active && Collision.CheckAABBvLineCollision(npc.position, npc.Size, Main.projectile[p].Center - QwertyMethods.PolarVector(maxProjectileWidth / 4f, Main.projectile[p].velocity.ToRotation() + (float)Math.PI / 2), Main.projectile[p].Center - QwertyMethods.PolarVector(maxProjectileWidth / 4f, Main.projectile[p].velocity.ToRotation() + (float)Math.PI / 2) + QwertyMethods.PolarVector(CollisionLineLength, Main.projectile[p].velocity.ToRotation()), maxProjectileWidth / 2f, ref col))
                    {
                        npc.velocity += QwertyMethods.PolarVector(maxSpeed, Main.projectile[p].velocity.ToRotation() - (float)Math.PI / 2);
                    }


                }
            }
            
            if (npc.velocity.Length() > maxSpeed)
            {
                npc.velocity = npc.velocity.SafeNormalize(-Vector2.UnitY) * maxSpeed;
            }
            npc.velocity *= (npc.confused ? -1 : 1);
            npc.rotation = npc.velocity.ToRotation() + (float)Math.PI / 2;
            maxSpeed = 4;
        }
        
       
        public override float SpawnChance(NPCSpawnInfo spawnInfo) //changes spawn rates must return a float
        {
            return 0f;
        }
        
        
            
        
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit) // this is run whenever the npc is hit by a projectile
        {
            
        }
        int frame = 0;
        public override void FindFrame(int frameHeight) // this part takes care of animations
        {
            npc.frameCounter++;
            if(npc.frameCounter %4==0)
            {
               frame= frame == 0 ? 1 : 0;
            }

            npc.frame.Y = frameHeight * frame;
        }

    }
    public class Swarm : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swarm");
            Main.npcFrameCount[npc.type] = 2;//number of frames, frames will be cut from your nps's png evenly vertically
        }

        public override void SetDefaults()
        {
            npc.width = 28; //should be the same as your npc's frame width
            npc.height = 28;//should be the same as your npc's frame height
            npc.aiStyle = -1; // -1 is blank (we will write our own)
            npc.damage = 20; // damage the enemy does on contact automaticly doubled in expert
            npc.defense = 3; // defense of enemy
            npc.lifeMax = 25; //maximum life doubled automaticly in expert
            npc.value = 3; // how much $$ it drops
            npc.HitSound = SoundID.NPCHit1; //sfx when hit
            npc.DeathSound = SoundID.NPCDeath1; // sfx when killed
            npc.knockBackResist = 0f; //knockback reducion 0 means it takes no knockback
            npc.noGravity = true; // recommended for flying enemies
            npc.dontCountMe = true;
            npc.noTileCollide = true;
            npc.timeLeft = 2;
            banner = npc.type;
            bannerItem = mod.ItemType("SwarmerBanner");
        }



        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<FortressBiome>(mod).TheFortress )
            {
                return 5f;
            }
            return 0f;

        }

        int swarmSize = 0;
        public override void AI()
        {
            if(Main.netMode !=1)
            {
                swarmSize = Main.rand.Next(14, 36);
                if(Main.hardMode)
                {
                    swarmSize *= 2;
                }
                for(int s=0; s<swarmSize; s++)
                {
                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Swarmer"));
                }
            }
            npc.active = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }

    }

}
