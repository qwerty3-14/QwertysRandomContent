using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class RhuthiniumMight : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Berzerk");
            Description.SetDefault("20% increased melee damage and max move speed");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeDamage += .2f;
        }
    }
}