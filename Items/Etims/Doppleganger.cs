using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Etims
{
	public class Doppleganger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Doppleganger");
			Tooltip.SetDefault("Pretends to be the accesory placed above it.\nWon't work if it's placed in the top most accesory slot.\nThe gods forbid equiping the same accesory twice!");
		}

		public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? base.Texture + "_Old" : base.Texture;

		public override void SetDefaults()
		{
			item.accessory = true;
			item.rare = 3;
			item.expert = true;
			item.value = 500000;
			item.width = 32;
			item.height = 22;
			item.GetGlobalItem<DoppleItem>().isDoppleganger = true;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			int mimicId = -1;
			Player player = Main.player[item.owner];
			for (int a = 4; a < 10; a++)
			{
				if (!player.armor[a].IsAir && player.armor[a] == item && !player.armor[a - 1].IsAir)
				{
					mimicId = player.armor[a - 1].type;
				}
			}
			Texture2D texture = Main.itemTexture[item.type];
			if (mimicId != -1)
			{
				//spriteBatch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 0f);;

				Texture2D mimicTexture = Main.itemTexture[mimicId];
				int frameCount = 1;
				if (Main.itemAnimations[mimicId] != null)
				{
					frameCount = Main.itemAnimations[mimicId].FrameCount;
				}
				float greaterLength = Math.Max(mimicTexture.Width, mimicTexture.Height / frameCount);
				spriteBatch.Draw(mimicTexture, position + Vector2.UnitY * -3, new Rectangle(0, 0, mimicTexture.Width, mimicTexture.Height / frameCount), new Color(180, 100, 100, 255), 0, origin, (44f / greaterLength) * scale, SpriteEffects.None, 0f);

				return false;
			}

			return true;
		}
	}

	public class DoppleItem : GlobalItem
	{
		public bool isDoppleganger = false;
		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => true;
	}

	public class DopplePlayer : ModPlayer
	{
		public override void PreUpdate()
		{
			for (int a = 4; a < 10; a++)
			{
				if (!player.armor[a].IsAir && player.armor[a].type == mod.ItemType("Doppleganger") && !player.armor[a - 1].IsAir)
				{
					player.armor[a].type = player.armor[a - 1].type;
				}
			}
		}

		public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
		{
			for (int a = 4; a < 10; a++)
			{
				if (!player.armor[a].IsAir && player.armor[a].GetGlobalItem<DoppleItem>().isDoppleganger)
				{
					if (!player.armor[a - 1].IsAir)
					{
						ItemLoader.UpdateAccessory(player.armor[a - 1], player, player.hideVisual[a]);
						ItemLoader.UpdateEquip(player.armor[a - 1], player);
						player.statDefense += player.armor[a - 1].defense;
					}
				}
			}
		}

		public override void PostUpdateEquips()
		{
			for (int a = 4; a < 10; a++)
			{
				if (!player.armor[a].IsAir && player.armor[a].GetGlobalItem<DoppleItem>().isDoppleganger)
				{
					player.armor[a].type = mod.ItemType("Doppleganger");
				}
			}
		}
	}
}