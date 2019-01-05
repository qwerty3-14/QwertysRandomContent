using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.RuneSpectorBoss
{
    class GreenRune : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            
            projectile.width = 200;
            projectile.height = 200;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.tileCollide = false;
            projectile.timeLeft = 128 + 720;
            projectile.light = 1f;

        }
        public bool runOnce = true;
        public int runeTimer = -128;
        public Projectile leechRune;
        public float theta;
        
        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (projectile.alpha > 0)
                projectile.alpha -= 2;
            else
                projectile.alpha = 0;

            runeTimer++;
                
                if (runeTimer >=120)
                {
                    
                      //if (Main.netMode != 1)
                      //{


                        leechRune = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("LeechRune"), projectile.damage, 3f, Main.myPlayer)];
                      //}
                    
                    runeTimer = 0;
                }
            
                
            


        }
        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 100; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("LeechRuneDeath"));
            }
        }
    }
    class LeechRune : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 40;
            projectile.height = 40;
            projectile.friendly = false;
            
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.tileCollide = false;
            projectile.timeLeft = 120;
            projectile.hostile = false;
            projectile.light = 1f;

        }
        public bool runOnce = true;
        public bool runOnceAfter = true;
        public int runeTimer;
        public Projectile leechRune;
        public float attackAngle;
        public float attackSpeed = 10f;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if(runOnce)
            {
                float theta = MathHelper.ToRadians(Main.rand.Next(0, 360));
                float startDistance = 100f;
                projectile.position = new Vector2(player.Center.X + (float)Math.Cos(theta) * startDistance, player.Center.Y + (float)Math.Sin(theta) * startDistance);
                runOnce = false;
            }
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 255 / 60;

                projectile.hostile = false;
            }
            else
            {

                projectile.alpha = 0;
            }
            runeTimer += 255 / 60;
            if (runeTimer >=255)
            { 
                if (runOnceAfter )
                {

                    attackAngle = (player.Center - projectile.Center).ToRotation();


                    runOnceAfter = false;



                }
                projectile.rotation += MathHelper.ToRadians(3);
                projectile.hostile = true;
                projectile.velocity = new Vector2((float)Math.Cos(attackAngle) * attackSpeed, (float)Math.Sin(attackAngle) * attackSpeed);


            }
            else
            {
                projectile.velocity = new Vector2(0, 0);
            }


        }
        public NPC runeSpector;

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            foreach (NPC npcSearch in Main.npc)
            {
                if (npcSearch.type == mod.NPCType("RuneSpector"))
                    runeSpector = npcSearch;
            }
            if (runeSpector.active)
            {
                if (Main.expertMode)
                {
                    runeSpector.life += damage * 100;
                    CombatText.NewText(runeSpector.getRect(), Color.Green, damage * 100, false, true);
                }
                else
                {
                    runeSpector.life += damage * 40;
                    CombatText.NewText(runeSpector.getRect(), Color.Green, damage * 40, false, true);
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 100; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("LeechRuneDeath"));
            }
        }
    }
}
