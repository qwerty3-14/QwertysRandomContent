using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
	public class ShieldMinion : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Shield Minion");
			Description.SetDefault("You got your own personal Phalax!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
			if (player.ownedProjectileCounts[mod.ProjectileType("ShieldMinion")] > 0)
			{
				modPlayer.ShieldMinion = true;
			}
			if (!modPlayer.ShieldMinion)
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