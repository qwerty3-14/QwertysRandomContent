using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace QwertysRandomContent.Items  
{
    public class RhuthiniumCrate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Crate");
			Tooltip.SetDefault("");
			
		}
        public override void SetDefaults()
        {
            
            item.maxStack = 999;     //This defines the item's max stack
            item.consumable = true;  //Tells the game that this should be used up once opened
            item.width = 34;  //The size in width of the sprite in pixels.
            item.height = 34;    //The size in height of the sprite in pixels.  item.toolTip = "Right click to open";  //The description of the item shown when hovering over the item ingame.
            item.rare = 9; //The color the title of your Weapon when hovering over it ingame  
            
            item.placeStyle = 0;
            item.useAnimation = 10; //How long the item is used for.
            item.useTime = 10;  //How fast the item is used.
            item.useStyle = 1; //The way your item will be used, 1 is the regular sword swing for example
 
 
        }
        public override bool CanRightClick() //this make so you can right click this item
        {
            return true;
        }

        public override void RightClick(Player player)  //this make so when you right click this item, then one of these items will drop
        {



            int RhuthiniumAmount = Main.rand.Next(2, 26);
            if (Main.rand.Next(2) == 0 && NPC.downedPlantBoss)
            {
                player.QuickSpawnItem(mod.ItemType("VarguleBar"), RhuthiniumAmount);
            }
            else
            {
                player.QuickSpawnItem(mod.ItemType("RhuthiniumBar"), RhuthiniumAmount);
            }


            player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(1, 3));
            if (Main.rand.Next(5) == 0 && Main.hardMode)
            {
                player.QuickSpawnItem(mod.ItemType("HydraSummon"));
            }
            if (Main.rand.Next(10) == 0 && Main.hardMode)
            {
                player.QuickSpawnItem(mod.ItemType("SummoningRune"));
            }
            if (Main.rand.Next(15) == 0 && QwertyWorld.downedRuneGhost)
            {
                player.QuickSpawnItem(mod.ItemType("B4Summon"));
            }

            if (Main.rand.Next(2) == 0)
            {
                switch (Main.rand.Next(2))
                {
                    case 0:
                        if (QwertyWorld.downedFortressBoss)
                        {
                            player.QuickSpawnItem(mod.ItemType("CaeliteBar"), Main.rand.Next(4, 9));
                        }
                        return;

                    case 1:
                        player.QuickSpawnItem(mod.ItemType("LuneBar"), Main.rand.Next(4, 9));
                        return;


                }
            }





        }
 
    }
}