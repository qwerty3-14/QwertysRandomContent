using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
 
 namespace QwertysRandomContent.Items.Weapons.Vargule       ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class VarguleStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Staff");
			Tooltip.SetDefault("Summons a Vargule disc that explodes into a bunch of shards");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
			
		}
 
        public override void SetDefaults()
        {

            item.damage = 90;  //The damage stat for the Weapon.
            item.mana = 9;      //this defines how many mana this weapon use
            item.width = 46;    //The size of the width of the hitbox in pixels.
            item.height = 46;     //The size of the height of the hitbox in pixels.
            item.useTime = 30;   //How fast the Weapon is used.
            item.useAnimation = 30;    //How long the Weapon is used for.
            item.useStyle = 5;  //The way your Weapon will be used, 1 is the regular sword swing for example
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1f;  //The knockback stat of your Weapon.
            item.value = 250000;
            item.rare = 8;
            item.UseSound = SoundID.Item43;   //The sound played when using your Weapon
            item.autoReuse = true;   //Weather your Weapon will be used again after use while holding down, if false you will need to click again after use to use it again.
            item.shoot = mod.ProjectileType("VarguleDisc");   //This defines what type of projectile this weapon will shot
            item.magic = true;    //This defines if it does Summon damage and if its effected by Summon increasing Armor/Accessories.
            
        }
		
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 SPos = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);   //this make so the projectile will spawn at the mouse cursor position
            position = SPos;
			                                                                 
            return true;
            
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VarguleBar"), 12);
			
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		
		
    }

    public class VarguleDisc : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Disc");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
		}
 
        public override void SetDefaults()
        {
 
			projectile.timeLeft = 60;
            projectile.width = 32; //Set the hitbox width
            projectile.height = 32;   //Set the hitbox heinght
            projectile.hostile = false;    //tells the game if is hostile or not.
            projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
            Main.projFrames[projectile.type] = 1;  //this is where you add how many frames u'r projectile has to make the animation
            projectile.knockBack = 1f;
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            projectile.tileCollide = true; //Tells the game whether or not it can collide with tiles/ terrain
            
        }
 
       
 
        public override void AI()
        {
            
            projectile.rotation += 1.5f;   //this make the projctile to rotate
            CreateDust();
            if (projectile.timeLeft <= 20)
            {
                projectile.alpha = (int)(255f - ((float)projectile.timeLeft / 20f) * 255f);
            }









        }
		public override void Kill( int timeLeft)
			{
				if (projectile.owner == Main.myPlayer)
				{
				
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -10f, 0f, mod.ProjectileType("VarguleShard"), projectile.damage, projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 10f, 0f, mod.ProjectileType("VarguleShard"), projectile.damage, projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 10f, mod.ProjectileType("VarguleShard"), projectile.damage, projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, -10f, mod.ProjectileType("VarguleShard"), projectile.damage, projectile.knockBack, Main.myPlayer);
				
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -7f, 7f, mod.ProjectileType("VarguleShard2"), projectile.damage, projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 7f, 7f, mod.ProjectileType("VarguleShard2"), projectile.damage, projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -7f, -7f, mod.ProjectileType("VarguleShard2"), projectile.damage, projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 7f, -7f, mod.ProjectileType("VarguleShard2"), projectile.damage, projectile.knockBack, Main.myPlayer);
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 24); 
				}
			}
        public virtual void CreateDust()
        {

            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("VarguleDust"), 0f , 0f, projectile.alpha);



        }
    }
	
	public class VarguleShard : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Shard");
			
			
		}
		public override void SetDefaults()
		{
			projectile.timeLeft=5;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet; 
			projectile.width = 10;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.magic= true;
			projectile.knockBack = 1f;
			projectile.damage=175;
			
			
		}
		
		 public override void Kill(int timeLeft)
        {
            
			

		}
        public override void AI()
        {
            CreateDust();
        }
        public virtual void CreateDust()
        {

            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Helix1Dust"));



        }


    }
	public class VarguleShard2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Shard");
			
			
		}
		public override void SetDefaults()
		{
			projectile.timeLeft=5;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet; 
			projectile.width = 10;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.magic= true;
			projectile.knockBack = 1f;
			projectile.damage=175;
			
			
			
		}
		
		 public override void Kill(int timeLeft)
        {
            
			

		}
        public override void AI()
        {
            CreateDust();
        }
        public virtual void CreateDust()
        {

            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Helix2Dust"));



        }



    }
}