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
    public class OrichalcumDrifterStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orichalcum Drifter Staff");
			Tooltip.SetDefault("Summons an Orichalcum Drifter to fight for you!");


        }
 
        public override void SetDefaults()
        {

            item.damage = 32; 
            item.mana = 20;      
            item.width = 32;    
            item.height = 32;     
            item.useTime = 25;  
            item.useAnimation = 25;   
            item.useStyle = 1; 
            item.noMelee = true; 
            item.knockBack = 1f;
            item.value = 126500;
            item.rare = 4;
            item.UseSound = SoundID.Item44;  
            item.autoReuse = true;   
            item.shoot = mod.ProjectileType("OrichalcumDrifter");  
            item.summon = true;    
            item.buffType = mod.BuffType("OrichalcumDrifter");	
			item.buffTime = 3600;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.OrichalcumBar, 12);
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

    public class OrichalcumDrifter : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orichalcum Drifter");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
			
		}
          
        public override void SetDefaults()
        {


            projectile.width = 14; 
            projectile.height = 18;   
            projectile.hostile = false;   
            projectile.friendly = true;   
            projectile.ignoreWater = true;    
            Main.projFrames[projectile.type] = 1; 
            projectile.knockBack = 10f;
            projectile.penetrate = -1; 
            projectile.tileCollide = false; 
			projectile.minion = true;
			projectile.minionSlots = 1;
			projectile.timeLeft = 2;
            projectile.aiStyle = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
        }

        Vector2 flyTo;
        int identity = 0;
        int drifterCount = 0;
        NPC target;
        NPC possibleTarget;
        bool foundTarget;
        float distance;
        float maxDistance = 1000f;
        Projectile Beam = new Projectile();
        bool runOnce = true;
        float flyDirection;
        float acceleration = .1f;
        float maxSpeed = 3f;
        float driftTimer = 0;
        float driftVariance = 1;
        bool flyBack;
        float speed =3;
        float turnSpeed = 2;
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            drifterCount = player.ownedProjectileCounts[mod.ProjectileType("OrichalcumDrifter")];
            if (modPlayer.OrichalcumDrifter)
            {
                projectile.timeLeft = 2;
            }
            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].type == mod.ProjectileType("OrichalcumDrifter"))
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
            if ((player.Center - projectile.Center).Length() > 750 || flyBack)
            {
                
                flyTo = player.Center;
                
                if (Collision.CheckAABBvAABBCollision(player.position, player.Size, projectile.position, projectile.Size))
                {
                    flyBack = false;
                    speed = 3;
                    turnSpeed = 2;
                }
                else
                {
                    flyBack = true;
                    speed = 12;
                    turnSpeed = 20;
                }
            }
            else
            {
                
                if (QwertyMethods.ClosestNPC(ref target, maxDistance, projectile.Center, false, player.MinionAttackTargetNPC))
                {
                    flyTo = target.Center;
                }
                else
                {
                    if (drifterCount != 0)
                    {
                        projectile.ai[0] = 40f;
                        flyTo = player.Center + QwertyMethods.PolarVector(projectile.ai[0], (2f * (float)Math.PI * identity) / drifterCount);
                    }
                    else
                    {
                        flyTo = player.Center;
                    }

                }
                speed = 3;
                turnSpeed = 2;

            }
            flyDirection = QwertyMethods.SlowRotation(flyDirection, (flyTo - projectile.Center).ToRotation(), turnSpeed);

            projectile.velocity = QwertyMethods.PolarVector(speed, flyDirection);
            driftTimer += (float)Math.PI / 120;
            driftVariance = (float)Math.Cos(driftTimer);
            projectile.velocity += QwertyMethods.PolarVector(driftVariance, flyDirection + (float)Math.PI / 2);
            projectile.rotation = projectile.velocity.ToRotation();
           // Main.NewText((player.Center - projectile.Center).Length());
            
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
            target.immune[projectile.owner] = 0;
        }


    }
    
}