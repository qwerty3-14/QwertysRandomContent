using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class MorphGem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Morph Gem");
            Tooltip.SetDefault("Reduces the cooldown on quick morphs by 10%");
        }

        public override void SetDefaults()
        {
            item.value = 10000;
            item.rare = 1;
            item.width = 14;
            item.height = 24;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration *= .9f;
        }
    }
}
