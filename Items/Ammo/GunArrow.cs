using QwertysRandomContent.Items.B4Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
    public class GunArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gun Arrow");
            Tooltip.SetDefault("Shoots 2 bullets from your inventory within its first second of flight!");
        }

        public override void SetDefaults()
        {
            item.damage = 2;
            item.ranged = true;
            item.knockBack = 2;
            item.value = 5;
            item.rare = 4;
            item.width = 14;
            item.height = 32;

            item.shootSpeed = 6;
            item.useAmmo = 97;
            item.consumable = true;
            item.shoot = mod.ProjectileType("GunArrowP");
            item.ammo = 40;
            item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }

    public class GunArrowP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gun Arrow");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.ranged = true;
            projectile.arrow = true;

            projectile.tileCollide = true;
        }

        public int timer = 0;
        public int bullet = 14;
        public bool canShoot;
        public float speed = 14f;

        //public Item item = new item();
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            timer++;
            int weaponDamage = projectile.damage;
            float weaponKnockback = projectile.knockBack;

            canShoot = player.HasAmmo(QwertyMethods.MakeItemFromID(mod.ItemType("GunArrow")), true) && player.inventory[player.selectedItem + 10].useAmmo == 97;
            if (timer == 30 || timer == 60)
            {
                if(projectile.UseAmmo(AmmoID.Bullet, ref bullet, ref speed, ref weaponDamage, ref weaponKnockback, false))
                {
                    Projectile b = Main.projectile[Projectile.NewProjectile(projectile.Center, projectile.velocity, bullet, weaponDamage, weaponKnockback, Main.myPlayer)];
                    if (projectile.GetGlobalProjectile<arrowgigantism>().GiganticArrow)
                    {
                        b.scale *= 3;
                        b.GetGlobalProjectile<arrowgigantism>().GiganticArrow = true;
                    }
                }
                
            }
            
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
        }
    }
}