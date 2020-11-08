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
using Terraria.World.Generation;

namespace QwertysRandomContent.Items.Weapons.DukeFishron
{
    public class BubbleBrewerBaton : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble Brewer Baton");
            Tooltip.SetDefault("Summons a bubble brewer\nWorks well as a last line of defense");
        }
        public override void SetDefaults()
        {
            item.damage = 60;
            item.summon = true;
            item.knockBack = 1f;
            item.value = Item.sellPrice(gold: 5);
            item.rare = 8;
            item.width = 22;
            item.height = 32;
            item.useStyle = 4;
            item.shootSpeed = 0;
            item.useTime = 25;
            item.useAnimation = 25;
            item.shoot = mod.ProjectileType("BubbleBrewer");
            item.noUseGraphic = false;
            item.noMelee = true;
            item.UseSound = SoundID.Item1;
            item.sentry = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            Point point;
            while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                {
                                            new Conditions.IsSolid()
                }), out point))
            {
                position.Y++;
            }
            position.Y -= 42;
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
    public class BubbleBrewer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
        }
        public override void SetDefaults()
        {
            projectile.sentry = true;
            projectile.timeLeft = Projectile.SentryLifeTime;
            projectile.width = 56;
            projectile.height = 84;
            projectile.friendly = true;
            projectile.penetrate = -1;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }
        int waterLevel = 0;
        int timer = 0;
        NPC target;
        Vector2 bubbleShooterLocation = new Vector2(27, 73);
        
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.UpdateMaxTurrets();
            timer++;
            if(timer == 1)
            {
                Main.PlaySound(SoundID.Item46, projectile.Center);
            }
            if(timer % 60 == 0 && waterLevel < 26)
            {
                waterLevel++;
                Main.PlaySound(SoundID.Item85, projectile.Center);
            }
            if(waterLevel > 0 && timer % 3 == 0)
            {
                if(QwertyMethods.ClosestNPC(ref target, 500, projectile.Center, false, player.MinionAttackTargetNPC))
                {
                    if(waterLevel > 10)
                    {
                        Main.PlaySound(29, (int)projectile.Center.X, (int)projectile.Center.Y, 20);
                    }
                    projectile.frameCounter = 30;
                    Projectile.NewProjectile(projectile.position + bubbleShooterLocation, (target.Center - (projectile.position + bubbleShooterLocation)).SafeNormalize(Vector2.UnitY) * 12f, mod.ProjectileType("BrewerBubble"), projectile.damage, projectile.knockBack, projectile.owner, -10f);
                    waterLevel--;
                }
            }
            if(projectile.frameCounter > 0)
            {
                projectile.frameCounter--;
                projectile.frame = 1;
            }
            else
            {
                projectile.frame = 0;
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Items/Weapons/DukeFishron/BubbleBrewerGauge");
            for (int i = 0; i < waterLevel; i++)
            {
                spriteBatch.Draw(texture, projectile.position + new Vector2(24, 36) - Vector2.UnitY * i - Main.screenPosition, null, lightColor, 0, new Vector2(0, 1), 1, 0, 0);
            }
                
        }
    }
    public class BrewerBubble : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 70;
            aiType = ProjectileID.FlaironBubble;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.timeLeft = 240;
        }
    }
}
