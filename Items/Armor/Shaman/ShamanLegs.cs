using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Shaman
{
	[AutoloadEquip(EquipType.Legs)]
	public class ShamanLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shaman Pants");
			Tooltip.SetDefault("+1 max sentries \nMelee attacks can inflict decay \nDecay reduces enemy offense and defense");
		}

		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			if (male) equipSlot = mod.GetEquipSlot("ShamanLegs", EquipType.Legs);
			if (!male) equipSlot = mod.GetEquipSlot("ShamanLegs_Female", EquipType.Legs);
		}

		public override void SetDefaults()
		{
			item.value = Item.sellPrice(gold: 1);
			item.rare = 1;
			item.defense = 5;
			item.width = 20;
			item.height = 20;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxTurrets++;
			player.GetModPlayer<ShamanLegsEffects>().effect = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.JungleSpores, 4);
			recipe.AddIngredient(ItemID.Bone, 25);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class ShamanLegsEffects : ModPlayer
	{
		public bool effect = false;

		public override void ResetEffects()
		{
			effect = false;
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (item.melee && Main.rand.Next(3) == 0 && effect)
			{
				target.AddBuff(mod.BuffType("Decay"), 180);
			}
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if (proj.melee && Main.rand.Next(3) == 0 && effect)
			{
				target.AddBuff(mod.BuffType("Decay"), 180);
			}
		}
	}
}