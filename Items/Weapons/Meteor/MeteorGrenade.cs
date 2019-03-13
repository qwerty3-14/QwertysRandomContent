using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Meteor
{
    public class MeteorGrenade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor Grenade");
            Tooltip.SetDefault("Calls meteors!");
        }
        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.shootSpeed = 6f;
            item.shoot = mod.ProjectileType("MeteorGrenadeP");
            item.width = 20;
            item.height = 20;
            item.maxStack = 999;
            item.consumable = true;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 45;
            item.useTime = 45;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.value = 75;
            item.damage = 65;
            item.knockBack = 8f;
            item.thrown = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar, 1);
            recipe.AddIngredient(ItemID.Grenade, 50);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
    }
    public class MeteorGrenadeP : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 16;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.thrown = true;
            projectile.timeLeft = 600;
            aiType = ProjectileID.Grenade;
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item14, projectile.position);
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 22;
            projectile.height = 22;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            for (int num761 = 0; num761 < 20; num761++)
            {
                int num762 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num762].velocity *= 1.4f;
            }
            for (int num763 = 0; num763 < 10; num763++)
            {
                int num764 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2.5f);
                Main.dust[num764].noGravity = true;
                Main.dust[num764].velocity *= 5f;
                num764 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num764].velocity *= 3f;
            }
            int num765 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
            Main.gore[num765].velocity *= 0.4f;
            Gore expr_18B0E_cp_0 = Main.gore[num765];
            expr_18B0E_cp_0.velocity.X = expr_18B0E_cp_0.velocity.X + 1f;
            Gore expr_18B2E_cp_0 = Main.gore[num765];
            expr_18B2E_cp_0.velocity.Y = expr_18B2E_cp_0.velocity.Y + 1f;
            num765 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
            Main.gore[num765].velocity *= 0.4f;
            Gore expr_18BB2_cp_0 = Main.gore[num765];
            expr_18BB2_cp_0.velocity.X = expr_18BB2_cp_0.velocity.X - 1f;
            Gore expr_18BD2_cp_0 = Main.gore[num765];
            expr_18BD2_cp_0.velocity.Y = expr_18BD2_cp_0.velocity.Y + 1f;
            num765 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
            Main.gore[num765].velocity *= 0.4f;
            Gore expr_18C56_cp_0 = Main.gore[num765];
            expr_18C56_cp_0.velocity.X = expr_18C56_cp_0.velocity.X + 1f;
            Gore expr_18C76_cp_0 = Main.gore[num765];
            expr_18C76_cp_0.velocity.Y = expr_18C76_cp_0.velocity.Y - 1f;
            num765 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
            Main.gore[num765].velocity *= 0.4f;
            Gore expr_18CFA_cp_0 = Main.gore[num765];
            expr_18CFA_cp_0.velocity.X = expr_18CFA_cp_0.velocity.X - 1f;
            Gore expr_18D1A_cp_0 = Main.gore[num765];
            expr_18D1A_cp_0.velocity.Y = expr_18D1A_cp_0.velocity.Y - 1f; Main.PlaySound(SoundID.Item62, projectile.position);
            for(int i =0; i <2; i++)
            {
                Projectile meteor = Main.projectile[Projectile.NewProjectile(projectile.Center + new Vector2(Main.rand.Next(-20, 20), -1000), QwertyMethods.PolarVector(10, (float)Math.PI / 2 + (float)Math.PI / 16 * Main.rand.NextFloat(-1, 1)), 424 + Main.rand.Next(2), (int)(projectile.damage), projectile.knockBack, projectile.owner, 0f, 0.5f + (float)Main.rand.NextDouble() * 0.3f)];
                meteor.magic = false;
                meteor.thrown = true;
                
            }
           
        }
    }
}
