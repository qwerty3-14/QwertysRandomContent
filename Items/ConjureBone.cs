using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items
{
	public class ConjureBone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Conjure Bone");
			Tooltip.SetDefault("Not the best damage but easy on the mana");
			
		}
		public override void SetDefaults()
		{
			item.damage = 18;
			item.magic = true;
			
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 5;
			item.knockBack = 1;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.width = 28;
			item.height = 32;
			item.crit = 10;
			item.mana =1;
			item.shoot = mod.ProjectileType("Bone");
			item.shootSpeed =9;
			item.noMelee=true;
			
			
			
		}

		
	}
		public class Bone : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 2;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.magic= true;
			
			
			
		}

		
	}
	
}

