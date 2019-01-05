using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
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
			
			player.setBonus = "Be like a clay statue and... \n Increased morph damage and morph defense when not moving";
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


        public static readonly PlayerLayer Mask = new PlayerLayer("QwertysRandomContent", "Mask", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
            if (drawPlayer.head == mod.GetEquipSlot("ClayMask", EquipType.Head))
            {
                //Main.NewText("Pug!");
                //Main.NewText(drawPlayer.bodyFrame);
                
                int fHeight = 56;

                Texture2D texture = mod.GetTexture("Items/Armor/Clay/ClayMask_HeadSimple");
                Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
                int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                Vector2 Position = drawInfo.position;

               

                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                if (drawPlayer.bodyFrame.Y == 7 * fHeight || drawPlayer.bodyFrame.Y == 8 * fHeight || drawPlayer.bodyFrame.Y == 9 * fHeight || drawPlayer.bodyFrame.Y == 14 * fHeight || drawPlayer.bodyFrame.Y == 15 * fHeight || drawPlayer.bodyFrame.Y == 16 * fHeight)
                {
                    pos.Y -= 2;

                }
                Rectangle frame = new Rectangle(0,  0, 40, fHeight);
                Vector2 origin = new Vector2((float)frame.Width * 0.5f, (float)frame.Height * 0.5f);
                //pos.Y -= drawPlayer.mount.PlayerOffset;
                DrawData data = new DrawData(texture, pos, frame, color12, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                data.shader = drawInfo.headArmorShader;
                Main.playerDrawData.Add(data);
            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Head"));
            if (headLayer != -1)
            {
                Mask.visible = true;
                layers.Insert(headLayer + 1, Mask);
            }

        }
    }

}

