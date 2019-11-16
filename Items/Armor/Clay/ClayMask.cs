using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Clay
{
	[AutoloadEquip(EquipType.Head)]
	public class ClayMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clay Mask");
			Tooltip.SetDefault("6% morph critical strike chance");
			
		}
		

		public override void SetDefaults()
		{

            item.value = 30000;
            item.rare = 1;


            item.width = 22;
			item.height = 14;
			item.defense = 1;
            item.GetGlobalItem<ShapeShifterItem>().equipedMorphDefense = 4;


        }
		
		public override void UpdateEquip(Player player)
		{

            player.GetModPlayer<ShapeShifterPlayer>().morphCrit += 6;
        }
		public override void DrawHair(ref bool  drawHair, ref bool  drawAltHair )
		{
			drawHair=true;
			
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("ClayPlate") && legs.type == mod.ItemType("ClayKneecaps");
			
		}
		
	
		
		public override void UpdateArmorSet(Player player)
		{
			
			player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.ClaySet");
			if(player.velocity.Length() <2f)
            {
                player.GetModPlayer<ShapeShifterPlayer>().morphDamage += .18f;
                player.GetModPlayer<ShapeShifterPlayer>().morphDef += 8;
            }
            




        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ClayBlock, 20);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }



       
			
	}
    public class SimpleMask : ModPlayer
    {


        public static readonly PlayerLayer Mask =LayerDrawing.DrawHeadSimple("ClayMask", "Items/Armor/Clay/ClayMask_HeadSimple", glowmask: false); 
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Head"));
            
            if (headLayer != -1)
            {
                Mask.visible = true;
                layers.Insert(headLayer + 1, Mask);
            }

        }
        public static readonly PlayerHeadLayer MapMask = LayerDrawing.DrawHeadLayer("ClayMask", "Items/Armor/Clay/ClayMask_HeadSimple");
        public override void ModifyDrawHeadLayers(List<PlayerHeadLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerHeadLayer => PlayerHeadLayer.Name.Equals("Armor"));
            if (headLayer != -1)
            {
                
                MapMask.visible = true;
                layers.Insert(headLayer + 1, MapMask);
            }
        }
    }

}

