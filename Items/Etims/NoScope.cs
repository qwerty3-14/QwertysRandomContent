using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Etims
{
    public class NoScope : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("No scope scope");
            Tooltip.SetDefault("Very breifly gain 50% ranged damage when turning around");
        }

        public override void SetDefaults()
        {
            item.value = 10000;
            item.rare = 1;
            item.width = 38;
            item.height = 34;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<NoScopeEffect>().effect = true;
        }
    }

    public class NoScopeEffect : ModPlayer
    {
        public bool effect = false;
        private int flipTime = 0;
        private int previusDirection;

        public override void ResetEffects()
        {
            effect = false;
        }

        public override void PostUpdateEquips()
        {
            if (flipTime > 0)
            {
                flipTime--;
                if (effect)
                {
                    player.rangedDamage += .5f;
                }
            }
            else
            {
                if (previusDirection != player.direction)
                {
                    flipTime = 20;
                }
                previusDirection = player.direction;
            }
        }
    }
}