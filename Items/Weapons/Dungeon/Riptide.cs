using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Dungeon
{
    public class Riptide : ModItem
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicDungeon ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Riptide Sentry Staff");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 4;
            item.mana = 20;
            item.width = 32;
            item.height = 32;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 0f;
            item.value = Item.sellPrice(silver: 54);
            item.rare = 2;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("RiptideP");
            item.summon = true;
            item.sentry = true;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            knockBack = 0f;
            position = Main.MouseWorld;   //this make so the projectile will spawn at the mouse cursor position

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
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.mod == "Terraria" && line.Name == "Knockback") //this checks if it's the line we're interested in
                {
                    line.text = "Absolutely no knockback";//change tooltip
                }
            }
        }
    }

    public class RiptideP : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicDungeon ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Riptide");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.sentry = true;
            projectile.width = 34;
            projectile.height = 34;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = Projectile.SentryLifeTime;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.sentry = true;
            projectile.usesLocalNPCImmunity = true;
            //projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }

        private NPC target;

        private float maxDistance = 1000f;
        private float distance;
        private int timer;
        private int reloadTime = 6;
        private int si = 1;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.UpdateMaxTurrets();

            if (QwertyMethods.ClosestNPC(ref target, maxDistance, projectile.Center, false, player.MinionAttackTargetNPC))
            {
                timer++;
                projectile.rotation = (target.Center - projectile.Center).ToRotation();
                if (timer % reloadTime == 0)
                {
                    if (timer % (reloadTime * 2) == 0)
                    {
                        projectile.frame = 1;
                        si = 1;
                    }
                    else
                    {
                        projectile.frame = 2;
                        si = -1;
                    }

                    Vector2 shootFrom = projectile.Center + QwertyMethods.PolarVector(12, projectile.rotation) + QwertyMethods.PolarVector(si * 4, projectile.rotation + (float)Math.PI / 2);
                    //if (Main.netMode != 1)
                    {
                        Projectile.NewProjectile(shootFrom, QwertyMethods.PolarVector(1, projectile.rotation), mod.ProjectileType("RiptideStream"), projectile.damage, projectile.knockBack, projectile.owner);
                    }
                }
            }
            else
            {
                timer = 0;
                projectile.frame = 0;
            }
        }
    }

    public class RiptideStream : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.extraUpdates = 99;
            projectile.timeLeft = 1200;
            projectile.friendly = true;
            projectile.minion = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            if (Main.rand.Next(8) == 0)
            {
                Dust d = Main.dust[Dust.NewDust(projectile.Center, 0, 0, 172)];
                d.velocity *= .1f;
                d.noGravity = true;
                d.position = projectile.Center;
            }
        }
    }
}