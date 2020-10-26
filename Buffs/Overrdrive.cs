using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class Overrdrive : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Overrdrive");
            Description.SetDefault("RWARRR!");
            Main.debuff[Type] = false;
            //longerExpertDebuff = false;
        }
    }
    public class OverrdriveCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Overrdrive Cooldown");
            Description.SetDefault("For your safety and the safety of others we cannot allow you to use overdrive at this moment");
            Main.debuff[Type] = false;
            longerExpertDebuff = false;
        }
    }
}
