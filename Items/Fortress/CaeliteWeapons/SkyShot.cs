using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.CaeliteWeapons
{
    public class SkyShot : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sky Shot");
            Tooltip.SetDefault("Uses darts as ammo\nHigher beings have loaded an additional dart.");
        }

        public override void SetDefaults()
        {
            item.damage = 10;
            item.ranged = true;
            item.knockBack = 1;
            item.value = 50000;
            item.rare = 3;
            item.width = 38;
            item.height = 32;
            item.useStyle = 5;
            item.shootSpeed = 12f;
            item.useTime = 30;
            item.useAnimation = 30;
            item.shoot = 10;
            item.useAmmo = AmmoID.Dart;
            item.noUseGraphic = false;
            item.noMelee = true;
            item.UseSound = SoundID.Item63;
            item.autoReuse = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float r = (new Vector2(speedX, speedY)).ToRotation() - (float)Math.PI/2f;
            Projectile.NewProjectile(position + QwertyMethods.PolarVector(4f * player.direction, r), new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(position + QwertyMethods.PolarVector(12f * player.direction, r), new Vector2(speedX, speedY), mod.ProjectileType("CaeliteDart"), damage, knockBack, player.whoAmI);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, -4);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    public class CaeliteDart : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.alpha = 255;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("PowerDown"), 60 * 30);
        }
        public override void Kill(int timeLeft)
        {
            for(int i=0; i<3; i++)
            {
                Dust.NewDustPerfect(projectile.Center, mod.DustType("CaeliteDust"), QwertyMethods.PolarVector(Main.rand.NextFloat()*4f, Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI)));
            }
        }
        public override void AI()
        {
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] > 3f)
            {
                projectile.alpha = 0;
            }
            if (projectile.ai[0] >= 20f)
            {
                projectile.ai[0] = 20f;

                projectile.velocity.Y += 0.075f;

            }
        }
    }
}
