using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class GlassSpike : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Glass Spike");
            Description.SetDefault("Way worse than stepping on legos!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (player.ownedProjectileCounts[mod.ProjectileType("GlassSpike")] > 0)
            {
                modPlayer.GlassSpike = true;
            }
            if (!modPlayer.GlassSpike)
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