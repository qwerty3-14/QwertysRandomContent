using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
	public class MiniTank : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mini Tank");
			Description.SetDefault("Build an army... trust nobody!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
			if (player.ownedProjectileCounts[mod.ProjectileType("MiniTank")] > 0)
			{
				modPlayer.miniTank = true;
			}
			if (!modPlayer.miniTank)
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