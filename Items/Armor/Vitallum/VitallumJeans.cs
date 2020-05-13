using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace QwertysRandomContent.Items.Armor.Vitallum
{
	[AutoloadEquip(EquipType.Legs)]
	public class VitallumJeans : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vitallum Jeans");
			Tooltip.SetDefault("Increases max life by 100 \n15% increased throwing damage \n40% chance not to consume thrown items \n40% increase thrown velocity when on the ground \nRegenerate 2 life/sec when on the ground");
		}

		public override void SetDefaults()
		{
			item.rare = 8;
			item.value = Item.sellPrice(gold: 6);
		}

		public override void UpdateEquip(Player player)
		{
			player.statLifeMax2 += 100;
			player.thrownDamage += .15f;
			player.GetModPlayer<QwertyPlayer>().throwReduction *= .60f;
			Point origin = player.Bottom.ToTileCoordinates();
			Point point;
			if (WorldUtils.Find(origin, Searches.Chain(new Searches.Down(3), new GenCondition[] { new Conditions.IsSolid() }), out point))
			{
				player.thrownVelocity += .4f;
				player.lifeRegen += 4;
			}
		}

		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			if (male) equipSlot = mod.GetEquipSlot("VitallumJeans", EquipType.Legs);
			if (!male) equipSlot = mod.GetEquipSlot("VitallumJeans_Female", EquipType.Legs);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 18);
			recipe.AddIngredient(ItemID.LifeCrystal, 6);
			recipe.AddIngredient(mod.ItemType("VitallumCoreCharged"));
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void OnCraft(Recipe recipe)
		{
			Main.player[item.owner].QuickSpawnItem(mod.ItemType("VitallumCoreUncharged"), 1);
		}
	}

	public class JeansEffects : ModPlayer
	{
		public static readonly PlayerLayer Vien = LayerDrawing.DrawOnLegs("VitallumJeans", "Items/Armor/Vitallum/VitallumJeans_LegsVien", "VitallumJeans_Female", "Items/Armor/Vitallum/VitallumJeans_FemaleLegsVien", "VitallumJeans", false, 3);

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int bodyLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Body"));
			if (bodyLayer != -1)
			{
				Vien.visible = true;
				layers.Insert(bodyLayer + 1, Vien);
			}
		}
	}
}