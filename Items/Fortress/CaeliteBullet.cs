using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress
{
    public class CaeliteBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Bullet");
            Tooltip.SetDefault("Inflicts Caelite Wrath reducing damage that enemies deal by 20%");

        }
        public override void SetDefaults()
        {
            item.damage = 8;
            item.ranged = true;
            item.knockBack = 2;
            item.value = 1;
            item.rare = 3;
            item.width = 12;
            item.height = 18;

            item.shootSpeed = 32;

            item.consumable = true;
            item.shoot = mod.ProjectileType("CaeliteBulletP");
            item.ammo = 97;
            item.maxStack = 999;


        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }

    }
    public class CaeliteBulletP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Bullet");


        }
        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.light = 0.5f;
            projectile.scale = 1.2f;
            projectile.timeLeft = 600;
            projectile.ranged = true;
            projectile.extraUpdates = 1;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void AI()
        {
            for (int d = 0; d < 1; d++)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, mod.DustType("CaeliteDust"), Vector2.Zero);
                dust.frame.Y = 0;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("PowerDown"), 300);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("PowerDown"), 300);
        }


    }

}

