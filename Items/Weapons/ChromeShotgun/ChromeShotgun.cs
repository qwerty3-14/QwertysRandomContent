using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.ChromeShotgun
{
	public class ChromeShotgunDefault : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chrome Shotgun");
			Tooltip.SetDefault("Right click to switch between 4 modes");
		}

		private Vector2 DefaultHoldOffset = new Vector2(-8, 0);
		private Vector2 DefaultMuzzleOffset = new Vector2(42, -7);
		private Vector2 ReverseHoldOffset = new Vector2(-12, 0);
		private Vector2 ReverseMuzzleOffset = new Vector2(10, -7);
		private Vector2 TightHoldOffset = new Vector2(-10, -4);
		private Vector2 TightMuzzleOffset = new Vector2(50, -5);
		private Vector2 MinionHoldOffset = new Vector2(-6, 0);
		private Vector2 MinionMuzzleOffset = new Vector2(50, -5);

		public override void SetDefaults()
		{
			item.damage = 48;
			item.ranged = true;

			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.knockBack = 1;
			item.value = 500000;
			item.rare = 7;

			item.noUseGraphic = true;
			item.width = 54;
			item.height = 22;

			item.shoot = 97;
			item.useAmmo = 97;
			item.shootSpeed = 9f;
			item.noMelee = true;
			item.GetGlobalItem<ItemUseGlow>().glowOffsetX = (int)DefaultHoldOffset.X;
			item.GetGlobalItem<ItemUseGlow>().glowOffsetY = (int)DefaultHoldOffset.Y;
			item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.SoulofFright, 10);
			recipe.AddIngredient(ItemID.SoulofMight, 10);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(ItemID.SoulofSight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool ConsumeAmmo(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				return false;
			}
			return base.ConsumeAmmo(player);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			switch (item.GetGlobalItem<ChromeGunToggle>().mode)
			{
				case 0:
					item.GetGlobalItem<ItemUseGlow>().glowOffsetX = (int)DefaultHoldOffset.X;
					item.GetGlobalItem<ItemUseGlow>().glowOffsetY = (int)DefaultHoldOffset.Y;
					break;

				case 1:
					item.GetGlobalItem<ItemUseGlow>().glowOffsetX = (int)ReverseHoldOffset.X;
					item.GetGlobalItem<ItemUseGlow>().glowOffsetY = (int)ReverseHoldOffset.Y;
					break;

				case 2:
					item.GetGlobalItem<ItemUseGlow>().glowOffsetX = (int)TightHoldOffset.X;
					item.GetGlobalItem<ItemUseGlow>().glowOffsetY = (int)TightHoldOffset.Y;
					break;

				case 3:
					item.GetGlobalItem<ItemUseGlow>().glowOffsetX = (int)MinionHoldOffset.X;
					item.GetGlobalItem<ItemUseGlow>().glowOffsetY = (int)MinionHoldOffset.Y;
					break;
			}
			if (player.altFunctionUse == 2)
			{
				item.useStyle = 103;
			}
			else
			{
				item.UseSound = SoundID.Item11;
				item.useStyle = 5;
			}
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (item.useStyle == 5)
			{
				float direction = new Vector2(speedX, speedY).ToRotation();
				float horizontalShift = DefaultMuzzleOffset.X;
				float verticalShift = DefaultMuzzleOffset.Y;
				switch (item.GetGlobalItem<ChromeGunToggle>().mode)
				{
					case 0:
						horizontalShift = DefaultMuzzleOffset.X;
						verticalShift = DefaultMuzzleOffset.Y;
						break;

					case 1:
						horizontalShift = ReverseMuzzleOffset.X;
						verticalShift = ReverseMuzzleOffset.Y;
						break;

					case 2:
						horizontalShift = TightMuzzleOffset.X;
						verticalShift = TightMuzzleOffset.Y;
						break;

					case 3:
						horizontalShift = MinionMuzzleOffset.X;
						verticalShift = MinionMuzzleOffset.Y;
						break;
				}
				position += QwertyMethods.PolarVector(horizontalShift, direction) + QwertyMethods.PolarVector(verticalShift * player.direction, direction + (float)Math.PI / 2);
				switch (item.GetGlobalItem<ChromeGunToggle>().mode)
				{
					case 0:
						for (int p = 0; p < 3; p++)
						{
							Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedByRandom(Math.PI / 16f), type, damage, knockBack, player.whoAmI);
						}
						break;

					case 1:
						for (int p = 0; p < 4; p++)
						{
							Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedBy(Math.PI).RotatedByRandom(Math.PI / 12f), type, damage, knockBack, player.whoAmI);
						}
						break;

					case 2:
						for (int p = 0; p < 2; p++)
						{
							Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedByRandom(Math.PI / 128f), type, damage, knockBack, player.whoAmI);
						}
						break;

					case 3:
						return true;
				}
			}
			return false;
		}
	}

	public class ShotgunMinion : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = projectile.height = 10;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.timeLeft = 2;
		}

		private NPC target;
		private bool runOnce = true;
		private Vector2[] GunPositions = new Vector2[2];
		private float[] GunRotations = new float[2];
		private int shotCounter = 0;

		public override void AI()
		{
			if (runOnce)
			{
				projectile.rotation = projectile.velocity.ToRotation();

				runOnce = false;
			}
			Player player = Main.player[projectile.owner];
			shotCounter++;
			if (player.HeldItem.GetGlobalItem<ChromeGunToggle>().mode == 3)
			{
				projectile.timeLeft = 300;
			}
			else if (player.HeldItem.type != mod.ItemType("ChromeShotgunDefault"))
			{
				projectile.Kill();
			}
			Vector2 spot = player.Center;
			if (QwertyMethods.ClosestNPC(ref target, 1000, projectile.Center, false, player.MinionAttackTargetNPC))
			{
				spot = target.Center;
			}
			Vector2 Difference = (spot - projectile.Center);
			projectile.rotation = QwertyMethods.SlowRotation(projectile.rotation, Difference.ToRotation(), 5);
			if (Difference.Length() > 100)
			{
				projectile.velocity = Difference *= .02f;
			}
			else
			{
				projectile.velocity = Vector2.Zero;
			}
			for (int i = 0; i < 2; i++)
			{
				NPC gunTarget = new NPC();
				if (QwertyMethods.ClosestNPC(ref gunTarget, 450, GunPositions[i], false, player.MinionAttackTargetNPC))
				{
					GunRotations[i] = QwertyMethods.SlowRotation(GunRotations[i], (gunTarget.Center - GunPositions[i]).ToRotation() + (float)Math.PI / 2 * (i == 1 ? 1 : -1) - projectile.rotation, 6);
					if (shotCounter % 30 == i * 15)
					{
						int bullet = ProjectileID.Bullet;
						bool canShoot = true;
						float speedB = 14f;
						int weaponDamage = projectile.damage;
						float kb = projectile.knockBack;
						player.PickAmmo(QwertyMethods.MakeItemFromID(mod.ItemType("ChromeShotgunDefault")), ref bullet, ref speedB, ref canShoot, ref weaponDamage, ref kb, false);
						if (canShoot)
						{
							Projectile.NewProjectile(GunPositions[i], QwertyMethods.PolarVector(16, GunRotations[i] + projectile.rotation + (float)Math.PI / 2 * (i == 0 ? 1 : -1)), bullet, weaponDamage, kb, projectile.owner);
						}
					}
				}
				else
				{
					GunRotations[i] = QwertyMethods.SlowRotation(GunRotations[i], 0f, 6);
				}
			}
			GunPositions[0] = projectile.Center + QwertyMethods.PolarVector(13, projectile.rotation) + QwertyMethods.PolarVector(14, projectile.rotation + (float)Math.PI / 2);
			GunPositions[1] = projectile.Center + QwertyMethods.PolarVector(13, projectile.rotation) + QwertyMethods.PolarVector(14, projectile.rotation - (float)Math.PI / 2);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D core = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(core, projectile.Center - Main.screenPosition, core.Frame(), lightColor, projectile.rotation - (float)Math.PI / 2, core.Size() * .5f, 1f, SpriteEffects.None, 0);

			Texture2D rightGun = mod.GetTexture("Items/Weapons/ChromeShotgun/ShotgunMinionRightGun");
			spriteBatch.Draw(rightGun, GunPositions[1] - Main.screenPosition, rightGun.Frame(), lightColor, projectile.rotation + GunRotations[1] - (float)Math.PI / 2, new Vector2(8, 8), 1f, SpriteEffects.None, 0);

			Texture2D leftGun = mod.GetTexture("Items/Weapons/ChromeShotgun/ShotgunMinionLeftGun");
			spriteBatch.Draw(leftGun, GunPositions[0] - Main.screenPosition, leftGun.Frame(), lightColor, projectile.rotation + GunRotations[0] - (float)Math.PI / 2, new Vector2(leftGun.Width - 8, 8), 1f, SpriteEffects.None, 0);
			return false;
		}
	}

	public class ChromeGunToggle : GlobalItem
	{
		public int mode = 0;
		public override bool CloneNewInstances => true;
		public override bool InstancePerEntity => true;
	}

	public class ChromeGunUseDraw : ModPlayer
	{
		public override void PostItemCheck()
		{
			if (!player.inventory[player.selectedItem].IsAir)
			{
				Item item = player.inventory[player.selectedItem];
				if (item.useStyle == 103 && player.itemAnimation > 0)
				{
					player.bodyFrame.Y = player.bodyFrame.Height * 3;
					player.itemRotation = (float)(-Math.PI * 2f * player.direction) * ((float)player.itemAnimation / player.itemAnimationMax);
					player.itemLocation.X = player.position.X + (float)player.width * 0.5f - (float)Main.itemTexture[item.type].Width * 0.5f - (float)(player.direction * 2);
					player.itemLocation.Y = player.MountedCenter.Y - (float)Main.itemTexture[item.type].Height * 0.5f;
					if (player.itemAnimation == player.itemAnimationMax / 2)
					{
						item.GetGlobalItem<ChromeGunToggle>().mode++;
						if (item.GetGlobalItem<ChromeGunToggle>().mode >= 4)
						{
							item.GetGlobalItem<ChromeGunToggle>().mode = 0;
						}
					}
				}
				if (player.HeldItem.type == mod.ItemType("ChromeShotgunDefault") && player.HeldItem.GetGlobalItem<ChromeGunToggle>().mode == 3 && player.ownedProjectileCounts[mod.ProjectileType("ShotgunMinion")] < 1)
				{
					Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("ShotgunMinion"), player.HeldItem.damage, player.HeldItem.knockBack, player.whoAmI);
				}
			}
		}

		public static readonly PlayerLayer ChromeGun = new PlayerLayer("QwertysRandomContent", "ChromeGun", PlayerLayer.HeldItem, delegate (PlayerDrawInfo drawInfo)
		{
			if (drawInfo.shadow != 0f)
			{
				return;
			}

			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("QwertysRandomContent");
			if (!drawPlayer.HeldItem.IsAir && drawPlayer.HeldItem.type == mod.ItemType("ChromeShotgunDefault"))
			{
				Item item = drawPlayer.HeldItem;
				Color color37 = Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16.0));
				Texture2D texture = mod.GetTexture("Items/Weapons/ChromeShotgun/ChromeShotgunDefault");
				switch (item.GetGlobalItem<ChromeGunToggle>().mode)
				{
					case 0:
						texture = mod.GetTexture("Items/Weapons/ChromeShotgun/ChromeShotgunDefault");
						break;

					case 1:
						texture = mod.GetTexture("Items/Weapons/ChromeShotgun/ChromeShotgunReverseFire");
						break;

					case 2:
						texture = mod.GetTexture("Items/Weapons/ChromeShotgun/ChromeShotgunTight");
						break;

					case 3:
						texture = mod.GetTexture("Items/Weapons/ChromeShotgun/ChromeShotgunMinionMode");
						break;
				}
				Vector2 zero2 = Vector2.Zero;

				if (texture != null && drawPlayer.itemAnimation > 0)
				{
					Vector2 vector10 = new Vector2((float)(Main.itemTexture[item.type].Width / 2), (float)(Main.itemTexture[item.type].Height / 2));
					Vector2 vector11 = new Vector2(10, texture.Height / 2);
					if (item.GetGlobalItem<ItemUseGlow>().glowOffsetX != 0)
					{
						vector11.X = item.GetGlobalItem<ItemUseGlow>().glowOffsetX;
					}
					vector11.Y += item.GetGlobalItem<ItemUseGlow>().glowOffsetY * drawPlayer.gravDir;
					int num107 = (int)vector11.X;
					vector10.Y = vector11.Y;
					Color chromeGunColor = drawPlayer.inventory[drawPlayer.selectedItem].GetAlpha(color37);
					Vector2 value2 = drawInfo.itemLocation;

					Vector2 origin5 = new Vector2((float)(-(float)num107), (float)(Main.itemTexture[item.type].Height / 2));
					if (drawPlayer.direction == -1)
					{
						origin5 = new Vector2((float)(Main.itemTexture[item.type].Width + num107), (float)(Main.itemTexture[item.type].Height / 2));
					}

					DrawData value = new DrawData(texture,
						new Vector2((float)((int)(value2.X - Main.screenPosition.X + vector10.X)), (float)((int)(value2.Y - Main.screenPosition.Y + vector10.Y))),
						new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, Main.itemTexture[item.type].Width, Main.itemTexture[item.type].Height)),
						chromeGunColor,
						drawPlayer.itemRotation,
						origin5,
						item.scale,
						drawInfo.spriteEffects, 0);
					Main.playerDrawData.Add(value);
				}
			}
		});

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int itemLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("HeldItem"));
			if (itemLayer != -1)
			{
				ChromeGun.visible = true;
				layers.Insert(itemLayer + 1, ChromeGun);
			}
		}
	}
}