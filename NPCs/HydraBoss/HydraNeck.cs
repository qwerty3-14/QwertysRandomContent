using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using QwertysRandomContent;
using System.Collections.Generic;
namespace QwertysRandomContent.NPCs.HydraBoss
{
	
	public class HydraNeck : ModProjectile
	{
	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Neck");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 30;
			projectile.height = 52;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 60;
			projectile.tileCollide = false;
			projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
			
			
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			
			// Add this projectile to the list of projectiles that will be drawn BEFORE tiles and NPC are drawn. This makes the projectile appear to be BEHIND the tiles and NPC.
			drawCacheProjsBehindNPCsAndTiles.Add(index);
		}
		public NPC Head;
		public NPC Body;
		public float V =0f;
		public override void AI()
		{
			
			foreach(NPC npcSearch in Main.npc)
			{	
				if(npcSearch.type == mod.NPCType("Hydra"))
				Body = npcSearch;
			}
			foreach(NPC npcSearch in Main.npc)
			{	
				if(npcSearch.type == mod.NPCType("Head1"))
				Head = npcSearch;
			}
			
			float targetAngle = (float)Math.Atan((Body.Center.Y - Head.Center.Y)/(Body.Center.X - Head.Center.X));
					if( (Body.Center.X - Head.Center.X)<0)
					{
						targetAngle += +MathHelper.ToRadians(180);
					}
					else if (targetAngle <0)
					{
						targetAngle += +MathHelper.ToRadians(360);
					}
			projectile.rotation = targetAngle+MathHelper.ToRadians(90);
			projectile.position.X= Head.Center.X+ V*(float)Math.Cos(targetAngle)-(projectile.width/2);
			projectile.position.Y= Head.Center.Y+ V*(float)Math.Sin(targetAngle)-(projectile.height/2);
			V += 30f;
			if (Math.Sqrt(((projectile.Center.X-Body.Center.X)*(projectile.Center.X-Body.Center.X))+((projectile.Center.Y-Body.Center.Y)*(projectile.Center.Y-Body.Center.Y))) < 20)
			{
				projectile.timeLeft = 0;
			}
			
		}
		
		
		
		
		
	}
	
}
