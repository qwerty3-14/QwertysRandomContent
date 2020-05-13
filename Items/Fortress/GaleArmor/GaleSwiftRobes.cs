using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.GaleArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class GaleSwiftRobes : ModItem
	{
		public override bool Autoload(ref string name)
		{
			if (!Main.dedServ)
			{
				// Add the female leg variant
				//mod.AddEquipTexture(null, EquipType.Legs, "GaleSwiftRobes_Female", "QwertysRandomContent/Items/Fortress/GaleArmor/GaleSwiftRobes_FemaleLegs");
			}
			return true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gale Swift Robes");
			Tooltip.SetDefault("+9% chance to dodge an attack" + "\n+10% thrown critical strike chance and velocity" + "\n15% chance not to consume thrown items" + "\nLets you dash (4.8 dash power)");
		}

		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			if (male) equipSlot = mod.GetEquipSlot("GaleSwiftRobes", EquipType.Legs);
			if (!male) equipSlot = mod.GetEquipSlot("GaleSwiftRobes_Female", EquipType.Legs);
		}

		public override void SetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 75, 0);
			item.rare = 4;
			item.defense = 1;
			//item.vanity = true;
			item.width = 20;
			item.height = 20;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<QwertyPlayer>().dodgeChance += 9;
			player.thrownCrit += 10;
			player.thrownVelocity += .1f;
			player.GetModPlayer<QwertyPlayer>().throwReduction *= .85f;
			var modPlayer = player.GetModPlayer<QwertyPlayer>();
			if (modPlayer.customDashSpeed < 4.8f)
			{
				modPlayer.customDashSpeed = 4.8f;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CaeliteBar"), 8);
			recipe.AddIngredient(mod.ItemType("FortressHarpyBeak"), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}