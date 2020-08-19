using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace QwertysRandomContent.AbstractClasses
{
    public abstract class StableMorph : ModProjectile
    {
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void SetDefaults()
        {
            SetSafeDefaults();
            projectile.timeLeft = 2;
        }

        public virtual void SetSafeDefaults()
        {
            projectile.width = 20;
            projectile.height = 42;
        }

        protected string buffName = "GodOfBlasphemyB";
        protected string itemName = "GodOfBlasphemy";
        private bool runOnce = true;
        private int shader = 0;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                shader = Main.player[projectile.owner].miscDyes[3].dye;
                //Main.NewText(shader);
                runOnce = false;
            }
            if (player.HasBuff(mod.BuffType(buffName)) && player.HeldItem.type == mod.ItemType(itemName))
            {
                projectile.timeLeft = 2;

                player.GetModPlayer<ShapeShifterPlayer>().morphed = true;
                player.GetModPlayer<ShapeShifterPlayer>().overrideWidth = projectile.width;
                player.GetModPlayer<ShapeShifterPlayer>().overrideHeight = projectile.height;
                player.itemTime = 2;
                player.itemAnimation = 2;
                player.noFallDmg = true;
                player.gravity = 0;
                player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
                player.statDefense = player.HeldItem.GetGlobalItem<ShapeShifterItem>().morphDef + player.GetModPlayer<ShapeShifterPlayer>().morphDef + player.HeldItem.GetGlobalItem<ShapeShifterItem>().prefixMorphDef;
                player.jumpSpeedBoost *= 0;
                player.wingTime = 0;
                player.rocketTime = 0;
                Effects(player);

                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && Main.projHook[Main.projectile[i].type])
                    {
                        Main.projectile[i].Kill();
                    }
                }
                if (player.mount.Active)
                {
                    player.mount.Dismount(player);
                }

                if (projectile.velocity.X != 0)
                {
                    projectile.direction = projectile.spriteDirection = player.direction = Math.Sign(projectile.velocity.X);
                }
                player.velocity = Vector2.Zero;
                player.velocity.Y = 1E-05f;
                if (Running())
                {
                    if (projectile.velocity.Y == 0)
                    {
                        if (player.controlJump)
                        {
                            jumpTime = jumpHeight;
                        }
                    }
                    if (jumpTime > 0 && player.controlJump)
                    {
                        jumpTime--;
                        projectile.velocity.Y = -jumpSpeed;
                    }
                    else
                    {
                        jumpTime = 0;
                        projectile.velocity.Y += .4f;
                        if (projectile.velocity.Y > 10)
                        {
                            projectile.velocity.Y = 10;
                        }
                    }

                    if (player.controlRight)
                    {
                        projectile.velocity.X += acceleration;
                    }
                    else if (player.controlLeft)
                    {
                        projectile.velocity.X -= acceleration;
                    }
                    else
                    {
                        projectile.velocity.X *= .9f;
                    }
                    if (projectile.velocity.X > speed)
                    {
                        projectile.velocity.X = speed;
                    }
                    if (projectile.velocity.X < -speed)
                    {
                        projectile.velocity.X = -speed;
                    }
                }
                Movement(player);
                player.Center = projectile.Center;
                player.GetModPlayer<ShapeShifterPlayer>().stableMorphCenter = projectile.Center;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.player[projectile.owner].Bottom = projectile.Bottom;
        }

        public virtual void Effects(Player player)
        {
        }

        public virtual void Movement(Player player)
        {
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = Main.player[projectile.owner].controlDown;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        private int jumpTime;
        protected float speed;
        protected float acceleration;
        protected int jumpHeight;
        protected float jumpSpeed;

        public virtual bool Running()
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            // As mentioned above, be sure not to forget this step.
            Player player = Main.player[projectile.owner];
            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];

            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
            }
            return DrawMorphExtras(spriteBatch, lightColor);
        }

        public virtual bool DrawMorphExtras(SpriteBatch spriteBatch, Color lightColor)
        {
            return true;
        }
    }
}