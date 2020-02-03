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
            Tooltip.SetDefault("+12% ranged damage" + "\n10% increased move speed");
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
            player.rangedDamage += .12f;
            player.moveSpeed += .1f;
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
            player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.RCapSet");
            if ((player.statLife * 1.0f) / (player.statLifeMax2 * 1.0f) == 1.0f)
            {
                player.rangedCrit = 100;
            }


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

