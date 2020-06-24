using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.B4Items
{
    public class B4Bag : ModItem
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
            item.width = 60;
            item.height = 34;
            item.rare = 9;
            item.expert = true;
            //bossBagNPC = mod.NPCType("WeakPoint");
        }

        public override int BossBagNPC => mod.NPCType("OLORDv2");

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            int selectWeapon = Main.rand.Next(1, 7);

            if (selectWeapon == 1)
            {
                player.QuickSpawnItem(mod.ItemType("B4Bow"));
            }
            if (selectWeapon == 2)
            {
                player.QuickSpawnItem(mod.ItemType("B4GiantBow"));
            }
            if (selectWeapon == 3)
            {
                player.QuickSpawnItem(mod.ItemType("DreadnoughtStaff"));
            }
            if (selectWeapon == 4)
            {
                player.QuickSpawnItem(mod.ItemType("BlackHoleStaff"));
            }
            if (selectWeapon == 5)
            {
                player.QuickSpawnItem(mod.ItemType("Jabber"));
            }
            if (selectWeapon == 6)
            {
                player.QuickSpawnItem(mod.ItemType("ExplosivePierce"));
            }
            if (Main.rand.Next(100) < 15)
            {
                player.QuickSpawnItem(mod.ItemType("TheDevourer"));
            }
            player.QuickSpawnItem(mod.ItemType("B4ExpertItem"));

            player.QuickSpawnItem(73, 60);
            if (Main.rand.Next(99) < 30)
            {
                int devItemSelect = Main.rand.Next(6);

                if (devItemSelect == 0)
                {
                    player.QuickSpawnItem(mod.ItemType("Toga"));
                }
                if (devItemSelect == 1)
                {
                    player.QuickSpawnItem(mod.ItemType("GodsSmite"));
                }
                if (devItemSelect == 2)
                {
                    player.QuickSpawnItem(mod.ItemType("DragonScaleHelm"));
                    player.QuickSpawnItem(mod.ItemType("DragonScaleGreaves"));
                    player.QuickSpawnItem(mod.ItemType("DragonScaleBreastplate"));
                }
                if (devItemSelect == 3)
                {
                    player.QuickSpawnItem(mod.ItemType("SUSsFishbowl"));
                }
                if (devItemSelect == 4)
                {
                    player.QuickSpawnItem(mod.ItemType("CryonicBolt"));
                }
                if (devItemSelect == 5)
                {
                    player.QuickSpawnItem(mod.ItemType("PugMask"));
                    player.QuickSpawnItem(mod.ItemType("Urizel"));
                    player.QuickSpawnItem(mod.ItemType("WaveOfDeathUrizel"));
                }
            }
        }
    }
}