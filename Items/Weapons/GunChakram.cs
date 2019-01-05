using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.Weapons 
{
	public class GunChakram : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gun Chakram");
			Tooltip.SetDefault("Shoots bullets when it bounces off walls or enemies!" + "\nMore melee speed will make it fire more bullets");


        }
		public override void SetDefaults()
		{
			item.damage = 49;
			item.melee = true;
			item.noMelee = true;
			
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.knockBack = 2;
            item.value = 250000;
            item.rare = 8;
            item.UseSound = SoundID.Item1;
			item.noUseGraphic = true;
			item.width = 72;
			item.height = 72;
			
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("GunChakramP");
			item.shootSpeed =12;
            item.useAmmo = AmmoID.Bullet;




        }
        public override bool ConsumeAmmo(Player player)
        {
            return false;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShroomiteBar, 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = mod.ProjectileType("GunChakramP");
            return true;
        }
        public override bool CanUseItem(Player player)
		{
			for (int i = 0; i < 1000; ++i)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
				{
					return false;
				}
			}
			return true;
		}
	}

	public class GunChakramP : ModProjectile
	{
		public override void SetDefaults()
		{
            projectile.aiStyle = -1;
            //aiType = ProjectileID.ThornChakram;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.width = 36;
			projectile.height = 36;
            projectile.melee = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gun Chakram");
            

		}
        //Thanks Mirsario for this chunk of code
        private static Dictionary<int, Item> vanillaItemCache = new Dictionary<int, Item>();
        public static Item GetReference(int type)
        {
            if (type <= 0)
            {
                return null;
            }
            if (type >= ItemID.Count)
            {
                return ItemLoader.GetItem(type).item;
            }
            else
            {
                Item item;
                if (!vanillaItemCache.TryGetValue(type, out item))
                {
                    item = new Item();
                    item.SetDefaults(type, true);
                    vanillaItemCache[type] = item;
                }
                return item;
            }
        }
        /*------------------------------------------------- */

        public bool runOnce = true;
        public int rotationDirection = 1;
        public int timer;
        public float direction;
        public float speed;
        public bool runOnceTwo=true;
        public float playerDirection;
        public bool hasAproachedPlayer;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                speed  = (float)Math.Sqrt((projectile.velocity.X * projectile.velocity.X) + (projectile.velocity.Y * projectile.velocity.Y)); 
                if (projectile.velocity.X>0)
                {
                    rotationDirection = -1;
                }
                runOnce = false;
            }
            projectile.rotation += (rotationDirection * 8 * (float)Math.PI) / 60;
            timer++;
            if(timer>30)
            {
                projectile.tileCollide = false;
                if(runOnceTwo)
                {
                    direction = (projectile.velocity).ToRotation();
                    runOnceTwo = false;
                }
                
                playerDirection = (player.Center - projectile.Center).ToRotation();
                direction = playerDirection;
                /*
                if ((Math.Abs(direction- playerDirection)< (float)Math.PI/2) || hasAproachedPlayer)
                {
                    direction = playerDirection;
                    hasAproachedPlayer = true;
                }
                else
                {
                    direction += (rotationDirection * 4 * (float)Math.PI) / 60;
                }
                */
                

                float distance = (float)Math.Sqrt((player.Center.X - projectile.Center.X) * (player.Center.X - projectile.Center.X) + (player.Center.Y - projectile.Center.Y) * (player.Center.Y - projectile.Center.Y));
                if (distance < 10 * (speed / 15))
                {
                    projectile.Kill();
                }
                projectile.velocity.X = speed * (float)Math.Cos(direction);
                projectile.velocity.Y = speed * (float)Math.Sin(direction);
            }
            
            

        }

        public int bullet = 14;
        public bool canShoot=true;
        public float speedB = 14f;
        public float BulVel = 12;
        public float dir = 0;
        public override bool OnTileCollide(Vector2 velocityChange)
        {
            Main.PlaySound(SoundID.Item38, projectile.Center);
            timer += 10;
            Player player = Main.player[projectile.owner];
            int weaponDamage = projectile.damage;
            float weaponKnockback = projectile.knockBack;
            int bulletCount = 8 + (int)((((1/player.meleeSpeed) - 1)*100)/10);
            
            float bulletSpacing = 2*(float)Math.PI / bulletCount;
            //CombatText.NewText(player.getRect(), new Color(38, 126, 126), bulletCount, true, false);
            player.PickAmmo(GetReference(95), ref bullet, ref speedB, ref canShoot, ref weaponDamage, ref weaponKnockback, false);
            for (; bulletCount > 0; bulletCount--)
            {
                Projectile bul = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(dir)*BulVel, (float)Math.Sin(dir) * BulVel, bullet, weaponDamage, weaponKnockback, Main.myPlayer)];
                bul.melee = true;
                bul.ranged = false;
                dir += bulletSpacing;
            }

            

            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.velocity.X = -velocityChange.X;
            }
            if (projectile.velocity.Y != velocityChange.Y)
            {
                projectile.velocity.Y = -velocityChange.Y;
            }
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.PlaySound(SoundID.Item38, projectile.Center);
            
            Player player = Main.player[projectile.owner];
            int weaponDamage = projectile.damage;
            float weaponKnockback = projectile.knockBack;
            int bulletCount = 8 + (int)((((1 / player.meleeSpeed) - 1) * 100) / 10);

            float bulletSpacing = 2 * (float)Math.PI / bulletCount;

            player.PickAmmo(GetReference(95), ref bullet, ref speedB, ref canShoot, ref weaponDamage, ref weaponKnockback, false);
            for (; bulletCount > 0; bulletCount--)
            {
                Projectile bul = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(dir) * BulVel, (float)Math.Sin(dir) * BulVel, bullet, weaponDamage, weaponKnockback, Main.myPlayer)];
                bul.melee = true;
                bul.ranged = false;
                dir += bulletSpacing;
            }
            projectile.velocity.X = -projectile.velocity.X;
            projectile.velocity.Y = -projectile.velocity.Y;
        }
    }

	
}

