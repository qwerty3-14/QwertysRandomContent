using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.CloakedDarkBoss
{
	public class Warning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 50;
			projectile.timeLeft = 60;
			projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
			projectile.tileCollide = false;
		}

		public override void AI()
		{
			projectile.rotation = projectile.ai[1];
		}

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			drawCacheProjsOverWiresUI.Add(index);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, new Rectangle(0, (int)projectile.ai[0] * 50, 50, 50), Color.White, projectile.rotation, new Vector2(25, 25), 1f, SpriteEffects.None, 0);
			return false;
		}
	}
}