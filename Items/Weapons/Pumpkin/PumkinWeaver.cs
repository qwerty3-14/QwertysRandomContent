using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.Weapons.Pumpkin     ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class PumkinWeaver : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pumpkin Weaver");
            Tooltip.SetDefault("Shoots a vine with exploding pumpkins attached!");
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun



        }

        public override void SetDefaults()
        {

            item.damage = 30;
            item.mana = 40;
            item.width = 46;
            item.height = 42;
            item.useTime = 60;
            item.useAnimation = 60;

            //item.reuseDelay = 60;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1f;
            item.value = 1000;
            item.rare = 1;
            item.UseSound = SoundID.Item43;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("Vine");
            item.magic = true;
            item.shootSpeed = 14;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Pumpkin, 30);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int p = 0; p < 1000; p++)
            {
                if ((Main.projectile[p].type == mod.ProjectileType("Vine") || Main.projectile[p].type == mod.ProjectileType("ExplodingPumpkin")) && Main.projectile[p].owner == player.whoAmI)
                {
                    Main.projectile[p].Kill();
                }
            }
            return true;
        }

    }
    public class Vine : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PumpkinVine");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 8;
            projectile.height = 8;
            //projectile.extraUpdates = 1;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;

            projectile.timeLeft = 1200;


        }
        float vineDirection;
        bool runOnce = true;
        float Length;
        bool dontPumpkin;
        float pumkinTimer;
        public override void AI()
        {
            if (runOnce)
            {
                vineDirection = projectile.velocity.ToRotation();
                projectile.velocity = Vector2.Zero;
                runOnce = false;
            }
            if (Length < 200 * (float)Math.PI)
            {
                Length += (float)Math.PI * 2;
            }

            pumkinTimer += Length / 20;
            if (Length > (float)Math.PI * 10 && pumkinTimer > (float)Math.PI * 100)
            {

                float s = Main.rand.NextFloat(Length);
                Vector2 offset = QwertyMethods.PolarVector(s, vineDirection) + QwertyMethods.PolarVector((float)Math.Sin(s / 30) * 40, vineDirection + (float)Math.PI / 2);
                Projectile pumkin = Main.projectile[Projectile.NewProjectile(projectile.Center + offset, Vector2.Zero, mod.ProjectileType("ExplodingPumpkin"), projectile.damage, projectile.knockBack, projectile.owner, projectile.whoAmI)];
                pumkin.rotation = (float)Math.Atan((float)Math.Cos(s / 30) * (1f / 3f)) + vineDirection;
                if (-(float)Math.Sin(s / 30) * (1f / 3f) * (1f / 30f) > 0) // this is the second derivitive which will tell us concavity
                {
                    pumkin.rotation += (float)Math.PI;
                }
                pumkinTimer = 0;
            }


        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            for (float s = 0; s < Length; s += (float)Math.PI / 2)
            {
                Vector2 offset = QwertyMethods.PolarVector(s, vineDirection) + QwertyMethods.PolarVector((float)Math.Sin(s / 30) * 20, vineDirection + (float)Math.PI / 2);
                spriteBatch.Draw(mod.GetTexture("Items/Weapons/Pumpkin/Vine"), new Vector2(projectile.Center.X - Main.screenPosition.X + offset.X, projectile.Center.Y - Main.screenPosition.Y + offset.Y),
                        new Rectangle(0, 0, projectile.width, projectile.height), lightColor, (float)Math.Atan((float)Math.Cos(s / 30) * (2f / 3f)) + vineDirection,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }


    }
    public class ExplodingPumpkin : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exploding Pumpkin");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 16;
            projectile.height = 16;
            //projectile.extraUpdates = 1;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;

            projectile.timeLeft = 120;


        }
        public override void AI()
        {
            projectile.scale += (1f / 60f);
            Projectile parent = Main.projectile[(int)projectile.ai[0]];
            if (!parent.active || parent.type != mod.ProjectileType("Vine"))
            {
                projectile.Kill();
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 100; i++)
            {
                //Main.PlaySound(SoundID.NPCDeath1);
                Dust.NewDust(QwertyMethods.PolarVector(Main.rand.Next(30), Main.rand.NextFloat((float)Math.PI * 2)) + projectile.Center, 0, 0, mod.DustType("PumpkinDust"));
            }
            Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("PumpkinBlast"), projectile.damage, projectile.knockBack, projectile.owner);
        }



    }
    public class PumpkinBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pumpkin Blast");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 40;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 2;
            projectile.usesLocalNPCImmunity = true;


        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }
    }




}