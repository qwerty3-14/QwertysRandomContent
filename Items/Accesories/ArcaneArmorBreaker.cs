using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class ArcaneArmorBreaker : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arcane Armor Breaker");
            Tooltip.SetDefault("Magic attacks ignore defense\n5% reduced magic damage");
        }
        public override void SetDefaults()
        {
            item.width = item.height = 32;
            item.accessory = true;
            item.value = Item.sellPrice(gold: 1);
            item.rare = 2;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MagicPierePlayer>().negateArmor = true;
            player.magicDamage -= .05f;
        }
        
    }
    public class ArmorBreakerEffect : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void AI(Projectile projectile)
        {
            if (Main.player[projectile.owner].GetModPlayer<MagicPierePlayer>().negateArmor  && projectile.friendly && projectile.magic)
            {
                projectile.GetGlobalProjectile<QwertyGlobalProjectile>().ignoresArmor = true;
            }
        }
    }
}
