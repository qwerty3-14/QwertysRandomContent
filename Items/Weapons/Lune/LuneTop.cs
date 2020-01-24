using QwertysRandomContent.AbstractClasses;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Lune
{
    public class LuneTop : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Top");
            Tooltip.SetDefault("Inflicts Lune curse making enemies more vulnerable to critical hits");

        }
        public override void SetDefaults()
        {
            item.damage = 28;
            item.thrown = true;
            item.knockBack = 5;
            item.value = 5;
            item.rare = 1;
            item.width = 30;
            item.height = 38;
            item.useStyle = 1;
            item.shootSpeed = 4.5f;
            item.useTime = 32;
            item.useAnimation = 32;
            item.consumable = true;
            item.shoot = mod.ProjectileType("LuneTopP");
            item.noUseGraphic = true;
            item.noMelee = true;
            item.maxStack = 999;
            item.autoReuse = true;


        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("LuneBar"), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
    public class LuneTopP : Top
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Top");
        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;

            projectile.width = 30;
            projectile.height = 38;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.thrown = true;


            projectile.tileCollide = true;
            friction = .004f;
            enemyFriction = .08f;
        }
    }
}

