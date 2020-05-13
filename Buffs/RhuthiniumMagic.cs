using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
	public class RhuthiniumMagic : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Magic");
			Description.SetDefault("400 max mana and +20% magic damage");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statManaMax2 += 800;
			player.magicDamage += .2f;
		}
	}
}