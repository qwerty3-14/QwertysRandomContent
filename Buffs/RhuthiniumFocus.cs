using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class RhuthiniumFocus : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Focus");
            Description.SetDefault("10% increased ranged damage");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.rangedDamage += .1f;
        }
    }
}