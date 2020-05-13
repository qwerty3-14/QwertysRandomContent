using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
	public class Withdraw : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Withdraw");
			Description.SetDefault("Must... more... H.Y.D.R.A");
			Main.debuff[Type] = true;
			longerExpertDebuff = false;
		}
	}
}