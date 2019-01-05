using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
	public class MythrilArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mythril Arrow");
			Tooltip.SetDefault("Pierces enemies and blocks, short range");
			
		}
		public override void SetDefaults()
		{
			item.damage = 16;
			item.ranged = true;
			item.knockBack = 2;
			item.value = 5;
			item.rare = 3;
			item.width = 22;
			item.height = 32;
			
			item.shootSpeed =30;
			
			item.consumable = true;
			item.shoot = mod.ProjectileType("MythrilArrowP");
			item.ammo = 40;
			item.maxStack = 999;
			
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MythrilBar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
	public class MythrilArrowP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mythril Arrow");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.ranged= true;
			projectile.arrow=true;
			projectile.timeLeft=25;
			projectile.tileCollide = false;
			projectile.alpha = 0;
			projectile.light = 0f;             
			
		}
		public override void AI()
		{
			projectile.alpha += 10;
			projectile.light += .08f;
		}
		
		
		

		
	}
	
}

