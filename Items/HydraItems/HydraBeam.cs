using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
    public class HydraBeam : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Beam");
            Tooltip.SetDefault("Creates a beam of destructive energy from the sky");

        }
        public override void SetDefaults()
        {
            item.damage = 60;
            item.magic = true;

            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = 5;
            item.knockBack = 40;
            item.value = 250000;
            item.rare = 5;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.width = 28;
            item.height = 30;

            item.mana = 80;
            item.shoot = mod.ProjectileType("BeamHead");
            item.shootSpeed = 9;
            item.noMelee = true;




        }
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            for (int l = 0; l < Main.projectile.Length; l++)
            {                                                                  //this make so you can only spawn one of this projectile at the time,
                Projectile proj = Main.projectile[l];
                if (proj.active && proj.type == item.shoot && proj.owner == player.whoAmI)
                {
                    proj.active = false;
                }
            }
            return true;
        }


    }
    public class BeamHead : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beam Head");


        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            projectile.timeLeft = 300;
            projectile.width = 72;
            projectile.height = 112;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;

        }
        public bool runOnce = true;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (runOnce)
            {

                projectile.position = new Vector2(player.Center.X, player.Center.Y - 900); ;
                runOnce = false;
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType("HeadBeam"), projectile.damage, projectile.knockBack, projectile.owner, projectile.whoAmI, 0f);
            }



            projectile.velocity.X = 10f * player.direction;
            projectile.position.Y = player.Center.Y - 900;
        }


    }
    public class HeadBeam : ModProjectile
    {
        //The distance charge particle from the player center
        private const float MoveDistance = 70f;

        // The actual distance is stored in the ai0 field
        // By making a property to handle this it makes our life easier, and the accessibility more readable
        public float Distance;


        public Projectile shooter;
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            shooter = Main.projectile[(int)projectile.ai[0]];
            hitDirection = shooter.velocity.X > 0 ? 1 : -1;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Beam");
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.hostile = false;
            projectile.hide = false;
        }
        // The AI of the projectile
        public bool runOnce = true;
        public override void AI()
        {


            float rOffset = (float)Math.PI / 2;
            shooter = Main.projectile[(int)projectile.ai[0]];

            Vector2 mousePos = Main.MouseWorld;
            Player player = Main.player[projectile.owner];
            if (!shooter.active)
            {
                projectile.Kill();
            }

            #region Set projectile position


            Vector2 diff = new Vector2((float)Math.Cos(shooter.rotation + rOffset) * 14f, (float)Math.Sin(shooter.rotation + rOffset) * 14f);
            diff.Normalize();
            projectile.velocity = diff;
            projectile.direction = projectile.Center.X > shooter.Center.X ? 1 : -1;
            projectile.netUpdate = true;

            projectile.position = new Vector2(shooter.Center.X, shooter.Center.Y) + projectile.velocity * MoveDistance;
            projectile.timeLeft = 2;
            int dir = projectile.direction;
            /*
            player.ChangeDir(dir);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
            */
            #endregion

            #region Charging process
            // Kill the projectile if the player stops channeling


            // Do we still have enough mana? If not, we kill the projectile because we cannot use it anymore

            Vector2 offset = projectile.velocity;
            offset *= MoveDistance - 20;
            Vector2 pos = new Vector2(shooter.Center.X, shooter.Center.Y) + offset - new Vector2(10, 10);





            #endregion



            Vector2 start = new Vector2(shooter.Center.X, shooter.Center.Y);
            Vector2 unit = projectile.velocity;
            unit *= -1;
            for (Distance = MoveDistance; Distance <= 2200f; Distance += 5f)
            {
                start = new Vector2(shooter.Center.X, shooter.Center.Y) + projectile.velocity * Distance;

            }





        }
        public int colorCounter;
        public Color lineColor;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {


            DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], shooter.Center,
                projectile.velocity, 10, projectile.damage, -1.57f, 1f, 4000f, Color.White, (int)MoveDistance);


            return false;
        }

        // The core function of drawing a laser
        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 4000f, Color color = default(Color), int transDist = 50)
        {
            Vector2 origin = start;
            float r = unit.ToRotation() + rotation;

            #region Draw laser body
            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                origin = start + i * unit;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 26, 28, 26), i < transDist ? Color.Transparent : c, r,
                    new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            }
            #endregion

            #region Draw laser tail
            spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(0, 0, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            #endregion

            #region Draw laser head
            spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 52, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            #endregion
        }

        // Change the way of collision check of the projectile
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // We can only collide if we are at max charge, which is when the laser is actually fired

            Player player = Main.player[projectile.owner];
            Vector2 unit = projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), new Vector2(shooter.Center.X, shooter.Center.Y),
                new Vector2(shooter.Center.X, shooter.Center.Y) + unit * Distance, 22, ref point);

            return false;
        }

        // Set custom immunity time on hitting an NPC
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 5;
        }



        public override bool ShouldUpdatePosition()
        {
            return false;
        }

    }

}

