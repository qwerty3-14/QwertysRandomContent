using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class SpaceFighter : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Space fighter");
            Description.SetDefault("Breaking news! You're in SPACE!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[mod.ProjectileType("SpaceFighter")] > 0)
            {
                modPlayer.SpaceFighter = true;
            }
            if (!modPlayer.SpaceFighter)
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