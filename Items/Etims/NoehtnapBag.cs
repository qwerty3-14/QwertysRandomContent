using QwertysRandomContent.Config;
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

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 48;
            item.height = 32;
            item.rare = 9;
            item.expert = true;
            //bossBagNPC = mod.NPCType("CloakedDarkBoss");
        }

        public override int BossBagNPC => mod.NPCType("CloakedDarkBoss");

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.QuickSpawnItem(73, 8);
            //player.QuickSpawnItem(mod.ItemType("Doppleganger"));

            int number = Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, mod.ItemType("Doppleganger"), 1, false, 0, false, false);
            if (Main.netMode == 1)
            {
                NetMessage.SendData(21, -1, -1, null, number, 1f, 0f, 0f, 0, 0, 0);
            }

            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("EyeOfDarkness"));
            }
            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("NoScope"));
            }
            player.QuickSpawnItem(mod.ItemType("EtimsMaterial"), 20 + Main.rand.Next(17));
        }
    }
}