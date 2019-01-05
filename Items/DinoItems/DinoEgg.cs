using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using QwertysRandomContent.Items.Accesories;
using QwertysRandomContent.Items.Armor.Rhuthinium;
using QwertysRandomContent.Items.Armor.Vargule;
using Terraria.DataStructures;

using System.Linq;

using Terraria.Localization;
using QwertysRandomContent;
namespace QwertysRandomContent.Items.DinoItems
{
	
	public class DinoEgg : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons the Dino Militia" + "\nThey never died out!");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 52;
			item.maxStack = 20;
			item.rare = 6;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
			item.UseSound = SoundID.Item44;
			item.consumable = true;
		}

		
		public override bool CanUseItem(Player player)
		{
			return true;
			
		}

		public override bool UseItem(Player player)
		{
            if (!NPC.downedMoonlord)
            {
                string key = "Mods.QwertysRandomContent.DinoEventStart";
                Color messageColor = Color.Orange;
                if (Main.netMode == 2) // Server
                {
                    NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText(Language.GetTextValue(key), messageColor);
                }
            }
            else
            {
                string key = "Mods.QwertysRandomContent.DinoHardStart";
                Color messageColor = Color.Orange;
                if (Main.netMode == 2) // Server
                {
                    NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText(Language.GetTextValue(key), messageColor);
                }
            }

					
			Main.PlaySound(SoundID.Roar, player.position, 0);
			QwertyWorld modWorld = (QwertyWorld)mod.GetModWorld("QwertyWorld");
			modWorld.DinoEvent = true;
			modWorld.DinoKillCount = 0;
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(1006, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		
	}
}