using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Duelist
{
    [AutoloadEquip(EquipType.Body)]
    public class DuelistShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Duelist Shirt");
            Tooltip.SetDefault("Attacking the same enemy continually with melee attaks reduces damage recieved from that enemy\n7% increased morph and melee crit chance");
        }

        public override void SetDefaults()
        {
            item.value = 50000;
            item.rare = 1;

            item.width = 26;
            item.height = 18;
            item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<DuelistEffects>().body = true;
            player.meleeCrit += 7;
            player.GetModPlayer<ShapeShifterPlayer>().morphCrit += 7;
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
            drawArms = true;
        }
    }

    public class DuelistShirtEffects : ModPlayer
    {
        public static readonly PlayerLayer frontGlove = LayerDrawing.DrawOnFrontHand("DuelistShirt", "Items/Armor/Duelist/FrontGlove", "Items/Armor/Duelist/FrontGlove_Female", glowmask: false, useShader: 2);
        public static readonly PlayerLayer backGlove = LayerDrawing.DrawOnFrontHand("DuelistShirt", "Items/Armor/Duelist/BackGlove", "Items/Armor/Duelist/BackGlove_Female", glowmask: false, useShader: 2);
        public static readonly PlayerLayer BodyExtra = LayerDrawing.DrawOnBody("DuelistShirt", "Items/Armor/Duelist/DuelistShirtExtra_Female", glowmask: false, useShader: 2);

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int bodyLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Body"));
            if (bodyLayer != -1 && !player.Male)
            {
                BodyExtra.visible = true;
                layers.Insert(bodyLayer + 1, BodyExtra);
            }
            int handOnLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("HandOnAcc"));
            if (handOnLayer != -1)
            {
                frontGlove.visible = true;
                layers.Insert(handOnLayer + 1, frontGlove);
            }
            int handOffLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("HandOffAcc"));
            if (handOffLayer != -1)
            {
                backGlove.visible = true;
                layers.Insert(handOffLayer + 1, backGlove);
            }
        }
    }
}