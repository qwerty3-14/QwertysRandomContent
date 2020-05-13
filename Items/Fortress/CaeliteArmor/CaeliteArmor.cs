using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace QwertysRandomContent.Items.Fortress.CaeliteArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class CaeliteArmor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caelite Armor");
			Tooltip.SetDefault("Magic attacks against airborn enemies do 20% more damage" + "\nThrown attacks against grounded enemies do 20% more damage" + "\n+3 recovery");
		}

		public override void SetDefaults()
		{
			item.value = 30000;
			item.rare = 3;

			item.width = 22;
			item.height = 12;
			item.defense = 7;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CaeliteBar"), 16);
			recipe.AddIngredient(mod.ItemType("CaeliteCore"), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<QwertyPlayer>().recovery += 3;
			for (int i = 0; i < 1000; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && !Main.projectile[i].GetGlobalProjectile<CaeliteArmorEffect>().g)
				{
					Main.projectile[i].GetGlobalProjectile<CaeliteArmorEffect>().g = true;
				}
			}
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == mod.ItemType("CaeliteHelm") && legs.type == mod.ItemType("CaeliteGreaves");
		}

		public override void ArmorSetShadows(Player player)
		{
			//Main.NewText("active set effect");

			player.armorEffectDrawOutlines = true;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.CaeliteSet");
			player.GetModPlayer<QwertyPlayer>().recovery += 2;
			player.GetModPlayer<CaeliteSetBonus>().setBonus = true;
		}

		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawArms = false;
			drawHands = false;
		}
	}

	public class CaeliteSetBonus : ModPlayer
	{
		public bool setBonus;

		public override void ResetEffects()
		{
			setBonus = false;
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if (damage > target.life && (proj.magic || proj.thrown))
			{
				target.value = (int)(target.value * 1.25f);
			}
			if (setBonus && (proj.thrown || proj.magic) && player.HasBuff(BuffID.PotionSickness))
			{
				player.buffTime[player.FindBuffIndex(BuffID.PotionSickness)] -= damage / 8;
			}
		}
	}

	public class CaeliteArmorEffect : GlobalProjectile
	{
		public bool g;

		public override bool InstancePerEntity => true;

		public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (g)
			{
				Point origin = target.Bottom.ToTileCoordinates();
				Point point;
				if (projectile.magic && !WorldUtils.Find(origin, Searches.Chain(new Searches.Down(4), new GenCondition[]
											{
											new Conditions.IsSolid()
											}), out point))
				{
					if (Main.player[projectile.owner].GetModPlayer<CaeliteSetBonus>().setBonus)
					{
						damage = (int)(damage * 1.25f);
					}
					else
					{
						damage = (int)(damage * 1.2f);
					}
				}
				if ((projectile.thrown || projectile.type == ProjectileID.BoneJavelin) && WorldUtils.Find(origin, Searches.Chain(new Searches.Down(4), new GenCondition[]
											{
											new Conditions.IsSolid()
											}), out point))
				{
					if (Main.player[projectile.owner].GetModPlayer<CaeliteSetBonus>().setBonus)
					{
						damage = (int)(damage * 1.25f);
					}
					else
					{
						damage = (int)(damage * 1.2f);
					}
				}
			}
		}
	}
}