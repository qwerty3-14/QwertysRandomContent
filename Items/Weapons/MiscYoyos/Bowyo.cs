
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscYoyos
{
    public class Bowyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bowyo");
            Tooltip.SetDefault("Fires arrows at nearby enemies");

            // These are all related to gamepad controls and don't seem to affect anything else
            ItemID.Sets.Yoyo[item.type] = true;
            ItemID.Sets.GamepadExtraRange[item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.width = 36;
            item.height = 32;
            item.useAnimation = 25;
            item.useTime = 25;
            item.shootSpeed = 16f;
            item.knockBack = 2.5f;
            item.damage = 90;
            item.value = 500000;
            item.rare = 9;
            item.melee = true;
            item.channel = true;
            item.noMelee = true;
            item.noUseGraphic = true;

            item.UseSound = SoundID.Item1;
            item.useAmmo = AmmoID.Arrow;
            item.shoot = mod.ProjectileType("BowyoP");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);


            recipe.AddIngredient(mod.ItemType("CraftingRune"), 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool ConsumeAmmo(Player player)
        {
            return false;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //reset damage and knockback to avoid ammo modifing
            damage = (int)(item.damage * player.meleeDamage);
            knockBack = item.knockBack * knockBack;
            type = mod.ProjectileType("BowyoP");
            return true;
        }

    }


    public class BowyoP : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // The following sets are only applicable to yoyo that use aiStyle 99.
            // YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player. 
            // Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = -1f;
            // YoyosMaximumRange is the maximum distance the yoyo sleep away from the player. 
            // Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 300f;
            // YoyosTopSpeed is top speed of the yoyo projectile. 
            // Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 18f;
        }

        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 28;
            projectile.height = 28;
            // aiStyle 99 is used for all yoyos, and is Extremely suggested, as yoyo are extremely difficult without them
            projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1f;
        }
        // notes for aiStyle 99: 
        // localAI[0] is used for timing up to YoyosLifeTimeMultiplier
        // localAI[1] can be used freely by specific types
        // ai[0] and ai[1] usually point towards the x and y world coordinate hover point
        // ai[0] is -1f once YoyosLifeTimeMultiplier is reached, when the player is stoned/frozen, when the yoyo is too far away, or the player is no longer clicking the shoot button.
        // ai[0] being negative makes the yoyo move back towards the player
        // Any AI method can be used for dust, spawning projectiles, etc specific to your yoyo.
        public int arrow = 1;
        public bool canShoot = true;
        public float speedB = 14f;
        public float BulVel = 12;
        NPC possibleTarget;
        NPC target;
        float distance;
        float maxDistance = 1000;
        bool foundTarget;
        int timer;
        float dir;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            for (int k = 0; k < 200; k++)
            {
                possibleTarget = Main.npc[k];
                distance = (possibleTarget.Center - projectile.Center).Length();
                if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0))
                {
                    target = Main.npc[k];
                    foundTarget = true;


                    maxDistance = (target.Center - projectile.Center).Length();
                }

            }
            timer++;
            if (foundTarget)
            {
                dir = (target.Center - projectile.Center).ToRotation();
                if (timer > 20)
                {
                    int weaponDamage = projectile.damage;
                    float weaponKnockback = projectile.knockBack;
                    player.PickAmmo(QwertyMethods.MakeItemFromID(ItemID.WoodenBow), ref arrow, ref speedB, ref canShoot, ref weaponDamage, ref weaponKnockback, Main.rand.Next(2) == 0);
                    if (Main.netMode != 1)
                    {
                        Projectile bul = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(dir) * BulVel, (float)Math.Sin(dir) * BulVel, arrow, weaponDamage, weaponKnockback, Main.myPlayer)];
                        bul.melee = true;
                        bul.ranged = false;
                    }

                    timer = 0;
                }

            }

            maxDistance = 1000;
            foundTarget = false;
        }
    }
}
