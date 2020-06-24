using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.AbstractClasses
{
    public abstract class Grenade : ModProjectile
    {
        //modified version of how vanilla does the grenade
        protected bool sticky = false;

        protected float bouncyness = .4f;
        protected int explosionSize = 128;

        public override void AI()
        {
            if (sticky)
            {
                projectile.tileCollide = false;
                int num223 = (int)(projectile.position.X / 16f) - 1;
                int num224 = (int)((projectile.position.X + (float)projectile.width) / 16f) + 2;
                int num225 = (int)(projectile.position.Y / 16f) - 1;
                int num226 = (int)((projectile.position.Y + (float)projectile.height) / 16f) + 2;
                if (num223 < 0)
                {
                    num223 = 0;
                }
                if (num224 > Main.maxTilesX)
                {
                    num224 = Main.maxTilesX;
                }
                if (num225 < 0)
                {
                    num225 = 0;
                }
                if (num226 > Main.maxTilesY)
                {
                    num226 = Main.maxTilesY;
                }
                for (int num227 = num223; num227 < num224; num227++)
                {
                    for (int num228 = num225; num228 < num226; num228++)
                    {
                        if (Main.tile[num227, num228] != null && Main.tile[num227, num228].nactive() && (Main.tileSolid[(int)Main.tile[num227, num228].type] || (Main.tileSolidTop[(int)Main.tile[num227, num228].type] && Main.tile[num227, num228].frameY == 0)))
                        {
                            Vector2 vector19;
                            vector19.X = (float)(num227 * 16);
                            vector19.Y = (float)(num228 * 16);
                            if (projectile.position.X + (float)projectile.width - 4f > vector19.X && projectile.position.X + 4f < vector19.X + 16f && projectile.position.Y + (float)projectile.height - 4f > vector19.Y && projectile.position.Y + 4f < vector19.Y + 16f)
                            {
                                projectile.velocity.X = 0f;
                                projectile.velocity.Y = -0.2f;
                            }
                        }
                    }
                }
            }
            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 10f)
            {
                projectile.ai[0] = 10f;
                if (projectile.velocity.Y == 0f && projectile.velocity.X != 0f)
                {
                    projectile.velocity.X = projectile.velocity.X * 0.97f;
                    if (projectile.type == 29 || projectile.type == 470 || projectile.type == 637)
                    {
                        projectile.velocity.X = projectile.velocity.X * 0.99f;
                    }
                    if ((double)projectile.velocity.X > -0.01 && (double)projectile.velocity.X < 0.01)
                    {
                        projectile.velocity.X = 0f;
                        projectile.netUpdate = true;
                    }
                }
                projectile.velocity.Y = projectile.velocity.Y + 0.2f;
            }
            projectile.rotation += projectile.velocity.X * 0.1f;

            if (projectile.owner == Main.myPlayer && projectile.timeLeft <= 3)
            {
                if (ExplosionArea((int)(explosionSize * Main.player[projectile.owner].GetModPlayer<QwertyPlayer>().GrenadeExplosionModifier)))
                {
                    projectile.tileCollide = false;
                    projectile.ai[1] = 0f;
                    projectile.alpha = 255;
                    projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
                    projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
                    projectile.width = (int)(explosionSize * Main.player[projectile.owner].GetModPlayer<QwertyPlayer>().GrenadeExplosionModifier);
                    projectile.height = (int)(explosionSize * Main.player[projectile.owner].GetModPlayer<QwertyPlayer>().GrenadeExplosionModifier);
                    projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                    projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
                    projectile.knockBack = 8f;

                    projectile.FriendlyFire();
                }
            }
            ExtraAI();
        }

        public virtual void ExtraAI()
        {
        }

        public virtual bool ExplosionArea(int explosionSize) //used to handle damage
        {
            return true;
        }

        public virtual bool ExplosionEffect(int explosionSize) //used for ending effects
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            if (ExplosionEffect((int)(explosionSize * Main.player[projectile.owner].GetModPlayer<QwertyPlayer>().GrenadeExplosionModifier)))
            {
                Main.PlaySound(SoundID.Item14, projectile.position);
                projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
                int visSize = (int)(explosionSize * Main.player[projectile.owner].GetModPlayer<QwertyPlayer>().GrenadeExplosionModifier) / 6 + 6;
                projectile.width = visSize;
                projectile.height = visSize;
                projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
                for (int d = 0; d < (visSize * visSize) / 24; d++)
                {
                    int index = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[index].velocity *= 1.4f;
                }
                for (int d = 0; d < (visSize * visSize) / 48; d++)
                {
                    int index = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2.5f);
                    Main.dust[index].noGravity = true;
                    Main.dust[index].velocity *= (visSize * .25f);
                    index = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[index].velocity *= (visSize / 7f);
                }
                for (int g = 0; g < 4; g++)
                {
                    Gore gore = Main.gore[Gore.NewGore(new Vector2(projectile.Center.X, projectile.Center.Y), default(Vector2), Main.rand.Next(61, 64), 1f)];
                    gore.velocity *= .4f;
                    gore.velocity.X = gore.velocity.X + (g % 2 == 0 ? 1 : -1);
                    gore.velocity.Y = gore.velocity.Y + (g < 2 ? 1 : -1);
                }
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = !sticky;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.timeLeft > 3)
            {
                projectile.timeLeft = 3;
            }
            if (target.Center.X < projectile.Center.X)
            {
                projectile.direction = -1;
            }
            else
            {
                projectile.direction = 1;
            }
        }

        public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
        {
            if (projectile.timeLeft > 3)
            {
                projectile.timeLeft = 3;
            }
            if (target.Center.X < projectile.Center.X)
            {
                projectile.direction = -1;
            }
            else
            {
                projectile.direction = 1;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (sticky)
            {
                projectile.velocity = Vector2.UnitY * -.2f;
            }
            else
            {
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = oldVelocity.X * -bouncyness;
                }
                if (projectile.velocity.Y != oldVelocity.Y && (double)oldVelocity.Y > 0.7)
                {
                    projectile.velocity.Y = oldVelocity.Y * -bouncyness;
                }
            }

            return false;
        }
    }

    public class ReplaceGrenade : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Grenade)
            {
                item.shoot = mod.ProjectileType("ModifiedGrenade");
            }
            if (item.type == ItemID.StickyGrenade)
            {
                item.shoot = mod.ProjectileType("ModifiedStickyGrenade");
            }
            if (item.type == ItemID.BouncyGrenade)
            {
                item.shoot = mod.ProjectileType("ModifiedBouncyGrenade");
            }
        }
    }

    public class ModifiedGrenade : Grenade
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grenade");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.thrown = true;
            projectile.timeLeft = 180;
            sticky = false;
            bouncyness = .4f;
            explosionSize = 128;
        }
    }

    public class ModifiedStickyGrenade : Grenade
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sticky Grenade");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.thrown = true;
            projectile.timeLeft = 180;
            sticky = true;
            bouncyness = .4f;
            explosionSize = 128;
        }
    }

    public class ModifiedBouncyGrenade : Grenade
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bouncy Grenade");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.thrown = true;
            projectile.timeLeft = 180;
            sticky = false;
            bouncyness = .9f;
            explosionSize = 128;
        }
    }
}