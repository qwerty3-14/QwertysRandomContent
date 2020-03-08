using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress
{
    public class EnchantedSwimmer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Swimmer");
        }
        public override void SetDefaults()
        {
            item.value = Item.sellPrice(0, 0, 30, 0);
            item.rare = 1;
            item.maxStack = 999;
        }
    }
}
