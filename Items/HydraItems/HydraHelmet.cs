using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace QwertysRandomContent.Items.HydraItems
{
	[AutoloadEquip(EquipType.Head)]
	public class HydraHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Helmet");
			Tooltip.SetDefault("Good for summoning and defense!" + "\n+1 life/sec regen" + "\n+10% summon damage");
			
		}
		public override bool Autoload(ref string name)
        {
		// All code below runs only if we're not loading on a server
			if (!Main.dedServ)
			{
				// Add certain equip textures
			mod.AddEquipTexture(new HydraHelmetGlow(), null, EquipType.Head, "HydraHelmet_Glow", "QwertysRandomContent/Items/HydraItems/HydraHelmet_Glow");
			
			}
			return true;
		}

		public override void SetDefaults()
		{
			
			item.value = 50000;
			item.rare = 5;
			
			
			item.width = 28;
			item.height = 22;
			item.defense = 13;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
			
			player.lifeRegen += 2;
			player.minionDamage  += .1f;
			
		}
		public override void DrawHair(ref bool  drawHair, ref bool  drawAltHair )
		{
			drawAltHair=true;
			
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("HydraScalemail") && legs.type == mod.ItemType("HydraLeggings");
			
		}
		
	
		
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Like a hydra, you get more 'heads' the more injured you are." + "\n+1 max minions when below 80% life" + "\n+2 max minions when below 60% life" + "\n+3 max minions when below 40% life" + "\n+4 max minions when below 20% life" + "\n+20 max minions when below 1% life";
			if (((player.statLife*1.0f) / (player.statLifeMax2*1.0f))<.01f)
			{
				player.maxMinions +=20;
			}
			else if (((player.statLife*1.0f) / (player.statLifeMax2*1.0f))<.2f)
			{
				player.maxMinions +=4;
			}
			else if (((player.statLife*1.0f) / (player.statLifeMax2*1.0f))<.4f)
			{
				player.maxMinions +=3;
			}
			else if (((player.statLife*1.0f) / (player.statLifeMax2*1.0f))<.6f)
			{
				player.maxMinions +=2;
			}
			else if (((player.statLife*1.0f) / (player.statLifeMax2*1.0f))<.8f)
			{
				player.maxMinions +=1;
			}
			
			
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("HydraScale"), 12);
			
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
			
	}
	public class HydraHelmetGlow : EquipTexture
    {
        /*
        public override void DrawArmorColor(Player  drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{	
			glowMask = mod.GetEquipSlot("HydraHelmet_Glow", EquipType.Head);
		}
        */
    }
    public class HydraHelmetGlowmask : ModPlayer
    {
       
        
        public static readonly PlayerLayer HydraEye = new PlayerLayer("QwertysRandomContent", "HydraEye", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
            if (drawPlayer.head == mod.GetEquipSlot("HydraHelmet", EquipType.Head))
            {
                //Main.NewText("Helmet!");
                //Main.NewText(drawPlayer.bodyFrame);
                Texture2D texture = mod.GetTexture("Items/HydraItems/HydraHelmet_Glow");
                int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y  - Main.screenPosition.Y);
                Vector2 Position = drawInfo.position;
                Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                //pos.Y -= drawPlayer.mount.PlayerOffset;
                DrawData data = new DrawData(texture, pos, drawPlayer.bodyFrame, Color.White , 0f, origin, 1f, drawInfo.spriteEffects, 0);
                data.shader = drawInfo.headArmorShader;
                Main.playerDrawData.Add(data);
            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Head"));
            if (headLayer != -1)
            {
                HydraEye.visible = true;
                layers.Insert(headLayer +1, HydraEye);
            }
            
        }
    }

}

