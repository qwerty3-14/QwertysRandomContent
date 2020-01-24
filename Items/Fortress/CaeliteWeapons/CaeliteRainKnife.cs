using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.CaeliteWeapons
{
    public class CaeliteRainKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Divine Hail Knife");
            Tooltip.SetDefault("Higher beings will throw these from the sky!");

        }
        public override void SetDefaults()
        {
            item.damage = 13;
            item.thrown = true;
            item.knockBack = 1;
            item.value = 50;
            item.rare = 3;
            item.width = 14;
            item.height = 34;
            item.useStyle = 1;
            item.shootSpeed = 12f;
            item.useTime = 4;
            item.useAnimation = 12;
            item.consumable = true;
            item.shoot = mod.ProjectileType("CaeliteRainKnifeP");
            item.noUseGraphic = true;
            item.noMelee = true;
            item.maxStack = 999;
            item.autoReuse = true;



        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 333);
            recipe.AddRecipe();
        }
        public int shotCounter = 2;
        public bool consumeKnife;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            position = new Vector2(Main.MouseWorld.X + Main.rand.Next(-100, 100), position.Y - 600);
            float trueSpeed = new Vector2(speedX, speedY).Length();
            int shift = Main.rand.Next(-100, 100);
            speedX = (float)Math.Cos((new Vector2(Main.MouseWorld.X + shift, Main.MouseWorld.Y) - position).ToRotation()) * trueSpeed;
            speedY = (float)Math.Sin((new Vector2(Main.MouseWorld.X + shift, Main.MouseWorld.Y) - position).ToRotation()) * trueSpeed;
            shotCounter++;
            if (shotCounter % 3 == 0)
            {
                consumeKnife = true;
            }
            else
            {
                consumeKnife = false;
            }
            return true;
        }
        public override bool ConsumeItem(Player player)
        {


            return consumeKnife;
        }
        public class CaeliteRainKnifeP : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Caelite Rain Knife");


            }
            public override void SetDefaults()
            {
                projectile.aiStyle = 1;
                //aiType = ProjectileID.Bullet;
                projectile.width = 18;
                projectile.height = 18;
                projectile.friendly = true;
                projectile.penetrate = -1;
                projectile.thrown = true;

                projectile.usesLocalNPCImmunity = true;

                projectile.tileCollide = false;


            }
            bool runOnce = true;
            float outOfPhaseHeight;
            public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
            {
                if (Main.rand.Next(10) == 0)
                {
                    target.AddBuff(mod.BuffType("PowerDown"), 120);
                }
                projectile.localNPCImmunity[target.whoAmI] = -1;
                target.immune[projectile.owner] = 0;
            }
            public override void AI()
            {
                if (runOnce)
                {
                    outOfPhaseHeight = Main.MouseWorld.Y;
                    runOnce = false;
                }

                if (projectile.Center.Y > outOfPhaseHeight)
                {
                    projectile.tileCollide = true;
                }
                //Main.NewText(outOfPhaseHeight);
            }
            public override void Kill(int timeLeft)
            {
                for (int i = 0; i < 6; i++)
                {

                    Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"))];


                }
            }






        }

    }
}

