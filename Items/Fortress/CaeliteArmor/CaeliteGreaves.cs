using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.CaeliteArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class CaeliteGreaves : ModItem
    {
        public override bool Autoload(ref string name)
        {
            return true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Greaves");
            Tooltip.SetDefault("Melee and magic attacks hasten the cooldown for healing potions" + "\n+2 recovery");
        }

        public override void SetDefaults()
        {
            item.value = 30000;
            item.rare = 3;

            item.width = 22;
            item.height = 18;
            item.defense = 5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 12);
            recipe.AddIngredient(mod.ItemType("CaeliteCore"), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<QwertyPlayer>().recovery += 3;
            player.GetModPlayer<CaeliteGreavesEffect>().hasEffect = true;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = mod.GetEquipSlot("CaeliteGreaves_Legs", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("CaeliteGreaves_FemaleLegs", EquipType.Legs);
        }

        public override bool DrawLegs()
        {
            return false;
        }
    }

    public class CaeliteGreavesEffect : ModPlayer
    {
        public bool hasEffect;
        private int healLimiter = 0;

        public override void PreUpdate()
        {
            if (healLimiter < 60)
            {
                healLimiter++;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (hasEffect && (proj.melee || proj.magic) && player.HasBuff(BuffID.PotionSickness))
            {
                int healAmount = damage / 2;
                if (healAmount > healLimiter)
                {
                    healAmount = healLimiter;
                    healLimiter = 0;
                }
                else
                {
                    healAmount -= healAmount;
                }
                if (player.GetModPlayer<CaeliteSetBonus>().setBonus)
                {
                    healAmount = (int)(healAmount * 1.25f);
                }
                player.buffTime[player.FindBuffIndex(BuffID.PotionSickness)] -= healAmount;
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (hasEffect && (item.melee) && player.HasBuff(BuffID.PotionSickness))
            {
                int healAmount = damage / 2;
                if (healAmount > healLimiter)
                {
                    healAmount = healLimiter;
                    healLimiter = 0;
                }
                else
                {
                    healAmount -= healAmount;
                }
                if (player.GetModPlayer<CaeliteSetBonus>().setBonus)
                {
                    healAmount = (int)(healAmount * 1.25f);
                }
                player.buffTime[player.FindBuffIndex(BuffID.PotionSickness)] -= healAmount;
            }
        }
    }
}