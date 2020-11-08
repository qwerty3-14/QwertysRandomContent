using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.DukeFishron
{
    public class DukeDrop : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if(!Main.expertMode)
            {
                string itemName = "";
                switch(Main.rand.Next(3))
                {
                    case 0:
                        itemName = "Cyclone";
                        break;
                    case 1:
                        itemName = "Whirlpool";
                        break;
                    case 2:
                        itemName = "BubbleBrewerBaton";
                        break;
                    case 3:
                        //Planning to add a 4th weapon here
                        break;
                }
                Item.NewItem(npc.getRect(), mod.ItemType(itemName));
            }
        }
    }
    public class BloomingBag : GlobalItem
    {
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag" && arg == ItemID.FishronBossBag )
            {
                string itemName = "";
                switch (Main.rand.Next(3))
                {
                    case 0:
                        itemName = "Cyclone";
                        break;
                    case 1:
                        itemName = "Whirlpool";
                        break;
                    case 2:
                        itemName = "BubbleBrewerBaton";
                        break;
                    case 3:
                        //Planning to add a 4th weapon here
                        break;
                }
                player.QuickSpawnItem(mod.ItemType(itemName));
            }
        }
    }
}
