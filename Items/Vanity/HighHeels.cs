using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Vanity
{
    [AutoloadEquip(EquipType.Shoes)]
    public class HighHeels : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("High Heels");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.value = 1000;
            item.rare = 1;

            item.width = 32;
            item.height = 32;
            item.accessory = true;
            item.vanity = true;
        }
    }

    public class StopLegDraw : GlobalItem
    {
        public override bool DrawLegs(int legs, int shoes)
        {
            return shoes != mod.GetEquipSlot("HighHeels", EquipType.Shoes);
        }
    }
}