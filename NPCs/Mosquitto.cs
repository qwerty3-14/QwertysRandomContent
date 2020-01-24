using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs
{
    public class Mosquitto : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mosquitto");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 10;
            npc.height = 12;
            npc.damage = 1;
            npc.defense = 0;
            npc.lifeMax = 1;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = 2f;
            npc.aiStyle = 14;
            aiType = 49;
            animationType = NPCID.Bee;
            npc.npcSlots = 0;


            npc.noGravity = true;
            npc.noTileCollide = false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            return 0f;

        }
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            if (Main.expertMode)
            {
                target.AddBuff(33, 480);
                target.AddBuff(mod.BuffType("DinoPox"), 480);
            }
            else
            {
                target.AddBuff(mod.BuffType("DinoPox"), 480);
            }
        }
        public bool runOnce = true;
        public int timer;
        public override void AI()
        {
            timer++;
            if (runOnce)
            {
                npc.velocity = new Vector2((float)Math.Cos(npc.ai[0]) * 6f * npc.direction, -(float)Math.Sin(npc.ai[0]) * 6f);

                runOnce = false;
            }
            if (timer < 20)
            {
                npc.aiStyle = -1;
            }
            else
            {
                npc.aiStyle = 14;
            }

        }





    }
}
