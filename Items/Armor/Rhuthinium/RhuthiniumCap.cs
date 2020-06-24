using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Rhuthinium
{
    [AutoloadEquip(EquipType.Head)]
    public class RhuthiniumCap : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Cap");
            Tooltip.SetDefault("10% increased ranged firing speed \n10% chance not to consume ammo");
            if (ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium && !Main.dedServ)
            {
                Main.itemTexture[item.type] = mod.GetTexture("Items/Armor/Rhuthinium/RhuthiniumCap_Old");
                Main.armorHeadTexture[item.headSlot] = mod.GetTexture("Items/Armor/Rhuthinium/RhuthiniumCap_Head_Old");
            }
        }

        public override void SetDefaults()
        {
            item.value = 50000;
            item.rare = 3;

            item.width = 26;
            item.height = 16;
            item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<AttackSpeedPlayer>().rangedSpeedBonus += .1f;
            player.GetModPlayer<QwertyPlayer>().ammoReduction *= .9f;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("RhuthiniumChestplate") && legs.type == mod.ItemType("RhuthiniumGreaves");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<RhuthiniumArmorEfffects>().rangedSet = true;
            player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.RCapSet");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 12);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}