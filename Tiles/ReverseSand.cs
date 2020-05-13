using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Tiles
{
	public class ReverseSand : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			ModTranslation name = CreateMapEntryName();
			dustType = mod.DustType("DnasDust");

			AddMapEntry(Color.Blue, name);
			name.SetDefault("Dnas");
			drop = mod.ItemType("ReverseSand");
		}

		public override void RandomUpdate(int i, int j)
		{
			if (!Main.tile[i, j - 1].active())
			{
				WorldGen.KillTile(i, j, noItem: true);
				Projectile.NewProjectile(new Vector2(i, j) * 16 + new Vector2(8, 8), Vector2.Zero, mod.ProjectileType("ReverseSandBall"), 50, 0f, Main.myPlayer);
			}
		}

		public override void PlaceInWorld(int i, int j, Item item)
		{
			if (!Main.tile[i, j - 1].active())
			{
				WorldGen.KillTile(i, j, noItem: true);
				Projectile.NewProjectile(new Vector2(i, j) * 16 + new Vector2(8, 8), Vector2.Zero, mod.ProjectileType("ReverseSandBall"), 50, 0f, Main.myPlayer);
			}
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			Vector2 entityCoord = new Vector2(i, j) * 16 + new Vector2(8, 8);
			if (!Main.tile[i, j - 1].active())
			{
				WorldGen.KillTile(i, j, noItem: true);
				Projectile.NewProjectile(entityCoord, Vector2.Zero, mod.ProjectileType("ReverseSandBall"), 50, 0f, Main.myPlayer);
			}

			//if(Main.LocalPlayer.Top.Y- entityCoord.Y <16 && Main.LocalPlayer.Top.Y - entityCoord.Y >0 && Math.Abs(Main.LocalPlayer.Top.X-entityCoord.X)<16)
			{
				//Main.LocalPlayer.GetModPlayer<QwertyPlayer>().forcedAntiGravity = true;
				// Main.LocalPlayer.gravDir = -1f;
				//Main.LocalPlayer.gravControl2 = true;
			}
		}
	}

	public class ReverseSandBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			projectile.knockBack = 6f;
			projectile.width = 14;
			projectile.height = 14;
			//projectile.aiStyle = 10;
			projectile.friendly = true;
			projectile.hostile = true;
			projectile.penetrate = -1;
		}

		public override void AI()
		{
			// Main.NewText(projectile.width);
			projectile.width = 14;
			projectile.height = 14;
			if (Main.rand.Next(2) == 0)
			{
				int num129 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("DnasDust"), 0f, projectile.velocity.Y / 2f, 0, default(Color), 1f);
				Dust dust = Main.dust[num129];
				dust.velocity.X = dust.velocity.X * 0.4f;
			}

			projectile.tileCollide = true;
			projectile.localAI[1] = 0f;

			projectile.velocity.Y = projectile.velocity.Y - 0.41f;

			projectile.rotation -= 0.1f;

			if (projectile.velocity.Y < -10f)
			{
				projectile.velocity.Y = -10f;
			}
			//projectile.velocity = Collision.TileCollision(projectile.position, projectile.velocity, projectile.width, projectile.height, false, false, 1);
		}

		public override void Kill(int timeLeft)
		{
			int i = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
			int j = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16;
			int tileToPlace = 0;
			int num835 = 2;

			{
				tileToPlace = mod.TileType("ReverseSand");
				num835 = 0;
			}
			/*
            if (Main.tile[i, j].halfBrick() && projectile.velocity.Y > 0f && Math.Abs(projectile.velocity.Y) > Math.Abs(projectile.velocity.X))
            {
                j--;
            }*/
			if (!Main.tile[i, j].active() && tileToPlace >= 0)
			{
				WorldGen.PlaceTile(i, j, tileToPlace, false, true, -1, 0);

				/*
                if (!flag5 && Main.tile[i, j].active() && (int)Main.tile[i, j].type == tileToPlace)
                {
                    if (Main.tile[i, j + 1].halfBrick() || Main.tile[i, j + 1].slope() != 0)
                    {
                        WorldGen.SlopeTile(i, j + 1, 0);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendData(17, -1, -1, null, 14, (float)i, (float)(j + 1), 0f, 0, 0, 0);
                        }
                    }
                    if (Main.netMode != 0)
                    {
                        NetMessage.SendData(17, -1, -1, null, 1, (float)i, (float)j, (float)tileToPlace, 0, 0, 0);
                    }
                }*/
			}
		}
	}

	public class ReverseGravity : ModPlayer
	{
		public override void PostUpdateEquips()
		{
			int xPos = (int)(player.Top.X) / 16;
			int yPos = (int)(player.Top.Y) / 16;
			int yUpper = (int)(player.Top.Y) / 16 - 1;
			if (xPos < Main.tile.GetLength(0) && yPos < Main.tile.GetLength(1) && yUpper < Main.tile.GetLength(1) && xPos > 0 && yPos > 0 && yUpper > 0) //hopefully this prevents index outside bounds of array error
			{
				if (Main.tile[xPos, yUpper].type == mod.TileType("ReverseSand") || Main.tile[xPos, yPos].type == mod.TileType("ReverseSand") ||
				Main.tile[xPos, yUpper].type == mod.TileType("DnasBrick") || Main.tile[xPos, yPos].type == mod.TileType("DnasBrick"))
				{
					//player.gravDir = -1f;
					//player.gravControl2 = true;
					if (player.GetModPlayer<QwertyPlayer>().forcedAntiGravity == 0)
					{
						player.velocity.Y = 0;
					}
					player.GetModPlayer<QwertyPlayer>().forcedAntiGravity = 10;
				}
			}
		}
	}
}