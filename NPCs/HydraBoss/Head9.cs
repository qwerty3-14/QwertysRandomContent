using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace QwertysRandomContent.NPCs.HydraBoss
{
	public class Head9 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Head");
			Main.npcFrameCount[npc.type] = 2;
		}

		public override void SetDefaults()
		{
			npc.width = 106;
			npc.height = 72;
			npc.damage = 50;
			npc.defense = 18;
			npc.boss = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 60f;
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			//aiType = 10;
			animationType = -1;
			npc.noGravity= true;
			npc.dontTakeDamage= false;
			npc.noTileCollide= true;
			music = MusicID.Boss5;
			npc.lifeMax = 6000;
			
			
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
			npc.damage = (int)(npc.damage *.7f);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			
				return 0f;
			
		}

		public override void HitEffect(int hitDirection, double damage)
		{
            
            for (int i = 0; i < 10; i++)
			{
				int dustType = mod.DustType("HydraBeamGlow");
				int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
				Dust dust = Main.dust[dustIndex];
				dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
				dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
				dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
			}
		}
		public int attackDelay =600;
		public int attackCooldown =0;
		
		public int varTime=0;
		
		public int YvarOld =0;
		
		public int XvarOld =0;
		public int numberOfAttacks =0;
		public int endAttack =0;
		public int damage = 0;
		public bool attackFrame = false;
		public float moveSpeedBoost = .04f;
		public NPC Hydra;
		public bool HoriSwitch = false;
		public int f=1;
		public float TargetDirection = (float)Math.PI/2;
		public float s =1;
        public Projectile laser;
        public override void AI()
		{
			
			if(Main.expertMode)
			{
				damage = npc.damage/4;
				attackDelay = 180;
			}
			else
			{
				damage = npc.damage/2;
			}



            Hydra = Main.npc[(int)npc.ai[0]];
            //npc.realLife = (int)npc.ai[0];

            Player player = Main.player[npc.target];
				npc.TargetClosest(true);
				
			
			if (!player.active || player.dead)
			{
				npc.TargetClosest(false);
				player = Main.player[npc.target];
				if (!player.active || player.dead)
				{
					npc.velocity = new Vector2(0f, 10f);
					if (npc.timeLeft > 10)
					{
						npc.timeLeft = 10;
					}
					return;
				}
			}
			
			
			if(attackCooldown > attackDelay)
			{
				endAttack=0;
                if (Main.netMode != 1)
                {
                    npc.ai[3] = Main.rand.Next(2, 4);
                    npc.netUpdate = true;
                }
				attackCooldown =0;
				
			}
			
			if(npc.ai[3] == 2)
			{
				attackFrame = true;
				TargetDirection = 0;
				varTime++;
				
				npc.ai[1] = 280;
                if (varTime == 30 && Main.netMode != 1)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 10f, 0f, mod.ProjectileType("HydraBreath"), (int)(damage * .8f), 3f, Main.myPlayer);
                }
                if (varTime >= 60)
				{
                    if (Main.netMode != 1)
                    {
                        npc.ai[2] = Main.rand.Next(100, 500);
                        npc.netUpdate = true;
                    }
					endAttack++;
					varTime=0;
				
				}
				if(endAttack >= 10)
				{
					npc.ai[3] =0;
				}
			}
			else if(npc.ai[3] == 3)
			{
				
				attackFrame = true;
				TargetDirection = -(float)Math.PI/2;
				
				
				
				varTime++;
				
				
				npc.ai[2] = 500;
				
				if(varTime >= 600)
				{
					npc.ai[3] =0;
                    if(Main.netMode !=1) laser.Kill();
				}
				else if( varTime >=300)
				{
					npc.ai[1]=100;
					
				}
				else if ( varTime >180)
				{
					npc.ai[1] -=  400/120;
					
				}
                else if( varTime ==180 && Main.netMode !=1)
                {
                    laser = Main.projectile[Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("HydraBeamT"), damage, 3f, Main.myPlayer, npc.whoAmI, 420)];
                }
				else
				{
					npc.ai[1] =500;
				}
				
				
			
			}
			else
			{
				attackFrame = false;
				moveSpeedBoost = .04f;
				varTime++;
				if(varTime > 100)
				{
                    if (Main.netMode != 1)
                    {
                        npc.ai[2] = Main.rand.Next(-50, 50);

                        npc.ai[1] = Main.rand.Next(0, 250);
                        npc.netUpdate = true;
                    }
					varTime=0;
				}
				attackCooldown++;
				TargetDirection = (float)Math.PI/2;
			}
            npc.rotation = new Vector2((float)Math.Cos(npc.rotation), (float)Math.Sin(npc.rotation)).ToRotation();
            if (Math.Abs(npc.rotation - TargetDirection) > Math.PI)
            {
                f = -1;
            }
            else
            {
                f = 1;
            }
            if (npc.rotation <= TargetDirection + MathHelper.ToRadians(4 * s) && npc.rotation >= TargetDirection - MathHelper.ToRadians(4 * s))
            {
                npc.rotation = TargetDirection;
            }
            else if (npc.rotation <= TargetDirection)
            {
                npc.rotation += MathHelper.ToRadians(2 * s) * f;
            }
            else if (npc.rotation >= TargetDirection)
            {
                npc.rotation -= MathHelper.ToRadians(2 * s) * f;
            }

            Vector2 moveTo = new Vector2(Hydra.Center.X-(150+npc.ai[1]), Hydra.Center.Y-(300f-npc.ai[2]))- npc.Center;	
			npc.velocity = (moveTo)*moveSpeedBoost;			
			
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (Main.netMode != 0)
            {
                Hydra = Main.npc[(int)npc.ai[0]];
                Vector2 neckOrigin = new Vector2(Hydra.Center.X, Hydra.Center.Y - 50);
                Vector2 center = npc.Center;
                Vector2 distToProj = neckOrigin - npc.Center;
                float projRotation = distToProj.ToRotation() - 1.57f;
                float distance = distToProj.Length();
                spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/HydraNeckBase"), neckOrigin - Main.screenPosition,
                            new Rectangle(0, 0, 52, 30), drawColor, projRotation,
                            new Vector2(52 * 0.5f, 30 * 0.5f), 1f, SpriteEffects.None, 0f);
                while (distance > 30f && !float.IsNaN(distance))
                {
                    distToProj.Normalize();                 //get unit vector
                    distToProj *= 30f;                      //speed = 30
                    center += distToProj;                   //update draw position
                    distToProj = neckOrigin - center;    //update distance
                    distance = distToProj.Length();


                    //Draw chain
                    spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/HydraNeck"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 52, 30), drawColor, projRotation,
                        new Vector2(52 * 0.5f, 30 * 0.5f), 1f, SpriteEffects.None, 0f);

                }
                spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/HydraNeckBase"), neckOrigin - Main.screenPosition,
                            new Rectangle(0, 0, 52, 30), drawColor, projRotation,
                            new Vector2(52 * 0.5f, 30 * 0.5f), 1f, SpriteEffects.None, 0f);
               
                    spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/Head3"), new Vector2(npc.Center.X - Main.screenPosition.X, npc.Center.Y - Main.screenPosition.Y),
                            npc.frame, drawColor, npc.rotation,
                            new Vector2(106 * 0.5f, 72 * 0.5f), 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(mod.GetTexture("NPCs/HydraBoss/Head3_Glow"), new Vector2(npc.Center.X - Main.screenPosition.X, npc.Center.Y - Main.screenPosition.Y),
                            npc.frame, Color.White, npc.rotation,
                            new Vector2(106 * 0.5f, 72 * 0.5f), 1f, SpriteEffects.None, 0f);
                
            }
        }
        public override void FindFrame(int frameHeight)
		{
			
			
				if (attackFrame)
				{
					npc.frame.Y = 1 * frameHeight;
				}
				else
				{
					npc.frame.Y = 0 * frameHeight;
				}
				
		}
		public override void BossHeadSlot(ref int index)
		{
			
				index = NPCHeadLoader.GetBossHeadSlot(QwertysRandomContent.Head3Head);
			
		}
		public override void BossHeadRotation(ref float rotation)
		{
			
			rotation =npc.rotation;	
			
		}
		// We use this hook to prevent any loot from dropping. We do this because this is a multistage npc and it shouldn't drop anything until the final form is dead.
		public override bool PreNPCLoot()
		{
			return false;
		}
		
	}
}
