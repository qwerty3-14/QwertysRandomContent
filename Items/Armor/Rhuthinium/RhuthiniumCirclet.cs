using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Rhuthinium
{
    [AutoloadEquip(EquipType.Head)]
    public class RhuthiniumCirclet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Circlet");
            Tooltip.SetDefault("+10% magic damage and crit chance" + "\n+10% melee damage" + "\nMana regen is disabled and -160 max mana, but melee attacks restore mana");

        }


        public override void SetDefaults()
        {

            item.value = 50000;
            item.rare = 3;


            item.width = 22;
            item.height = 14;
            item.defense = 2;



        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += .10f;
            player.meleeDamage += .10f;
            player.magicCrit += 10;
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            modPlayer.siphon = true;
            modPlayer.meleeSiphon = true;
            player.manaRegenBonus = -999;
            player.statManaMax2 += -160;

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

            player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.RCircletSet");
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            modPlayer.circletSetBonus = true;
            modPlayer.meleeCircletSetBonus = true;





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

