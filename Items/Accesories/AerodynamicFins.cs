using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class AerodynamicFins : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aerodynamic Fins");
            Tooltip.SetDefault("Flechettes acceslerate faster, reaching their top speed sooner");
        }
        public override void SetDefaults()
        {
            item.value = 10000;
            item.rare = 1;
            item.width = 38;
            item.height = 34;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<QwertyPlayer>().FlechetteDropAcceleration += 2f;
        }
    }
}
