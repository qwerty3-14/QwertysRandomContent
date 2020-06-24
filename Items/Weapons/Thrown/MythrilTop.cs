using QwertysRandomContent.AbstractClasses;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Thrown
{
    public class MythrilTop : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mythril Top");
            Tooltip.SetDefault("Behold! The terrifying power of the spinning top!!");
        }

        public override void SetDefaults()
        {
            item.damage = 40;
            item.melee = true;
            item.knockBack = 5;
            item.value = Item.sellPrice(gold: 2);
            item.rare = 3;
            item.width = 30;
            item.height = 38;
            item.useStyle = 1;
            item.shootSpeed = 5f;
            item.useTime = 21;
            item.useAnimation = 21;
            item.shoot = mod.ProjectileType("MythrilTopP");
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MythrilBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class MythrilTopP : Top
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mythril Top");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;

            projectile.width = 30;
            projectile.height = 38;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.tileCollide = true;
            friction = .002666f;
            enemyFriction = .1f;
        }
    }
}