using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.DinoItems
{
    public class TheTyrantsExtinctionGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Tyrant's Extinction Gun");
            Tooltip.SetDefault("Left click shoots climate change!" + "\nRight click shoots disease!");

        }
        public override void SetDefaults()
        {
            item.damage = 48;
            item.magic = true;
            item.knockBack = 1;
            item.rare = 6;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.width = 66;
            item.height = 24;
            item.useStyle = 5;
            item.shootSpeed = 10f;
            item.useTime = 10;
            item.useAnimation = 10;
            item.mana = 5;
            item.shoot = mod.ProjectileType("SnowFlakeF");
            item.noUseGraphic = false;
            item.noMelee = true;
            //item.UseSound = 16
            item.autoReuse = true;



        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.shoot = mod.ProjectileType("MosquittoF");
            }
            else
            {
                item.shoot = mod.ProjectileType("SnowFlakeF");
            }
                Main.PlaySound(16, player.Center, 0);
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 28f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Vector2 trueSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
            speedX = trueSpeed.X;
            speedY = trueSpeed.Y;
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, -4);
        }

    }
    public class MosquittoF : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mosquitto");
            Main.projFrames[projectile.type] = 4;

        }
        public override void SetDefaults()
        {
            projectile.magic = true;
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = 36;
            projectile.friendly = true;
            projectile.penetrate = 3;
            projectile.alpha = 255;
            projectile.timeLeft = 600;
            projectile.extraUpdates = 3;
            aiType = ProjectileID.Bee;
            //animationType = ProjectileID.Bee;

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("DinoPox"), 480);
        }





    }
    public class SnowFlakeF : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snow Flake");


        }
        public override void SetDefaults()
        {
            projectile.magic = true;
            projectile.aiStyle = 0;

            projectile.width = 22;
            projectile.height = 22;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;

        }


        public override void AI()
        {
            projectile.rotation += 1.5f;
        }
        public override bool OnTileCollide(Vector2 velocityChange)
        {
            for (int k = 0; k < 200; k++)
            {
                projectile.localNPCImmunity[k] = 0;
            }
            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.velocity.X = -velocityChange.X;
            }
            if (projectile.velocity.Y != velocityChange.Y)
            {
                projectile.velocity.Y = -velocityChange.Y;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }


    }
}

