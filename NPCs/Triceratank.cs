using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent;
namespace QwertysRandomContent.NPCs
{
	public class Triceratank : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Triceratank");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 195;
			npc.height = 98;
            if (NPC.downedMoonlord)
            {
                npc.damage = 240;
                npc.defense = 100;
                npc.lifeMax = 4000;
            }
            else
            {
                npc.damage = 200;
                npc.defense = 40;
                npc.lifeMax = 2000;
            }
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 6000f;
			npc.knockBackResist = 0f;
			npc.aiStyle = 3;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/OldDinosNewGuns");
            aiType = 28;
            banner = npc.type;
            bannerItem = mod.ItemType("TriceratankBanner");

            //animationType = 3;
            npc.buffImmune[BuffID.Confused] = false;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            
            if (QwertyWorld.DinoEvent)
			{
				return 35f;
			}
			else
			{
				return 0f;
			}
			
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			
			for (int i = 0; i < 10; i++)
			{
				int dustType = mod.DustType("DinoSkin2");
				int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
				Dust dust = Main.dust[dustIndex];
				dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
				dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
				dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
			}
		}
		
			
		public int AI_Timer = 0;
		public int damage = 30;
		public int walkTime = 300;
		
		
		
		
		
		public override void AI()
		{
            if(NPC.downedMoonlord)
            {
                damage = 40;
            }
			AI_Timer++;
			
				Player player = Main.player[npc.target];
				npc.TargetClosest(true);
			
			
			if(AI_Timer >walkTime)
			{
				
				
				//Projectile.NewProjectile(npc.Center.X+(78f*npc.direction), npc.Center.Y-34f, 1f*npc.direction, 0, 102, damage, 3f, Main.myPlayer);
				if(Main.netMode !=1)
                {
                    Projectile.NewProjectile(npc.Center.X + (78f * npc.direction), npc.Center.Y - 34f, 10f * npc.direction, 0, mod.ProjectileType("TankCannonBall"), damage, 3f, Main.myPlayer);

                }

                AI_Timer =0;
					
				
			}	
						

		}
		
		public override void FindFrame(int frameHeight)
		{
			// This makes the sprite flip horizontally in conjunction with the npc.direction.
			npc.spriteDirection = npc.direction;
			npc.frameCounter++;
				if (npc.frameCounter < 10)
				{
					npc.frame.Y = 0 * frameHeight;
				}
				else if (npc.frameCounter < 20)
				{
					npc.frame.Y = 1 * frameHeight;
				}
				else if (npc.frameCounter < 30)
				{
					npc.frame.Y = 2 * frameHeight;
				}
				else if (npc.frameCounter < 40)
				{
					npc.frame.Y = 3 * frameHeight;
				}
				else
				{
					npc.frameCounter = 0;
				}
		}
		public override void NPCLoot()
		{
            QwertyWorld.DinoKillCount += 5;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            if (Main.rand.Next(0, 100) == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DinoTooth"));
				}
                if (Main.rand.Next(0, 100) == 1)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("WornPrehistoricBow"));
                }
                if (Main.expertMode)
				{
					if (Main.rand.Next(0, 100) <= 8)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Tricerashield"));
					}
				}
				else
				{
					if (Main.rand.Next(0, 100) <= 5)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Tricerashield"));
					}
				}

				

				
				
			
		}
	}
    public class TankCannonBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Triceratank's cannon");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
            projectile.tileCollide = true;


        }
        public bool runOnce = true;
        
        public override void AI()
        {
            if (runOnce)
            {
                Main.PlaySound(SoundID.Item62, projectile.position);
                for (int i = 0; i < 50; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= .6f;
                }
                // Fire Dust spawn
                for (int i = 0; i < 80; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 2f;
                    dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1f;
                }
                runOnce = false;
            }
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            projectile.width = 75;
            projectile.height = 75;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            Projectile.NewProjectile(projectile.Center, new Vector2(0, 0), mod.ProjectileType("TankCannonBallExplosion"), projectile.damage, projectile.knockBack, player.whoAmI);
            Main.PlaySound(SoundID.Item62, projectile.position);
            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 80; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }
        }

    }
    public class TankCannonBallExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tank Cannon Ball Explosion");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 75;
            projectile.height = 75;
            projectile.friendly = true;
            projectile.hostile = true;
            projectile.penetrate = -1;
            
            projectile.tileCollide = false;
            projectile.timeLeft = 2;



        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        
    }
}
