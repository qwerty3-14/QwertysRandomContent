using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class OrichalcumDrifter : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Orichalcum Drifter");
            Description.SetDefault("The Orichalcum Drifter will fight for you!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (player.ownedProjectileCounts[mod.ProjectileType("OrichalcumDrifter")] > 0)
            {
                modPlayer.OrichalcumDrifter = true;
            }
            if (!modPlayer.OrichalcumDrifter)
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