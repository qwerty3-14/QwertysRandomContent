using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons
{
    public class DragonFire : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dragonfire");
            Tooltip.SetDefault("Launches flaming dragon heads which spit explosive fireballs!");

        }
        public override void SetDefaults()
        {
            item.damage = 120;
            item.magic = true;

            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = 5;
            item.knockBack = 10;
            item.value = 500000;
            item.rare = 9;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.width = 28;
            item.height = 40;

            item.mana = 12;
            item.shoot = mod.ProjectileType("DragonHead");
            item.shootSpeed = 5;
            item.noMelee = true;




        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);


            recipe.AddIngredient(mod.ItemType("CraftingRune"), 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }


    }
    public class DragonHead : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dragon Head");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;

            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = true;




        }
        public int timer;
        public override void AI()
        {
            if (projectile.velocity.X > 0)
            {
                projectile.spriteDirection = -1;
            }
            else
            {
                projectile.spriteDirection = 1;
            }
            Player player = Main.player[projectile.owner];
            timer++;
            if (timer > 10)
            {
                Projectile.NewProjectile(projectile.Center, new Vector2((float)Math.Cos(projectile.rotation - Math.PI / 2) * 10, (float)Math.Sin(projectile.rotation - Math.PI / 2) * 10), mod.ProjectileType("FireBall"), projectile.damage, projectile.knockBack, player.whoAmI);
                timer = 0;
            }

        }


    }
    public class FireBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fire Ball");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = true;




        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void AI()
        {
            for (int d = 0; d <= 4; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire);
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.Kill();
        }
        public override void Kill(int timeLeft)
        {


            Player player = Main.player[projectile.owner];
            projectile.width = 75;
            projectile.height = 75;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            Projectile.NewProjectile(projectile.Center, new Vector2(0, 0), mod.ProjectileType("FireBallExplosion"), projectile.damage, projectile.knockBack, player.whoAmI);
            projectile.hostile = true;
            Main.PlaySound(SoundID.Item62, projectile.position);
            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 80; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }

        }



    }
    public class FireBallExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fire Ball");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 75;
            projectile.height = 75;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 2;



        }
        public override void AI()
        {
            projectile.FriendlyFire();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
    }

}

