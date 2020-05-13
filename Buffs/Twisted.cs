using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
	public class Twisted : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Twisted");
			Description.SetDefault("14% increased morph critical strike chance");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<ShapeShifterPlayer>().morphCrit += 14;
		}
	}
}