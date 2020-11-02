using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using QwertysRandomContent.NPCs.RuneGhost.RuneBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscBows
{
    public class RuneLongbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hold to charge up" + "\nFires 3 arrows at max charge" + "\nWooden arrows become aggro rune strikes");
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
            item.damage = 250;
            //item.reuseDelay = 30;
            item.shoot = mod.ProjectileType("RuneLongbowP");
            item.value = 500000;
            item.rare = 9;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.ranged = true;
            item.channel = true;
            item.useAmmo = AmmoID.Arrow;

            item.autoReuse = true;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = mod.ProjectileType("RuneLongbowP");
            position = player.Center;
            for (int l = 0; l < Main.projectile.Length; l++)
            {
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
            Texture2D texture = mod.GetTexture("Items/Weapons/MiscBows/RuneLongbow_Glow");
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
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("AncientLongbow"));
            recipe.AddIngredient(mod.ItemType("CraftingRune"), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class RuneLongbowP : ModProjectile
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
        private Projectile arrow = null;
        private float speed = 15f;
        private int maxTime = 120;
        private int weaponDamage = 10;
        private int Ammo = 0;
        private float weaponKnockback = 0;
        private bool giveTileCollision = false;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                runOnce = false;
            }
            projectile.timeLeft = 2;

            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            bool firing = (player.channel || timer < 30) && player.HasAmmo(player.HeldItem, true) && !player.noItems && !player.CCed;

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

                #endregion drill ai

                if (timer == 0)
                {
                    player.PickAmmo(player.HeldItem, ref Ammo, ref speed, ref firing, ref weaponDamage, ref weaponKnockback);

                    if (Ammo == ProjectileID.WoodenArrowFriendly)
                    {
                        Ammo = mod.ProjectileType("RuneArrow");
                    }
                    if (Main.netMode != 2)
                    {
                        arrow = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, Ammo, weaponDamage, weaponKnockback, projectile.owner)];
                    }
                }
                arrow.velocity = QwertyMethods.PolarVector(speed, projectile.rotation - (float)Math.PI / 2);
                arrow.Center = projectile.Center + QwertyMethods.PolarVector(40 - 2 * speed, projectile.rotation - (float)Math.PI / 2);
                arrow.friendly = false;
                arrow.rotation = projectile.rotation;
                arrow.timeLeft += arrow.extraUpdates + 1;
                arrow.alpha = 1 - (int)(((float)timer / maxTime) * 255f);
                speed = (8f * (float)timer / maxTime) + 7f;
                //Main.NewText(arrow.damage);
                // Main.NewText("AI0: " + arrow.ai[0] + ", AI1: " + arrow.ai[1] + ", LocalAI0: " + arrow.localAI[0] + ", LocalAI1: " + arrow.localAI[1]);
                if (arrow.tileCollide)
                {
                    giveTileCollision = true;
                    arrow.tileCollide = false;
                }
                if (timer < maxTime)
                {
                    timer++;
                    
                    if (timer == maxTime)
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
                Projectile.NewProjectile(arrow.Center, QwertyMethods.PolarVector(arrow.velocity.Length(), arrow.velocity.ToRotation() + (float)Math.PI / 64f) , arrow.type, arrow.damage, arrow.knockBack, projectile.owner);
                Projectile.NewProjectile(arrow.Center, QwertyMethods.PolarVector(arrow.velocity.Length(), arrow.velocity.ToRotation() - (float)Math.PI / 64f), arrow.type, arrow.damage, arrow.knockBack, projectile.owner);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(mod.GetTexture("Items/Weapons/MiscBows/RuneLongbowP"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 50, 34), drawColor, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("Items/Weapons/MiscBows/RuneLongbowP_Glow"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 50, 34), Color.White, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
    public class RuneArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 18;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 30;
            projectile.ranged = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }
        bool runOnce = true;
        int timer;
        public override void AI()
        {
            timer = 30 - projectile.timeLeft;
            if (timer > 3 && runOnce)
            {
                runOnce = false;
                projectile.rotation = projectile.velocity.ToRotation();
                projectile.velocity = Vector2.Zero;
            }
            Player player = Main.player[projectile.owner];
            projectile.Center = player.Center;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (timer < 5)
            {
                return false;
            }
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + QwertyMethods.PolarVector(1000, projectile.rotation));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (timer > 3)
            {
                int frame = timer / 2;
                if (timer > 22)
                {
                    frame = (30 - timer) / 2;
                }
                if (frame > 3)
                {
                    frame = 3;
                }
                float c = (float)frame / 3f;
                for (int i = 0; i < 3000; i += 8)
                {
                    spriteBatch.Draw(RuneSprites.aggroStrike[frame], projectile.Center + QwertyMethods.PolarVector(i, projectile.rotation) - Main.screenPosition, null, new Color(c, c, c, c), projectile.rotation, new Vector2(0, 3), Vector2.One * 2, 0, 0);
                }
            }
            else
            {
                Texture2D texture = Main.projectileTexture[projectile.type];
                spriteBatch.Draw(texture, projectile.Center + QwertyMethods.PolarVector( 8, projectile.rotation - (float)Math.PI/2f) - Main.screenPosition, null, Color.White, projectile.rotation, projectile.Size * .5f, 1f, 0, 0);
            }
            return false;
        }
    }
    
}
