using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.FortressBoss
{
    [AutoloadEquip(EquipType.Head)]
    public class DivineLightMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Divine Light Mask");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.value = 0;
            item.rare = 1;

            item.vanity = true;
            item.width = 20;
            item.height = 20;
        }

        public override bool DrawHead()
        {
            return false;
        }
    }
}