using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.DevItems.Kerdo
{
    public class Urizel : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Urizel");
            Tooltip.SetDefault("This sword once killed a sleeper /nDev item");

        }
        public override void SetDefaults()
        {
            item.damage = 321;
            item.melee = true;

            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.knockBack = 5;
            item.value = 0;
            item.rare = 10;
            item.UseSound = SoundID.Item1;

            item.width = 68;
            item.height = 68;
           

            item.autoReuse = true;
            
            item.shootSpeed = 9;




        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(BuffID.Electrified, 1200);
        }
    }
}
