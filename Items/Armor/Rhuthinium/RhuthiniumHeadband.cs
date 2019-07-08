using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Rhuthinium
{
	[AutoloadEquip(EquipType.Head)]
	public class RhuthiniumHeadband : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Headband");
			Tooltip.SetDefault("+28% melee speed");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 50000;
			item.rare = 3;
			
			
			item.width = 32;
			item.height = 20;
			item.defense = 2;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
			player.meleeSpeed += .28f;
			
		}
		public override void DrawHair(ref bool  drawHair, ref bool  drawAltHair )
		{
			drawHair=true;
			
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("RhuthiniumChestplate") && legs.type == mod.ItemType("RhuthiniumGreaves");
			
		}
		
	
		
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.RHeadbandSet");
            float speedBonus = (1.0f - ((player.statLife * 1.0f) / (player.statLifeMax2 * 1.0f)));
            if(speedBonus >0)
            {
                player.meleeSpeed += speedBonus;
            }
            
            
			
			
		}
		
		
		

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 12);
			
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
			
	}
    public class HeadbandDraw : ModPlayer
    {


        public static readonly PlayerLayer Headband = new PlayerLayer("QwertysRandomContent", "Headband", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
            if (drawPlayer.head == mod.GetEquipSlot("RhuthiniumHeadband", EquipType.Head))
            {
                //Main.NewText("Helmet!");
                //Main.NewText(drawPlayer.bodyFrame);
                Texture2D texture = mod.GetTexture("Items/Armor/Rhuthinium/RhuthiniumHeadband_UnderHead");
                Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
                int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                Vector2 Position = drawInfo.position;
                Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                //pos.Y -= drawPlayer.mount.PlayerOffset;
                DrawData data = new DrawData(texture, pos, drawPlayer.bodyFrame, color12, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                data.shader = drawInfo.headArmorShader;
                Main.playerDrawData.Add(data);
            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Face"));
            if (headLayer != -1)
            {
                Headband.visible = true;
                layers.Insert(headLayer + 1, Headband);
            }

        }
    }

}

