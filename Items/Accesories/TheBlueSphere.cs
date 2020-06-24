using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class TheBlueSphere : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Blue Sphere");
            Tooltip.SetDefault("Magic attacks pierce 2 extra enemies\n Projectiles that normally don't pierce will use local immunity\n10% reduced magic damage");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 26;
            item.value = Item.sellPrice(gold: 1);
            item.rare = 2;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MagicPierePlayer>().pierceBoost += 2;
            player.magicDamage -= .1f;
        }
    }

    public class MagicPierePlayer : ModPlayer
    {
        public int pierceBoost = 0;
        public bool negateArmor = false;

        public override void ResetEffects()
        {
            pierceBoost = 0;
            negateArmor = false;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (proj.magic && negateArmor)
            {
                player.armorPenetration += 15;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (proj.magic && negateArmor)
            {
                player.armorPenetration -= 15;
            }
        }
    }

    public class SpherePierce : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private bool gotBoost = false;

        public override void AI(Projectile projectile)
        {
            if (Main.player[projectile.owner].GetModPlayer<MagicPierePlayer>().pierceBoost > 0 && !gotBoost && projectile.friendly && projectile.magic)
            {
                gotBoost = true;
                if (projectile.penetrate > 0)
                {
                    if (!projectile.usesLocalNPCImmunity && projectile.penetrate == 1)
                    {
                        projectile.localNPCHitCooldown = -10;
                        projectile.usesLocalNPCImmunity = true;
                    }
                    projectile.penetrate += Main.player[projectile.owner].GetModPlayer<MagicPierePlayer>().pierceBoost;
                }
            }
        }
    }

    public class ShpereDrop : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.DarkCaster && Main.rand.Next(50) == 0)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("TheBlueSphere"));
            }
        }
    }
}