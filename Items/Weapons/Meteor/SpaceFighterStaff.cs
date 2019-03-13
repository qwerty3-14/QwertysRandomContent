using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;

namespace QwertysRandomContent.Items.Weapons.Meteor       
{
    public class SpaceFighterStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Fighter Staff");
			Tooltip.SetDefault("");


        }
 
        public override void SetDefaults()
        {

            item.damage = 9; 
            item.mana = 20;      
            item.width = 44;    
            item.height = 44;     
            item.useTime = 25;  
            item.useAnimation = 25;   
            item.useStyle = 1; 
            item.noMelee = true; 
            item.knockBack = 5f;
            item.value = Item.sellPrice(silver: 40);
            item.rare = 1;
            item.UseSound = SoundID.Item44;  
            item.autoReuse = true;   
            item.shoot = mod.ProjectileType("SpaceFighter");  
            item.summon = true;    
            item.buffType = mod.BuffType("SpaceFighter");	
			item.buffTime = 3600;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar, 20);
            recipe.AddTile(TileID.Anvils);
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

    public class SpaceFighter : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Fighter");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
			
		}
          
        public override void SetDefaults()
        {


            projectile.width = 30;
            projectile.height = 30;   
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

        NPC target;
        const float maxSpeed = 12f;
        int shotCounter = 0;
        void Thrust()
        {
            projectile.velocity += QwertyMethods.PolarVector(-.1f, projectile.velocity.ToRotation());
            projectile.velocity += QwertyMethods.PolarVector(.2f, projectile.rotation);
            Dust d = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(-8, projectile.rotation) + QwertyMethods.PolarVector(12, projectile.rotation + (float)Math.PI / 2), 6);
            d.noGravity = true;
            d.noLight = true;
            d = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(-8, projectile.rotation) + QwertyMethods.PolarVector(-12, projectile.rotation + (float)Math.PI / 2), 6);
            d.noGravity = true;
            d.noLight = true;

        }
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            //Main.NewText(moveTo);
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            shotCounter++;
            if (modPlayer.SpaceFighter)
            {
                projectile.timeLeft = 2;
            }
            if (QwertyMethods.ClosestNPC(ref target, 1000, projectile.Center, false, player.MinionAttackTargetNPC) && (player.Center - projectile.Center).Length() < 1000)
            {
                projectile.rotation = QwertyMethods.SlowRotation(projectile.rotation, (target.Center - projectile.Center).ToRotation(), 6);
                if ((target.Center-projectile.Center).Length() < 300)
                {
                    if(shotCounter>=20)
                    {
                        shotCounter = 0;
                        Projectile l = Main.projectile[Projectile.NewProjectile(projectile.Center, QwertyMethods.PolarVector(12f, projectile.rotation), ProjectileID.GreenLaser, projectile.damage, projectile.knockBack, projectile.owner)];
                        l.magic = false;
                        l.minion = false;
                        //l.scale = .5f;
                        l.penetrate = 1;
                        //l.alpha = 0;
                        Main.PlaySound(SoundID.Item12);
                    }
                    
                    
                }
                else
                {
                    Thrust();

                }
                
            }
            else
            {
                projectile.rotation = QwertyMethods.SlowRotation(projectile.rotation, (player.Center - projectile.Center).ToRotation(), 6);
                if ((player.Center - projectile.Center).Length() < 300)
                {
                    
                }
                else
                {
                    Thrust();


                }
            }
            for (int k = 0; k < 200; k++)
            {
                if (Main.projectile[k].type == projectile.type && k != projectile.whoAmI)
                {
                    if (Collision.CheckAABBvAABBCollision(projectile.position + new Vector2(projectile.width / 4, projectile.height / 4), new Vector2(projectile.width / 2, projectile.height / 2), Main.projectile[k].position + new Vector2(Main.projectile[k].width / 4, Main.projectile[k].height / 4), new Vector2(Main.projectile[k].width / 2, Main.projectile[k].height / 2))) 
                    {
                        projectile.velocity += new Vector2((float)Math.Cos((projectile.Center - Main.projectile[k].Center).ToRotation()) * .1f, (float)Math.Sin((projectile.Center - Main.projectile[k].Center).ToRotation()) * .1f);
                    }
                }
            }
            if (projectile.velocity.Length() > maxSpeed)
            {
                projectile.velocity = projectile.velocity.SafeNormalize(-Vector2.UnitY) * maxSpeed;
            }
        }

        
        

    }
    

}