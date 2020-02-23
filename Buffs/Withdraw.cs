using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class Withdraw : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Withdraw");
            Description.SetDefault("Must... more... H.Y.D.R.A");
            Main.debuff[Type] = true;
            longerExpertDebuff = false;
        }
    }
}
