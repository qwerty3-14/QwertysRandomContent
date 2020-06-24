using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class LuneArcher : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Lune Archer");
            Description.SetDefault("Will shoot your enemies in the knee!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[mod.ProjectileType("LuneArcher")] > 0)
            {
                modPlayer.LuneArcher = true;
            }
            if (!modPlayer.LuneArcher)
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