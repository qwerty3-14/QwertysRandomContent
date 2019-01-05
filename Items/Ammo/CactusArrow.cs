using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
	public class CactusArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cactus Arrow");
			Tooltip.SetDefault("Sounds painful.\nPierces 1 enemy");
			
		}
		public override void SetDefaults()
		{
			item.damage = 7;
			item.ranged = true;
			item.knockBack = 2;
			item.value = 5;
			item.rare = 1;
			item.width = 14;
			item.height = 32;
			
			item.shootSpeed =6;
			
			item.consumable = true;
			item.shoot = mod.ProjectileType("CactusArrowP");
			item.ammo = 40;
			item.maxStack = 999;
			
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Cactus, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 15);
			recipe.AddRecipe();
		}
	}
	public class CactusArrowP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cactus Arrow");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.penetrate = 2;
			projectile.ranged= true;
			projectile.arrow=true;
			
			
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(30, 120);
			
		}
		

		
	}
	
}

