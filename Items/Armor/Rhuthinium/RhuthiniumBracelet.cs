using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Rhuthinium
{
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
    public class RhuthiniumBracelet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Bracelet");
            Tooltip.SetDefault("8% increased attack speed \nAnd you thoguht this was useless?");
            if (ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium && !Main.dedServ)
            {
                Main.itemTexture[item.type] = mod.GetTexture("Items/Armor/Rhuthinium/RhuthiniumBracelet_Old");
                Main.accHandsOnTexture[item.handOnSlot] = mod.GetTexture("Items/Armor/Rhuthinium/RhuthiniumBracelet_HandsOn_Old");
                Main.accHandsOffTexture[item.handOffSlot] = mod.GetTexture("Items/Armor/Rhuthinium/RhuthiniumBracelet_HandsOff_Old");
            }
        }

        public override void SetDefaults()
        {
            item.value = 10000;
            item.rare = 3;

            item.width = 28;
            item.height = 22;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AttackSpeedPlayer>().allSpeed += .08f;
            base.UpdateAccessory(player, hideVisual);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}