using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
	public class CaeliteImbune : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Weapon Imbue: Caelite Wrath");
			Description.SetDefault("Melee Attacks reduce the damage enemies deal");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = false;
			longerExpertDebuff = false;
			Main.meleeBuff[Type] = true;
			Main.persistentBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			/*
            if(player.meleeEnchant >0)
            {
                player.buffTime[player.FindBuffIndex(mod.BuffType("CaeliteImbune"))] = 0;
            }
            */
		}
	}

	public class InflictCaelite : GlobalNPC
	{
		public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
		{
			if (player.HasBuff(mod.BuffType("CaeliteImbune")))
			{
				npc.AddBuff(mod.BuffType("PowerDown"), 420);
			}
		}

		public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
		{
			if (Main.player[projectile.owner].HasBuff(mod.BuffType("CaeliteImbune")) && projectile.melee)
			{
				npc.AddBuff(mod.BuffType("PowerDown"), 420);
			}
		}
	}

	public class CaeliteImbunedProjectile : GlobalProjectile
	{
		public override void AI(Projectile projectile)
		{
			if (Main.player[projectile.owner].HasBuff(mod.BuffType("CaeliteImbune")) && projectile.melee)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"));
			}
		}
	}

	public class CaeliteImbunedItem : GlobalItem
	{
		public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
		{
			if (player.HasBuff(mod.BuffType("CaeliteImbune")))
			{
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("CaeliteDust"));
			}
		}
	}
}