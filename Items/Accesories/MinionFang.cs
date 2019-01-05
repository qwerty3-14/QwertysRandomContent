using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    class MinionFang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Minion Fang");
            Tooltip.SetDefault("Makes minion attacks trigger melee effects" + "\nFor example weapon imbunes or beetle scalemail set bonus" + "\nTHIS DOES NOT MEAN THEY GET BOOSTED BY MELEE DAMAGE!");

        }
        public override void SetDefaults()
        {

            item.value = 30000;
            item.rare = 3;


            item.width = 30;
            item.height = 36;

            item.accessory = true;



        }
        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            modPlayer.minionFang = true;
        }
    }
    class FangEffect : GlobalProjectile
    {
        
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            if(modPlayer.minionFang && projectile.minion)
            {
                projectile.melee = true;
            }
            else if(projectile.minion)
            {
                projectile.melee = false;
            }
        }



    }
}
