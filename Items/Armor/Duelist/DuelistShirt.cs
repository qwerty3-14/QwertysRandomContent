using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Duelist
{
	[AutoloadEquip(EquipType.Body)]
	public class DuelistShirt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duelist Shirt");
			Tooltip.SetDefault("Attacking the same enemy continually with melee attaks reduces damage recieved from that enemy\nResets when you switch targets\n7% increased morph and melee crit chance");
			
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
			drawHands=true;
            drawArms = true;
			
		}
		
		

		
			
	}
    public class DuelistShirtEffects : ModPlayer
    {
        public static readonly PlayerLayer frontGlove = new PlayerLayer("QwertysRandomContent", "ReagalGloveF", PlayerLayer.HandOnAcc, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);

            if (drawPlayer.body == mod.GetEquipSlot("DuelistShirt", EquipType.Body))
            {
                Texture2D texture = mod.GetTexture("Items/Armor/Duelist/FrontGlove");
                if (!drawPlayer.Male)
                {
                    texture = mod.GetTexture("Items/Armor/Duelist/FrontGlove_Female");
                }
                DrawData value = new DrawData(texture, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = drawInfo.legArmorShader;
                Main.playerDrawData.Add(value);

            }
        });

        public static readonly PlayerLayer backGlove = new PlayerLayer("QwertysRandomContent", "ReagalGloveB", PlayerLayer.HandOffAcc, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
            if (drawPlayer.body == mod.GetEquipSlot("DuelistShirt", EquipType.Body))
            {
                Texture2D texture = mod.GetTexture("Items/Armor/Duelist/BackGlove");
                if(!drawPlayer.Male)
                {
                    texture = mod.GetTexture("Items/Armor/Duelist/BackGlove_Female");
                }
                DrawData value = new DrawData(texture, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = drawInfo.legArmorShader;
                Main.playerDrawData.Add(value);

            }
        });
        public static readonly PlayerLayer BodyExtra = new PlayerLayer("QwertysRandomContent", "Body", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), drawInfo.shadow);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
            if (drawPlayer.body == mod.GetEquipSlot("DuelistShirt", EquipType.Body) && !drawPlayer.Male)
            {
               
                //Main.NewText(drawPlayer.bodyFrame);
                
                    Texture2D texture = mod.GetTexture("Items/Armor/Duelist/DuelistShirtExtra_Female");
                DrawData value = new DrawData(texture, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = (int)drawPlayer.dye[2].dye;
                Main.playerDrawData.Add(value);


                int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                Vector2 Position = drawInfo.position;
                Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                
                DrawData data = new DrawData(texture, pos, drawPlayer.bodyFrame, color12, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                data.shader = drawInfo.legArmorShader;
                Main.playerDrawData.Add(data);
            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {

            int bodyLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Body"));
            if (bodyLayer != -1)
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

