using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscBows
{
	public class AerousLongbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aerous");
			Tooltip.SetDefault("The crazy bows are cool and all, but sometimes you just need a fairly normal bow with good stats");
			
		}
		public override void SetDefaults()
		{
			item.damage = 160;
			item.ranged = true;
			
			item.useTime = 29;
			item.useAnimation = 29;
			item.useStyle = 5;
			item.knockBack = 0;
            item.value = 1000000;
            item.rare = 8;
			item.UseSound = SoundID.Item5;
			
			item.width = 26;
			item.height = 40;
			item.crit = 20;
			item.shoot = 40;
			item.useAmmo = 40;
			item.shootSpeed =12f;
			item.noMelee=true;
            item.autoReuse = true;


        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("TrueBloomingBow"));
            recipe.AddIngredient(mod.ItemType("TrueDeathString"));
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }


    }
    


}

