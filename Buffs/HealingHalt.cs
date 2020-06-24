using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            player.GetModPlayer<QwertyPlayer>().noRegen = true;
            if (player.HasBuff(BuffID.PotionSickness))
            {
                player.buffTime[player.FindBuffIndex(BuffID.PotionSickness)]++;
            }
        }
    }
}