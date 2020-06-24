using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class Pierce : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Pierce");
            Description.SetDefault("Increased armor penetration by 12");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.armorPenetration += 12;
        }
    }
}