using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
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
			Tooltip.SetDefault("+1 life/sec regen rate" + "\n+1 max minions");
			
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
			
			player.lifeRegen += 2;
			player.maxMinions +=1;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawArms=false;
			drawHands=false;
			
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


        public static readonly PlayerLayer HydraBody = new PlayerLayer("QwertysRandomContent", "Body", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
            if (drawPlayer.body == mod.GetEquipSlot("HydraScalemail", EquipType.Body))
            {
                //Main.NewText("Helmet!");
                //Main.NewText(drawPlayer.bodyFrame);
                Texture2D texture = mod.GetTexture("Items/HydraItems/HydraScalemail_Body_Glow");
                if (!drawPlayer.Male)
                {
                    texture = mod.GetTexture("Items/HydraItems/HydraScalemail_FemaleBody_Glow");
                }


                int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                Vector2 Position = drawInfo.position;
                Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                //pos.Y -= drawPlayer.mount.PlayerOffset;
                DrawData data = new DrawData(texture, pos, drawPlayer.bodyFrame, Color.White, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                data.shader = drawInfo.bodyArmorShader;
                Main.playerDrawData.Add(data);
            }
        });
        public static readonly PlayerLayer HydraArm = new PlayerLayer("QwertysRandomContent", "HydraArm", PlayerLayer.Arms, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
            if (drawPlayer.body == mod.GetEquipSlot("HydraScalemail", EquipType.Body))
            {
                //Main.NewText("Helmet!");
                //Main.NewText(drawPlayer.bodyFrame);
                Texture2D texture = mod.GetTexture("Items/HydraItems/HydraScalemail_Arms_Glow");
                

                
                int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                Vector2 Position = drawInfo.position;
                Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                //pos.Y -= drawPlayer.mount.PlayerOffset;
                
                DrawData data = new DrawData(texture, pos, drawPlayer.bodyFrame, Color.White, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                data.shader = drawInfo.bodyArmorShader;
                Main.playerDrawData.Add(data);
                
            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int bodyLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Body"));
            if (bodyLayer != -1)
            {
                HydraBody.visible = true;
                layers.Insert(bodyLayer+1, HydraBody);
            }
            int armLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Arms"));
            if (armLayer != -1)
            {
                HydraArm.visible = true;
                layers.Insert(armLayer+1, HydraArm);
            }

        }
    }

}

