using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
    [AutoloadEquip(EquipType.Body)]
    public class HydraScalemail : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Scalemail");
            Tooltip.SetDefault("+0.5 life/sec regen rate" + "\n+1 max minions");
        }

        public override void SetDefaults()
        {
            item.value = 50000;
            item.rare = 5;

            item.width = 30;
            item.height = 20;
            item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.lifeRegen += 1;
            player.maxMinions += 1;
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = false;
            drawHands = false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("HydraScale"), 24);

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class HydraScaleMailGlowmask : ModPlayer
    {
        public static readonly PlayerLayer HydraBody = LayerDrawing.DrawOnBody("HydraScalemail", "Items/HydraItems/HydraScalemail_Body_Glow", "Items/HydraItems/HydraScalemail_FemaleBody_Glow");
        public static readonly PlayerLayer HydraArm = LayerDrawing.DrawOnArms("HydraScalemail", "Items/HydraItems/HydraScalemail_Arms_Glow");

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int bodyLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Body"));
            if (bodyLayer != -1)
            {
                HydraBody.visible = true;
                layers.Insert(bodyLayer + 1, HydraBody);
            }
            int armLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Arms"));
            if (armLayer != -1)
            {
                HydraArm.visible = true;
                layers.Insert(armLayer + 1, HydraArm);
            }
        }
    }
}