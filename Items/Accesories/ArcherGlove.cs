using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class ArcherGlove : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Archer Glove");
            Tooltip.SetDefault("Bows shoot faster");
        }

        public override void SetDefaults()
        {
            item.value = 10000;
            item.rare = 1;

            item.width = 22;
            item.height = 28;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ArcherGloveEffect>().effect = true;
        }
    }

    public class ArcherGloveEffect : ModPlayer
    {
        public bool effect;
        private int frameSkipCounter = 0;

        public override void ResetEffects()
        {
            effect = false;
        }

        public override void PostItemCheck()
        {
            if (effect)
            {
                if (!player.HeldItem.IsAir && player.HeldItem.useAmmo == AmmoID.Arrow)
                {
                    Item bow = player.HeldItem;
                    frameSkipCounter++;
                    if (frameSkipCounter > 5)
                    {
                        frameSkipCounter = 0;
                        if (player.itemAnimation > 0)
                        {
                            player.itemAnimation--;
                        }

                        if (player.attackCD > 0)
                        {
                            player.attackCD--;
                        }
                        if (player.itemTime > 0)
                        {
                            player.itemTime--;
                        }
                    }
                }
            }
        }
    }
}