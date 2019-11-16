using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.RuneGhostItems
{
	public class ExpertItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hyper Runestone");
			Tooltip.SetDefault("+10 dash power" + "\nMakes you invincible when dashing" + "\nThis effect needs two seconds to recharge");


        }
		
		public override void SetDefaults()
		{

			item.width = 76;
			item.height = 76;
			item.maxStack = 1;
			item.value = 500000;
			item.rare = 3;
			
			item.rare = 9;
            item.expert=true;
            item.accessory = true;
		}
        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            modPlayer.hyperRune = true;
            modPlayer.customDashBonusSpeed += 10;
            

        }





    }
    class SignalRune : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 76;
            projectile.height = 76;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.tileCollide = false;
            projectile.light = 1f;
            projectile.timeLeft = 2;


        }
        public bool runOnce = true;
        public int time;
        public override void AI()
        {
            
            Player player = Main.player[projectile.owner];


            if (projectile.alpha > 0)
                projectile.alpha--;
            else
                projectile.alpha = 0;

            
                projectile.position.X = player.Center.X - 38;
                projectile.position.Y = player.Center.Y - 38;
                projectile.rotation += MathHelper.ToRadians(3);
            

        }

    }
}
