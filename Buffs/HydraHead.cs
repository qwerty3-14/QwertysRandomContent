using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
	public class HydraHead : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Hydra Head");
			Description.SetDefault("The Hydra Head will assist your firepower!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
			if (player.ownedProjectileCounts[mod.ProjectileType("MinionHead")] > 0)
			{
				modPlayer.HydraHeadMinion = true;
			}
			if (!modPlayer.HydraHeadMinion)
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