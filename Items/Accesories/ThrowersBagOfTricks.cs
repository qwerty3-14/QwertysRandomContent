using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
	internal class ThrowersBagOfTricks : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thrower's Bag of Tricks");
			Tooltip.SetDefault("Doubles the amount of javelins you can stick into an enemy \nTops spin for longer \nFlechettes accelerate faster, reaching their top speed sooner \nThrown grenades have a larger explosion");
		}

		public override void SetDefaults()
		{
			item.value = 100000;
			item.rare = 2;
			item.width = 32;
			item.height = 26;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<QwertyPlayer>().PincusionMultiplier *= 2f;
			player.GetModPlayer<QwertyPlayer>().TopFrictionMultiplier -= .3f;
			player.GetModPlayer<QwertyPlayer>().FlechetteDropAcceleration += 2f;
			player.GetModPlayer<QwertyPlayer>().GrenadeExplosionModifier += .75f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("JavelinPincusin"), 1);
			recipe.AddIngredient(mod.ItemType("PrimedGrenadeCore"), 1);
			recipe.AddIngredient(mod.ItemType("Gyroscope"), 1);
			recipe.AddIngredient(mod.ItemType("AerodynamicFins"), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}