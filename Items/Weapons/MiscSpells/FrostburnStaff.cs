using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSpells       ///We need projectile to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class FrostburnStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frostburn Staff");
            Tooltip.SetDefault("Places burning cold flames on the ground");
            Item.staff[item.type] = true; //projectile makes the useStyle animate as a staff instead of as a gun
        }

        public override void SetDefaults()
        {
            item.damage = 14;
            item.mana = 4;
            item.width = 48;
            item.height = 48;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1f;
            item.value = 10000;
            item.rare = 1;
            item.UseSound = SoundID.Item43;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("FrostFlame");
            item.magic = true;
            item.shootSpeed = 8;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 68f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddIngredient(ItemID.IceBlock, 10);
            recipe.AddIngredient(ItemID.BorealWood, 10);
            recipe.AddIngredient(ItemID.WandofSparking, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class FrostFlame : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 12;
            projectile.penetrate = 3;

            projectile.friendly = true;
            projectile.timeLeft = 360;
            projectile.ranged = true;
            projectile.noEnchantments = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            if (projectile.ai[1] == 0f && projectile.type >= 326 && projectile.type <= 328)
            {
                projectile.ai[1] = 1f;
                Main.PlaySound(SoundID.Item13, projectile.position);
            }
            int num199 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135, 0f, 0f, 100, default(Color), 1f);
            Dust expr_8946_cp_0 = Main.dust[num199];
            expr_8946_cp_0.position.X = expr_8946_cp_0.position.X - 2f;
            Dust expr_8964_cp_0 = Main.dust[num199];
            expr_8964_cp_0.position.Y = expr_8964_cp_0.position.Y + 2f;
            Main.dust[num199].scale += (float)Main.rand.Next(50) * 0.01f;
            Main.dust[num199].noGravity = true;
            Dust expr_89B7_cp_0 = Main.dust[num199];
            expr_89B7_cp_0.velocity.Y = expr_89B7_cp_0.velocity.Y - 2f;
            if (Main.rand.Next(2) == 0)
            {
                int num200 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135, 0f, 0f, 100, default(Color), 1f);
                Dust expr_8A1E_cp_0 = Main.dust[num200];
                expr_8A1E_cp_0.position.X = expr_8A1E_cp_0.position.X - 2f;
                Dust expr_8A3C_cp_0 = Main.dust[num200];
                expr_8A3C_cp_0.position.Y = expr_8A3C_cp_0.position.Y + 2f;
                Main.dust[num200].scale += 0.3f + (float)Main.rand.Next(50) * 0.01f;
                Main.dust[num200].noGravity = true;
                Main.dust[num200].velocity *= 0.1f;
            }
            if ((double)projectile.velocity.Y < 0.25 && (double)projectile.velocity.Y > 0.15)
            {
                projectile.velocity.X = projectile.velocity.X * 0.8f;
            }
            projectile.rotation = -projectile.velocity.X * 0.05f;

            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 5f)
            {
                projectile.ai[0] = 5f;
                if (projectile.velocity.Y == 0f && projectile.velocity.X != 0f)
                {
                    projectile.velocity.X = projectile.velocity.X * 0.97f;
                    if ((double)projectile.velocity.X > -0.01 && (double)projectile.velocity.X < 0.01)
                    {
                        projectile.velocity.X = 0f;
                        projectile.netUpdate = true;
                    }
                }
                projectile.velocity.Y = projectile.velocity.Y + 0.2f;
            }
            projectile.rotation += projectile.velocity.X * 0.1f;

            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
                return;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 240);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity.X *= .2f;

            /*
            if (projectile.oldVelocity.X != oldVelocity.X)
            {
                projectile.oldVelocity.X = oldVelocity.X * -0.1f;
            }
            */

            return false;
        }
    }
}