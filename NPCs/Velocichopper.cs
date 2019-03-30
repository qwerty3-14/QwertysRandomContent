using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent;
using System.IO;

namespace QwertysRandomContent.NPCs
{
    public class Velocichopper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Velocichopper");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 286;
            npc.height = 104;
            if (NPC.downedMoonlord)
            {
                npc.damage = 100;
                npc.defense = 30;
                npc.lifeMax = 18000;

            }
            else
            {
                npc.damage = 90;
                npc.defense = 20;
                npc.lifeMax = 4800;
            }
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 6000f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = -1;
            //aiType = 86;
            //animationType = 3;
            npc.buffImmune[BuffID.Confused] = false;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/OldDinosNewGuns");
            banner = npc.type;
            bannerItem = mod.ItemType("VelocichopperBanner");
        }
        public override void HitEffect(int hitDirection, double damage)
        {


        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (QwertyWorld.DinoEvent)
            {
                if (!NPC.AnyNPCs(mod.NPCType<Velocichopper>()) && !NPC.downedMoonlord)
                {
                    return 7f;
                }
                else
                {
                    if (NPC.downedMoonlord)
                    {
                        return 7f;
                    }
                    return 0f;
                }
            }
            else
            {

                return 0f;
            }

        }








        public int Pos = 1;
        public int damage = 40;
        public int reloadTime = 2;
        public int moveCount = 0;
        public int fireCount = 0;
        public int attackType = 1;
        public int AI_Timer = 0;
        public int Reload_Timer = 0;
        public int attackTime = 300;
        public int numberOfShots = 0;
        public int rushDirection = 1;
        public int bombTimer;
        public int bombReload = 30;
        public override void AI()
        {

            if (NPC.downedMoonlord)
            {
                damage = 45;
            }
            AI_Timer++;

            Player player = Main.player[npc.target];
            npc.TargetClosest(true);

            if (AI_Timer > 481)
            {
                bombTimer++;
                npc.direction = rushDirection;
                npc.velocity = new Vector2(10 * rushDirection, 0f);
                if ((npc.Center.X > player.Center.X + 1200 && rushDirection == 1) || (npc.Center.X < player.Center.X - 1200 && rushDirection == -1) && Main.netMode != 1)
                {
                    AI_Timer = 0;
                    npc.netUpdate = true;
                }
                if (bombTimer > bombReload && Main.netMode != 1)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("DinoBomb"), damage, 3f, Main.myPlayer);
                    bombTimer = 0;
                }

            }
            else if (AI_Timer > 480)
            {
                if (npc.Center.X < player.Center.X)
                {
                    rushDirection = 1;

                }
                else
                {
                    rushDirection = -1;

                }
            }
            else if (AI_Timer > 420)
            {
                Vector2 moveTo = new Vector2(player.Center.X - (900f * npc.direction), player.Center.Y + -300f) - npc.Center;
                npc.velocity = (moveTo) * .03f;
            }
            else if (AI_Timer > attackTime)
            {
                npc.velocity = new Vector2(0, 0f);

                Reload_Timer++;
                if (Reload_Timer > reloadTime && Main.netMode != 1)
                {
                    int Xvar = Main.rand.Next(-50, 50);

                    int Yvar = 50 - Xvar;

                    Projectile.NewProjectile(npc.Center.X + (100f * npc.direction), npc.Center.Y, 5.00f * (1 + Xvar * .01f) * npc.direction, 5.00f * (1 + Yvar * .01f), 110, damage, 3f, Main.myPlayer);







                    Reload_Timer = 0;

                }


            }
            else if (AI_Timer > attackTime - 120)
            {
                npc.velocity = new Vector2(0, 0f);
            }
            else
            {
                Vector2 moveTo = new Vector2(player.Center.X - (300f * npc.direction), player.Center.Y + -300f) - npc.Center;
                npc.velocity = (moveTo) * .03f;
            }
            QwertyMethods.ServerClientCheck(AI_Timer);
        }
        
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(AI_Timer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            AI_Timer = reader.ReadInt32();
        }

        public override void FindFrame(int frameHeight)
        {
            // This makes the sprite flip horizontally in conjunction with the npc.direction.
            npc.spriteDirection = npc.direction;
            npc.frameCounter++;
            if (npc.frameCounter < 1)
            {
                npc.frame.Y = 0 * frameHeight;
            }
            else if (npc.frameCounter < 2)
            {
                npc.frame.Y = 1 * frameHeight;
            }
            else if (npc.frameCounter < 3)
            {
                npc.frame.Y = 2 * frameHeight;
            }
            else if (npc.frameCounter < 4)
            {
                npc.frame.Y = 3 * frameHeight;
            }
            if (npc.frameCounter < 5)
            {
                npc.frame.Y = 2 * frameHeight;
            }
            else if (npc.frameCounter < 6)
            {
                npc.frame.Y = 1 * frameHeight;
            }
            else if (npc.frameCounter < 7)
            {
                npc.frame.Y = 0 * frameHeight;
            }

            else
            {
                npc.frameCounter = 0;
            }
        }
        public override void NPCLoot()
        {
            QwertyWorld.DinoKillCount += 5;
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
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DinoVulcan"));
                }
            }
            else
            {
                if (Main.rand.Next(0, 100) <= 10)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DinoVulcan"));
                }
            }






        }

    }
    public class DinoBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DinoBomb");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;

            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
            projectile.tileCollide = true;


        }
        public bool runOnce = true;

        public override void AI()
        {
            projectile.timeLeft = 2;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            projectile.width = 150;
            projectile.height = 150;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            Projectile.NewProjectile(projectile.Center, new Vector2(0, 0), mod.ProjectileType("DinoBombExplosion"), projectile.damage, projectile.knockBack, player.whoAmI);
            Main.PlaySound(SoundID.Item62, projectile.position);
            for (int i = 0; i < 100; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 160; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }
        }

    }
    public class DinoBombExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dino Bomb Explosion");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 150;
            projectile.height = 150;
            projectile.friendly = true;
            projectile.hostile = true;
            projectile.penetrate = -1;

            projectile.tileCollide = false;
            projectile.timeLeft = 2;



        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
    }
}
