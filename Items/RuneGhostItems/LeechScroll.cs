using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QwertysRandomContent.Items.RuneGhostItems
{


    public class LeechScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leech Scroll");
            Tooltip.SetDefault("Ranged attacks may summon leech runes that can heal you" + "\nHigher damaging attacks are more likely to summon leech runes");

        }

        public override void SetDefaults()
        {

            item.value = 500000;
            item.rare = 9;
            item.ranged = true;
            item.damage = 25;

            item.width = 54;
            item.height = 56;

            item.accessory = true;



        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            
            modPlayer.leechScroll = true;
            

        }

        

    }
    class LeechRuneFreindly : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            
            projectile.tileCollide = true;
            projectile.timeLeft = 180;
            projectile.ranged = true;


        }
        public int runeTimer;
        public float startDistance = 200f;
        public float direction;
        public float runeSpeed = 10;
        public bool runOnce = true;
        public float aim;

        public override void AI()
        {
            
            
            projectile.rotation += MathHelper.ToRadians(3);



        }
        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 100; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("LeechRuneDeath"));
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.immortal && !target.SpawnedFromStatue && Main.rand.Next(0,3) ==0)
            {
                Player player = Main.player[projectile.owner];
                player.statLife += damage / 10;
                CombatText.NewText(player.getRect(), Color.Green, damage / 10, false, false);
            }

        }
        
    }



}

