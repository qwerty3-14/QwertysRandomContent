using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Vitallum
{
    [AutoloadEquip(EquipType.Body)]
    public class VitallumLifeguard : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vitallum Lifeguard");
            Tooltip.SetDefault("Increases max life by 120 \n20% increased throwing damage \nEvery 10 missing health increases thrown damage by 1% \nGrants regeneration that recoveors 2% of your missing health every second.");
        }
        public override void SetDefaults()
        {
            item.rare = 8;
            item.value = Item.sellPrice(gold: 6);
        }
        public override void UpdateEquip(Player player)
        {

            player.statLifeMax2 += 120;
            player.thrownDamage += .20f;
            player.GetModPlayer<LifeGuardEffects>().effect = true;

        }
        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = true;
            drawHands = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 24);
            recipe.AddIngredient(ItemID.LifeCrystal, 8);
            recipe.AddIngredient(mod.ItemType("VitallumCoreCharged"));
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void OnCraft(Recipe recipe)
        {
            Main.player[item.owner].QuickSpawnItem(mod.ItemType("VitallumCoreUncharged"), 1);
        }
    }
    public class LifeGuardEffects : ModPlayer
    {
        public bool effect = false;
        public override void ResetEffects()
        {
            effect = false;
        }
        public override void PostUpdateEquips()
        {
            if (effect)
            {
                int missingHealth = player.statLifeMax2 - player.statLife;
                player.thrownDamage += (missingHealth / 10) * .01f;
                player.lifeRegen += (missingHealth / 25);
            }
        }
        public static readonly PlayerLayer Body = LayerDrawing.DrawOnBodySimple("VitallumLifeguard", "Items/Armor/Vitallum/VitallumLifeguard_BodySimple", "Items/Armor/Vitallum/VitallumLifeguard_FemaleBodySimple", "VitallumBody", false);
        public static readonly PlayerLayer BodyVien = LayerDrawing.DrawOnBodySimple("VitallumLifeguard", "Items/Armor/Vitallum/VitallumLifeguard_BodySimpleVien", "Items/Armor/Vitallum/VitallumLifeguard_FemaleBodySimpleVien", "VitallumBody", false, 3, 4, true);

        
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int bodyLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Body"));
            if (bodyLayer != -1)
            {
                Body.visible = true;
                layers.Insert(bodyLayer + 1, Body);
                layers.Insert(bodyLayer + 2, BodyVien);
            }

        }
    }
}
