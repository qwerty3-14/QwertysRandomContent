using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
    public class OrichalcumBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orichalcum Bullet");
            Tooltip.SetDefault("Upon hitting an enemy flies toward another enemy");
        }

        public override void SetDefaults()
        {
            item.damage = 10;
            item.ranged = true;
            item.knockBack = 2;
            item.value = 1;
            item.rare = 3;
            item.width = 12;
            item.height = 18;

            item.shootSpeed = 32;

            item.consumable = true;
            item.shoot = mod.ProjectileType("OrichalcumBulletP");
            item.ammo = 97;
            item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.OrichalcumBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }

    public class OrichalcumBulletP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orichalcum Bullet");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = 2;
            projectile.ranged = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.extraUpdates = 1;
        }

        public bool runOnce = true;
        private float maxSpeed;

        public override void AI()
        {
            if (runOnce)
            {
                maxSpeed = projectile.velocity.Length();
                runOnce = false;
            }
        }

        public bool firstHit = true;

        private NPC ConfirmedTarget;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;

            if (QwertyMethods.ClosestNPC(ref ConfirmedTarget, 300, projectile.Center, specialCondition: delegate (NPC possibleTarget) { return projectile.localNPCImmunity[possibleTarget.whoAmI] == 0; }))
            {
                projectile.velocity = QwertyMethods.PolarVector(maxSpeed, (ConfirmedTarget.Center - projectile.Center).ToRotation());
            }
            else
            {
                projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
        }
    }
}