using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.BladeBossItems
{
	public class ImperiousTheIV : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Imperious The IV");
			Tooltip.SetDefault("Hitting enemies builds up swords which can be sent with right click");
			
		}
		public override void SetDefaults()
		{
			item.damage = 41;
			item.melee = true;
			
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 1;
			item.knockBack = 0;
			item.value = Item.sellPrice(gold: 10);
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			
			item.width = 40;
			item.height = 40;
			item.crit = 20;
			item.autoReuse = true;
            //item.scale = 5;
			
			
			
		}


        
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
           
        }
        
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if(player.whoAmI == Main.myPlayer && !target.immortal && player.ownedProjectileCounts[mod.ProjectileType("ImperiousTheV")] <40)
            {
                Projectile.NewProjectile(target.Center, Vector2.Zero, mod.ProjectileType("ImperiousTheV"), item.damage, item.knockBack, player.whoAmI);
            }
        }
    }

    public class ImperiousTheV : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Imperious The V");
           

        }
        public override void SetDefaults()
        {


            projectile.width = 22;
            projectile.height = 22;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            Main.projFrames[projectile.type] = 1;
            projectile.knockBack = 10f;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.melee = true;
            //projectile.minionSlots = 1;
            projectile.timeLeft = 2;
            projectile.aiStyle = -1;
            projectile.usesLocalNPCImmunity = true;
           
        }

        int AttackMode = 1;
        const int idle = 1;
        const int charging = 2;
        const int returning = 3;
        float Xvar = 0;
        float Yvar = 0;
        Vector2 moveTo;
        float acceleration = .3f;
        float maxSpeed = 6f;
        int timer = 61;
        float distanceFromPlayer;
        float maxDistanceFromPlayer = 500;
        int attackTimer;
        int attackCooldown = 30;
        NPC target;
        NPC possibleTarget;
        float targetDistanceFromPlayer;
        float targetMaxDistanceFromPlayer = 400;
        bool foundTarget;
        float chargeSpeed = 14f;
        int chargeTime = 30;
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            //Main.NewText(moveTo);
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
           
            
            
            distanceFromPlayer = (player.Center - projectile.Center).Length();
            timer++;
            switch (AttackMode)
            {

                case idle:
                    projectile.timeLeft = 120;
                    attackTimer++;
                    if (timer > 60)
                    {
                        if (Main.netMode != 2)
                        {
                            Yvar = Main.rand.Next(0, 80);
                            Xvar = Main.rand.Next(-80, 80);
                        }
                        timer = 0;
                    }
                    moveTo = new Vector2(player.Center.X + Xvar, player.Center.Y - Yvar);
                    if(Main.mouseRight && Main.myPlayer == projectile.owner)
                    {
                        projectile.velocity = (Main.MouseWorld- projectile.Center).SafeNormalize(-Vector2.UnitY) * chargeSpeed;
                        attackTimer = 0;
                        AttackMode = charging;
                    }
                    
                    break;
                case charging:
                    
                    attackTimer++;
                    
                    break;
                



            }
            if (AttackMode == charging)
            {
                projectile.friendly = true;
                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;
            }
            else
            {
                projectile.friendly = false;
                projectile.velocity += (moveTo - projectile.Center).SafeNormalize(-Vector2.UnitY) * acceleration;
                if (projectile.velocity.Length() > maxSpeed)
                {
                    projectile.velocity = (moveTo - projectile.Center).SafeNormalize(-Vector2.UnitY) * maxSpeed;
                }
                projectile.rotation = QwertyMethods.SlowRotation(projectile.rotation, projectile.velocity.ToRotation() + (float)Math.PI / 2, 3);
            }
            targetMaxDistanceFromPlayer = 400;
            foundTarget = false;
            //projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }



    }


}

