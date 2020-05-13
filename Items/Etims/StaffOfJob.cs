using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Etims
{
	public class StaffOfJob : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Staff Of Job");
			Tooltip.SetDefault("Inflicts grave misery at the victim near your cursor!");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			item.magic = true;
			item.autoReuse = true;
			item.noMelee = true;
			item.width = item.height = 48;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.damage = 200;
			item.mana = 4;
			item.shootSpeed = 1f;
			item.shoot = 1;
			item.useTime = 1;
			item.useAnimation = 10;
			item.UseSound = SoundID.Item20;
			item.rare = 3;
			item.value = 120000;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EtimsMaterial"), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips) //runs through all tooltip lines
			{
				if (line.mod == "Terraria" && line.Name == "Damage") //this checks if it's the line we're interested in
				{
					line.text = (int)(item.damage * Main.player[item.owner].magicDamage) + " damage per second";//change tooltip
				}
				if (line.mod == "Terraria" && (line.Name == "CritChance" || line.Name == "Knockback" || line.Name == "Speed"))
				{
					line.text = "";
				}
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			NPC target = new NPC();
			if (QwertyMethods.ClosestNPC(ref target, 100, Main.MouseWorld, true))
			{
				target.GetGlobalNPC<GraveMisery>().MiseryIntensity = (int)(item.damage * 2 * player.magicDamage);
			}
			return false;
		}
	}

	public class GraveMisery : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public int MiseryIntensity = 0;

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (MiseryIntensity > 0)
			{
				npc.lifeRegenExpectedLossPerSecond = 1;
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= MiseryIntensity;
			}
		}

		private float trigCounter = 0f;

		public override void AI(NPC npc)
		{
			if (MiseryIntensity > 0)
			{
				MiseryIntensity--;
				trigCounter += MiseryIntensity * (float)Math.PI / (60f * 240f);
				if (MiseryIntensity > 5)
				{
					MiseryIntensity = 5;
				}
			}
		}

		public Texture2D DrawCurve(int width, int height, float shift, bool increasing)
		{
			if (Math.Sin(trigCounter + shift) < 0)
			{
				increasing = !increasing;
			}
			if (Math.Cos(trigCounter + shift) < 0)
			{
				increasing = !increasing;
			}
			height = (int)(height * Math.Abs(Math.Sin(trigCounter + shift)));
			width /= 2;
			height /= 2;
			if (width % 2 == 0)
			{
				width++;
			}
			if (height % 2 == 0)
			{
				height++;
			}

			int major = Math.Max(height, width);
			int minor = Math.Min(height, width);
			int semiMajor = (major - 1) / 2;
			int semiMinor = (minor - 1) / 2;
			if (major != 0 && minor != 0 && semiMajor != 0 && semiMinor != 0)
			{
				Texture2D curve = new Texture2D(Main.graphics.GraphicsDevice, width, height);
				Color[] dataColors = new Color[width * height];
				for (int x = 0; x < width; x++)
				{
					int y = (int)(((float)semiMinor / semiMajor) * Math.Sqrt((semiMajor * semiMajor) - ((x - width / 2) * (x - width / 2))));
					dataColors[x + (height / 2 + y * (increasing ? 1 : -1)) * width] = new Color(122, 24, 24);
					// dataColors[x + (height / 2 + y) * width] = new Color(122, 24, 24);
				}
				curve.SetData(0, null, dataColors, 0, width * height);
				return curve;
			}
			return new Texture2D(Main.graphics.GraphicsDevice, 1, 1); ;
		}

		private int painRings = 2;

		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			if (MiseryIntensity > 0)
			{
				for (int i = 0; i < painRings; i++)
				{
					Texture2D curve = DrawCurve(npc.width + 50, npc.height + 50, i * ((float)Math.PI) / painRings, false);
					spriteBatch.Draw(curve, npc.Center - Main.screenPosition,
						   curve.Frame(), Color.White, 0f,
						   curve.Size() * .5f, 2f, SpriteEffects.None, 0f);
				}
			}
			return base.PreDraw(npc, spriteBatch, drawColor);
		}

		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			if (MiseryIntensity > 0)
			{
				for (int i = 0; i < painRings; i++)
				{
					Texture2D curve = DrawCurve(npc.width + 50, npc.height + 50, i * ((float)Math.PI) / painRings, true);
					spriteBatch.Draw(curve, npc.Center - Main.screenPosition,
						   curve.Frame(), Color.White, 0f,
						   curve.Size() * .5f, 2f, SpriteEffects.None, 0f);
				}
			}
		}
	}
}