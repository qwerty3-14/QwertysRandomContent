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
            item.rare = 2; //The color the title of your Weapon when hovering over it ingame  

            item.placeStyle = 0;
            item.useAnimation = 10; //How long the item is used for.
            item.useTime = 10;  //How fast the item is used.
            item.useStyle = 1; //The way your item will be used, 1 is the regular sword swing for example


        }
        public override bool CanRightClick() //this make so you can right click this item
        {
            return true;
        }
        bool noReward = true;
        void RewardChance(Player player, int type, float chance, ref float specialReward)
        {
            if (specialReward < chance && specialReward >= 0)
            {
                noReward = false;
                player.QuickSpawnItem(type);

            }
            specialReward -= chance;

        }
        public override void RightClick(Player player)  //this make so when you right click this item, then one of these items will drop
        {
            noReward = true;
            while (noReward)
            {
                float specialReward = Main.rand.NextFloat();
                RewardChance(player, mod.ItemType("Biomass"), .04f, ref specialReward);
                RewardChance(player, mod.ItemType("Gyroscope"), .04f, ref specialReward);
                if(Main.rand.Next(4)==0)
                {
                    noReward = false;
                    PickCratePotions(player, 2 + Main.rand.Next(4));
                }
                if (Main.rand.Next(4) == 0)
                {
                    noReward = false;
                    PickBars(player, 8 + Main.rand.Next(8));
                }
                if(Main.rand.Next(2)==0)
                {
                    noReward = false;
                    if (Main.rand.Next(2) == 0)
                    {
                        player.QuickSpawnItem(ItemID.JourneymanBait, 1 + Main.rand.Next(5));
                    }
                    else
                    {
                        player.QuickSpawnItem(ItemID.MasterBait, 1 + Main.rand.Next(5));
                    }
                }
                if(Main.rand.Next(4)==0)
                {
                    noReward = false;
                    player.QuickSpawnItem(ItemID.GoldCoin, 6 + Main.rand.Next(7));
                }
            }
        }
        public static void PickCratePotions(Player player, int qty)
        {
            switch(Main.rand.Next(6))
            {
                case 0:
                    player.QuickSpawnItem(QwertysRandomContent.Instance.ItemType("DodgePotion"), qty);
                    break;
                case 1:
                    player.QuickSpawnItem(QwertysRandomContent.Instance.ItemType("Luneshine"), qty);
                    break;
                case 2:
                    player.QuickSpawnItem(QwertysRandomContent.Instance.ItemType("RushPotion"), qty);
                    break;
                case 3:
                    player.QuickSpawnItem(QwertysRandomContent.Instance.ItemType("ThrowReductionPotion"), qty);
                    break;
                case 4:
                    player.QuickSpawnItem(QwertysRandomContent.Instance.ItemType("TwistedPotion"), qty);
                    break;
                case 5:
                    player.QuickSpawnItem(QwertysRandomContent.Instance.ItemType("PiercePotion"), qty);
                    break;
            }
        }
        public static void PickBars(Player player, int qty)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    player.QuickSpawnItem(NPC.downedBoss3 ?  QwertysRandomContent.Instance.ItemType("RhuthiniumBar") : ItemID.GoldBar, qty);
                    break;
                case 1:
                    player.QuickSpawnItem(QwertysRandomContent.Instance.ItemType("LuneBar"), qty);
                    break;
                case 2:
                    player.QuickSpawnItem(QwertysRandomContent.Instance.ItemType("CaeliteBar"), qty);
                    break;
            }
        }
    }
    public class FortressCrate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortress Crate");
            Tooltip.SetDefault("");

        }
        public override void SetDefaults()
        {

            item.maxStack = 999;     //This defines the item's max stack
            item.consumable = true;  //Tells the game that this should be used up once opened
            item.width = 34;  //The size in width of the sprite in pixels.
            item.height = 34;    //The size in height of the sprite in pixels.  item.toolTip = "Right click to open";  //The description of the item shown when hovering over the item ingame.
            item.rare = 2; //The color the title of your Weapon when hovering over it ingame  

            item.placeStyle = 0;
            item.useAnimation = 10; //How long the item is used for.
            item.useTime = 10;  //How fast the item is used.
            item.useStyle = 1; //The way your item will be used, 1 is the regular sword swing for example
            item.createTile = mod.TileType("FortressCrate");

        }
        public override bool CanRightClick() //this make so you can right click this item
        {
            return true;
        }
        bool noReward = true;
        void RewardChance(Player player, int type, float chance, ref float specialReward)
        {
            if (specialReward < chance && specialReward >= 0)
            {
                noReward = false;
                player.QuickSpawnItem(type);

            }
            specialReward -= chance;

        }
        public override void RightClick(Player player)  //this make so when you right click this item, then one of these items will drop
        {
            noReward = true;
            while (noReward)
            {
                float specialReward = Main.rand.NextFloat();
                RewardChance(player, ItemID.LuckyHorseshoe, (1f / 9f), ref specialReward);
                RewardChance(player, ItemID.Starfury, (1f / 9f), ref specialReward);
                RewardChance(player, ItemID.ShinyRedBalloon, (1f/9f), ref specialReward);
                if (Main.rand.Next(4) == 0)
                {
                    noReward = false;
                    RhuthiniumCrate.PickCratePotions(player, 2 + Main.rand.Next(4));
                }
                if (Main.rand.Next(4) == 0)
                {
                    noReward = false;
                    RhuthiniumCrate.PickBars(player, 8 + Main.rand.Next(8));
                }
                if (Main.rand.Next(2) == 0)
                {
                    noReward = false;
                    if (Main.rand.Next(2) == 0)
                    {
                        player.QuickSpawnItem(ItemID.JourneymanBait, 1 + Main.rand.Next(5));
                    }
                    else
                    {
                        player.QuickSpawnItem(ItemID.MasterBait, 1 + Main.rand.Next(5));
                    }
                }
                if (Main.rand.Next(4) == 0)
                {
                    noReward = false;
                    player.QuickSpawnItem(ItemID.GoldCoin, 6 + Main.rand.Next(7));
                }
            }
        }
        
    }
}