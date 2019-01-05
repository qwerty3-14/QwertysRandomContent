using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using QwertysRandomContent.Items.Accesories;
using QwertysRandomContent.Items.Armor.Rhuthinium;
using QwertysRandomContent.Items.Armor.Vargule;
using Terraria.DataStructures;
using QwertysRandomContent.NPCs;
using System.Linq;
using QwertysRandomContent;
using Terraria.Localization;
using QwertysRandomContent.Items.B4Items;

namespace QwertysRandomContent.Items.Ammo
{
	public class GunArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gun Arrow");
			Tooltip.SetDefault("Shoots 2 bullets from your inventory within its first second of flight!");
			
		}
		public override void SetDefaults()
		{
			item.damage = 2;
			item.ranged = true;
			item.knockBack = 2;
			item.value = 5;
			item.rare = 4;
			item.width = 14;
			item.height = 32;
			
			item.shootSpeed =6;
			item.useAmmo = 97;
			item.consumable = true;
			item.shoot = mod.ProjectileType("GunArrowP");
			item.ammo = 40;
			item.maxStack = 999;
			
			
		}
		

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
	public class GunArrowP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gun Arrow");
			
			
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
			
			projectile.tileCollide = true;
			
			
		}


		//Thanks Mirsario for this chunk of code
		private static Dictionary<int,Item> vanillaItemCache=    new Dictionary<int,Item>();
		public static Item GetReference(int type)
		{
    		if(type<=0) 
			{
        		return null;
    		}
    		if(type>=ItemID.Count) 
			{
        		return ItemLoader.GetItem(type).item;
    		}
			else
			{
        		Item item;
        		if(!vanillaItemCache.TryGetValue(type,out item)) 
				{
        	    	item=    new Item();
        	    	item.SetDefaults(type,true);
        	    	vanillaItemCache[type]=    item;
        		}
       		 return item;
   	 		}
		}
		/*------------------------------------------------- */



		public int timer =0;
		public int bullet = 14;
		public bool canShoot;
		public float speed = 14f;
		//public Item item = new item();
		public override void AI()
		{

			Player player = Main.player[projectile.owner];
			timer++;
			int weaponDamage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
			float weaponKnockback = player.inventory[player.selectedItem].knockBack;
			
			canShoot = player.HasAmmo(GetReference(95), true)  && player.inventory[player.selectedItem+10].useAmmo ==97; 
			if(timer ==30)
			{
				player.PickAmmo(GetReference(95), ref bullet, ref speed, ref canShoot, ref weaponDamage, ref weaponKnockback, false);
					
					Projectile b = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, bullet, weaponDamage, weaponKnockback, Main.myPlayer)];
                if (projectile.GetGlobalProjectile<arrowgigantism>(mod).GiganticArrow)
                {
                    b.scale *= 3;

                    b.GetGlobalProjectile<arrowgigantism>(mod).GiganticArrow = true;

                    
                }
            }
			if(timer ==60)
			{
				player.PickAmmo(GetReference(95), ref bullet, ref speed, ref canShoot, ref weaponDamage, ref weaponKnockback, true);

                Projectile b = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, bullet, weaponDamage, weaponKnockback, Main.myPlayer)];
                if (projectile.GetGlobalProjectile<arrowgigantism>(mod).GiganticArrow)
                {
                    b.scale *= 3;

                    b.GetGlobalProjectile<arrowgigantism>(mod).GiganticArrow = true;


                }

            }


		}
		
		

		
	}
	
}

