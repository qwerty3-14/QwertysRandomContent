using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.B4Items
{
	public class B4GiantBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Giant Bow");
			Tooltip.SetDefault("Arrows fired from this are YUUUUUGE! which split into more arrows!");
			
		}
		public override void SetDefaults()
		{
			item.damage = 112;
			item.ranged = true;
			
			item.useTime = 47;
			item.useAnimation = 47;
            
			item.useStyle = 5;
			item.knockBack = 10f;
            item.value = 750000;
            item.rare = 10;
			item.UseSound = SoundID.Item5;
			
			item.width = 64;
			item.height = 188;
			
			item.shoot = 40;
			item.useAmmo = 40;
			item.shootSpeed =12f;
			item.noMelee=true;
            item.autoReuse = true;


        }
        public Projectile arrow;
        
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            
            
            arrow= Main.projectile[Projectile.NewProjectile(position.X , position.Y , speedX, speedY, type, damage, knockBack, player.whoAmI)];
            arrow.scale *= 3;
            arrow.GetGlobalProjectile<arrowgigantism>(mod).GiganticArrow = true;
            if (Main.netMode == 1)
            {
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)ModMessageType.ScaleMessage); // Message type, you would need to create an enum for this
                packet.Write(arrow.identity); // tells which projectile is being modified by the effect, the effect is then applied on the receiving end
                packet.Write((byte)player.whoAmI); // the player that shot the projectile, will be useful later
                packet.Send();
            }
            return false;
        }


    }

    public class arrowgigantism : GlobalProjectile
    {
        public bool GiganticArrow;
        
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override void AI(Projectile projectile)
        {
            if (GiganticArrow)
            {
                projectile.scale = 3;
            }
        }
        public override void Kill(Projectile projectile, int timeLeft)        
        {

            if(GiganticArrow)
            {
                projectile.netUpdate = true;


                for (int r = 0; r < 8; r++)
                {
                    float shotSpeed = projectile.velocity.Length();
                    if(projectile.type == mod.ProjectileType("ReverseArrowP"))
                    {
                        Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(r * (2 * Math.PI / 8)) * shotSpeed, (float)Math.Sin(r * (2 * Math.PI / 8)) * shotSpeed, mod.ProjectileType("ReverseArrowS"), projectile.damage / 2, 0, Main.myPlayer);
                        Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(r * (2 * Math.PI / 8) + Math.PI / 8) * shotSpeed * 1.5f, (float)Math.Sin(r * (2 * Math.PI / 8) + Math.PI / 8) * shotSpeed * 1.5f, mod.ProjectileType("ReverseArrowS"), projectile.damage / 2, 0, Main.myPlayer);
                    }
                    else
                    {
                        Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(r * (2 * Math.PI / 8)) * shotSpeed, (float)Math.Sin(r * (2 * Math.PI / 8)) * shotSpeed, projectile.type, projectile.damage / 2, 0, Main.myPlayer);
                        Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(r * (2 * Math.PI / 8) + Math.PI / 8) * shotSpeed * 1.5f, (float)Math.Sin(r * (2 * Math.PI / 8) + Math.PI / 8) * shotSpeed * 1.5f, projectile.type, projectile.damage / 2, 0, Main.myPlayer);
                    }
                    
                }






            }
        }


    }
    


}

