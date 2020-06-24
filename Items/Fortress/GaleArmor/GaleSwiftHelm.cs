using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.GaleArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class GaleSwiftHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gale Swift Helm");
            Tooltip.SetDefault("+6% chance to dodge an attack" + "\n+8% critical strike chance" + "\nGreatly increased damage after dodging");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(0, 0, 75, 0);
            item.rare = 4;
            item.defense = 1;
            //item.vanity = true;
            item.width = 20;
            item.height = 20;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<QwertyPlayer>().dodgeChance += 6;
            player.thrownCrit += 8;
            player.meleeCrit += 8;
            player.rangedCrit += 8;
            player.magicCrit += 8;
            player.GetModPlayer<QwertyPlayer>().dodgeDamageBoost = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 6);
            recipe.AddIngredient(mod.ItemType("FortressHarpyBeak"), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}