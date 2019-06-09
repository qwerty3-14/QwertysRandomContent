using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.AncientItems
{
	public class AncientBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Blade");
			Tooltip.SetDefault("Launches a spread of orbs");
			
		}
		public override void SetDefaults()
		{
			item.damage = 18;
			item.melee = true;
			
			item.useTime = 1;
			item.useAnimation = 35;
			item.useStyle = 1;
			item.knockBack = 5;
			item.value = 150000;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
            
            item.width = 70;
			item.height = 70;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/AncientItems/AncientBlade_Glow");
            }

            item.autoReuse = true;
			item.shoot = mod.ProjectileType("AncientOrb");
			item.shootSpeed =9;
			
			
			
			
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/AncientItems/AncientBlade_Glow");
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            float outAmount = 10f;
            float direction = new Vector2(speedX, speedY).ToRotation();
            position += new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction)) * outAmount;
            int percentage = (int)(player.itemAnimationMax * .1f);
            if(player.itemAnimation == player.itemAnimationMax- percentage || player.itemAnimation == player.itemAnimationMax - 2 * percentage || player.itemAnimation == player.itemAnimationMax - 3* percentage)
            {
                return true;
            }
            return false;
            
		}
		
		

		
	}
		public class AncientOrb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 2;
			DisplayName.SetDefault("Ancient Orb");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet; 
			projectile.width = 20;
			projectile.height = 20;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.melee= true;
			projectile.tileCollide= true;
			projectile.timeLeft = 40;
            projectile.alpha = 255;



        }
        public int dustTimer;
        public override void AI()
        {
            if (projectile.alpha > 0)
            {
                //projectile.alpha -= (int)(255f / 180f);
                projectile.alpha -= 4;
            }
            else
            {
                projectile.alpha = 0;
            }
            projectile.scale = .5f + (.5f * 1 - (projectile.alpha / 255f));
            for (int d = 0; d < projectile.alpha / 30; d++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(25, theta), mod.DustType("AncientGlow"), QwertyMethods.PolarVector(-6, theta) + projectile.velocity);
                dust.scale = .5f;
                dust.alpha = 255;
            }
           
            projectile.frameCounter++;
            if (projectile.frameCounter > 10)
            {
                if (projectile.frame == 1)
                {
                    projectile.frame = 0;
                }
                else
                {
                    projectile.frame = 1;
                }
                projectile.frameCounter = 0;
            }
            dustTimer++;
            if (dustTimer > 5)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("AncientGlow"), 0, 0, 0, default(Color), .2f);
                dustTimer = 0;
            }
        }
        public override bool OnTileCollide(Vector2 velocityChange)  
        {
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
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {

            spriteBatch.Draw(mod.GetTexture("Items/AncientItems/AncientOrb"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), Color.Lerp(new Color(1f, 1f, 1f, 1f), new Color(0, 0, 0, 0), (float)projectile.alpha / 255f), projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), projectile.scale, SpriteEffects.None, 0f);
            return false;
        }


    }
		
	
}

