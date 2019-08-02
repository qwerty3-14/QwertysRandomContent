using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using QwertysRandomContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace QwertysRandomContent.Items.AncientItems
{
    public class AncientLongbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hold to charge up" + "\nFires 3 arrows at max charge" + "\nWooden arrows become ancient arrows");

        }

        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.useAnimation = 30;
            item.useTime = 30;
            item.shootSpeed = 20f;
            item.knockBack = 2f;
            item.width = 50;
            item.height = 18;
            item.damage = 60;
            //item.reuseDelay = 30;
            item.shoot = mod.ProjectileType("AncientLongbowP");
            item.value = 150000;
            item.rare = 3;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.ranged = true;
            item.channel = true;
            item.useAmmo = AmmoID.Arrow;
            
            item.autoReuse = true;
        }
        
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = mod.ProjectileType("AncientLongbowP");
            position = player.Center;
            for (int l = 0; l < Main.projectile.Length; l++)
            {                                                                  //this make so you can only spawn one of this projectile at the time,
                Projectile proj = Main.projectile[l];
                if (proj.active && proj.type == item.shoot && proj.owner == player.whoAmI)
                {
                    return false;
                }
            }
            return true;
        }
        public override bool ConsumeAmmo(Player player)
        {
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/AncientItems/AncientLongbow_Glow");
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }


    }
    public class AncientLongbowP : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 18;

            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.hide = false;
            projectile.ranged = true;
            projectile.ignoreWater = true;
            
        }
        
        

        public int timer = 0;
        public int reloadTime;
        public float direction;
        
        public float Radd;
        public bool runOnce = true;
        Projectile arrow = null;
        float speed = 15f;
        int maxTime = 120;
        int weaponDamage = 10;
        int Ammo = 0;
        float weaponKnockback = 0;
        bool giveTileCollision = false;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                
                runOnce = false;

            }
            projectile.timeLeft = 2;
            
            var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            bool firing = (player.channel||timer<30) && player.HasAmmo(QwertyMethods.GetAmmoReference(39), true) && !player.noItems && !player.CCed;

            Ammo = AmmoID.Arrow;


            weaponDamage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
            direction = (Main.MouseWorld - player.Center).ToRotation();
            weaponKnockback = player.inventory[player.selectedItem].knockBack;
            if (firing)
            {
                #region drill ai
                ///////////////////////////////////// copied from vanilla drill/chainsaw AI
                Vector2 vector24 = Main.player[projectile.owner].RotatedRelativePoint(Main.player[projectile.owner].MountedCenter, true);
                if (Main.myPlayer == projectile.owner)
                {
                    if (Main.player[projectile.owner].channel || timer < 30)
                    {
                        float num264 = Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].shootSpeed * projectile.scale;
                        Vector2 vector25 = vector24;
                        float num265 = (float)Main.mouseX + Main.screenPosition.X - vector25.X;
                        float num266 = (float)Main.mouseY + Main.screenPosition.Y - vector25.Y;
                        if (Main.player[projectile.owner].gravDir == -1f)
                        {
                            num266 = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector25.Y;
                        }
                        float num267 = (float)Math.Sqrt((double)(num265 * num265 + num266 * num266));
                        num267 = (float)Math.Sqrt((double)(num265 * num265 + num266 * num266));
                        num267 = num264 / num267;
                        num265 *= num267;
                        num266 *= num267;
                        if (num265 != projectile.velocity.X || num266 != projectile.velocity.Y)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.velocity.X = num265;
                        projectile.velocity.Y = num266;
                    }
                    else
                    {
                        projectile.Kill();
                    }
                }
                if (projectile.velocity.X > 0f)
                {
                    Main.player[projectile.owner].ChangeDir(1);
                }
                else if (projectile.velocity.X < 0f)
                {
                    Main.player[projectile.owner].ChangeDir(-1);
                }
                projectile.spriteDirection = projectile.direction;
                Main.player[projectile.owner].ChangeDir(projectile.direction);
                Main.player[projectile.owner].heldProj = projectile.whoAmI;
                Main.player[projectile.owner].itemTime = 2;
                Main.player[projectile.owner].itemAnimation = 2;
                projectile.position.X = vector24.X - (float)(projectile.width / 2);
                projectile.position.Y = vector24.Y - (float)(projectile.height / 2);
                projectile.rotation = (float)(Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.5700000524520874);
                if (Main.player[projectile.owner].direction == 1)
                {
                    Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(projectile.velocity.Y * (float)projectile.direction), (double)(projectile.velocity.X * (float)projectile.direction));
                }
                else
                {
                    Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(projectile.velocity.Y * (float)projectile.direction), (double)(projectile.velocity.X * (float)projectile.direction));
                }
                projectile.velocity.X = projectile.velocity.X * (1f + (float)Main.rand.Next(-3, 4) * 0.01f);

                ///////////////////////////////
                #endregion

               
                
                if(timer ==0)
                {
                    player.PickAmmo(QwertyMethods.GetAmmoReference(39), ref Ammo, ref speed, ref firing, ref weaponDamage, ref weaponKnockback);
                    if(Ammo == ProjectileID.WoodenArrowFriendly)
                    {
                        Ammo = mod.ProjectileType("AncientArrow");
                    }
                    if(Main.netMode != 2)
                    {
                        arrow = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, Ammo, weaponDamage, weaponKnockback, projectile.owner)];
                    }
                    
                }
                arrow.velocity = QwertyMethods.PolarVector(speed, projectile.rotation - (float)Math.PI/2);
                arrow.Center = projectile.Center + QwertyMethods.PolarVector(40-2*speed, projectile.rotation - (float)Math.PI / 2);
                arrow.friendly = false;
                arrow.rotation = projectile.rotation;
                arrow.timeLeft+= arrow.extraUpdates +1;
                arrow.alpha = 1- (int)(((float)timer / maxTime) * 255f);
                speed = (8f * (float)timer / maxTime) + 7f;
               // Main.NewText("AI0: " + arrow.ai[0] + ", AI1: " + arrow.ai[1] + ", LocalAI0: " + arrow.localAI[0] + ", LocalAI1: " + arrow.localAI[1]);
                if(arrow.tileCollide)
                {
                    giveTileCollision = true;
                    arrow.tileCollide = false;
                }
                if (timer<maxTime)
                {
                    timer++;
                    for (int d = 0; d < 3; d++)
                    {
                        float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                        Dust dust = Dust.NewDustPerfect(arrow.Center + QwertyMethods.PolarVector(40, theta), mod.DustType("AncientGlow"), QwertyMethods.PolarVector(-8, theta) );
                        dust.scale = .5f;
                        dust.alpha = 255;
                    }
                    if(timer == maxTime)
                    {
                        Main.PlaySound(25, player.position, 0);
                    }
                }





            }
            else
            {
                

                projectile.Kill();
            }
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item5, projectile.position);
            arrow.velocity = QwertyMethods.PolarVector(speed, projectile.rotation - (float)Math.PI / 2);
            arrow.friendly = true;
            if (arrow != null && giveTileCollision)
            {
                arrow.tileCollide = true;
            }
            if (timer >= maxTime)
            {
                Projectile.NewProjectile(arrow.Center, arrow.velocity*.8f, arrow.type, arrow.damage, arrow.knockBack, projectile.owner);
                Projectile.NewProjectile(arrow.Center, arrow.velocity*1.2f, arrow.type, arrow.damage, arrow.knockBack, projectile.owner);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(mod.GetTexture("Items/AncientItems/AncientLongbowP"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 50, 34), drawColor, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("Items/AncientItems/AncientLongbowP_Glow"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 50, 34), Color.White, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
    public class AncientArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Arrow");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ranged = true;
            projectile.arrow = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.tileCollide = true;

           
        }
        
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
           

        }
        void drawArrowCore(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(mod.GetTexture("Items/AncientItems/AncientArrow"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 18, 36), drawColor, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("Items/AncientItems/AncientArrow_Glow"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 18, 36), Color.White, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
        }
        void drawOrbital(SpriteBatch spriteBatch, Color drawColor, Vector2 Loc)
        {
            spriteBatch.Draw(mod.GetTexture("Items/AncientItems/AncientArrow_Orbital"), Loc - Main.screenPosition,
                        new Rectangle(0, 0, 6, 10), drawColor, projectile.rotation,
                        new Vector2(3, 5), 1f, SpriteEffects.None, 0f);
        }
        float orbitalCounter = 0;
        float lengthDown = 10;
        float orbitRadius = 11;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            orbitalCounter += (float)Math.PI / 60;
            
            if(Math.Cos(orbitalCounter)>0)
            {
                Vector2 orbitalLocation = projectile.Center + QwertyMethods.PolarVector(lengthDown, projectile.rotation + (float)Math.PI / 2) + QwertyMethods.PolarVector(orbitRadius * (float)Math.Sin(orbitalCounter), projectile.rotation);
                drawOrbital(spriteBatch, drawColor, orbitalLocation);
                drawArrowCore(spriteBatch, drawColor);
                orbitalLocation = projectile.Center + QwertyMethods.PolarVector(lengthDown, projectile.rotation + (float)Math.PI / 2) - QwertyMethods.PolarVector(orbitRadius * (float)Math.Sin(orbitalCounter), projectile.rotation);
                drawOrbital(spriteBatch, drawColor, orbitalLocation);
            }
            else
            {
                Vector2 orbitalLocation = projectile.Center + QwertyMethods.PolarVector(lengthDown, projectile.rotation + (float)Math.PI / 2) - QwertyMethods.PolarVector(orbitRadius * (float)Math.Sin(orbitalCounter), projectile.rotation);
                drawOrbital(spriteBatch, drawColor, orbitalLocation);
                drawArrowCore(spriteBatch, drawColor);
                orbitalLocation = projectile.Center + QwertyMethods.PolarVector(lengthDown, projectile.rotation + (float)Math.PI / 2) + QwertyMethods.PolarVector(orbitRadius * (float)Math.Sin(orbitalCounter), projectile.rotation);
                drawOrbital(spriteBatch, drawColor, orbitalLocation);
            }


            
            return false;
        }





    }

}
