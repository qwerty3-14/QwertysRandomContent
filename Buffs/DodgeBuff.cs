using Terraria;
using Terraria.ModLoader;
using QwertysRandomContent.NPCs;
using QwertysRandomContent;

namespace QwertysRandomContent.Buffs
{
	public class DodgeBuff : ModBuff
	{
		
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Dodge");
			Description.SetDefault("10% dodge chance");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{

            player.GetModPlayer<QwertyPlayer>().dodgeChance += 10;
        }

		
	}
}