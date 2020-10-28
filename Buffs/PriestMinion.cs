using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class PriestMinion : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Priest Minion");
            Description.SetDefault("Higher beings fight for you!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[mod.ProjectileType("PriestMinion")] > 0)
            {
                modPlayer.PriestMinion = true;
            }
            if (!modPlayer.PriestMinion)
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
