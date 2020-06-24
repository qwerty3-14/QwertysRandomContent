using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Glass
{
    [AutoloadEquip(EquipType.Body)]
    public class GlassAbsorber : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glass Absorber");
            Tooltip.SetDefault("12% chance not to consume ammo\n12% reduced mana usage");
        }

        public override void SetDefaults()
        {
            item.value = 10000;
            item.rare = 1;

            item.width = 22;
            item.height = 12;
            item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<QwertyPlayer>().ammoReduction *= .88f;
            player.manaCost *= .88f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Glass, 45);
            recipe.AddRecipeGroup("QwertysrandomContent:SilverBar", 8);
            recipe.AddTile(TileID.GlassKiln);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
        }
    }

    public class AbsorberDraw : ModPlayer
    {
        public static readonly PlayerLayer GlassBack = LayerDrawing.DrawOnBody("GlassAbsorber", "Items/Armor/Glass/GlassAbsorber_Body_Glass", name: "GlassBack", glowmask: false, useShader: 3);

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int capeLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("BackAcc"));
            if (capeLayer != -1)
            {
                GlassBack.visible = true;
                layers.Insert(capeLayer + 1, GlassBack);
            }
        }
    }
}