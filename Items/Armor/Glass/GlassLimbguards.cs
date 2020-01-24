using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Glass
{
    [AutoloadEquip(EquipType.Legs)]
    public class GlassLimbguards : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glass Limbguards");
            Tooltip.SetDefault("Walk right for 12% increased ranged damage\nWalk left for 12% increased magic damage");

        }



        public override void SetDefaults()
        {

            item.value = 10000;
            item.rare = 1;


            item.width = 22;
            item.height = 12;
            item.defense = 4;



        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Glass, 30);
            recipe.AddRecipeGroup("QwertysrandomContent:SilverBar", 6);
            recipe.AddTile(TileID.GlassKiln);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool DrawLegs()
        {
            return false;
        }
        public override void UpdateEquip(Player player)
        {
            if (player.velocity.X > 0)
            {
                player.rangedDamage += .12f;
            }
            else if (player.velocity.X < 0)
            {
                player.magicDamage += .12f;
            }
        }

    }

    public class LimbguardDraw : ModPlayer
    {
        public static readonly PlayerLayer GlassLegs = LayerDrawing.DrawOnLegs("GlassLimbguards", "Items/Armor/Glass/GlassLimbguards_Legs_Glass", name: "GlassLegs", glowmask: false, useShader: 3);
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {

            int legLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Legs"));
            if (legLayer != -1)
            {
                GlassLegs.visible = true;
                layers.Insert(legLayer + 1, GlassLegs);
            }
        }
    }

}

