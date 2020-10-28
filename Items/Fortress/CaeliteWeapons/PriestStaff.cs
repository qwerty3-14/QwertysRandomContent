using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.CaeliteWeapons
{
    public class PriestStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Priest Staff");
            Tooltip.SetDefault("Do I even need to explain how higher beings are involved with this?");
        }
        public override void SetDefaults()
        {
            item.damage = 18;
            item.mana = 20;
            item.width = 32;
            item.height = 32;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 2f;
            item.value = 25000;
            item.rare = 3;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("PriestMinion");
            item.summon = true;
            item.buffType = mod.BuffType("PriestMinion");
            item.buffTime = 3600;
        }
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
    }
    public class PriestMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Priest Minion");
            Main.projFrames[projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.friendly = true;
            projectile.width = 26;
            projectile.height = 32;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        NPC target;
        NPC savedTarget;
        bool attacking = false;
        bool justAttacked = false;
        int attackCycleTime = 60;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (player.GetModPlayer<MinionManager>().PriestMinion)
            {
                projectile.timeLeft = 2;
            }
            int identity = 0;
            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].type == projectile.type)
                {
                    if (p == projectile.whoAmI)
                    {
                        break;
                    }
                    else
                    {
                        identity++;
                    }
                }
            }
            int priestCount = player.ownedProjectileCounts[projectile.type];
            if (priestCount != 0)
            {
                int timer = player.GetModPlayer<MinionManager>().PriestSynchroniser;
                if (timer % attackCycleTime == 0)
                {
                    justAttacked = false;
                    if (QwertyMethods.ClosestNPC(ref target, 2000, player.Center, false, player.MinionAttackTargetNPC))
                    {
                        Main.PlaySound(SoundID.Item8, projectile.position);
                        for (int num67 = 0; num67 < 15; num67++)
                        {
                            int num75 = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"), 0f, 0f, 100, default(Color), 2.5f);
                            Main.dust[num75].velocity *= 3f;
                            Main.dust[num75].noGravity = true;
                        } 

                        projectile.Center = target.Center + QwertyMethods.PolarVector(80f + 10f * priestCount, player.GetModPlayer<MinionManager>().PriestAngle) + QwertyMethods.PolarVector((40f * priestCount) * ((float)(identity+1)/(priestCount+1)) - (20f * priestCount), player.GetModPlayer<MinionManager>().PriestAngle + (float)Math.PI/2f);
                        projectile.velocity = Vector2.Zero;
                        attacking = true;
                        savedTarget = target;

                        Main.PlaySound(SoundID.Item8, projectile.position);
                        for (int num76 = 0; num76 < 15; num76++)
                        {
                            int num84 = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"), 0f, 0f, 100, default(Color), 2.5f);
                            Main.dust[num84].velocity *= 3f;
                            Main.dust[num84].noGravity = true;
                        }
                    }
                    else
                    {
                        attacking = false;
                    }
                }

                if(attacking)
                {
                    if (!justAttacked)
                    {
                        if (savedTarget != null && !savedTarget.active)
                        {
                            savedTarget = null;
                        }
                        Vector2? aimAt = null;
                        if (savedTarget != null)
                        {
                            aimAt = savedTarget.Center + savedTarget.velocity * ((savedTarget.Center-projectile.Center).Length()/20f);
                        }
                        else if (QwertyMethods.ClosestNPC(ref target, 2000, projectile.Center, false, player.MinionAttackTargetNPC))
                        {
                            aimAt = target.Center + target.velocity * ((target.Center - projectile.Center).Length() / 20f); 
                        }
                        if(aimAt != null)
                        {
                            projectile.spriteDirection = Math.Sign(((Vector2)aimAt).X - projectile.Center.X);
                        }
                        if ((timer % attackCycleTime) == (int)((float)(identity + 1) / (priestCount + 1) * attackCycleTime))
                        {
                            justAttacked = true;
                            if (aimAt != null)
                            {
                                Vector2 shotPos = projectile.Center + new Vector2(20 * projectile.spriteDirection, 3);
                                Projectile.NewProjectile(shotPos, QwertyMethods.PolarVector(10f, (((Vector2)aimAt) - shotPos).ToRotation()), mod.ProjectileType("PriestPulse"), projectile.damage, projectile.knockBack, projectile.owner);
                            }
                        }
                    }
                }
                else
                {
                    Vector2 goHere = player.Center + QwertyMethods.PolarVector(25f + 5f*priestCount, -(float)Math.PI / 2 + ((float)(identity + 1) / (priestCount + 1)) * (float)Math.PI / 2f * player.direction * -1);
                    Vector2 dif = goHere - projectile.Center;
                    projectile.spriteDirection = Math.Sign(player.Center.X - projectile.Center.X);
                    if (dif.Length() >300f)
                    {
                        Main.PlaySound(SoundID.Item8, projectile.position);
                        for (int num67 = 0; num67 < 15; num67++)
                        {
                            int num75 = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"), 0f, 0f, 100, default(Color), 2.5f);
                            Main.dust[num75].velocity *= 3f;
                            Main.dust[num75].noGravity = true;
                        }

                        projectile.velocity = Vector2.Zero;
                        projectile.Center = goHere;

                        Main.PlaySound(SoundID.Item8, projectile.position);
                        for (int num67 = 0; num67 < 15; num67++)
                        {
                            int num75 = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"), 0f, 0f, 100, default(Color), 2.5f);
                            Main.dust[num75].velocity *= 3f;
                            Main.dust[num75].noGravity = true;
                        }
                    }
                    else if(dif.Length() > 18f)
                    {
                        projectile.velocity = dif.SafeNormalize(Vector2.UnitY) * 18f;
                    }
                    else
                    {
                        projectile.velocity = Vector2.Zero;
                        projectile.Center = goHere;
                    }
                }
            }

            //animation
            projectile.frameCounter++;
            projectile.frame = projectile.frameCounter % 20 > 10 ? 1 : 0;
            if(attacking)
            {
                projectile.frame += 2;
            }
            if(justAttacked)
            {
                projectile.frame += 2;
            }
            
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if(projectile.frame == 2 || projectile.frame == 3)
            {
                Texture2D texture = mod.GetTexture("Items/Fortress/CaeliteWeapons/PriestPulse");
                spriteBatch.Draw(texture, projectile.Center + new Vector2(projectile.spriteDirection * 5, -12) - Main.screenPosition, new Rectangle(0, (projectile.frame - 2) * 14, 14, 14), Color.White, 0f, Vector2.One * 7, Vector2.One, 0, 0);
            }
            
        }
    }
    public class PriestPulse : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Preist Pulse");
            Main.projFrames[projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            projectile.minion = true;
            projectile.friendly = true;
            projectile.width = 14;
            projectile.height = 14;
            projectile.penetrate = 1;
            projectile.tileCollide = false;
            projectile.timeLeft = 360;
            projectile.extraUpdates = 1;
            projectile.light = 1f;
        }
        public override void AI()
        {
            projectile.frameCounter++;
            projectile.frame = projectile.frameCounter % 40 > 20 ? 1 : 0;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(10) == 0)
            {
                target.AddBuff(mod.BuffType("PowerDown"), 120);
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"))];
                dust.velocity *= 3f;
            }
        }
    }
}
