using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;

namespace QwertysRandomContent.Items.Armor.TwistedDark
{
    [AutoloadEquip(EquipType.Body)]
    public class TwistedDarkBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Etimsic Half Plate");
            Tooltip.SetDefault("Emerald Eye affect \nEye's knowledge preserve's the mask and robe bonuses \n10% reduced cooldown on quick morphs");

        }
        public override void SetDefaults()
        {

            item.value = Item.sellPrice(gold: 1);
            item.rare = 3;
            item.value = 120000;
            item.width = 22;
            item.height = 12;
            item.defense = 1;
            item.GetGlobalItem<ShapeShifterItem>().equipedMorphDefense = 5;
            item.GetGlobalItem<ShapeShifterItem>().equipedMorphDefense = 7;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("EtimsMaterial"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {

            return head.type == mod.ItemType("TwistedDarkMask") && legs.type == mod.ItemType("TwistedDarkLegs");

        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+100 max life when morphed \nWith Eye's knowledge this extra health will be filled instantly upon morphing";
            player.GetModPlayer<ShapeShifterPlayer>().TwistedDarkSetBonus = true;
            if(player.GetModPlayer<ShapeShifterPlayer>().morphTime > 0)
            {
                player.statLifeMax2 += 100;
                
            }
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration *= .9f;
            player.GetModPlayer<ShapeShifterPlayer>().EyeEquiped = true;
            if (player.GetModPlayer<ShapeShifterPlayer>().morphTime > 600)
            {
                player.GetModPlayer<ShapeShifterPlayer>().EyeBlessing = true;
            }
        }
        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            
            drawHands = true;

            drawArms = true;

        }
        
    }

}

