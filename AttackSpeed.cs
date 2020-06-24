using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent
{
    public class AttackSpeedPlayer : ModPlayer
    {
        public float allSpeed = 1f;
        public float rangedSpeedBonus = 0f;
        public float meleeUseSpeedBonus = 0f; //melee speed from vanilla does not effect use time
        public float magicSpeedBonus = 0f;
        public bool swordBadge = false;

        public override void ResetEffects()
        {
            allSpeed = 1f;
            rangedSpeedBonus = 0f;
            meleeUseSpeedBonus = 0f;
            magicSpeedBonus = 0f;
            swordBadge = false;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (swordBadge && !target.immortal)
            {
                player.AddBuff(mod.BuffType("ImperialCourage"), 7 * 60);
            }
        }

        public float GetSpeed(Item item, Player player)
        {
            float speed = allSpeed;
            if (item.magic)
            {
                speed += magicSpeedBonus;
            }
            if (item.ranged)
            {
                speed += rangedSpeedBonus;
            }
            if (item.melee)
            {
                speed += meleeUseSpeedBonus;
            }
            return speed;
        }

        public override float MeleeSpeedMultiplier(Item item)
        {
            return GetSpeed(item, player);
        }

        public override float UseTimeMultiplier(Item item)
        {
            return GetSpeed(item, player);
        }
    }
}