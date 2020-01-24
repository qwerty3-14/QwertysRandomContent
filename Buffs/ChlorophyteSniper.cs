using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class ChlorophyteSniper : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Sniper");
            Description.SetDefault("The Chlorophyte Sniper will fight for you!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (player.ownedProjectileCounts[mod.ProjectileType("ChlorophyteSniper")] > 0)
            {
                modPlayer.chlorophyteSniper = true;
            }
            if (!modPlayer.chlorophyteSniper)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}