using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
	public class MythrilPrism : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mythril Prism");
			Description.SetDefault("The Mythril Prism will fight for you!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
			if (player.ownedProjectileCounts[mod.ProjectileType("MythrilPrism")] > 0)
			{
				modPlayer.mythrilPrism = true;
			}
			if (!modPlayer.mythrilPrism)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}