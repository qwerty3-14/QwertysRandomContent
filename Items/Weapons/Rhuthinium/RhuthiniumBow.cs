using QwertysRandomContent.Config;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Rhuthinium
{
	public class RhuthiniumBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Bow");
			Tooltip.SetDefault("");
		}

		public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium ? base.Texture + "_Old" : base.Texture;

		public override void SetDefaults()
		{
			item.damage = 31;
			item.ranged = true;

			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 5;
			item.knockBack = 2;
			item.value = 25000;
			item.rare = 3;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.width = 26;
			item.height = 62;
			item.crit = 5;
			item.shoot = 40;
			item.useAmmo = 40;
			item.shootSpeed = 8;
			item.noMelee = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);

			recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}