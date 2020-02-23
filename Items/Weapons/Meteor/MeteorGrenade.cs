using Microsoft.Xna.Framework;
using QwertysRandomContent.AbstractClasses;
using System;
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
    public class MeteorGrenadeP : Grenade
    {
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            //projectile.aiStyle = 16;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.thrown = true;
            projectile.timeLeft = 180;
            //aiType = ProjectileID.Grenade;
            sticky = false;
            bouncyness = .4f;
            explosionSize = 128;
        }
        public override bool ExplosionEffect(int explosionSize)
        {
            Main.PlaySound(SoundID.Item62, projectile.position);
            for (int i = 0; i < 2; i++)
            {
                Projectile meteor = Main.projectile[Projectile.NewProjectile(projectile.Center + new Vector2(Main.rand.Next(-20, 20), -1000), QwertyMethods.PolarVector(10, (float)Math.PI / 2 + (float)Math.PI / 16 * Main.rand.NextFloat(-1, 1)), 424 + Main.rand.Next(2), (int)(projectile.damage), projectile.knockBack, projectile.owner, 0f, 0.5f + (float)Main.rand.NextDouble() * 0.3f)];
                meteor.magic = false;
                meteor.thrown = true;

                if (Main.netMode == 1)
                {
                    QwertysRandomContent.UpdateProjectileClass(meteor);
                }

            }
            return true;
        }
    }
}
