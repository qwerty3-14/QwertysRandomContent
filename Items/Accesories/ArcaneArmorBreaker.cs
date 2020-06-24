using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class ArcaneArmorBreaker : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arcane Armor Breaker");
            Tooltip.SetDefault("Magic attacks ignore 15 defense");
        }

        public override void SetDefaults()
        {
            item.width = item.height = 32;
            item.accessory = true;
            item.value = Item.sellPrice(gold: 1);
            item.rare = 2;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MagicPierePlayer>().negateArmor = true;
        }
    }
}