using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Thrown
{
	public class TitaniumKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanium Knife");
			Tooltip.SetDefault("Flies straight and reverses direction soon after hitting an enemy");
			
		}
		public override void SetDefaults()
		{
			item.damage = 52;
			item.thrown = true;
			item.knockBack = 1;
			item.value = 80;
			item.rare = 3;
			item.width = 14;
			item.height = 34;
            item.useStyle = 1;
			item.shootSpeed =12f;
            item.useTime = 18;
            item.useAnimation = 18;
			item.consumable = true;
			item.shoot = mod.ProjectileType("TitaniumKnifeP");
            item.noUseGraphic = true;
            item.noMelee = true;
			item.maxStack = 999;
            item.autoReuse = true;
			
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TitaniumBar, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 333);
			recipe.AddRecipe();
		}
	}
	public class TitaniumKnifeP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanium Knife");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.thrown= true;
			
			
			projectile.tileCollide = true;
			
			
		}
        public NPC grabbed = new NPC();
        
        public bool re;
        public int reTimer;
        public override void AI()
        {
            if(re)
            {
                reTimer++;
                if (reTimer > 30)
                {
                    projectile.velocity.X *= -1;
                    projectile.velocity.Y *= -1;
                    re = false;
                }
            }
            else
            {
                reTimer = 0;
            }
            
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            re = true;


        }
        





            }

        }

