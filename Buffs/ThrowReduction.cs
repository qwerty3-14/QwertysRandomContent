using Terraria;
using Terraria.ModLoader;
using QwertysRandomContent.NPCs;
using QwertysRandomContent;

namespace QwertysRandomContent.Buffs
{
	public class ThrowReduction : ModBuff
	{
		
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Throw Reduction");
			Description.SetDefault("40% chance not to consume thrown items");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{

            player.GetModPlayer<QwertyPlayer>().throwReduction *= .6f;
        }

		
	}
}