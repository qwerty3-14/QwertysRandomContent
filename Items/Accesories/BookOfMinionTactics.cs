using Microsoft.Xna.Framework;
using QwertysRandomContent.Items.B4Items;
using QwertysRandomContent.Items.Fortress.CaeliteWeapons;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{


    public class BookOfMinionTactics : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Book of Minion Tactics");
            Tooltip.SetDefault("Minions do 20% more damage against the enemy you targeted");

        }

        public override void SetDefaults()
        {

            item.value = 200000;
            item.rare = 2;


            item.width = 28;
            item.height = 32;

            item.accessory = true;



        }

        public override void UpdateEquip(Player player)
        {
            
            player.GetModPlayer<MinionBookEffect>().effect = true;
        }



    }
    public class MinionBookEffect : ModPlayer
    {
        public bool effect;
        public override void ResetEffects()
        {
            effect = false;

        }
    }
    public class MinionBookHit : GlobalProjectile
    {
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if(Main.player[projectile.owner].GetModPlayer<MinionBookEffect>().effect)
            {
                if(projectile.minion && target.whoAmI == Main.player[projectile.owner].MinionAttackTargetNPC)
                {
                    damage = (int)(damage * 1.2f);
                }
            }
        }
    }
    
}

