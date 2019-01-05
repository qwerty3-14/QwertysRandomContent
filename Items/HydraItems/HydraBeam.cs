using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
	public class HydraBeam : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Beam");
			Tooltip.SetDefault("Creates a beam of destructive energy from the sky");
			
		}
		public override void SetDefaults()
		{
			item.damage = 60;
			item.magic = true;
			
			item.useTime = 28;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.knockBack = 100;
            item.value = 250000;
            item.rare = 5;   
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.width = 28;
			item.height = 30;
			
			item.mana =80;
			item.shoot = mod.ProjectileType("BeamHead");
			item.shootSpeed =9;
			item.noMelee=true;
			
			
			
			
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            
            for (int l = 0; l < Main.projectile.Length; l++)
            {                                                                  //this make so you can only spawn one of this projectile at the time,
                Projectile proj = Main.projectile[l];
                if (proj.active && proj.type == item.shoot && proj.owner == player.whoAmI)
                {
                    proj.active = false;
                }
            }
            return true;
        }

		
	}
		public class BeamHead : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beam Head");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.timeLeft=300;
			projectile.width = 72;
			projectile.height = 112;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.magic= true;
			projectile.tileCollide= false;
			
		}
		public bool runOnce=true;
		public override void AI()
        {
			Player player = Main.player[projectile.owner];
			if(runOnce)
			{
			
            projectile.position = new Vector2(player.Center.X, player.Center.Y-500);;
			runOnce=false;
			}
			if(Main.netMode != 1)
			{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType("HeadBeam"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f); 
			}
			projectile.velocity.X=10f*player.direction;
			projectile.position.Y=player.Center.Y-500;
		}

		
	}
		public class HeadBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Head Beam");
			
			
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			
			projectile.width = 18;
			projectile.height = 120;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.magic= true;
			projectile.tileCollide= false;
			projectile.timeLeft=40;
			
			
		}
		public Projectile Head;
		public override void AI()
        {
			foreach(Projectile projSearch in Main.projectile)
			{	
				if(projSearch.type == mod.ProjectileType("BeamHead"))
				Head = projSearch;
			}
			Player player = Main.player[projectile.owner];
			
			
			projectile.velocity.X=Head.velocity.X;
			projectile.velocity.Y=50f;
			CreateDust();

		}
		public virtual void CreateDust()
		{
			
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBeamGlow"));
				
				Main.dust[dust].velocity.X =projectile.velocity.X;
			
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            if (!target.immortal && !target.boss)
            {
                target.velocity.X += .3f * player.direction * target.knockBackResist;
                target.velocity.Y -= .2f * target.knockBackResist;
            }
        }


    }
	
}

