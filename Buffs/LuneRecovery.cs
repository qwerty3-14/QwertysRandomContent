using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class LuneRecovery : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Lune Recovery");
            Description.SetDefault("+8 recovery at night");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>();

            modPlayer.recovery += 8;
        }
    }
}