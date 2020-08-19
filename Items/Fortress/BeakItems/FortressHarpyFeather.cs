using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.BeakItems
{
    public class FortressHarpyFeather : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortress Harpy Feather");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.GiantHarpyFeather);
        }
    }
}