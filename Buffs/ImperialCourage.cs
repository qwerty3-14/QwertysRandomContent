using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class ImperialCourage : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Imperial Courage");
            Description.SetDefault("25% increased attack speed");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<AttackSpeedPlayer>().allSpeed += .25f;
        }
    }
}