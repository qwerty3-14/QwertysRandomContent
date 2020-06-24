using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class SpiritCallCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Spirit Call Cooldown");
            Description.SetDefault("War spirits need rest too");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }
    }
}