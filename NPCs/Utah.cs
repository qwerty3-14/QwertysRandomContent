using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using QwertysRandomContent;
namespace QwertysRandomContent.NPCs
{
	public class Utah : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Utah");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 80;
			npc.height = 48;
            if (NPC.downedMoonlord)
            {
                npc.damage = 80;
                npc.defense = 30;
                npc.lifeMax = 1000;
            }
            else
            {
                npc.damage = 60;
                npc.defense = 18;
                npc.lifeMax = 340;
            }
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 60f;
			npc.knockBackResist = 0.5f;
			npc.aiStyle = 26;
			//aiType = 86;
			//animationType = 3;
			npc.buffImmune[BuffID.Confused] = false;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/OldDinosNewGuns");
            banner = npc.type;
            bannerItem = mod.ItemType("UtahBanner");
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            
            if (QwertyWorld.DinoEvent)
			{
				return 45f;
			}
			else
			{
				return 0f;
			}
			
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			
			for (int i = 0; i < 10; i++)
			{
				int dustType = mod.DustType("DinoSkin");
				int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
				Dust dust = Main.dust[dustIndex];
				dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
				dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
				dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
			}
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
				npc.TargetClosest(true);
				
				
		}
		
		public override void FindFrame(int frameHeight)
		{
			// This makes the sprite flip horizontally in conjunction with the npc.direction.
			npc.spriteDirection = npc.direction;
			npc.frameCounter++;
				if (npc.frameCounter < 10)
				{
					npc.frame.Y = 0 * frameHeight;
				}
				else if (npc.frameCounter < 20)
				{
					npc.frame.Y = 1 * frameHeight;
				}
				else if (npc.frameCounter < 30)
				{
					npc.frame.Y = 2 * frameHeight;
				}
				else if (npc.frameCounter < 40)
				{
					npc.frame.Y = 3 * frameHeight;
				}
				else
				{
					npc.frameCounter = 0;
				}
		}
		public override void NPCLoot()
		{
            QwertyWorld.DinoKillCount += 1;
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





            
		}
	}
}
