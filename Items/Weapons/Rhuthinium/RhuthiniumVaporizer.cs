using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Rhuthinium
{
    public class RhuthiniumVaporizer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Vaporizer");
        }

        public override void SetDefaults()
        {
            item.damage = 35;
            item.width = 54;
            item.height = 22;
            item.mana = 8;
            item.useTime = 10;
            item.useAnimation = 10;
            item.shootSpeed = 1f;
            item.shoot = mod.ProjectileType("RhuthiniumVaporizerP");
            item.magic = true;
            item.channel = true;
            item.autoReuse = true;
            item.value = 25000;
            item.rare = 3;
            item.useStyle = 5;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.crit = 5;
            item.knockBack = .5f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UseStyle(Player player)
        {
            Vector2 vector24 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector24.X = player.bodyFrame.Width - vector24.X;
            }
            if (player.gravDir != 1f)
            {
                vector24.Y = player.bodyFrame.Height - vector24.Y;
            }
            vector24 -= new Vector2(player.bodyFrame.Width - player.width, player.bodyFrame.Height - 42) / 2f;
            player.itemLocation = player.position + vector24;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return player.ownedProjectileCounts[type] == 0;
        }

        /*
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-24, -6);
        }*/
    }

    public class RhuthiniumVaporizerP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Vaporizer");
        }

        public override void SetDefaults()
        {
            projectile.magic = true;
            projectile.width = projectile.height = 2;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.timeLeft = 2;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.hide = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = 5;
            target.immune[projectile.owner] = 0;
        }

        public int beamLength
        {
            get => (int)projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public float chargeUp
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {
            if (chargeUp < 30)
            {
                chargeUp++;
            }
            for (int i = 0; i < 100; i++)
            {
                beamLength = i;
                if (!Collision.CanHit(projectile.Center, 0, 0, projectile.Center + QwertyMethods.PolarVector(i, projectile.rotation), 0, 0))
                {
                    break;
                }
            }
            Player player = Main.player[projectile.owner];
            Vector2 vector24 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector24.X = player.bodyFrame.Width - vector24.X;
            }
            if (player.gravDir != 1f)
            {
                vector24.Y = player.bodyFrame.Height - vector24.Y;
            }
            vector24 -= new Vector2(player.bodyFrame.Width - player.width, player.bodyFrame.Height - 42) / 2f;
            projectile.rotation = player.itemRotation + (player.direction == 1 ? 0 : (float)Math.PI);
            projectile.Center = player.position + vector24 + QwertyMethods.PolarVector(22, projectile.rotation) + QwertyMethods.PolarVector(-10 * (player.direction == 1 ? 1 : -1), projectile.rotation + (float)Math.PI / 2);

            if (player.channel && player.itemAnimation > 0)
            {
                projectile.damage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
                projectile.timeLeft = 2;
            }
            if (Main.rand.Next(200) < beamLength)
            {
                Dust d = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(Main.rand.Next(beamLength), projectile.rotation), mod.DustType("RhuthiniumDust"));
                d.velocity *= 2;
                d.noGravity = true;
                d.frame.Y = 0;
            }
            if (beamLength < 99)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust d = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(beamLength, projectile.rotation), mod.DustType("RhuthiniumDust"));
                    d.velocity *= 2;
                    d.noGravity = true;
                    d.frame.Y = 0;
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (int)(damage * (chargeUp / 30f));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            /*
            Texture2D gun = mod.GetTexture("Items/Weapons/Rhuthinium/RhuthiniumVaporizer");
            spriteBatch.Draw(gun, projectile.Center - Main.screenPosition + (Main.player[projectile.owner].direction == 1 ? Vector2.Zero :  QwertyMethods.PolarVector(-10, projectile.rotation + (float)Math.PI / 2)), null, lightColor, projectile.rotation, new Vector2(54, 6), Vector2.One, (Main.player[projectile.owner].direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);

            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, 0, beamLength, 12), new Color((chargeUp / 30f) * 100, (chargeUp / 30f) * 100, (chargeUp / 30f) * 100, (chargeUp / 30f) * 100), projectile.rotation, new Vector2(0, 6), new Vector2(1f, chargeUp / 30f),  SpriteEffects.None , 0);
            */
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + QwertyMethods.PolarVector(beamLength, projectile.rotation), 12, ref point);
        }
    }

    public class DrawVaporizer : ModPlayer
    {
        public static readonly PlayerLayer Vaporizer = new PlayerLayer("QwertysRandomContent", "Vaporizer", PlayerLayer.HeldItem, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color lightColor = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            if (!drawPlayer.HeldItem.IsAir && drawPlayer.HeldItem.type == mod.ItemType("RhuthiniumVaporizer") && drawPlayer.itemAnimation > 0)
            {
                Projectile projectile = null;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == mod.ProjectileType("RhuthiniumVaporizerP") && Main.projectile[i].owner == drawPlayer.whoAmI)
                    {
                        projectile = Main.projectile[i];
                        break;
                    }
                }
                if (projectile != null)
                {
                    Texture2D gun = mod.GetTexture("Items/Weapons/Rhuthinium/RhuthiniumVaporizer");
                    DrawData d = new DrawData(gun, projectile.Center - Main.screenPosition + (Main.player[projectile.owner].direction == 1 ? Vector2.Zero : QwertyMethods.PolarVector(-10, projectile.rotation + (float)Math.PI / 2)), null, lightColor, projectile.rotation, new Vector2(54, 6), Vector2.One, (Main.player[projectile.owner].direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
                    Main.playerDrawData.Add(d);
                    Texture2D texture = Main.projectileTexture[projectile.type];
                    d = new DrawData(texture, projectile.Center - Main.screenPosition, new Rectangle(0, 0, (int)projectile.ai[1], 12), new Color((int)((projectile.ai[0] / 30f) * 100), (int)((projectile.ai[0] / 30f) * 100), (int)((projectile.ai[0] / 30f) * 100), (int)((projectile.ai[0] / 30f) * 100)), projectile.rotation, new Vector2(0, 6), new Vector2(1f, projectile.ai[0] / 30f), SpriteEffects.None, 0);
                    Main.playerDrawData.Add(d);
                }
            }
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int itemLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("HeldItem"));
            if (itemLayer != -1)
            {
                Vaporizer.visible = true;
                layers.Insert(itemLayer + 1, Vaporizer);
            }
        }
    }
}