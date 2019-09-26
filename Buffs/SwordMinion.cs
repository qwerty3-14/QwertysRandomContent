using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class SwordMinionBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sword Minion");
            Description.SetDefault("The embodiment of force multiplication");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            if (player.ownedProjectileCounts[mod.ProjectileType("SwordMinion")] > 0)
            {
                modPlayer.SwordMinion = true;
            }
            if (!modPlayer.SwordMinion)
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