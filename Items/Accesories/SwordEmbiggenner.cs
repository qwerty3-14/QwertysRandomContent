using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{


    public class SwordEmbiggenner : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sword Enlarger");
            Tooltip.SetDefault("Greatly increases the size of your sword!" + "\nI know what you're thinking, and no, it doesn't work on body parts");

        }

        public override void SetDefaults()
        {

            item.value = 200000;
            item.rare = 2;


            item.width = 16;
            item.height = 22;

            item.accessory = true;



        }

        public override void UpdateEquip(Player player)
        {
            //player.HeldItem.scale = 2;
            player.GetModPlayer<BigSword>(mod).Enlarger = true;
            
        }



    }
    public class BigSword : ModPlayer
    {
        public float size = 1f;
        public float oldSize = 1f;
        public bool Enlarger;
        Item previousItem = new Item();
        public override void ResetEffects()
        {
            size = 1f;
            Enlarger = false;
        }
        
        public override void PreUpdate()
        {
            
            previousItem.scale /= oldSize;
            previousItem = new Item();
        }
        public override void PostUpdateEquips()
        {
            if (!player.HeldItem.IsAir)
            {
                if (Enlarger && (player.HeldItem.useStyle == ItemUseStyleID.SwingThrow || player.HeldItem.useStyle == ItemUseStyleID.Stabbing || player.HeldItem.useStyle == 101) && player.HeldItem.melee && player.HeldItem.pick == 0 && player.HeldItem.hammer == 0 && player.HeldItem.axe == 0)
                {
                    size += 1f;
                }
                //Main.NewText(player.selectedItem);
                if (player.selectedItem != 58)
                {
                    player.inventory[player.selectedItem].scale *= size;
                    previousItem = player.inventory[player.selectedItem];
                }


                oldSize = size;
            }
           
            
        }
        
        




    }
    

}

