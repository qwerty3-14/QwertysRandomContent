using Terraria;
using Terraria.ModLoader;
using QwertysRandomContent.NPCs;

namespace QwertysRandomContent.Buffs
{
	public class LuneCurse : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Moon Cooldown");
			Description.SetDefault("Can't shoot another moon yet!");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

        public override void Update(NPC npc, ref int buffIndex)
        {
            Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("LuneDust"));
        }


    }
}