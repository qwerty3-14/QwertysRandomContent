using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class VarguleMight : ModBuff
    {

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Vargule Might");
            Description.SetDefault("+30% melee damage");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {

            player.meleeDamage += .3f;
        }


    }
}