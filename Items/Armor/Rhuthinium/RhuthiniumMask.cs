using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Rhuthinium
{
	[AutoloadEquip(EquipType.Head)]
	public class RhuthiniumMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Mask");
			Tooltip.SetDefault("+1 max minions and 8% increased minion damage");
			if (ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium && !Main.dedServ)
			{
				Main.itemTexture[item.type] = mod.GetTexture("Items/Armor/Rhuthinium/RhuthiniumMask_Old");
				Main.armorHeadTexture[item.headSlot] = mod.GetTexture("Items/Armor/Rhuthinium/RhuthiniumMask_Head_Old");
			}
		}

		public override void SetDefaults()
		{
			item.value = 50000;
			item.rare = 3;

			item.width = 22;
			item.height = 14;
			item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			player.minionDamage += .08f;
			player.maxMinions += 1;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = true;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("RhuthiniumChestplate") && legs.type == mod.ItemType("RhuthiniumGreaves");
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.RMaskSet");
			player.maxMinions += 1;
			var modPlayer = player.GetModPlayer<QwertyPlayer>();
			modPlayer.minionIchor = true;
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
}