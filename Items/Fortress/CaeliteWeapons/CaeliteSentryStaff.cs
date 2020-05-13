using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.CaeliteWeapons
{
	public class CaeliteSentryStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sky Spiral Staff");
			Tooltip.SetDefault("Higher beings will punish all enemies near this sentry!");
		}

		public override void SetDefaults()
		{
			item.damage = 18;
			item.mana = 20;
			item.width = 32;
			item.height = 32;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 1;
			item.noMelee = true;
			item.knockBack = 0f;
			item.value = 25000;
			item.rare = 3;
			item.UseSound = SoundID.Item44;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("CaeliteSentry");
			item.summon = true;
			item.sentry = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CaeliteBar"), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;   //this make so the projectile will spawn at the mouse cursor position

			return true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				player.MinionNPCTargetAim();
			}
			return base.UseItem(player);
		}
	}

	public class CaeliteSentry : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sky bound spiral");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.sentry = true;
			projectile.width = 32;
			projectile.height = 32;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.ignoreWater = true;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.knockBack = 10f;
			projectile.penetrate = -1;
			projectile.tileCollide = true;
			projectile.sentry = true;
			projectile.minion = true;
		}

		private NPC target;
		private NPC possibleTarget;
		private bool foundTarget;
		private float maxDistance = 300f;
		private float distance;
		private int timer;

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			player.UpdateMaxTurrets();
			timer++;
			if (timer % 120 == 0)
			{
				for (int k = 0; k < 200; k++)
				{
					possibleTarget = Main.npc[k];
					distance = (possibleTarget.Center - projectile.Center).Length();
					if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && !possibleTarget.immortal && Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0))
					{
						possibleTarget.StrikeNPC(projectile.damage, projectile.knockBack, 0, false, false);

						//Projectile.NewProjectile(possibleTarget.Center, Vector2.Zero, mod.ProjectileType("CaeliteZap"), projectile.damage, 0, projectile.owner, k);
						for (int d = 0; d < distance; d += 4)
						{
							Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(d, (possibleTarget.Center - projectile.Center).ToRotation()), mod.DustType("CaeliteDust"));
							dust.frame.Y = 0;
						}
					}
				}
			}
			Dust dust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"))];
			dust2.scale = .5f;
			projectile.frameCounter++;
			if (projectile.frameCounter > 10)
			{
				projectile.frame++;
				if (projectile.frame >= 4)
				{
					projectile.frame = 0;
				}
				projectile.frameCounter = 0;
			}
		}
	}

	/*
    public class CaeliteZap : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Zap");
        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.minion = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 2;
            projectile.usesLocalNPCImmunity = true;
            projectile.tileCollide = false;
            for (int n = 0; n < 200; n++)
            {
                projectile.localNPCImmunity[n] = -1;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void AI()
        {
                    projectile.localNPCImmunity[(int)projectile.ai[0]] = 0;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(10) == 0)
            {
                target.AddBuff(mod.BuffType("PowerDown"), 120);
            }
        }
    }
    */
}