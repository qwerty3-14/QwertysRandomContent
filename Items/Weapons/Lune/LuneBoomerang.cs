using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Lune
{
    public class LuneBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Boomerang");
            Tooltip.SetDefault("Rapidly thrown in every direction");
        }

        public override void SetDefaults()
        {
            item.damage = 11;
            item.melee = true;
            item.noMelee = true;

            item.useTime = 4;
            item.useAnimation = 4;

            item.useStyle = 5;
            item.knockBack = 0;
            item.value = 20000;
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.noUseGraphic = true;
            item.width = 18;
            item.height = 32;

            item.autoReuse = true;
            item.shoot = mod.ProjectileType("LuneBoomerangP");
            item.shootSpeed = 10f;
            item.channel = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("LuneBar"), 8);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        /*
        public override bool CanUseItem(Player player)
		{
			for (int i = 0; i < 1000; ++i)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
				{
					return false;
				}
			}
			return true;
		}*/

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 s = new Vector2(speedX, speedY);
            s = s.RotatedBy(Main.rand.NextFloat(-1f, 1f) * (float)Math.PI);
            s *= Main.rand.NextFloat(1f, 1.5f);
            speedX = s.X;
            speedY = s.Y;

            return true;
        }
    }

    public class LuneBoomerangP : ModProjectile
    {
        public override void SetDefaults()
        {
            //projectile.aiStyle = ProjectileID.WoodenBoomerang;
            //aiType = 52;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.width = 18;
            projectile.height = 32;
            projectile.melee = true;
            projectile.usesLocalNPCImmunity = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Boomerang");
        }

        private float speed;
        private float maxSpeed;
        private bool runOnce = true;
        private float decceleration = 1f / 4f;
        private int spinDirection;
        private bool returnToPlayer;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                spinDirection = player.direction;
                speed = projectile.velocity.Length();
                maxSpeed = speed;
                runOnce = false;
            }
            Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("LuneDust"));
            projectile.rotation += MathHelper.ToRadians(maxSpeed * spinDirection);
            if (returnToPlayer)
            {
                if (Collision.CheckAABBvAABBCollision(player.position, player.Size, projectile.position, projectile.Size))
                {
                    projectile.Kill();
                }
                projectile.tileCollide = false;
                //projectile.friendly = false;
                projectile.velocity = QwertyMethods.PolarVector(speed, (player.Center - projectile.Center).ToRotation());
                speed += decceleration;
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
            }
            else
            {
                projectile.velocity = projectile.velocity.SafeNormalize(-Vector2.UnitY) * speed;
                speed -= decceleration;
                if (speed < 1f)
                {
                    returnToPlayer = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("LuneCurse"), 60);
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
            returnToPlayer = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            returnToPlayer = true;
            return false;
        }
    }
}