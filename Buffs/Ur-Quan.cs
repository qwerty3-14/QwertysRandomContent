using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
	public class UrQuan : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Ur-Quan Dreadnought");
			Description.SetDefault("Submit or die foolish human!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
			if (player.ownedProjectileCounts[mod.ProjectileType("Dreadnought")] > 0)
			{
				modPlayer.Dreadnought = true;
			}
			if (!modPlayer.Dreadnought)
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