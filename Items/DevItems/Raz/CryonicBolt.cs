using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.DevItems.Raz     ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
	public class CryonicBolt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Raz's Cryonic Bolt");
			Tooltip.SetDefault("The first cryo weapon ever made" + "\nDev Item");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.mana = 25;
			item.width = 100;
			item.height = 100;
			item.useTime = 3;
			item.useAnimation = 18;
			item.reuseDelay = 54;
			//item.reuseDelay = 60;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 1f;
			item.rare = 10;
			item.value = 0;
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("CryonicBoltP");
			item.magic = true;
			item.shootSpeed = 14;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 100f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			int numberOfProjectiles = Main.rand.Next(2, 4);
			for (int i = 0; i < numberOfProjectiles; i++)
			{
				Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedByRandom((float)Math.PI / 16f), type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}

	public class CryonicBoltP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryonic BoltP");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 0;
			//aiType = ProjectileID.Bullet;
			projectile.width = 6;
			projectile.height = 6;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.magic = true;
			projectile.tileCollide = true;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135);
		}

		public override void Kill(int timeLeft)
		{
			int numOfShards = Main.rand.Next(2, 5);
			for (int i = 0; i < numOfShards; i++)
			{
				Vector2 vel = Vector2.UnitX.RotatedByRandom((float)Math.PI * 2) * 14;
				Projectile.NewProjectile(projectile.Center, vel, mod.ProjectileType("CryonicShard"), (int)(projectile.damage * .67f), projectile.knockBack, projectile.owner);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Items/DevItems/Raz/CryonicBoltP");
			spriteBatch.Draw(texture, new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y + 2),
						new Rectangle(0, 0, texture.Width, texture.Height), lightColor, projectile.rotation,
						new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
			return false;
		}
	}

	public class CryonicShard : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryonic BoltP");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.width = 6;
			projectile.height = 6;
			//projectile.extraUpdates = 1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.magic = true;
			projectile.tileCollide = true;

			projectile.timeLeft = 20;
		}
	}
}