using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
	public class VarguleMana : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Vargule Mana");
			Description.SetDefault("magic attacks consume no mana");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.manaCost -= 1.0f;
		}
	}
}