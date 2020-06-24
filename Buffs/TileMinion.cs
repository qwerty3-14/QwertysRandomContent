using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class TileMinion : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Tile Minion");
            Description.SetDefault("Up up down down left right left right...");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[mod.ProjectileType("TileMinion")] > 0)
            {
                modPlayer.TileMinion = true;
            }
            if (!modPlayer.TileMinion)
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