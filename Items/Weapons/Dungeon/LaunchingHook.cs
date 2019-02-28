using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Dungeon
{
	class LaunchingHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Launching Hook");
		}

		public override void SetDefaults()
		{
			
			item.CloneDefaults(ItemID.AmethystHook);
			item.shootSpeed = 18f; // how quickly the hook is shot.
			item.shoot = mod.ProjectileType("LaunchingHookP");
			item.value = 50000;
			item.rare = 5;   
		}
	}
	class LaunchingHookP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");
		}

		public override void SetDefaults()
		{
			
			projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
            projectile.width = 18;
            projectile.height = 20;
		}

		// Use this hook for hooks that can have multiple hooks mid-flight: Dual Hook, Web Slinger, Fish Hook, Static Hook, Lunar Hook
		public override bool? CanUseGrapple(Player player)
		{
			int hooksOut = 0;
			for (int l = 0; l < 1000; l++)
			{
				if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == projectile.type)
				{
					hooksOut++;
				}
			}
			if (hooksOut > 0) 
			{
				return false;
			}
			return true;
		}

		
		// Amethyst Hook is 300, Static Hook is 600
		public override float GrappleRange()
		{
			return 160f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks)
		{
			numHooks = 1;
		}

		// default is 11, Lunar is 24
		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			speed = 20f;
		}

		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			speed = 24f;
		}
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if((projectile.Center - player.Center).Length()<100 && player.grappling[0] >= 0)
            {
                projectile.Kill();
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
            Player player = Main.player[projectile.owner];
            float directionToHook = (projectile.Center - player.Center).ToRotation();
            float distanceToHook = (projectile.Center - player.Center).Length();
            Texture2D texture = mod.GetTexture("Items/Weapons/Dungeon/LHChain");
            for(int d =0; d < distanceToHook; d += texture.Height)
            {
                spriteBatch.Draw(texture, player.Center+QwertyMethods.PolarVector(d, directionToHook) - Main.screenPosition,
                       new Rectangle(0, 0, texture.Width, texture.Height), lightColor, directionToHook+(float)Math.PI/2,
                       new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }

            return true;
		}
       
    }

	

}
