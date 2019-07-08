using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;

namespace QwertysRandomContent.Items.Armor.Clay
{
    [AutoloadEquip(EquipType.Body)]
    public class ClayPlate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clay Plate");
            Tooltip.SetDefault("Grants knockback immunity when morphed");

        }


        public override void SetDefaults()
        {

            item.value = 30000;
            item.rare = 1;


            item.width = 22;
            item.height = 12;
            item.defense = 2;
            item.GetGlobalItem<ShapeShifterItem>().equipedMorphDefense = 5;


        }

        public override void UpdateEquip(Player player)
        {
            //player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration *= .88f;
            player.GetModPlayer<plateEffect>().effect = true ;
        }




        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = true;
            drawHands = true;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ClayBlock, 30);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }





    }
    public class plateEffect : ModPlayer
    {
        public bool effect = false;
        
        public override void PostUpdateEquips()
        {
            if (effect)
            {
                //Main.NewText(player.GetModPlayer<ShapeShifterPlayer>().morphed);
                if (player.GetModPlayer<ShapeShifterPlayer>().morphed)
                {

                    player.noKnockback = true;
                }
            }
            effect = false;
        }
    }

}

