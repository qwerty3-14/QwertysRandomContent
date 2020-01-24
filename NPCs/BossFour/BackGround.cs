using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.BossFour
{
    public class BackGround : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.width = 2364;
            npc.height = 1164;
            npc.damage = 0;
            npc.defense = 50;
            //npc.boss = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 1;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;

            animationType = -1;
            npc.noGravity = true;
            npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            //music = MusicID.Boss5;
            npc.lifeMax = 100000;
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
        public NPC b4;
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
            if (Main.netMode != 0)
            {
                Vector2 target = new Vector2(npc.ai[0], npc.ai[1]);
                Vector2 moveTo = new Vector2(target.X, target.Y) - npc.position;


                npc.velocity = (moveTo) * 1f;
            }
            /*
            if (Main.netMode == 1)
            {
                Main.NewText("client: " + npc.whoAmI + ", " + npc.ai[2] + ", " + npc.ai[3]);
            }


            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + npc.whoAmI + ", " + npc.ai[2] + ", " + npc.ai[3]), Color.White);
            }
            */
            /*
            b4 = Main.npc[(int)npc.ai[0]];
            
            npc.position = new Vector2(b4.Center.X - npc.width / 2, b4.Center.Y - npc.height / 2);
            */
        }





    }


}
