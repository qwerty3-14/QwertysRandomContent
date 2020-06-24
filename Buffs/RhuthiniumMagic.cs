using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class RhuthiniumMagic : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Magic");
            Description.SetDefault("10% increased magic damage");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.magicDamage += .1f;
        }
    }
}