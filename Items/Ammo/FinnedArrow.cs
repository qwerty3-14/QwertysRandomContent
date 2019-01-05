using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
	public class FinnedArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Finned Arrow");
			Tooltip.SetDefault("Awkward in the air but swims toward enemies when submerged");
			
		}
		public override void SetDefaults()
		{
			item.damage = 6;
			item.ranged = true;
			item.knockBack = 2;
			item.value = 20;
			item.rare = 2;
			item.width = 22;
			item.height = 32;
			
			item.shootSpeed =1;
			
			item.consumable = true;
			item.shoot = mod.ProjectileType("FinnedArrowP");
			item.ammo = 40;
			item.maxStack = 999;
			
			
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WoodenArrow, 200);
            recipe.AddIngredient(ItemID.SharkFin, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 200);
            recipe.AddRecipe();
        }


    }
	public class FinnedArrowP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Finned Arrow");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.ranged= true;
			projectile.arrow=true;
            
			
			
			
		}
        public float swimSpeed=10;
        public float swimDirection;
        public bool foundTarget;
       
        public int wanderTimer=61;
        public bool runOnce = true;
        public float actDirection;
        public int f=1;
        public float wiggle;
        public float wiggleTime;
        public float maxDistance = 1000f;
        public NPC prey;
        public NPC possiblePrey;
        public float distance;
        float wander;
        public override void AI()
		{
            if(runOnce)
            {
                
                actDirection = projectile.velocity.ToRotation();
                projectile.velocity /= 5;
                runOnce = false;
            }
           
            wanderTimer++;
            if (wanderTimer > 60 )
            {
                if (Main.netMode == 1)
                {
                    projectile.ai[1]= Main.rand.NextFloat(2 * (float)Math.PI);
                    
                    ModPacket packet = mod.GetPacket();
                    packet.Write((byte)ModMessageType.ArrowMessage); // Message type, you would need to create an enum for this
                    packet.Write(projectile.identity); // tells which projectile is being modified by the effect, the effect is then applied on the receiving end
                    packet.Write((byte)projectile.whoAmI); // the player that shot the projectile, will be useful later
                    packet.Write(projectile.ai[1]);
                    packet.Send();
                    
                    projectile.netUpdate = true;
                    //projectile.netUpdate2 = true;
                }
                else if(Main.netMode ==0)
                {
                    projectile.ai[1] = Main.rand.NextFloat(2 * (float)Math.PI);
                }
                wanderTimer = 0;

            }
            if (projectile.wet)
            {
                for (int k = 0; k < 200; k++)
                {
                    possiblePrey = Main.npc[k];
                    distance= (possiblePrey.Center - projectile.Center).Length();
                    if (distance < maxDistance && possiblePrey.active && !possiblePrey.dontTakeDamage && !possiblePrey.friendly && possiblePrey.lifeMax > 5 && !possiblePrey.immortal && Collision.CanHit(projectile.Center, 0, 0, possiblePrey.Center, 0, 0))
                    {
                        prey = Main.npc[k];
                        foundTarget = true;
                       
                        swimDirection = (projectile.Center - prey.Center).ToRotation() - (float)Math.PI;
                        maxDistance = (prey.Center - projectile.Center).Length();
                    }

                }
                maxDistance = 10000f;
                if (!foundTarget)
                {

                    
                    swimDirection = projectile.ai[1] - (float)Math.PI;
                    
                }


                actDirection = QwertyMethods.SlowRotation(actDirection, swimDirection, 4);
                projectile.velocity.X = (float)Math.Cos(actDirection) * swimSpeed;
                projectile.velocity.Y = (float)Math.Sin(actDirection) * swimSpeed;
                projectile.rotation = actDirection + (float)Math.PI / 2;
                actDirection = projectile.velocity.ToRotation();
                
            }
            else
            {
                actDirection = projectile.velocity.ToRotation();
            }
            


            foundTarget = false;
            
            if (Main.netMode == 1)
            {
                Main.NewText("client: " + projectile.ai[1]);
            }
            
            
            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + projectile.ai[1]), Color.White);
            }
            

        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(wander);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            wander = reader.Read();
        }
        public override void Kill(int timeLeft)
        {
            
			

		}
       

    }
	
}

