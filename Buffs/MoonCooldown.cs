using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class MoonCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Moon Cooldown");
            Description.SetDefault("Can't shoot another moon yet!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }




    }
}