using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
	public class GoldDagger : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Gold Dagger");
			Description.SetDefault("The Gold Dagger will fight for you!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
			if (player.ownedProjectileCounts[mod.ProjectileType("GoldDagger")] > 0 )
			{
				modPlayer.GoldDagger = true;
			}
			if (!modPlayer.GoldDagger)
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