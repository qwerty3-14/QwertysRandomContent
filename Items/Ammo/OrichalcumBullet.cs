using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
    public class OrichalcumBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orichalcum Bullet");
            Tooltip.SetDefault("Upon hitting an enemy flies toward another enemy");

        }
        public override void SetDefaults()
        {
            item.damage = 10;
            item.ranged = true;
            item.knockBack = 2;
            item.value = 1;
            item.rare = 3;
            item.width = 12;
            item.height = 18;

            item.shootSpeed = 32;

            item.consumable = true;
            item.shoot = mod.ProjectileType("OrichalcumBulletP");
            item.ammo = 97;
            item.maxStack = 999;


        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.OrichalcumBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }

    }
    public class OrichalcumBulletP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orichalcum Bullet");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = 2;
            projectile.ranged = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.extraUpdates = 1;


        }
        public bool runOnce = true;
        float maxSpeed;
        public override void AI()
        {
            if (runOnce)
            {

                maxSpeed = projectile.velocity.Length();
                runOnce = false;
            }
        }
        public bool firstHit = true;




        NPC ConfirmedTarget;
        NPC possibleTarget;
        float distance;
        float maxDistance = 1200;
        bool foundTarget;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;


            Player player = Main.player[projectile.owner];
            for (int k = 0; k < 200; k++)
            {
                possibleTarget = Main.npc[k];
                distance = (possibleTarget.Center - projectile.Center).Length();
                if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && projectile.localNPCImmunity[k] >= 0 && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0))
                {
                    ConfirmedTarget = Main.npc[k];
                    foundTarget = true;


                    maxDistance = (ConfirmedTarget.Center - projectile.Center).Length();
                }

            }
            if (foundTarget)
            {
                projectile.velocity = QwertyMethods.PolarVector(maxSpeed, (ConfirmedTarget.Center - projectile.Center).ToRotation());

            }
            else
            {
                projectile.Kill();
            }
            foundTarget = false;
            maxDistance = 300;


        }


    }




}

