using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.B4Items
{
    public class BlackHoleStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Black Hole Staff");
            Tooltip.SetDefault("Summons a black hole to suck up your enemies!" + "\nThe higher the black hole's damage the stronger the pull strength");

            Item.staff[item.type] = true;
            //Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(30, 29));
        }

        public override void SetDefaults()
        {
            item.damage = 40;
            item.mana = 14;
            item.width = 100;
            item.height = 114;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 0f;
            item.value = 750000;
            item.rare = 10;
            item.UseSound = SoundID.Item44;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("BlackHolePlayer");
            item.magic = true;
            item.channel = true;
        }

        public Projectile BlackHole;

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 SPos = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
            position = SPos;
            speedX = 0;
            speedY = 0;
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
    }

    public class BlackHolePlayer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BlackHole");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 200;
            projectile.height = 200;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            projectile.tileCollide = false;
        }

        public float horiSpeed;
        public float vertSpeed;
        public float direction;
        public float pullSpeed = .5f;
        public float dustSpeed = 20f;
        public NPC mass;
        public Projectile proj;
        public int frameTimer;
        public Dust dust;
        public Item item;
        public int manaTimer;

        public override void AI()
        {
            pullSpeed = projectile.damage / 100f;
            projectile.velocity = new Vector2(0, 0);
            projectile.scale = projectile.damage / 40f;

            Player player = Main.player[projectile.owner];
            player.itemAnimation = 2;
            if (!player.channel)
            {
                projectile.Kill();
            }
            else
            {
                manaTimer++;
                if (manaTimer % 15 == 0)
                {
                    if (player.statMana > player.inventory[player.selectedItem].mana)
                    {
                        player.statMana -= player.inventory[player.selectedItem].mana;
                        projectile.timeLeft = 60;
                    }
                    else
                    {
                        projectile.Kill();
                    }
                }
                else
                {
                }
            }

            direction = (projectile.Center - player.Center).ToRotation();
            horiSpeed = (float)Math.Cos(direction) * pullSpeed / 2;
            vertSpeed = (float)Math.Sin(direction) * pullSpeed / 2;
            player.velocity += new Vector2(horiSpeed, vertSpeed);
            for (int d = 0; d < (int)(80 * projectile.scale); d++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(Main.rand.NextFloat(10 * projectile.scale, 200 * projectile.scale), theta), mod.DustType("BlackHoleMatter"), QwertyMethods.PolarVector(6 * projectile.scale, theta + (float)Math.PI / 2));
                dust.scale = 1f;
            }

            for (int i = 0; i < Main.dust.Length; i++)
            {
                dust = Main.dust[i];
                if (!dust.noGravity)
                {
                    direction = (projectile.Center - dust.position).ToRotation();
                    horiSpeed = (float)Math.Cos(direction) * pullSpeed * 5;
                    vertSpeed = (float)Math.Sin(direction) * pullSpeed * 5;
                    dust.velocity += new Vector2(horiSpeed, vertSpeed);
                }
                if (dust.type == mod.DustType("BlackHoleMatter"))
                {
                    direction = (projectile.Center - dust.position).ToRotation();
                    dust.velocity += QwertyMethods.PolarVector(.8f, direction);
                    if ((dust.position - projectile.Center).Length() < 10 * projectile.scale)
                    {
                        dust.scale = 0f;
                    }
                    else
                    {
                        dust.scale = .35f;
                    }
                }
            }
            for (int i = 0; i < 200; i++)
            {
                mass = Main.npc[i];
                if (!mass.boss && mass.active && mass.knockBackResist != 0f)
                {
                    direction = (projectile.Center - mass.Center).ToRotation();
                    horiSpeed = (float)Math.Cos(direction) * pullSpeed;
                    vertSpeed = (float)Math.Sin(direction) * pullSpeed;
                    mass.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(mass.position, mass.width, mass.height, mod.DustType("B4PDust"), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }
            for (int i = 0; i < 200; i++)
            {
                item = Main.item[i];
                if (item.position != new Vector2(0, 0))
                {
                    //This part of the code puts the items the black hole grabs in the player inventory. It's partialy based on the lugage from The Luggage mod
                    if (item.active && item.noGrabDelay == 0 && item.owner == projectile.owner && ItemLoader.CanPickup(item, player))
                    {
                        int num = Player.defaultItemGrabRange;
                        if (new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height).Intersects(new Rectangle((int)item.position.X, (int)item.position.Y, item.width, item.height)))
                        {
                            if (projectile.owner == Main.myPlayer && (player.inventory[player.selectedItem].type != 0 || player.itemAnimation <= 0))
                            {
                                if (ItemID.Sets.NebulaPickup[item.type])
                                {
                                    item.velocity = new Vector2(0, 0);
                                    item.position = player.Center;
                                }
                                if (item.type == 58 || item.type == 1734 || item.type == 1867)
                                {
                                    item.velocity = new Vector2(0, 0);
                                    item.position = player.Center;
                                }
                                else if (item.type == 184 || item.type == 1735 || item.type == 1868)
                                {
                                    item.velocity = new Vector2(0, 0);
                                    item.position = player.Center;
                                }
                                else
                                {
                                    for (int g = 0; g < 58; g++)
                                    {
                                        if (!player.inventory[g].active)
                                        {
                                            item.velocity = new Vector2(0, 0);
                                            item.position = player.Center;
                                        }
                                    }

                                    //item = player.GetItem(projectile.owner, item, false, false);
                                }
                            }
                        }
                    }
                    /////////////////////
                    direction = (projectile.Center - item.Center).ToRotation();
                    horiSpeed = (float)Math.Cos(direction) * pullSpeed;
                    vertSpeed = (float)Math.Sin(direction) * pullSpeed;
                    item.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(item.position, item.width, item.height, mod.DustType("B4PDust"), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }
            for (int i = 0; i < 1000; i++)
            {
                proj = Main.projectile[i];
                if (proj.active && proj.type != mod.ProjectileType("BlackHolePlayer") && proj.type != mod.ProjectileType("SideLaser"))
                {
                    direction = (projectile.Center - proj.Center).ToRotation();
                    horiSpeed = (float)Math.Cos(direction) * pullSpeed;
                    vertSpeed = (float)Math.Sin(direction) * pullSpeed;
                    proj.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(proj.position, proj.width, proj.height, mod.DustType("B4PDust"), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }
        }
    }
}