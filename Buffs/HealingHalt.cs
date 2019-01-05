using Terraria;
using Terraria.ModLoader;
using QwertysRandomContent.NPCs;
using Terraria.ID;

namespace QwertysRandomContent.Buffs
{
	public class HealingHalt : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Healing Halt");
			Description.SetDefault("Can't regenrate life or get over your potion sickness");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<QwertyPlayer>(mod).noRegen=true;
            if(player.HasBuff(BuffID.PotionSickness))
            {
                player.buffTime[player.FindBuffIndex(BuffID.PotionSickness)]++;
            }
		}

		
	}
}