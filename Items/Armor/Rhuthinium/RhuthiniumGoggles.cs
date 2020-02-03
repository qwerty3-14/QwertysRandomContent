using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Rhuthinium
{
    [AutoloadEquip(EquipType.Head)]
    public class RhuthiniumGoggles : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Goggles");
            Tooltip.SetDefault("+20% throwing crit chance and velocity");
            if (ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium && !Main.dedServ)
            {
                Main.itemTexture[item.type] = mod.GetTexture("Items/Armor/Rhuthinium/RhuthiniumGoggles_Old");
                Main.armorHeadTexture[item.headSlot] = mod.GetTexture("Items/Armor/Rhuthinium/RhuthiniumGoggles_Head_Old");
            }
        }


        public override void SetDefaults()
        {

            item.value = 50000;
            item.rare = 3;


            item.width = 14;
            item.height = 8;
            item.defense = 2;



        }

        public override void UpdateEquip(Player player)
        {

            player.thrownVelocity += .2f;
            player.thrownCrit += 20;

        }
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("RhuthiniumChestplate") && legs.type == mod.ItemType("RhuthiniumGreaves");

        }



        public override void UpdateArmorSet(Player player)
        {

            player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.RGogglesSet");
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            modPlayer.ninjaSabatoge = true;
            player.thrownDamage += .1f;


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

