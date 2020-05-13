using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.TundraBossItems
{
	public class FrostCompass : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Compass");
			Tooltip.SetDefault("Points toward the 'North pole'");
		}

		public override void SetDefaults()
		{
			item.value = 20000;
			item.rare = 1;

			item.width = 28;
			item.height = 32;

			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<FrostCompassEffect>().effect = true;
		}
	}

	public class FrostCompassEffect : ModPlayer
	{
		public bool effect;

		public override void ResetEffects()
		{
			effect = false;
		}

		public static readonly PlayerLayer IceArrow = new PlayerLayer("QwertysRandomContent", "HydraShoes", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
		{
			if (drawInfo.shadow != 0f)
			{
				return;
			}
			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("QwertysRandomContent");
			//ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
			if (drawPlayer.GetModPlayer<FrostCompassEffect>().effect && FrozenDen.BearSpawn.X != -1 && FrozenDen.BearSpawn.Y != -1)
			{
				//Main.NewText("Legs!");
				//Main.NewText(drawPlayer.bodyFrame);
				Texture2D texture = mod.GetTexture("Items/TundraBossItems/FrostArrow");

				int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
				int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
				Vector2 Position = drawPlayer.position;
				Vector2 origin = texture.Size() / 2;
				Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
				pos.Y -= drawPlayer.mount.PlayerOffset;

				float North = (FrozenDen.BearSpawn - drawPlayer.Center).ToRotation();
				DrawData data = new DrawData(texture, pos, new Rectangle(0, 0, (int)texture.Size().X, (int)texture.Size().Y), Color.White, North, origin, 1f, 0, 0);
				//data.shader = drawInfo.legArmorShader;
				Main.playerDrawData.Add(data);
			}
		});

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int legLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Wings"));
			if (legLayer != -1 && FrozenDen.BearSpawn.X != -1 && FrozenDen.BearSpawn.Y != -1)
			{
				IceArrow.visible = true;
				layers.Insert(legLayer + 1, IceArrow);
			}
			else
			{
				IceArrow.visible = false;
			}
		}
	}
}