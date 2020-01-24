using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Shaman
{
    [AutoloadEquip(EquipType.Body)]
    public class ShamanBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shaman Warplate");
            Tooltip.SetDefault("3% increased minion damage and melee critical strike chance \n+1 max minions \n14% increased melee speed");

        }
        public override void SetDefaults()
        {

            item.value = Item.sellPrice(gold: 1);
            item.rare = 1;


            item.width = 22;
            item.height = 12;
            item.defense = 6;



        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.JungleSpores, 8);
            recipe.AddIngredient(ItemID.Bone, 30);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void UpdateEquip(Player player)
        {
            player.minionDamage += .03f;
            player.meleeCrit += 3;
            player.maxMinions++;
            player.meleeSpeed += .14f;
        }
        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {

            drawHands = true;

        }

    }

}

