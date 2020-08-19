using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.HydraBoss
{
    public class Hydra : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            npc.width = 560;
            npc.height = 250;
            npc.damage = 0;
            npc.defense = 18;
            npc.boss = true;

            npc.value = 60f;
            npc.knockBackResist = 40;
            npc.aiStyle = -1;

            animationType = -1;
            npc.noGravity = true;
            //npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/BeastOfThreeHeads");
            npc.lifeMax = 12;
            bossBag = mod.ItemType("HydraBag");
            npc.immortal = true;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
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

        public int damage = 30;
        private bool runOnce = true;

        public override bool PreAI()
        {
            Player player = Main.player[npc.target];
            if (npc.ai[3] > 0)
            {
                npc.dontTakeDamage = true;
                npc.velocity = new Vector2(0, -10);
                if ((player.Center - npc.Center).Length() > 1000f)
                {
                    npc.life = 0;
                    npc.checkDead();
                }
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n].type == mod.NPCType("HydraHead") && Main.npc[n].active && Main.npc[n].ai[0] == npc.whoAmI)
                    {
                        Main.npc[n].DeathSound = null;
                    }
                }
            }

            return !(npc.ai[3] > 0);
        }

        public override void AI()
        {
            if (runOnce)
            {
                if (Main.expertMode)
                {
                    for (int p = 0; p < Main.player.Length; p++)
                    {
                        if (Main.player[p].active)
                        {
                            npc.lifeMax += 3;
                        }
                    }
                    npc.life = npc.lifeMax;
                }
                for (int h = 0; h < 3; h++)
                {
                    if (Main.netMode != 1)
                    {
                        NPC.NewNPC((int)npc.Center.X + h, (int)npc.Center.Y, mod.NPCType("HydraHead"), ai0: npc.whoAmI, ai1: h);
                    }
                }
                runOnce = false;
            }

            Player player = Main.player[npc.target];
            if (Main.netMode != 1)
            {
                player = Main.player[npc.target];
                npc.TargetClosest(true);
            }
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0f, 10f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }

            Vector2 target = player.Center;
            Vector2 moveTo = target - npc.Center;

            npc.velocity = (moveTo) * .04f;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter < 5)
            {
                npc.frame.Y = 0 * frameHeight;
            }
            else if (npc.frameCounter < 10)
            {
                npc.frame.Y = 1 * frameHeight;
            }
            else if (npc.frameCounter < 15)
            {
                npc.frame.Y = 2 * frameHeight;
            }
            else if (npc.frameCounter < 20)
            {
                npc.frame.Y = 3 * frameHeight;
            }
            else if (npc.frameCounter < 25)
            {
                npc.frame.Y = 4 * frameHeight;
            }
            else
            {
                npc.frameCounter = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
        }

        public override void NPCLoot()
        {
            if (!QwertyWorld.downedhydra)
            {
                QwertyWorld.downedhydra = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state
                }
            }
            if (Main.netMode != 1)
            {
                int centerX = (int)(npc.position.X + npc.width / 2) / 16;
                int centerY = (int)(npc.position.Y + npc.height / 2) / 16;
                int halfLength = npc.width / 2 / 16 + 1;

                int trophyChance = Main.rand.Next(0, 10);

                if (Main.expertMode)
                {
                    npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("HydraBag"), 1, false);
                }
                else
                {
                    if (Main.rand.Next(20) < 3)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraHook"));
                    }
                    if (Main.rand.Next(20) < 3)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Hydrator"));
                    }
                    string[] spawnThese = QwertysRandomContent.HydraLoot.Draw(2);
                    Item.NewItem(npc.Center, Vector2.Zero, mod.ItemType(spawnThese[0]));
                    Item.NewItem(npc.Center, Vector2.Zero, mod.ItemType(spawnThese[1]));
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraScale"), Main.rand.Next(20, 31));
                }
                if (trophyChance == 1)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HydraTrophy"));
                }
            }
        }
    }
}