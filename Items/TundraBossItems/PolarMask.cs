using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.TundraBossItems
{
    [AutoloadEquip(EquipType.Head)]
    public class PolarMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Polar Exterminator Mask");
            Tooltip.SetDefault("");
        }


        public override void SetDefaults()
        {
            item.value = 0;
            item.rare = 1;
            item.vanity = true;
            item.width = 20;
            item.height = 20;
        }

        public override bool DrawHead()
        {
            return false;
        }


    }
}
