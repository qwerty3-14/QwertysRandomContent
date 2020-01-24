using QwertysRandomContent.Items.Weapons.Dungeon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.BladeBossItems
{
    public class BladedArrow : Aqueous
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bladed Arrow");
            Tooltip.SetDefault("Shot from your bow alongside normal arrows");

        }
        public override void SetDefaults()
        {
            item.damage = 50;
            item.ranged = true;
            item.knockBack = .5f;
            item.rare = 7;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.width = 2;
            item.height = 2;
            item.crit = 25;
            item.shootSpeed = 12f;
            item.useTime = 100;

            item.maxStack = 1;


        }
        public override void ReturningDust()
        {

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("BladedArrowShaft"));
            recipe.AddIngredient(mod.ItemType("Aqueous"));
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    public class BladedArrowShot : AqueousShot
    {

        public override void Initialize()
        {
            arrowID = mod.ItemType("BladedArrow");
            shootID = mod.ProjectileType("BladedArrowP");
        }
    }
    public class BladedArrowP : AqueousP
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            projectile.width = projectile.height = 14;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.ranged = true;
            projectile.arrow = true;

            projectile.tileCollide = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            assosiatedItemID = mod.ItemType("BladedArrow");

        }
        public override void AI()
        {

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }
    }
}
