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

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.HeldItem.scale = 2;
            player.GetModPlayer<BigSword>().Enlarger += 1f;

        }



    }
    public class BigSword : ModPlayer
    {
        public float size = 1f;
        public float oldSize = 1f;
        public float Enlarger = 0;

        Item previousItem = new Item();
        public override void ResetEffects()
        {
            size = 1f;
            Enlarger = 0;
        }

        public override void PreUpdate()
        {

            previousItem.scale /= oldSize;
            previousItem = new Item();
        }
        public override bool PreItemCheck()
        {
            if (!player.HeldItem.IsAir)
            {
                if ((player.HeldItem.useStyle == ItemUseStyleID.SwingThrow || player.HeldItem.useStyle == ItemUseStyleID.Stabbing || player.HeldItem.useStyle == 101) && player.HeldItem.melee && player.HeldItem.pick == 0 && player.HeldItem.hammer == 0 && player.HeldItem.axe == 0)
                {
                    size += Enlarger;
                    player.HeldItem.scale = player.HeldItem.GetGlobalItem<EnalargeItem>().defaultScale * player.GetModPlayer<BigSword>().size;
                }
            }
            return base.PreItemCheck();
        }
    }
    public class EnalargeItem : GlobalItem
    {
        public float defaultScale = 1f;
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }
        public override void SetDefaults(Item item)
        {
            defaultScale = item.scale;
        }
        public override void PostReforge(Item item)
        {
            defaultScale = item.scale;
        }
        
    }

}

