using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.BladeBossItems
{
    public class SwordsmanBadge : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordsman Badge");
            Tooltip.SetDefault("Swing swords faster when standing still");
        }
        public override void SetDefaults()
        {

            item.rare = 7;
            item.value = Item.sellPrice(0, 10, 0, 0);


            item.width = item.height = 20;

            item.accessory = true;



        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SwordsmanBadgeEffect>().effect = true;
        }


        public class SwordsmanBadgeEffect : ModPlayer
        {
            public bool effect;
            public override void ResetEffects()
            {
                effect = false;

            }
            public override void PostItemCheck()
            {
                if (effect && Math.Abs(player.velocity.X) < 1f && !player.HeldItem.IsAir && (player.HeldItem.useStyle == 1 || player.HeldItem.useStyle == 3 || player.HeldItem.useStyle == 101))
                {
                    if (player.itemAnimation > 0)
                    {
                        player.itemAnimation--;
                    }
                    else
                    {
                        player.itemAnimation = 0;
                    }
                }
            }
            public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
            {

                if(effect)
                {
                    target.immune[player.whoAmI] = player.itemAnimation / 2;
                }
                
            }
        }
    }
}
