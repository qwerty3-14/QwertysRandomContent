using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.TundraBoss
{
	public class Sleeping : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ZZZZZ...");
			Main.npcFrameCount[npc.type] = 2;
		}

		public override void SetDefaults()
		{
			npc.width = 94;
			npc.height = 70;
			npc.HitSound = SoundID.NPCHit1;
			npc.chaseable = false;
			//npc.value = 6000f;
			npc.knockBackResist = 0f;
			npc.aiStyle = 0;
			npc.lifeMax = 200;
			npc.defense = 0;
			npc.noGravity = false;
		}

		public override bool CheckActive()
		{
			return false;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0f;
		}

		public override void NPCLoot()
		{
			FrozenDen.activeSleeper = false;
			if (Main.netMode == NetmodeID.Server)
				NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
			NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("PolarBear"));
		}

		private int frame;

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;
			if (npc.frameCounter > 10)
			{
				frame++;
				if (frame >= 2)
				{
					frame = 0;
				}
				npc.frameCounter = 0;
			}
			npc.frame.Y = frameHeight * frame;
		}
	}
}