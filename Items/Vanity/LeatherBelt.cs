using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Vanity
{
	//[AutoloadEquip(EquipType.Waist)]

	public class LeatherBelt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leather Belt");
			Tooltip.SetDefault("It's a belt");
		}

		public override void SetDefaults()
		{
			item.value = 1000;
			item.rare = 1;
			item.width = 28;
			item.height = 28;
			item.vanity = true;
			item.accessory = true;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<BetterBelt>().BeltTexture = mod.GetTexture("Items/Vanity/LeatherBelt_Belt");
			}
		}
	}

	public class BetterBelt : GlobalItem
	{
		public Texture2D BeltTexture = null;
		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => true;
	}

	public class BetterBelts : ModPlayer
	{
		public static readonly PlayerLayer BetterBelt = new PlayerLayer("QwertysRandomContent", "BetterBelt", PlayerLayer.NeckAcc, delegate (PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("QwertysRandomContent");
			//ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
			Vector2 Position = drawInfo.position;
			Color color12 = drawInfo.middleArmorColor;
			int shader8 = 0;
			//Main.NewText(drawPlayer.dye.Length);
			for (int i = 0; i < 20; i++)
			{
				if (!drawPlayer.armor[i].IsAir)
				{
					if (drawPlayer.armor[i].GetGlobalItem<BetterBelt>().BeltTexture != null && (i > 10 || !drawPlayer.hideVisual[i % 10]))
					{
						shader8 = (int)drawPlayer.dye[i % 10].dye;
						Texture2D texture = drawPlayer.armor[i].GetGlobalItem<BetterBelt>().BeltTexture;
						DrawData value = new DrawData(texture, new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
						value.shader = shader8;
						Main.playerDrawData.Add(value);
					}
				}
			}
		});

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int neckLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("ShieldAcc"));
			if (neckLayer != -1)
			{
				BetterBelt.visible = true;
				layers.Insert(neckLayer + 1, BetterBelt);
			}
		}
	}
}