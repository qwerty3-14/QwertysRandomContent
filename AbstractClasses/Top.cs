using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.AbstractClasses
{
    public abstract class Top : ModProjectile
    {
        private bool runOnce = true;
        private float initVel;
        protected bool hitGround;
        private int timeOutTimer;
        protected float friction = .002666f;
        protected float enemyFriction = .1f;
        protected int frameDelay = 1;
        public override void AI()
        {
            if (runOnce)
            {
                initVel = (float)Math.Abs(projectile.velocity.Length());
                friction = friction * (initVel - 2);
                runOnce = false;
            }
            projectile.frameCounter++;
            if(projectile.frameCounter % (frameDelay *(initVel < 2 ? 2 : 1)) == 0)
            {
                projectile.frame++;
                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
            if (hitGround)
            {
                if (projectile.velocity.X < 0)
                {
                    projectile.velocity.X = -initVel;
                }
                else
                {
                    projectile.velocity.X = initVel;
                }

                if (initVel < 2)
                {
                    projectile.friendly = false;
                    initVel = .5f;
                    timeOutTimer++;
                    if (timeOutTimer > 325)
                    {
                        projectile.Kill();
                    }
                    else if (timeOutTimer > 255)
                    {
                        initVel = 0f;
                        projectile.rotation = (float)MathHelper.ToRadians(-45);
                        projectile.frame = 0;
                    }
                    else if (timeOutTimer > 180)
                    {
                        projectile.rotation = (float)MathHelper.ToRadians(210 - timeOutTimer);
                    }
                    else if (timeOutTimer > 120)
                    {
                        projectile.rotation = (float)MathHelper.ToRadians(timeOutTimer - 150);
                    }
                    else if (timeOutTimer > 60)
                    {
                        projectile.rotation = (float)MathHelper.ToRadians(90 - timeOutTimer);
                    }
                    else if (timeOutTimer > 30)
                    {
                        projectile.rotation = (float)MathHelper.ToRadians(timeOutTimer - 30);
                    }
                    else
                    {
                        projectile.rotation = 0;
                        projectile.rotation += (float)MathHelper.ToRadians(1);
                    }
                }
                else
                {
                    projectile.rotation = 0;

                    initVel -= friction * Main.player[projectile.owner].GetModPlayer<QwertyPlayer>().TopFrictionMultiplier;
                }
            }
            else
            {
                projectile.rotation = 0;
            }
            ExtraTopNonesense();
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            hitGround = true;
            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.velocity.X = -velocityChange.X;
                initVel -= friction * Main.player[projectile.owner].GetModPlayer<QwertyPlayer>().TopFrictionMultiplier * 60;
            }

            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.usesIDStaticNPCImmunity = true;
            int immutime = 10;
            Projectile.perIDStaticNPCImmunity[projectile.type][target.whoAmI] = (uint)(Main.GameUpdateCount + immutime);
            //target.immune[projectile.owner] = immutime;

            initVel -= enemyFriction;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            knockback = ((float)Math.Abs(projectile.velocity.X) / initVel) * projectile.knockBack;
            hitDirection = projectile.velocity.X > 0 ? -1 : 1;
            TopHit(target);
        }

        public virtual void ExtraTopNonesense()
        {
        }
        public virtual void TopHit(NPC target)
        {

        }
    }
}