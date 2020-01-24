using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace QwertysRandomContent.NPCs.TundraBoss
{
    public class SlidingPenguin : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sliding Penguin");
            //Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 42;
            npc.height = 18;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            //npc.value = 6000f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.lifeMax = 25;
            npc.defense = 4;
            npc.damage = 20;

            npc.noGravity = false;



        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = 30;
            npc.lifeMax = 25;


        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }
        public override void NPCLoot()
        {


        }
        int timer;
        public override void AI()
        {
            npc.velocity.X = 5 * npc.ai[0];
            timer++;
            if (npc.collideX && timer > 5)
            {
                npc.ai[0] *= -1;
                timer = 0;
            }
            npc.spriteDirection = (int)npc.ai[0];
        }
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {

            npc.ai[0] *= -1;
            npc.netUpdate = true;
        }
    }
}
