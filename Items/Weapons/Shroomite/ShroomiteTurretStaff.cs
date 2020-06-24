using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace QwertysRandomContent.Items.Weapons.Shroomite
{
    public class ShroomiteTurretStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shrooite Turret Staff");
            Tooltip.SetDefault("Fire bullets from your inventory.\n50% chance not to consume ammo.");
        }

        public override void SetDefaults()
        {
            item.damage = 11;
            item.rare = 8;
            item.useStyle = 1;
            item.value = Item.sellPrice(gold: 1);
            item.width = item.height = 28;
            item.sentry = true;
            item.summon = true;
            item.mana = 20;
            item.useTime = item.useAnimation = 25;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("ShroomiteTurretBase");
            item.UseSound = SoundID.Item44;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ShroomiteBar, 18);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            Point point;
            Point origin = position.ToTileCoordinates();
            while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                {
                                            new Conditions.IsSolid()
                }), out point))
            {
                position.Y++;
                origin = position.ToTileCoordinates();
            }
            position.Y -= 8;
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
    }

    public class ShroomiteTurretBase : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 18;
            projectile.tileCollide = true;
            projectile.sentry = true;
            projectile.timeLeft = Projectile.SentryLifeTime;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        private float gunRotation = 0;
        private float aimRotation = 0;
        private bool runOnce = true;
        private NPC target;
        private Vector2 gunRotationOrigionOffset = Vector2.UnitY * -5;
        private int shotCooldown = 8;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.UpdateMaxTurrets();
            if (runOnce)
            {
                if (player.direction == -1)
                {
                    gunRotation = (float)Math.PI;
                }
                runOnce = false;
            }
            if (QwertyMethods.ClosestNPC(ref target, 1000, projectile.Center, false, player.MinionAttackTargetNPC))
            {
                aimRotation = (target.Center - projectile.Center).ToRotation();
                shotCooldown++;
                if (shotCooldown >= 12)
                {
                    if (QwertyMethods.AngularDifference(aimRotation, gunRotation) < (float)Math.PI / 8)
                    {
                        Shoot();
                    }
                }
            }
            else
            {
                aimRotation = player.direction == 1 ? 0 : (float)Math.PI;
            }
            gunRotation = QwertyMethods.SlowRotation(gunRotation, aimRotation, 5);
        }

        public int bullet = 1;
        public bool canShoot = true;
        public float speedB = 14f;

        private void Shoot()
        {
            Player player = Main.player[projectile.owner];
            Main.PlaySound(SoundID.Item11, projectile.position);
            int weaponDamage = projectile.damage;
            float weaponKnockback = projectile.knockBack;
            Item sItem = QwertyMethods.MakeItemFromID(ItemID.Handgun);
            sItem.damage = weaponDamage;
            player.PickAmmo(sItem, ref bullet, ref speedB, ref canShoot, ref weaponDamage, ref weaponKnockback, Main.rand.Next(2) == 0);
            Projectile bul = Main.projectile[Projectile.NewProjectile(projectile.Center + QwertyMethods.PolarVector(29, gunRotation), QwertyMethods.PolarVector(10, gunRotation), bullet, weaponDamage, weaponKnockback, Main.myPlayer)];
            bul.ranged = false;
            bul.minion = true;
            if (Main.netMode == 1)
            {
                QwertysRandomContent.UpdateProjectileClass(bul);
            }
            shotCooldown = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Items/Weapons/Shroomite/ShroomiteTurretGun");
            spriteBatch.Draw(texture, projectile.Center + gunRotationOrigionOffset - Main.screenPosition, null, lightColor, gunRotation, new Vector2(13, 11), projectile.scale, SpriteEffects.None, 0f);
            return true;
        }
    }
}