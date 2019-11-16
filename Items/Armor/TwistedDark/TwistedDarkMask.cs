using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.TwistedDark
{
	[AutoloadEquip(EquipType.Head)]
	public class TwistedDarkMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Etimsic Facemask");
			Tooltip.SetDefault("Morph critiacl strike chance increases the longer you're morphed. Max: 30%" + "\n");
			
		}
		

		public override void SetDefaults()
		{

            item.value = 60000;
            item.rare = 3;


            item.width = 22;
			item.height = 14;
			item.defense = 1;
            item.GetGlobalItem<ShapeShifterItem>().equipedMorphDefense = 7;


        }
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("EtimsMaterial"), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        int bonus = 0;
        float b = 0;
        string end = "% morph critical strike chance (not morphed)";
        public override void UpdateEquip(Player player)
        {
            
            
            if (player.GetModPlayer<ShapeShifterPlayer>().morphTime>0)
            {
                b += .125f;
                if (b > 30)
                {
                    b = 30;
                }
                end = "% morph critical strike chance";
            }
            else if (!player.GetModPlayer<ShapeShifterPlayer>().EyeBlessing)
            {
                b -= .125f;
                if (b < 0)
                {
                    b = 0;
                }
                end = "% morph critical strike chance (not morphed)";
            }
            bonus = (int)b;
            
            player.GetModPlayer<ShapeShifterPlayer>().morphCrit += bonus;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

            foreach (TooltipLine line in tooltips) //runs through all tooltip lines
            {
                if (line.mod == "Terraria" && line.Name == "Tooltip1") //this checks if it's the line we're interested in
                {
                    line.text = "Current Bonus: " + bonus + end;//change tooltip
                }

            }
        }
        public override void DrawHair(ref bool  drawHair, ref bool  drawAltHair )
		{
			drawHair=true;
			
		}

			
	}
    public class DrawHelmet : ModPlayer
    {
        public static readonly PlayerLayer Head = LayerDrawing.DrawHeadSimple("TwistedDarkMask", "Items/Armor/TwistedDark/TwistedDarkMask_HeadSimple", glowmask: false);
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Face"));

            if (headLayer != -1)
            {
                Head.visible = true;
                layers.Insert(headLayer + 1, Head);
            }

        }

        public static readonly PlayerHeadLayer MapMask = LayerDrawing.DrawHeadLayer("TwistedDarkMask", "Items/Armor/TwistedDark/TwistedDarkMask_HeadSimple");
        public override void ModifyDrawHeadLayers(List<PlayerHeadLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerHeadLayer => PlayerHeadLayer.Name.Equals("Head"));
            if (headLayer != -1)
            {

                MapMask.visible = true;
                layers.Insert(headLayer + 1, MapMask);
            }
        }
    }

}

