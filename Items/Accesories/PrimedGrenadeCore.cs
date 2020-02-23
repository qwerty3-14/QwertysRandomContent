using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class PrimedGrenadeCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grenade Core");
            Tooltip.SetDefault("Thrown grenades have a larger explosion");
        }
        public override void SetDefaults()
        {
            item.value = 10000;
            item.rare = 1;
            item.width = 20;
            item.height = 18;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<QwertyPlayer>().GrenadeExplosionModifier += .75f;
        }
    }
}
