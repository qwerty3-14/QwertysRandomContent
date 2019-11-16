using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
 
 
 namespace QwertysRandomContent.Items.Weapons.Lune       ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class LuneArcherStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lune Archer Staff");
			Tooltip.SetDefault("Summons a lune archer to shoot arrows from your inventory at enemies");


        }
 
        public override void SetDefaults()
        {

            item.damage = 5;  //The damage stat for the Weapon.
            item.mana = 20;      //this defines how many mana this weapon use
            item.width = 32;    //The size of the width of the hitbox in pixels.
            item.height = 32;     //The size of the height of the hitbox in pixels.
            item.useTime = 25;   //How fast the Weapon is used.
            item.useAnimation = 25;    //How long the Weapon is used for.
            item.useStyle = 1;  //The way your Weapon will be used, 1 is the regular sword swing for example
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1f;  //The knockback stat of your Weapon.
            item.value = 10000;
            item.rare = 1;
            item.UseSound = SoundID.Item44;   //The sound played when using your Weapon
            item.autoReuse = true;   //Weather your Weapon will be used again after use while holding down, if false you will need to click again after use to use it again.
            item.shoot = mod.ProjectileType("LuneArcher");   //This defines what type of projectile this weapon will shot
            item.summon = true;    //This defines if it does Summon damage and if its effected by Summon increasing Armor/Accessories.
            item.buffType = mod.BuffType("LuneArcher");	//The buff added to player after used the item
			item.buffTime = 3600;
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
            recipe.AddIngredient(mod.ItemType("LuneBar"), 8);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
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

    public class LuneArcher : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lune Archer");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
			
		}
          
        public override void SetDefaults()
        {
 
			
            projectile.width = 40; //Set the hitbox width
            projectile.height = 40;   //Set the hitbox height
            projectile.hostile = false;    //tells the game if is hostile or not.
            projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly projectiles or not
            projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
            Main.projFrames[projectile.type] = 1;  //this is where you add how many frames u'r projectile has to make the animation
            projectile.knockBack = 10f;
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            projectile.tileCollide = false; //Tells the game whether or not it can collide with tiles/ terrain
			projectile.minion = true;
			projectile.minionSlots = 1;
			projectile.timeLeft = 2;
            
        }
       
        NPC target;
       
       
        int timer;
        public int arrow = 1;
        public bool canShoot = true;
        public float speedB = 14f;
        public float BulVel = 12;
        int varTime;
        int Yvar;
        int Xvar;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
            varTime++;
            if (varTime >= 60)
            {
               

                varTime = 0;
                if (Main.netMode != 2)
                {
                    Yvar = Main.rand.Next(0, 80);
                    Xvar = Main.rand.Next(-80, 80);
                }
            }

            Vector2 moveTo = new Vector2(player.Center.X + Xvar, player.Center.Y - Yvar) - projectile.Center;
            projectile.velocity = (moveTo) * .04f;
            if (modPlayer.LuneArcher)
            {
                projectile.timeLeft = 2;
            }
           
            timer++;
            if (QwertyMethods.ClosestNPC(ref target, 10000, player.Center, false, player.MinionAttackTargetNPC))
            {
                projectile.rotation = (target.Center - projectile.Center).ToRotation();
                if(timer>90)
                {
                    int weaponDamage = projectile.damage;
                    float weaponKnockback = projectile.knockBack;
                    player.PickAmmo(QwertyMethods.MakeItemFromID(39), ref arrow, ref speedB, ref canShoot, ref weaponDamage, ref weaponKnockback, Main.rand.Next(2) ==0);
                    if (Main.netMode != 1)
                    {
                        Projectile bul = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation) * BulVel, (float)Math.Sin(projectile.rotation) * BulVel, arrow, weaponDamage, weaponKnockback, Main.myPlayer)];
                        bul.ranged = false;
                        bul.minion = true;
                        if (Main.netMode == 1)
                        {
                            QwertysRandomContent.UpdateProjectileClass(bul);
                        }
                    }
                    timer = 0;
                }
                projectile.rotation += (float)Math.PI / 2;
            }

            

          
        }


    }

}