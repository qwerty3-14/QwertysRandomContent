using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.AncientMachine
{
    public class AncientMinion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Minion");
            Main.npcFrameCount[npc.type] = 1;
            if (ModContent.GetInstance<SpriteSettings>().ClassicAncient)
            {
                Main.npcTexture[npc.type] = mod.GetTexture("NPCs/AncientMachine/AncientMinion_Old");
            }
        }

        public override void SetDefaults()
        {
            npc.width = 42;
            npc.height = 56;
            npc.damage = 40;
            npc.defense = 8;

            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;

            npc.value = 0f;
            npc.knockBackResist = 0f;
            //npc.aiStyle = 10;
            //aiType = 10;
            npc.aiStyle = -1;
            animationType = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = -1;
            npc.lifeMax = 70;

        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            return 0f;

        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Vector2 pos = npc.Center + QwertyMethods.PolarVector(-16, npc.rotation + (float)Math.PI / 2);
                Gore gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/MiniDebris_1" + (ModContent.GetInstance<SpriteSettings>().ClassicAncient ? "_Old" : "")), 1f)];
                gore.rotation = npc.rotation;

                pos = npc.Center + QwertyMethods.PolarVector(14, npc.rotation + (float)Math.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/MiniDebris_2" + (ModContent.GetInstance<SpriteSettings>().ClassicAncient ? "_Old" : "")), 1f)];
                gore.rotation = npc.rotation;
                for (int i = 0; i < 180; i++)
                {
                    float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                    Dust dust = Dust.NewDustPerfect(npc.Center, mod.DustType("AncientGlow"), QwertyMethods.PolarVector(Main.rand.Next(1, 9), theta));
                    dust.noGravity = true;
                }
            }
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
        public const int minionRingRadius = 50;
        public const int minionRingDustQty = 50;
        const int AI_Timer_Slot = 1;

        public int timer;
        public int Pos = 1;
        public int damage = 30;
        public int switchTime = 300;
        public int moveCount = 0;
        public int fireCount = 0;
        public int attackType = 1;
        public bool charging;
        public NPC parent;
        int waitTime = 120;
        int chargeTime = 120;
        Vector2 moveTo;
        bool justTeleported;
        float chargeSpeed = 12;
        bool runOnce = true;
        public override void AI()
        {
            if (justTeleported)
            {

                justTeleported = false;
            }
            if (runOnce)
            {

                if (Main.netMode != 1)
                {
                    npc.ai[2] = npc.Center.X;
                    npc.ai[3] = npc.Center.Y;
                    npc.netUpdate = true;
                }
                runOnce = false;
            }
            Player player = Main.player[npc.target];
            //parent = Main.npc[(int)npc.ai[0]];
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0f, 10f);
                    if (npc.timeLeft > 1)
                    {
                        npc.timeLeft = 1;
                    }
                    return;
                }
            }
            timer++;
            if (timer > waitTime + chargeTime)
            {
                for (int i = 0; i < minionRingDustQty; i++)
                {
                    float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);

                    Dust dust = Dust.NewDustPerfect(npc.Center + QwertyMethods.PolarVector(minionRingRadius, theta), mod.DustType("AncientGlow"), QwertyMethods.PolarVector(-minionRingRadius / 10, theta));
                    dust.noGravity = true;
                }
                if (Main.netMode != 1)
                {



                    npc.ai[1] = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                    npc.netUpdate = true;
                }
                moveTo = new Vector2(player.Center.X + (float)Math.Cos(npc.ai[1]) * 600, player.Center.Y + (float)Math.Sin(npc.ai[1]) * 350);
                if (Main.netMode != 1)
                {
                    npc.ai[2] = moveTo.X;
                    npc.ai[3] = moveTo.Y;
                    npc.netUpdate = true;
                }
                justTeleported = true;
                timer = 0;
            }
            else if (timer > waitTime)
            {
                charging = true;
            }
            else
            {
                if (timer == 2)
                {
                    Main.PlaySound(SoundID.Item8, npc.position);
                    for (int i = 0; i < minionRingDustQty; i++)
                    {
                        float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                        Dust dust = Dust.NewDustPerfect(npc.Center, mod.DustType("AncientGlow"), QwertyMethods.PolarVector(minionRingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                }
                charging = false;
            }
            if (charging)
            {
                npc.velocity = new Vector2((float)Math.Cos(npc.rotation) * chargeSpeed, (float)Math.Sin(npc.rotation) * chargeSpeed);
            }
            else
            {
                npc.Center = new Vector2(npc.ai[2], npc.ai[3]);
                npc.velocity = new Vector2(0, 0);
                float targetAngle = new Vector2(player.Center.X - npc.Center.X, player.Center.Y - npc.Center.Y).ToRotation();
                npc.rotation = targetAngle;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(Main.npcTexture[npc.type], new Vector2(npc.Center.X - Main.screenPosition.X, npc.Center.Y - Main.screenPosition.Y),
                        npc.frame, drawColor, npc.rotation,
                        new Vector2(npc.width * 0.5f, npc.height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("NPCs/AncientMachine/AncientMinion_Glow" + (ModContent.GetInstance<SpriteSettings>().ClassicAncient ? "_Old" : "")), new Vector2(npc.Center.X - Main.screenPosition.X, npc.Center.Y - Main.screenPosition.Y),
                        npc.frame, Color.White, npc.rotation,
                        new Vector2(npc.width * 0.5f, npc.height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
