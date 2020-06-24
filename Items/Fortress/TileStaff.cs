using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress
{
    public class TileStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tile Staff");
            Tooltip.SetDefault("Moving diagonally? What's that?");
        }

        public override void SetDefaults()
        {
            item.damage = 28;
            item.mana = 20;
            item.width = item.height = 32;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.rare = 4;
            item.shoot = mod.ProjectileType("TileMinion");
            item.summon = true;
            item.buffType = mod.BuffType("TileMinion");
            item.buffTime = 3600;
            item.knockBack = 2.5f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 SPos = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);   //this make so the projectile will spawn at the mouse cursor position
            position = SPos;

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

    public class TileMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tile Minion");
            Main.projFrames[projectile.type] = 3;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 18;
            projectile.minionSlots = 1;
            projectile.minion = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
            target.immune[projectile.owner] = 0;
        }

        private Vector2 flyTo;
        private NPC target;
        private int velocityTime = 0;
        private Vector2 direction;
        private const float maxSpeed = 20f;
        private float acceleration = maxSpeed / 30;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (player.GetModPlayer<MinionManager>().TileMinion)
            {
                projectile.timeLeft = 2;
            }

            if (QwertyMethods.ClosestNPC(ref target, 2000, projectile.Center, true, player.MinionAttackTargetNPC, delegate (NPC possibleTarget) { return Collision.CanHit(player.Center, 1, 1, possibleTarget.position, possibleTarget.width, possibleTarget.height); }))
            {
                flyTo = target.Center + target.velocity * 10 + new Vector2(Main.rand.Next(-60, 61), Main.rand.Next(-60, 61));
            }
            else
            {
                flyTo = player.Center + player.velocity * 10 + new Vector2(Main.rand.Next(-60, 61), Main.rand.Next(-60, 61));
            }
            if (velocityTime == 0)
            {
                Vector2 oldDirection = direction;

                direction = (flyTo - projectile.Center);
                if ((direction.Y > 0 && oldDirection.Y < 0) || (direction.X > 0 && oldDirection.X < 0) || (direction.Y < 0 && oldDirection.Y > 0) || (direction.X < 0 && oldDirection.X > 0) || oldDirection == Vector2.Zero)
                {
                    if (Math.Abs(direction.Y) > Math.Abs(direction.X))
                    {
                        direction = direction.Y > 0 ? Vector2.UnitY : -Vector2.UnitY;
                        projectile.frame = 0;
                    }
                    else
                    {
                        direction = direction.X > 0 ? Vector2.UnitX : -Vector2.UnitX;
                        if (direction.X > 0)
                        {
                            projectile.frame = 1;
                        }
                        else
                        {
                            projectile.frame = 2;
                        }
                    }
                }
                else
                {
                    direction = oldDirection;
                }

                if (direction != oldDirection)
                {
                    velocityTime = 10;
                    projectile.velocity = Vector2.Zero;
                }
            }
            else
            {
                velocityTime--;
            }
            projectile.velocity += direction * acceleration;
            if (projectile.velocity.Length() > maxSpeed)
            {
                projectile.velocity = direction * maxSpeed;
            }
            if ((player.Center - projectile.Center).Length() > 2000)
            {
                projectile.Center = player.Center;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.velocity.Length() > 16f)
            {
                Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                    Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                    spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
                }
            }
            return base.PreDraw(spriteBatch, lightColor);
        }
    }
}