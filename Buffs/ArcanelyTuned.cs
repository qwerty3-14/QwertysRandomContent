using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class ArcanelyTuned : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Arcanely Tuned");
            Description.SetDefault("If you can read this you're hacking!");
            Main.debuff[Type] = true;

            longerExpertDebuff = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.rand.Next(12) == 0)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("DazzleSparkle"));
            }
        }
    }
}