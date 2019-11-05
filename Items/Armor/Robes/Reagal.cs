using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Robes
{
	[AutoloadEquip(EquipType.Body)]

	class Reagal : ModItem
	{
		
		public override bool Autoload(ref string name)
        {
		// All code below runs only if we're not loading on a server
			if (!Main.dedServ)
			{
				// Add certain equip textures
			mod.AddEquipTexture(new ReagalLegs(), null, EquipType.Legs, "Reagal_Legs", "QwertysRandomContent/Items/Armor/Robes/Reagal_Legs");
			mod.AddEquipTexture(new ReagalLegsFemale(), null, EquipType.Legs, "Reagal_FemaleLegs", "QwertysRandomContent/Items/Armor/Robes/Reagal_FemaleLegs");
			}
			return true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unfinished, mostly did this as an experiment");
			
			
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 30;
			item.rare = 1;
			item.vanity = true;
		}
		
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			
			if (male) equipSlot = mod.GetEquipSlot("Reagal_Legs", EquipType.Legs);
			if (!male) equipSlot = mod.GetEquipSlot("Reagal_FemaleLegs", EquipType.Legs);
            
		}
		
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
			drawArms = true;
		}
        
		
        
		
		
		
	}
	
	public class ReagalLegs : EquipTexture
    {	
    }
	
	public class ReagalLegsFemale : EquipTexture
    {
        public override bool DrawLegs()
        {
            return false;
        }
    }
    public class ReagalDraw : ModPlayer
    {


        public static readonly PlayerLayer HeelLegs = new PlayerLayer("QwertysRandomContent", "HeelLegs", PlayerLayer.Skin, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            if ((drawPlayer.legs == mod.GetEquipSlot("DuelistPants_FemaleLegs", EquipType.Legs) && !drawPlayer.Male) || (drawPlayer.body == mod.GetEquipSlot("Reagal", EquipType.Body) && !drawPlayer.Male) || (drawPlayer.legs == mod.GetEquipSlot("ConduitRobes_Female", EquipType.Legs) && !drawPlayer.Male) || (drawPlayer.shoe == mod.GetEquipSlot("HighHeels", EquipType.Shoes)) || (drawPlayer.legs == mod.GetEquipSlot("TwistedDarkLegs_FemaleLegs", EquipType.Legs)))
            {
                Texture2D texture = mod.GetTexture("Items/Armor/Robes/HeelLegs");
                Vector2 Position = drawInfo.position;
                Position.Y += 14;
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                DrawData value = new DrawData(texture, pos, new Microsoft.Xna.Framework.Rectangle?(drawPlayer.legFrame), drawInfo.legColor, drawPlayer.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0);

                Main.playerDrawData.Add(value);

            }
        });
        public static readonly PlayerLayer ReagalLegsGold = new PlayerLayer("QwertysRandomContent", "ReagalLegsGold", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            if (drawPlayer.body == mod.GetEquipSlot("Reagal", EquipType.Body))
            {
                Texture2D texture = mod.GetTexture("Items/Armor/Robes/Reagal_Legs_Gold");
                if (!drawPlayer.Male)
                {
                     texture = mod.GetTexture("Items/Armor/Robes/Reagal_FemaleLegs_Gold");
                }
                Vector2 Position = drawInfo.position;
                Position.Y += 14;
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                DrawData value = new DrawData(texture, pos, new Microsoft.Xna.Framework.Rectangle?(drawPlayer.legFrame), color12, drawPlayer.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = (int)drawPlayer.dye[2].dye;
                Main.playerDrawData.Add(value);

            }
        });
        public static readonly PlayerLayer ReagalBodyGold = new PlayerLayer("QwertysRandomContent", "ReagalBodyGold", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            if (drawPlayer.body == mod.GetEquipSlot("Reagal", EquipType.Body))
            {
                Texture2D texture = mod.GetTexture("Items/Armor/Robes/Reagal_Body_Gold");
                if (!drawPlayer.Male)
                {
                     texture = mod.GetTexture("Items/Armor/Robes/Reagal_FemaleBody_Gold");
                }
                DrawData value = new DrawData(texture, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = (int)drawPlayer.dye[2].dye;
                Main.playerDrawData.Add(value);

            }
        });
        public static readonly PlayerLayer ReagalCape = new PlayerLayer("QwertysRandomContent", "ReagalCape", PlayerLayer.BackAcc, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            //Main.NewText(drawPlayer.wings);
            if (drawPlayer.body == mod.GetEquipSlot("Reagal", EquipType.Body) && drawPlayer.back ==-1 && drawPlayer.wings ==0)
            {
                Texture2D texture = mod.GetTexture("Items/Armor/Robes/ReagalCape");
                if(!drawPlayer.Male)
                {
                    texture = mod.GetTexture("Items/Armor/Robes/ReagalCape_Female");
                }
                DrawData value = new DrawData(texture, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.legFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = drawInfo.bodyArmorShader;
                Main.playerDrawData.Add(value);

            }
        });
        public static readonly PlayerLayer ReagalArmsGold = new PlayerLayer("QwertysRandomContent", "ReagalArmsGold", PlayerLayer.Arms, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            if (drawPlayer.body == mod.GetEquipSlot("Reagal", EquipType.Body))
            {
                Texture2D texture = mod.GetTexture("Items/Armor/Robes/Reagal_Arms_Gold");
                DrawData value = new DrawData(texture, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = (int)drawPlayer.dye[2].dye;
                Main.playerDrawData.Add(value);

            }
        });
        public static readonly PlayerLayer ReagalGloveF = new PlayerLayer("QwertysRandomContent", "ReagalGloveF", PlayerLayer.HandOnAcc, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            if (drawPlayer.body == mod.GetEquipSlot("Reagal", EquipType.Body))
            {
                Texture2D texture = mod.GetTexture("Items/Armor/Robes/ReagalGloveF");
                if(!drawPlayer.Male)
                {
                    texture = mod.GetTexture("Items/Armor/Robes/ReagalGloveF_Female");
                }
                DrawData value = new DrawData(texture, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = drawInfo.bodyArmorShader;
                Main.playerDrawData.Add(value);

            }
        });
        
        public static readonly PlayerLayer ReagalGloveB = new PlayerLayer("QwertysRandomContent", "ReagalGloveB", PlayerLayer.HandOffAcc, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            if (drawPlayer.body == mod.GetEquipSlot("Reagal", EquipType.Body))
            {
                Texture2D texture = mod.GetTexture("Items/Armor/Robes/ReagalGloveB");
                if (!drawPlayer.Male)
                {
                    texture = mod.GetTexture("Items/Armor/Robes/ReagalGloveB_Female");
                }
                DrawData value = new DrawData(texture, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = drawInfo.bodyArmorShader;
                Main.playerDrawData.Add(value);

            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            
            int armLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Arms"));
            if (armLayer != -1)
            {
                ReagalArmsGold.visible = true;
                layers.Insert(armLayer + 1, ReagalArmsGold);
            }
            int bodyLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Body"));
            if (bodyLayer != -1)
            {
                ReagalBodyGold.visible = true;
                layers.Insert(bodyLayer + 1, ReagalBodyGold);
            }
            int legLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Skin"));
            if (legLayer != -1)
            {
                HeelLegs.visible = true;
                layers.Insert(legLayer + 1, HeelLegs);
                
            }
            legLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Legs"));
            if (legLayer != -1)
            {
                ReagalLegsGold.visible = true;
                layers.Insert(legLayer + 1, ReagalLegsGold);
            }
            int handOnLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("HandOnAcc"));
            if (handOnLayer != -1)
            {
                ReagalGloveF.visible = true;
                layers.Insert(handOnLayer + 1, ReagalGloveF);
            }
            int handOffLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("HandOffAcc"));
            if (handOffLayer != -1)
            {
                ReagalGloveB.visible = true;
                layers.Insert(handOffLayer + 1, ReagalGloveB);
            }
            int capeLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("BackAcc"));
            if (capeLayer != -1)
            {
                ReagalCape.visible = true;
                layers.Insert(capeLayer + 1, ReagalCape);
            }
        }
    }

}
