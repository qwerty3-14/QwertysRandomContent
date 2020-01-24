using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class AncientMinion : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ancient Minion");
            Description.SetDefault("The Ancient Minion will fight for you!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (player.ownedProjectileCounts[mod.ProjectileType("AncientMinionFreindly")] > 0 || player.ownedProjectileCounts[mod.ProjectileType("RunicMinionFreindly")] > 0)
            {
                modPlayer.AncientMinion = true;
            }
            if (!modPlayer.AncientMinion)
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