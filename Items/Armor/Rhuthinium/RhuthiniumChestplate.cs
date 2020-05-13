using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Rhuthinium
{
	[AutoloadEquip(EquipType.Body)]
	public class RhuthiniumChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Chestplate");
			Tooltip.SetDefault("10% increased damage");
			if (ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium && !Main.dedServ)
			{
				Main.itemTexture[item.type] = mod.GetTexture("Items/Armor/Rhuthinium/RhuthiniumChestplate_Old");
				Main.armorBodyTexture[item.bodySlot] = mod.GetTexture("Items/Armor/Rhuthinium/RhuthiniumChestplate_Body_Old");
				Main.femaleBodyTexture[item.bodySlot] = mod.GetTexture("Items/Armor/Rhuthinium/RhuthiniumChestplate_FemaleBody_Old");
				Main.armorArmTexture[item.bodySlot] = mod.GetTexture("Items/Armor/Rhuthinium/RhuthiniumChestplate_Arms_Old");
			}
		}

		public override void SetDefaults()
		{
			item.value = 50000;
			item.rare = 3;

			item.width = 26;
			item.height = 18;
			item.defense = 4;
		}

		public override void UpdateEquip(Player player)
		{
			player.allDamage += .1f;
		}

		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
			drawArms = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 18);

			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}