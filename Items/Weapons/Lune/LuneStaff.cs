using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
 
 namespace QwertysRandomContent.Items.Weapons.Lune       ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class LuneStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lune Staff");
			Tooltip.SetDefault("Fires a Lune crest" + "\nWhen the Lune crest hits an enemy it will orbit it shooting nearby enemies" +"\nInflicts Lune curse making enemies more vulnerable to critical hits");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
			
		}
 
        public override void SetDefaults()
        {

            item.damage = 16; 
            item.mana = 9;      
            item.width = 42;    
            item.height = 40;    
            item.useTime = 30;  
            item.useAnimation = 30;    
            item.useStyle = 5; 
            item.noMelee = true;
            item.knockBack = 1f; 
            item.value = 250000;
            item.rare = 8;
            item.UseSound = SoundID.Item43;  
            item.autoReuse = true;   
            item.shoot = mod.ProjectileType("LuneCrest");   
            item.magic = true;    
            item.shootSpeed = 8;
            
        }
		
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
           
			                                                                 
            return true;
            
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("LuneBar"), 8);
			
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		
		
    }
    public class LuneCrest : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.magic = true;
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.light =.5f;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
        }
        bool runOnce = true;
        bool orbitting;
        NPC planet;
        float radius = 10;
        float direciton;
        float orbitSpeed=1;
        int shootTimer;
        int shootCooldown=20;
        NPC prey;
        bool foundPrey;
        float preyDistance;
        float maxPreyDistance = 1000;
        NPC possiblePrey;
        float preyDirection;
        public override void AI()
        {
            if(runOnce)
            {

                runOnce = false;
            }
            if(orbitting)
            {
               
                //Main.NewText(direciton);
                direciton += (float)((2 * Math.PI) / (6 * radius / orbitSpeed));
                projectile.velocity = new Vector2((float)Math.Cos(direciton) * orbitSpeed, (float)Math.Sin(direciton) * orbitSpeed);
                projectile.velocity += planet.velocity;
                if(!planet.active)
                {
                    projectile.Kill();
                }
                shootTimer++;
                if(shootTimer>shootCooldown)
                {
                    for(int n =0; n<200; n++)
                    {
                        possiblePrey = Main.npc[n];
                        preyDistance = (possiblePrey.Center-projectile.Center).Length();
                        if (n != planet.whoAmI && preyDistance< maxPreyDistance && possiblePrey.active && !possiblePrey.immortal && !possiblePrey.friendly && !possiblePrey.dontTakeDamage)
                        {
                            prey = Main.npc[n];
                            maxPreyDistance = preyDistance;
                            preyDirection = (possiblePrey.Center - projectile.Center).ToRotation();
                            foundPrey = true;
                        }
                    }
                    if (foundPrey)
                    {

                        Projectile shot = Main.projectile[Projectile.NewProjectile(prey.Center, new Vector2(0, 0), mod.ProjectileType("FighterShot"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f)];
                        shot.minion = false;
                        shot.magic = true;
                        for(int d =0; d< maxPreyDistance; d+=4)
                        {
                            Dust.NewDust(projectile.Center + new Vector2((float)Math.Cos(preyDirection)*d, (float)Math.Sin(preyDirection)*d), 0, 0, mod.DustType("LuneDust"));
                        }
                    }
                    shootTimer = 0;
                }
            }
            Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("LuneDust"));
            foundPrey = false;
            maxPreyDistance = 1000;

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("LuneCurse"), 120);
            planet = target;
            orbitting = true;
            projectile.friendly = false;
            radius = target.width ;
            orbitSpeed = radius/10;
            projectile.Center = new Vector2(target.Center.X, target.Center.Y-radius);
            projectile.timeLeft = 180;
            projectile.tileCollide = false;
        }
    }

    



    
}