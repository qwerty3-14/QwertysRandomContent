using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Etims
{
    public class EyeOfDarkness : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of Darkness");
            Tooltip.SetDefault("You are more likely to critically hit in the dark!\nDarkness was the first thing the gods tried to banish... \nThey completely failed.");
        }
        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = 3;
            item.value = 500000;
            item.width = 32;
            item.height = 22;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Color playerLight = Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16);
            int lightValue = playerLight.R + playerLight.G + playerLight.B;
            int critBoost = 0;
            if (lightValue < 300)
            {
                critBoost = (int)(40f * (1 - (lightValue / 300f)));
            }
            player.meleeCrit += critBoost;
            player.magicCrit += critBoost;
            player.rangedCrit += critBoost;
            player.thrownCrit += critBoost;
        }
    }
}
