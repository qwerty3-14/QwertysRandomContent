using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
	public class EyeBless : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Eye's Knowledge");
			Description.SetDefault("Prevents morph sickness");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			
		}
	}
}