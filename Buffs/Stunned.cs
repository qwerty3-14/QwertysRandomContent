using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
	public class Stunned : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Stunned");
			Description.SetDefault("If you can read this you're hacking!");
			Main.debuff[Type] = true;

			longerExpertDebuff = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
		}
	}
}