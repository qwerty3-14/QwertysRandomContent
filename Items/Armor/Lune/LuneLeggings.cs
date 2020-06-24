using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Lune
{
    [AutoloadEquip(EquipType.Legs)]
    public class LuneLeggings : ModItem
    {
        public override bool Autoload(ref string name)
        {
            return true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Leggings");
            Tooltip.SetDefault("10% increased movement speed" + "\n8% increased ranged crit");
        }

        public override void SetDefaults()
        {
            item.value = 25000;
            item.rare = 1;

            item.width = 22;
            item.height = 18;
            item.defense = 4;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("LuneBar"), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedCrit += 8;
            player.moveSpeed += .1f;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = mod.GetEquipSlot("LuneLeggings", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("LuneLeggings_Female", EquipType.Legs);
        }
    }
}