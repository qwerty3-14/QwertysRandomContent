using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
	public class PhantomSphere : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom Sphere");
			Tooltip.SetDefault("Magic attacks pierce 2 extra enemies\nProjectiles that normally don't pierce will use local immunity\nMagic attacks ignore defense\n10% reduced magic damage");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 26;
			item.value = Item.sellPrice(gold: 3);
			item.rare = 2;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<MagicPierePlayer>().pierceBoost += 2;
			player.magicDamage -= .1f;
			player.GetModPlayer<MagicPierePlayer>().negateArmor = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("TheBlueSphere"), 1);
			recipe.AddIngredient(mod.ItemType("ArcaneArmorBreaker"), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}