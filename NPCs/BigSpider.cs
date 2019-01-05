using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
/*
namespace QwertysRandomContent.NPCs
{
    public class BigSpider : ModNPC
    {
        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Big Spider");
            Main.npcFrameCount[npc.type] = 1;

        }

        public override void SetDefaults()
        {

            npc.width = 75;
            npc.height = 75;
            npc.damage = 50;
            npc.defense = 18;
            npc.boss = true;
            npc.HitSound = SoundID.NPCHit29;
            npc.DeathSound = SoundID.NPCDeath31;
            npc.value = 60f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            //aiType = 10;
            animationType = -1;
            npc.noGravity = false;
            npc.noTileCollide = false;
           
            npc.lifeMax = 7500;
            
            npc.buffImmune[BuffID.Venom] = true;
            

        }
        
        Vector2 runTo; //This vector's X will be used for X location to run to, the Y is for direction
        bool runOnce = true;
        int minDistSelectFromPlayer = 400;
        int maxDistanceSelectFromPlayer = 1000;
        int runDirection = 1;
        float runSpeed = 10;
        float climbSpeed = 10;
        int attackTimer;
        int attackCooldown = 300;
        int attackDelay = 30;
        int attackDuration = 120;
        int minionCooldown = 10;
        int webCooldown = 10;
        int venomCooldown = 10;
        int damage =20; //projectile damage will be doubled in normal quadrupled in expert
        float webSpeed = 8;
        public override void AI()
        {
            
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];
            if(runOnce)
            {
                runTo = getRunTo(player);
                npc.netUpdate = true;
                runOnce = false;
            }
            if((runTo.Y == 1 && npc.Center.X> runTo.X) || (runTo.Y == -1 && npc.Center.X < runTo.X))
            {

                runTo = getRunTo(player);
                npc.netUpdate = true;
            }
            attackTimer++;
            if (attackTimer > attackCooldown + attackDelay + attackDuration)
            {
                attackTimer = 0;
                npc.netUpdate = true;
            }
            else if (attackTimer > attackCooldown)
            {
                npc.velocity.X = 0;
                if (attackTimer > attackCooldown + attackDelay)
                {

                    if (attackTimer % minionCooldown == 0 && Main.netMode != 1 && npc.ai[0] == 0f)
                    {
                        NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.BlackRecluse);
                    }


                    if (attackTimer % venomCooldown == 0 && Main.netMode != 1 && npc.ai[0] == 1f)
                    {

                    }

                    if (attackTimer % webCooldown == 0 && npc.ai[0] == 2f)
                    {
                        Projectile.NewProjectile(npc.Center, new Vector2((float)Math.Cos((player.Center - npc.Center).ToRotation()), (float)Math.Sin((player.Center - npc.Center).ToRotation())) * webSpeed, ProjectileID.WebSpit, damage, 0f, player.whoAmI);
                    }


                }



            }
            else if (attackTimer == attackCooldown)
            {
                if (Main.netMode != 1)
                {
                    if (Main.expertMode && npc.life < npc.lifeMax / 2)
                    {
                        npc.ai[0] = Main.rand.Next(3);
                    }
                    else
                    {
                        npc.ai[0] = Main.rand.Next(2);
                    }

                    npc.netUpdate = true;
                }
            }
            else
            {
                npc.velocity.X = runSpeed * runTo.Y;

                if (npc.collideX)
                {
                    npc.velocity.Y = -climbSpeed;
                }
            }
            
                if (Main.netMode == 1)
                {
                    Main.NewText("client: " + npc.ai[0]);
                }


                if (Main.netMode == 2) // Server
                {
                    NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + npc.ai[0]), Color.Blue);
                }
             

        }
        public Vector2 getRunTo(Player player)
        {
            if (Main.netMode != 1) //not client
            {
                if (npc.Center.X > player.Center.X)
                {
                    return new Vector2(player.Center.X - Main.rand.Next(minDistSelectFromPlayer, maxDistanceSelectFromPlayer + 1), -1);
                    
                }
                else if (npc.Center.X < player.Center.X)
                {
                    return new Vector2(player.Center.X + Main.rand.Next(minDistSelectFromPlayer, maxDistanceSelectFromPlayer + 1), 1);
                }
            }
            return new Vector2(player.Center.X, 1);


        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WritePackedVector2(runTo);
            writer.Write(attackTimer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            runTo = reader.ReadPackedVector2();
            attackTimer = reader.ReadInt32();
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {

            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * .6f);

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            
                return 0f;

        }
        
        


       
    }
}
*/