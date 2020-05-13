using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.HydraBoss
{
	// The following laser shows a channeled ability, after charging up the laser will be fired
	// Using custom drawing, dust effects, and custom collision checks for tiles
	public class HydraBeamT : ModProjectile
	{
		//The distance charge particle from the player center
		private const float MoveDistance = 70f;

		// The actual distance is stored in the ai0 field
		// By making a property to handle this it makes our life easier, and the accessibility more readable
		public float Distance;

		public NPC shooter;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Beam");
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.hide = false;
		}

		// The AI of the projectile
		public bool runOnce = true;

		public override void AI()
		{
			float rOffset = 0;
			shooter = Main.npc[(int)projectile.ai[0]];

			Vector2 mousePos = Main.MouseWorld;
			Player player = Main.player[projectile.owner];
			if (!shooter.active || shooter.life <= 0)
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

			#endregion Set projectile position

			#region Charging process

			// Kill the projectile if the player stops channeling

			// Do we still have enough mana? If not, we kill the projectile because we cannot use it anymore

			Vector2 offset = projectile.velocity;
			offset *= MoveDistance - 20;
			Vector2 pos = new Vector2(shooter.Center.X, shooter.Center.Y) + offset - new Vector2(10, 10);

			#endregion Charging process

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

			#endregion Draw laser body

			#region Draw laser tail

			spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
				new Rectangle(0, 0, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);

			#endregion Draw laser tail

			#region Draw laser head

			spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition,
				new Rectangle(0, 52, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);

			#endregion Draw laser head
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

		/*
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = projectile.velocity;
            Utils.PlotTileLine(projectile.Center, projectile.Center + unit * Distance, (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
        }
        */
	}
}