using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using QwertysRandomContent;
namespace QwertysRandomContent.NPCs.HydraBoss
{
	
	public class HydraBreath : ModProjectile
	{
	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Breath");
            Main.projFrames[projectile.type] = 2;

        }
		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet; 
			projectile.width = 96;
			projectile.height = 52;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 240;
			projectile.tileCollide = false;
			projectile.light = 1f;
			
		}
        int frameCounter;
		public override void AI()
		{
            frameCounter++;
            if(frameCounter>20)
            {
                frameCounter = 0;
            }
            else if(frameCounter>10)
            {
                projectile.frame = 1;
            }
            else
            {
                projectile.frame = 0;
            }
			CreateDust();
		}
		public virtual void CreateDust()
		{
			
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBreathGlow"));
				
				
			
		}
		
		
		
		
	}
	
}
