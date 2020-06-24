using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Rhuthinium
{
    public class RhuthiniumScepter : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Scepter");
        }

        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 36;
            item.damage = 14;
            item.crit = 5;
            item.useTime = 5;
            item.useAnimation = 20;
            item.useTurn = false;
            item.noMelee = true; //can't hit as item since they have custom animation
            item.useStyle = 6; //Vanilla doesn't use this for anything but it still starts the useAnimation timer, good for custom animations
            item.mana = 16;
            item.autoReuse = true;
            item.shootSpeed = 8f;
            item.shoot = mod.ProjectileType("RhuthiniumBolt");
            item.UseSound = SoundID.Item39;
            item.knockBack = 1.2f;
            item.magic = true;
            item.value = 25000;
            item.rare = 3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        private Vector2 staveHoldOffset = new Vector2(0, -10);
        private float staveHoldRotation = (float)Math.PI / 8;

        public override void UseStyle(Player player)
        {
            player.bodyFrame.Y = player.bodyFrame.Height * 3;
            player.itemRotation = -1 * staveHoldRotation * player.direction;
            Vector2 vector24 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector24.X = player.bodyFrame.Width - vector24.X;
            }
            if (player.gravDir != 1f)
            {
                vector24.Y = player.bodyFrame.Height - vector24.Y;
            }
            vector24 -= new Vector2(player.bodyFrame.Width - player.width, player.bodyFrame.Height - 42) / 2f;
            player.itemLocation = player.position + vector24;
            float trueRotation = (float)Math.PI / 2 - player.itemRotation + (float)Math.PI;
            player.itemLocation += new Vector2((float)Math.Cos(trueRotation), (float)Math.Sin(trueRotation)) * staveHoldOffset.Y;
            player.itemLocation += new Vector2((float)Math.Cos(trueRotation + (float)Math.PI / 2), (float)Math.Sin(trueRotation + (float)Math.PI / 2)) * staveHoldOffset.X * player.direction;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = new Vector2(player.Center.X + (float)Main.rand.Next(-100, 101), player.Center.Y + (float)Main.rand.Next(-100, 101) - 600);
            Vector2 speed = QwertyMethods.PolarVector(item.shootSpeed, (Main.MouseWorld - position).ToRotation() + (float)Math.PI / 16 - (float)Math.PI / 8 * Main.rand.NextFloat());
            speedX = speed.X;
            speedY = speed.Y;
            return true;
        }
    }

    public class RhuthiniumBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Bolt");
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 10;
            projectile.extraUpdates = 2;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.magic = true;
        }

        private NPC target;
        private bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                runOnce = false;
                projectile.ai[0] = Main.rand.Next(2);
            }
            if (true)
            {
                Dust d = Dust.NewDustPerfect(projectile.Center, mod.DustType("RhuthiniumDust"), Vector2.Zero);
                d.frame.Y = (int)(projectile.ai[0] * 10);
                d.noGravity = true;
                d.velocity = Vector2.Zero;
            }
            if (QwertyMethods.ClosestNPC(ref target, 250, projectile.Center))
            {
                float rot = (projectile.velocity.ToRotation());
                rot.SlowRotation((target.Center - projectile.Center).ToRotation(), (float)Math.PI / 60);
                projectile.velocity = QwertyMethods.PolarVector(10, rot);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
    }
}