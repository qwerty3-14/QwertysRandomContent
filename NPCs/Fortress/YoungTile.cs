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
    public class YoungTile : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Young Tile");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 16;
            npc.height = 20;
            npc.aiStyle = -1;
            npc.damage = 28;
            npc.defense = 18;
            npc.lifeMax = 80;
            npc.value = 50;
            //npc.alpha = 100;
            //npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            //npc.dontTakeDamage = true;
            //npc.scale = 1.2f;
            npc.buffImmune[20] = true;
            npc.buffImmune[24] = true;
            banner = npc.type;
            //bannerItem = mod.ItemType("HopperBanner");
            npc.buffImmune[BuffID.Confused] = false;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int dustType = mod.DustType("FortressDust"); ;
                    int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
                    Dust dust = Main.dust[dustIndex];
                    dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                    dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                    dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                }

            }
            for (int i = 0; i < 1; i++)
            {
                int dustType = mod.DustType("FortressDust"); ;
                int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }

        }
        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FortressBrick"), Main.rand.Next(1));
        }
       
        int frame;
        int timer;
        float jumpSpeedY = -10.5f;
        float jumpSpeedX = 4;
        float aggroDistance = 400;
        float aggroDistanceY = 200;
        bool jump;
        float gravity = .3f;
        public override void AI()
        {
            if(frame ==0)
            {
                npc.dontTakeDamage = true;
            }
            else
            {
                npc.dontTakeDamage = false;
            }
            gravity = .3f;
            float worldSizeModifier = (float)(Main.maxTilesX / 4200);
            worldSizeModifier *= worldSizeModifier;
            //small =1
            //medium =2.25
            //large =4
            float num2 = (float)((double)(npc.position.Y / 16f - (60f + 10f * worldSizeModifier)) / (Main.worldSurface / 6.0));
            if ((double)num2 < 0.25)
            {
                num2 = 0.25f;
            }
            if (num2 > 1f)
            {
                num2 = 1f;
            }
            gravity *= num2;
            jumpSpeedY = gravity * -35;
            //Main.NewText("gravity: " +gravity);
            //Main.NewText("jump: " +jumpSpeedY);
            Player player = Main.player[npc.target];
            npc.TargetClosest(true);
            //Main.NewText(Math.Abs(player.Center.X - npc.Center.X));
            if(Math.Abs(player.Center.X-npc.Center.X) < aggroDistance && Math.Abs(player.Bottom.Y - npc.Bottom.Y) < aggroDistanceY)
            {
                jumpSpeedX =Math.Abs(player.Center.X - npc.Center.X) / 70 * (npc.confused ? -1 : 1);
                timer++;
                if (timer > 30)
                {
                    frame = 3;
                    if (!jump)
                    {
                        
                        if (player.Center.X > npc.Center.X)
                        {
                            npc.velocity.X = jumpSpeedX ;
                            npc.velocity.Y = jumpSpeedY;
                        }
                        else
                        {
                            npc.velocity.X = -jumpSpeedX ;
                            npc.velocity.Y = jumpSpeedY;
                        }
                        jump = true;
                    }
                }
                else if(timer >20)
                {
                    frame = 1;
                }
                else if(timer >10)
                {
                    frame = 2;
                }
                else
                {
                    frame = 1;
                }
            }
            else if(!jump)
            {
                frame = 0;
                timer = 0;
            }
            if(npc.collideX)
            {
                npc.velocity.X *= -1;
            }
            if(timer >62  && npc.collideY)
            {
                npc.velocity.X = 0;
                npc.velocity.Y = 0;
                jump = false;
                timer = 0;
            }
            npc.velocity.Y += gravity;
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frame * frameHeight;
        }
    }
    
}
