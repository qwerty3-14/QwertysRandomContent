using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.Weapons.Rhuthinium 
{
	public class RhuthiniumSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rhuthinium Sword");
			Tooltip.SetDefault("Killing enemies builds up a charge. Right click to realease this charge.");
			
		}
		public override void SetDefaults()
		{
			item.damage = 22;
			item.melee = true;
			
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 3;
			item.value = 25000;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			
			item.width = 64;
			item.height = 64;
			item.crit = 5;
			item.autoReuse = true;
            //item.scale = 5;
			
			
			
		}
		

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            
            if (target.life <= 0 && !target.SpawnedFromStatue)
            {
                modPlayer.RhuthiniumCharge++;
                CombatText.NewText(target.getRect(), new Color(38, 126, 126), modPlayer.RhuthiniumCharge, true, false);
            }
        }
    }
    public class RhuthiniumCharge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Charge");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.melee = true;
            projectile.knockBack = 10f;
            



        }
        public override void AI()
        {





        }

        public override void Kill(int timeLeft)
        {



        }
    }



}

