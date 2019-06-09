using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;

namespace QwertysRandomContent.Items.Weapons.MiscSummons       
{
    public class MythrilPrismStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mythril Prism Staff");
			Tooltip.SetDefault("Summons a Mythril Prism to vaporize enemies that come near you!");


        }
 
        public override void SetDefaults()
        {

            item.damage = 30; 
            item.mana = 20;      
            item.width = 32;    
            item.height = 32;     
            item.useTime = 25;  
            item.useAnimation = 25;   
            item.useStyle = 1; 
            item.noMelee = true; 
            item.knockBack = 1f;
            item.value = 103500;
            item.rare = 4;
            item.UseSound = SoundID.Item44;  
            item.autoReuse = true;   
            item.shoot = mod.ProjectileType("MythrilPrism");  
            item.summon = true;    
            item.buffType = mod.BuffType("MythrilPrism");	
			item.buffTime = 3600;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MythrilBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 SPos = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);   //this make so the projectile will spawn at the mouse cursor position
            position = SPos;
			
            return true;
        }
		
		
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool UseItem(Player player)
		{
			if(player.altFunctionUse == 2)
			{
				player.MinionNPCTargetAim();
			}
			return base.UseItem(player);
		}
		
    }

    public class MythrilPrism : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mythril Prism");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
			
		}
          
        public override void SetDefaults()
        {


            projectile.width = 14; 
            projectile.height = 18;   
            projectile.hostile = false;   
            projectile.friendly = false;   
            projectile.ignoreWater = true;    
            Main.projFrames[projectile.type] = 1; 
            projectile.knockBack = 10f;
            projectile.penetrate = -1; 
            projectile.tileCollide = false; 
			projectile.minion = true;
			projectile.minionSlots = 1;
			projectile.timeLeft = 2;
            projectile.aiStyle = -1;
            //projectile.usesLocalNPCImmunity = true;
        }

        Vector2 flyTo;
        int identity = 0;
        int prismCount = 0;
        float beamRange = 100f;
        NPC target;
        NPC possibleTarget;
        bool foundTarget;
        float distance;
        float maxDistance = 200f;
        Projectile Beam = new Projectile();
        bool runOnce = true;
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            //Main.NewText(moveTo);
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            prismCount = player.ownedProjectileCounts[mod.ProjectileType("MythrilPrism")];
            if (modPlayer.mythrilPrism)
            {
                projectile.timeLeft = 2;
            }
            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].type == mod.ProjectileType("MythrilPrism"))
                {
                    if (p == projectile.whoAmI)
                    {
                        break;
                    }
                    else
                    {
                        identity++;
                    }
                }
            }
            if (player.MinionAttackTargetNPC != -1)
            {
                for (int k = 0; k < 200; k++)
                {
                    possibleTarget = Main.npc[k];
                    distance = (possibleTarget.Center - player.Center).Length();
                    if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal)
                    {
                        target = Main.npc[k];
                        foundTarget = true;


                        maxDistance = (target.Center - player.Center).Length();
                    }

                }
            }
            else
            {
                for (int k = 0; k < 200; k++)
                {
                    possibleTarget = Main.npc[k];
                    distance = (possibleTarget.Center - player.Center).Length();
                    if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal)
                    {
                        target = Main.npc[k];
                        foundTarget = true;


                        maxDistance = (target.Center - player.Center).Length();
                    }

                }
            }
            if (runOnce)
            {
                Beam = Main.projectile[Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("MiniBeam"), projectile.damage, projectile.knockBack, projectile.owner, projectile.whoAmI, 0)];
                runOnce = false;
            }
            if (prismCount != 0)
            {
                projectile.ai[0] = 40f;
                if (foundTarget)
                {
                    flyTo = target.Center + QwertyMethods.PolarVector(maxDistance / 2f, (player.Center - target.Center).ToRotation() + (.5f * (float)Math.PI * (identity + 1)) / (prismCount + 1) - (float)Math.PI * .25f);

                    Beam.ai[1] = target.whoAmI;

                }
                else
                {
                    Beam.ai[1] = -1;
                    flyTo = player.Center + QwertyMethods.PolarVector(projectile.ai[0], modPlayer.mythrilPrismRotation + (2f * (float)Math.PI * identity) / prismCount);
                }
                projectile.velocity = (flyTo - projectile.Center) * .1f;
                //Main.NewText(projectile.Center);/
                //projectile.Center = flyTo;

            }







            identity = 0;
            maxDistance = beamRange * 2;
            foundTarget = false;
        }

        
        

    }
    public class MiniBeam : ModProjectile
    {
        // The maximum charge value
        private const float MaxChargeValue = 0f;
        //The distance charge particle from the player center
        private const float MoveDistance = 0f;

        // The actual distance is stored in the ai0 field
        // By making a property to handle this it makes our life easier, and the accessibility more readable
        public float Distance;

        // The actual charge value is stored in the localAI0 field
        public float Charge
        {
            get { return projectile.localAI[0]; }
            set { projectile.localAI[0] = value; }
        }
        public Projectile shooter;
        // Are we at max charge? With c#6 you can simply use => which indicates this is a get only property
        public bool AtMaxCharge { get { return Charge == MaxChargeValue; } }
        Vector2 diff;
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.hostile = false;
            projectile.hide = false;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
            projectile.minion = true;
        }
        // The AI of the projectile

        public override void AI()
        {

            shooter = Main.projectile[(int)projectile.ai[0]];
            Vector2 mousePos = Main.MouseWorld;
            Player player = Main.player[projectile.owner];
            if (!shooter.active || shooter.type != mod.ProjectileType("MythrilPrism"))
            {
                projectile.Kill();
            }

            #region Set projectile position

            if (projectile.ai[1] != -1)
            {
                diff = QwertyMethods.PolarVector(2f, (Main.npc[(int)projectile.ai[1]].Center - projectile.Center).ToRotation());
            }
            else
            {
                diff = new Vector2(2f, 0);
            }
            
                
            
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




                Vector2 offset = projectile.velocity;
                offset *= MoveDistance - 20;
                Vector2 pos = new Vector2(shooter.Center.X, shooter.Center.Y) + offset - new Vector2(10, 10);

                if (Charge < MaxChargeValue)
                {
                    Charge++;
                }

                int chargeFact = (int)(Charge / 20f);



                #endregion


                if (Charge < MaxChargeValue) return;
                Vector2 start = new Vector2(shooter.Center.X, shooter.Center.Y);
                Vector2 unit = projectile.velocity;
                unit *= -1;

                for (Distance = MoveDistance; Distance <= 50f; Distance += 1f)
                {
                    start = new Vector2(shooter.Center.X, shooter.Center.Y) + projectile.velocity * Distance;
                    /*
                    if (!Collision.CanHit(new Vector2(shooter.Center.X, shooter.Center.Y), 1, 1, start, 1, 1))
                    {
                        Distance -= 5f;
                        break;
                    }
                    */
                }



                //Add lights
                DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
                Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (Distance - MoveDistance), 26,
                    DelegateMethods.CastLight);
            
            
            

        }
        public int colorCounter;
        public Color lineColor;
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            if (projectile.ai[1] != -1)
            {
                DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], shooter.Center,
                    projectile.velocity, 2f, projectile.damage, -1.57f, 1f, 100f, Color.White, (int)MoveDistance);
            }
            Texture2D text = mod.GetTexture("Items/Weapons/MiscSummons/MythrilPrism");
            spriteBatch.Draw(text, new Vector2(shooter.Center.X - Main.screenPosition.X, shooter.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, text.Width, text.Height), lightColor, projectile.rotation,
                        new Vector2(text.Width * 0.5f, text.Height * 0.5f), 1f, SpriteEffects.None, 0f);

        }

        // The core function of drawing a laser
        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 4000f, Color color = default(Color), int transDist = 2)
        {
            Vector2 origin = start;
            float r = unit.ToRotation() + rotation;

            #region Draw laser body
            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                origin = start + i * unit;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 0, 6, 2), Color.Lerp(new Color(255, 255, 255, 255), new Color(0, 0, 0, 0), i / Distance), r,
                    new Vector2(3, 1), scale, 0, 0);
            }
           
            
            
            #endregion


        }

        // Change the way of collision check of the projectile
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {

            if (projectile.ai[1] != -1)
            {

                Player player = Main.player[projectile.owner];
                Vector2 unit = projectile.velocity;
                float point = 0f;
                // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
                // It will look for collisions on the given line using AABB
                if(Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projHitbox.TopLeft(), projHitbox.Size()))
                {
                    return true;
                }
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), new Vector2(shooter.Center.X, shooter.Center.Y),
                    new Vector2(shooter.Center.X, shooter.Center.Y) + unit * Distance, 6, ref point);
            }
            return false;
        }

        // Set custom immunity time on hitting an NPC
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
            target.immune[projectile.owner] = 0;
        }



        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = projectile.velocity;
            Utils.PlotTileLine(projectile.Center, projectile.Center + unit * Distance, (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
        }
    }
}