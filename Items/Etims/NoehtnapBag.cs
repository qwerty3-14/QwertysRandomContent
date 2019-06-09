using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Etims
{
    public class NoehtnapBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 48;
            item.height = 32;
            item.rare = 9;
            item.expert = true;
            bossBagNPC = mod.NPCType("CloakedDarkBoss");
        }

       

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {

            string[] loot = QwertysRandomContent.AMLoot.Draw(3);

            
            player.QuickSpawnItem(73, 8);
            player.QuickSpawnItem(mod.ItemType("EyeOfDarkness"));
            player.QuickSpawnItem(mod.ItemType("EtimsMaterial"), 20 + Main.rand.Next(17));
        }
    }
}
