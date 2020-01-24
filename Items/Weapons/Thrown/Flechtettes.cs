using Microsoft.Xna.Framework;
using QwertysRandomContent.AbstractClasses;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Thrown
{
    public class Flechettes : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flechettes");
            Tooltip.SetDefault("Flechettes do more damage as they pick up speed from gravity");

        }
        public override void SetDefaults()
        {
            item.damage = 9;
            item.thrown = true;
            item.knockBack = 5;
            item.value = 15;
            item.rare = 1;
            item.width = 14;
            item.height = 26;
            item.useStyle = 1;
            item.shootSpeed = 5f;
            item.useTime = 9;
            item.useAnimation = 18;
            item.consumable = true;
            item.shoot = mod.ProjectileType("FlechetteP");
            item.noUseGraphic = true;
            item.noMelee = true;
            item.maxStack = 999;
            item.autoReuse = true;
            item.UseSound = SoundID.Item39;


        }
        public override bool ConsumeItem(Player player)
        {
            return Main.rand.Next(2) == 0;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float speed = new Vector2(speedX, speedY).Length();
            int numberOfProjectiles = 2 + Main.rand.Next(2);

            for (int p = 0; p < numberOfProjectiles; p++)
            {
                float direction = Main.rand.NextFloat(5 * (float)Math.PI / 8, 3 * (float)Math.PI / 8);
                Projectile.NewProjectile(position, QwertyMethods.PolarVector(speed, direction), type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }
    public class FlechetteP : Flechette
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FlechetteP");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;

        }
        public override void SetDefaults()
        {

            projectile.width = 6;
            projectile.height = 6;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.thrown = true;

            projectile.tileCollide = true;


        }





    }

}

