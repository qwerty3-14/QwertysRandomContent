using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Clay
{
	[AutoloadEquip(EquipType.Legs)]
	public class ClayKneecaps : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clay Kneecaps");
			Tooltip.SetDefault("7% incresed morph damage");
		}

		public override bool Autoload(ref string name)
		{
			if (!Main.dedServ)
			{
				// Add the female leg variant
				mod.AddEquipTexture(null, EquipType.Legs, "ClayKneecaps_Female", "QwertysRandomContent/Items/Armor/Clay/ClayKneecaps_FemaleLegs");
			}
			return true;
		}

		public override void SetDefaults()
		{
			item.value = 30000;
			item.rare = 1;

			item.width = 22;
			item.height = 12;
			item.defense = 1;
			item.GetGlobalItem<ShapeShifterItem>().equipedMorphDefense = 4;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<ShapeShifterPlayer>().morphDamage += .07f;
		}

		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			if (male) equipSlot = mod.GetEquipSlot("ClayKneecaps", EquipType.Legs);
			if (!male) equipSlot = mod.GetEquipSlot("ClayKneecaps_Female", EquipType.Legs);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ClayBlock, 25);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	public class KneecapDrawing : ModPlayer
	{
		public static readonly PlayerLayer Pants = new PlayerLayer("QwertysRandomContent", "Pants", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
		{
			if (drawInfo.shadow != 0f)
			{
				return;
			}
			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("QwertysRandomContent");
			//ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
			if (drawPlayer.legs == mod.GetEquipSlot("ClayKneecaps", EquipType.Legs) || drawPlayer.legs == mod.GetEquipSlot("ClayKneecaps_Female", EquipType.Legs))
			{
				Texture2D texture = mod.GetTexture("Items/Armor/Clay/Pants");
				if (!drawPlayer.Male)
				{
					texture = mod.GetTexture("Items/Armor/Clay/Pants_Female");
				}
				Vector2 Position = drawInfo.position;
				Position.Y += 14;
				Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
				DrawData value = new DrawData(texture, pos, new Microsoft.Xna.Framework.Rectangle?(drawPlayer.legFrame), drawInfo.pantsColor, drawPlayer.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0);

				Main.playerDrawData.Add(value);

				texture = mod.GetTexture("Items/Armor/Clay/Shoes");
				if (!drawPlayer.Male)
				{
					texture = mod.GetTexture("Items/Armor/Clay/Shoes_Female");
				}
				value = new DrawData(texture, pos, new Microsoft.Xna.Framework.Rectangle?(drawPlayer.legFrame), drawInfo.shoeColor, drawPlayer.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0);

				Main.playerDrawData.Add(value);
			}
		});

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int legLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Skin"));
			if (legLayer != -1)
			{
				Pants.visible = true;
				layers.Insert(legLayer + 1, Pants);
			}
		}
	}
}