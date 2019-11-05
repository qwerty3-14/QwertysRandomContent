using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscBows
{
	public class AerousLongbowWithStormEnchantment : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Storm Enchanted Aerous");
			Tooltip.SetDefault("The crazy bows are cool and all, but sometimes you just need a fairly normal bow with good stats" + "\nKilling enemies builds up a charge right click to shoot a powerful storm arrow using this charge");


        }
		public override void SetDefaults()
		{
            item.CloneDefaults(mod.ItemType("AerousLongbow"));


        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void HoldItem(Player player)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            modPlayer.stormEnchantment = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {


            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (player.altFunctionUse == 2)
            {
                if (modPlayer.charge > 0)
                {

                    Projectile.NewProjectile(position.X, position.Y, speedX*3, speedY*3, mod.ProjectileType("StormArrow"), damage* modPlayer.charge, knockBack* modPlayer.charge, player.whoAmI);
                    modPlayer.charge = 0;
                }

                return false;

            }
            else
            {
                return true;
            }
                
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddRecipeGroup("QwertysrandomContent:AerousBow");
            recipe.AddIngredient(mod.ItemType("CraftingRune"), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        


    }
    public class StormArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm Arrow");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate =-1;
            projectile.ranged = true;
            projectile.arrow = true;
            projectile.tileCollide = false;
            projectile.light=10f;

        }
        public override void AI()
        {

            for (int d = 0; d <= 20; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("StormArrowDust"));
            }


        }




    }



}

