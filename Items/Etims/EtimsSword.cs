using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using static QwertysRandomContent.Items.Accesories.SkywardHilt;

namespace QwertysRandomContent.Items.Etims
{
	public class EtimsSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Massacre");
			Tooltip.SetDefault("Right click on the ground for an uppercut" + "\nRight click in the air to slam down!");
		}

		public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? base.Texture + "_Old" : base.Texture;

		public override void SetDefaults()
		{
			item.useStyle = 101; //custom use style
			item.autoReuse = true;
			item.melee = true;
			item.noMelee = true;
			item.damage = 39;
			item.knockBack = 7f;
			item.width = 36;
			item.height = 34;
			item.noUseGraphic = true;
			item.useTime = 18;
			item.useAnimation = 18;
			item.rare = 3;
			item.value = 120000;
			item.useTurn = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EtimsMaterial"), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool CanUseItem(Player player)
		{
			return player.itemAnimation == 0;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
	}

	public class AltSword : ModPlayer
	{
		private int[] localNPCImmunity = new int[Main.npc.Length];
		private bool uppercut = false;
		private bool slam = false;
		private bool hasRightClicked = false;

		public override void PostItemCheck()
		{
			if (!player.inventory[player.selectedItem].IsAir)
			{
				Point origin = player.Bottom.ToTileCoordinates();
				Point point;
				Item item = player.inventory[player.selectedItem];
				//Main.NewText(player.itemAnimation  + " / " + player.itemAnimationMax);

				if (item.useStyle == 101)
				{
					if (Main.mouseRight && Main.myPlayer == item.owner && !hasRightClicked)
					{
						if (WorldUtils.Find(origin, Searches.Chain(new Searches.Down(3), new GenCondition[]
											{
											new Conditions.IsSolid()
											}), out point))
						{
							player.itemAnimation = player.itemAnimationMax;
							player.velocity.Y = -10 - player.jumpSpeedBoost;
							uppercut = true;
							slam = false;
						}
						else
						{
							player.velocity.Y = 10;
							slam = true;
							uppercut = false;
						}
					}
					float shift = 0f;
					if (player.itemAnimation > 0 && uppercut || slam)
					{
						if (slam)
						{
							//Main.NewText("Slamming");
							player.bodyFrame.Y = player.bodyFrame.Height * 4;
							shift = (float)Math.PI / 2;

							if (player.velocity.Y != 0)
							{
								player.itemAnimation = 2;
							}
							else
							{
								player.itemAnimation = 0;
								slam = false;
							}
						}
						else if (uppercut)
						{
							shift = (float)Math.PI / 2 * ((float)player.itemAnimation / (float)player.itemAnimationMax) - (float)Math.PI / 4;

							if (player.itemAnimation < player.itemAnimationMax * .5f)
							{
								player.bodyFrame.Y = player.bodyFrame.Height * 2;
							}
							else if (player.itemAnimation < player.itemAnimationMax * .25f)
							{
								player.bodyFrame.Y = player.bodyFrame.Height * 3;
							}
							else
							{
								player.bodyFrame.Y = player.bodyFrame.Height * 4;
							}
							if (player.itemAnimation < 2)
							{
								player.itemAnimation = 2;
							}
							if (player.velocity.Y >= 0)
							{
								player.itemAnimation = 0;
								uppercut = false;
							}
						}
					}
					else
					{
						if (player.itemAnimation < player.itemAnimationMax * .25f)
						{
							shift = (float)Math.PI / -4 * ((player.itemAnimation) / (player.itemAnimationMax * .25f));
						}
						else if (player.itemAnimation < player.itemAnimationMax * .75f)
						{
							shift = (float)Math.PI / -2 * (1 - (player.itemAnimation - (player.itemAnimationMax * .25f)) / (player.itemAnimationMax * .5f)) + (float)Math.PI / 4;
						}
						else
						{
							shift = (float)Math.PI / 4 * (1 - (player.itemAnimation - (player.itemAnimationMax * .75f)) / (player.itemAnimationMax * .25f));
						}
						if (player.itemAnimation < player.itemAnimationMax * .15f)
						{
							player.bodyFrame.Y = player.bodyFrame.Height * 3;
						}
						else if (player.itemAnimation < player.itemAnimationMax * .35f)
						{
							player.bodyFrame.Y = player.bodyFrame.Height * 2;
						}
						else if (player.itemAnimation < player.itemAnimationMax * .65f)
						{
							player.bodyFrame.Y = player.bodyFrame.Height * 3;
						}
						else if (player.itemAnimation < player.itemAnimationMax * .85f)
						{
							player.bodyFrame.Y = player.bodyFrame.Height * 4;
						}
						else
						{
							player.bodyFrame.Y = player.bodyFrame.Height * 3;
						}
					}
					if (Main.mouseRight && Main.myPlayer == item.owner && !slam && !uppercut)
					{
						player.itemAnimation = 0;
					}

					player.itemRotation = (float)Math.PI / -4 + player.direction * ((float)Math.PI / 2 + shift);
					//Main.NewText(MathHelper.ToDegrees(player.itemRotation));

					Vector2 vector24 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
					if (player.direction != 1)
					{
						vector24.X = (float)player.bodyFrame.Width - vector24.X;
					}
					if (player.gravDir != 1f)
					{
						vector24.Y = (float)player.bodyFrame.Height - vector24.Y;
					}
					vector24 -= new Vector2((float)(player.bodyFrame.Width - player.width), (float)(player.bodyFrame.Height - 42)) / 2f;
					player.itemLocation = player.position + vector24;

					float swordLength = new Vector2(Main.itemTexture[item.type].Width, Main.itemTexture[item.type].Height).Length();
					swordLength *= item.scale;
					for (int n = 0; n < Main.npc.Length; n++)
					{
						localNPCImmunity[n]--;
						if (Main.npc[n].active && !Main.npc[n].dontTakeDamage && (!Main.npc[n].friendly || (Main.npc[n].type == 22 && player.killGuide) || (Main.npc[n].type == 54 && player.killClothier)) && player.itemAnimation > 0 && localNPCImmunity[n] <= 0 && Collision.CheckAABBvLineCollision(Main.npc[n].position, Main.npc[n].Size, player.itemLocation, player.itemLocation + QwertyMethods.PolarVector(swordLength, player.itemRotation - (float)Math.PI / 4)))
						{
							localNPCImmunity[n] = item.useAnimation / 3;
							int damageBeforeVariance = item.damage;
							if (item.melee)
							{
								damageBeforeVariance = (int)((float)item.damage * player.meleeDamage);
							}
							if (item.ranged)
							{
								damageBeforeVariance = (int)((float)item.damage * player.rangedDamage);
							}
							if (item.magic)
							{
								damageBeforeVariance = (int)((float)item.damage * player.magicDamage);
							}
							if (item.summon)
							{
								damageBeforeVariance = (int)((float)item.damage * player.minionDamage);
							}
							if (item.thrown)
							{
								damageBeforeVariance = (int)((float)item.damage * player.thrownDamage);
							}
							if (slam || uppercut)
							{
								damageBeforeVariance *= 2;
							}
							if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(3), new GenCondition[]
											{
											new Conditions.IsSolid()
											}), out point) && player.GetModPlayer<SkywardHiltEffect>().effect && player.grappling[0] == -1)
							{
								damageBeforeVariance *= 2;
							}
							//////////////////////

							Projectile p = QwertyMethods.PokeNPC(player, Main.npc[n], damageBeforeVariance, item.knockBack, true);
							if (item.type == mod.ItemType("EtimsSword"))
							{
								p.GetGlobalProjectile<Etims>().effect = true;
							}
						}
					}
					hasRightClicked = (Main.mouseRight && Main.myPlayer == item.owner);
				}
			}
		}

		public static readonly PlayerLayer AltSwordVisual = new PlayerLayer("QwertysRandomContent", "AltSwordVisual", PlayerLayer.HeldItem, delegate (PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("QwertysRandomContent");
			Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
			if (!drawPlayer.HeldItem.IsAir && drawPlayer.HeldItem.type == mod.ItemType("EtimsSword") && drawPlayer.itemAnimation > 0)
			{
				Item item = drawPlayer.HeldItem;
				Texture2D texture = Main.itemTexture[item.type];
				Vector2 origin = new Vector2(0, texture.Height);
				DrawData value = new DrawData(texture, drawPlayer.itemLocation - Main.screenPosition, texture.Frame(), color12, drawPlayer.itemRotation, origin, item.scale, SpriteEffects.None, 0);
				Main.playerDrawData.Add(value);
			}
		});

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int itemLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("HeldItem"));
			if (itemLayer != -1)
			{
				AltSwordVisual.visible = true;
				layers.Insert(itemLayer + 1, AltSwordVisual);
			}
		}
	}
}