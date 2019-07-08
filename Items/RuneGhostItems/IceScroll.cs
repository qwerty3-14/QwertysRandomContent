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


    public class IceScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Scroll");
            Tooltip.SetDefault("Summons two ice runes to orbit you");

        }

        public override void SetDefaults()
        {

            item.value = 500000;
            item.rare = 9;
            item.melee = true;
            item.damage = 300;

            item.width = 48;
            item.height = 60;

            item.accessory = true;



        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            
            modPlayer.iceScroll = true;
            

        }

        

    }
    class IceRuneFreindly : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 42;
            projectile.height = 42;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.tileCollide = false;
            projectile.timeLeft = (int)(2 * Math.PI * 10);
            projectile.melee = true;


        }
        public int runeTimer;
        public float startDistance = 200f;
        public float direction;
        public float runeSpeed = 10;
        public bool runOnce = true;
        public float aim;

        public override void AI()
        {
            
            Player player = Main.player[projectile.owner];
            var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            if (runOnce)
            {
                projectile.rotation = (player.Center - projectile.Center).ToRotation() - (float)Math.PI / 2;
                runOnce = false;
            }



            if (modPlayer.iceScroll)
            {
                projectile.rotation += (float)((2 * Math.PI) / (Math.PI * 2 * 100 / runeSpeed));
                projectile.velocity.X = runeSpeed * (float)Math.Cos(projectile.rotation) + player.velocity.X;
                projectile.velocity.Y = runeSpeed * (float)Math.Sin(projectile.rotation) + player.velocity.Y;
            }
            else
            {
                projectile.Kill();

            }
            


        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 1200);
        }
        
    }



}

