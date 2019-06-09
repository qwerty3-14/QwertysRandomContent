using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace QwertysRandomContent
{
    public class QwertyGlobalItem : GlobalItem
    {
        
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            if (item.type == ItemID.Tabi || item.type == ItemID.MasterNinjaGear)
            {
               
                
                
                    player.dash = 0;
               
                
                    
                if (modPlayer.customDashSpeed < 6.9f)
                {
                    modPlayer.customDashSpeed = 6.9f;
                }
            }
            if(player.ammoCost75)
            {
                player.GetModPlayer<QwertyPlayer>(mod).ammoReduction *= .75f;
                player.ammoCost75 = false;
            }
            if (player.ammoCost80)
            {
                player.GetModPlayer<QwertyPlayer>(mod).ammoReduction *= .8f;
                player.ammoCost80 = false;
            }

        }
        public override void UpdateEquip(Item item, Player player)
        {
            if (player.ammoCost75)
            {
                player.GetModPlayer<QwertyPlayer>(mod).ammoReduction *= .75f;
                player.ammoCost75 = false;
            }
            if (player.ammoCost80)
            {
                player.GetModPlayer<QwertyPlayer>(mod).ammoReduction *= .8f;
                player.ammoCost80 = false;
            }
        }
        public override void UpdateArmorSet(Player player, string set)
        {
            if (player.ammoCost75)
            {
                player.GetModPlayer<QwertyPlayer>(mod).ammoReduction *= .75f;
                player.ammoCost75 = false;
            }
            if (player.ammoCost80)
            {
                player.GetModPlayer<QwertyPlayer>(mod).ammoReduction *= .8f;
                player.ammoCost80 = false;
            }
        }
        public override bool CanUseItem(Item item, Player player)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            if (modPlayer.cantUse)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type > ItemID.Count &&item.summon && item.modItem.mod.Name == "QwertysRandomContent")
            {
                foreach (TooltipLine line in tooltips) //runs through all tooltip lines
                {
                    if (line.mod == "Terraria" && line.Name == "BuffTime") //this checks if it's the line we're interested in
                    {
                        //tooltips.Remove(line);
                        line.text = "";
                    }

                }
            }
            
        }
        
    }
}
