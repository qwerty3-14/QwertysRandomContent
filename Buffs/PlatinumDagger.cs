using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class PlatinumDagger : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Platinum Dagger");
            Description.SetDefault("The Platinum Dagger will fight for you!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[mod.ProjectileType("PlatinumDagger")] > 0)
            {
                modPlayer.PlatinumDagger = true;
            }
            if (!modPlayer.PlatinumDagger)
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