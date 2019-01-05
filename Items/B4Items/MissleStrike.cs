using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.B4Items
{
	public class MissileStrike : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile Strike");
			Tooltip.SetDefault("Fire rockets from the sky!");
			
		}
		public override void SetDefaults()
		{
			item.damage = 93;
			item.ranged = true;
			
			item.useTime = 11;
			item.useAnimation = 33;
            
			item.useStyle = 5;
			item.knockBack = 4f;
            item.value = 750000;
            item.rare = 10;
			item.UseSound = SoundID.Item11;
			
			item.width = 64;
			item.height = 188;
			
			item.shoot = 134;
			item.useAmmo = AmmoID.Rocket;
			item.shootSpeed =12f;
			item.noMelee=true;
            item.autoReuse = true;


        }
        
        public int shotCounter=2;
        public bool consumeRocket;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            position = new Vector2(Main.MouseWorld.X + Main.rand.Next(-50, 50), position.Y-600);
            float trueSpeed = new Vector2(speedX, speedY).Length();
            int shift = Main.rand.Next(-100, 100);
            speedX = (float)Math.Cos(( new Vector2 (Main.MouseWorld.X + shift, Main.MouseWorld.Y)-position).ToRotation()) * trueSpeed;
            speedY = (float)Math.Sin(( new Vector2(Main.MouseWorld.X + shift, Main.MouseWorld.Y) - position).ToRotation()) * trueSpeed;
            shotCounter++;
            if(shotCounter%3==0)
            {
                consumeRocket = true;
            }
            else
            {
                consumeRocket = false;
            }
            return true;
        }
        public override bool ConsumeAmmo(Player player)
        {


            return consumeRocket;
        }


    }

    
    


}

