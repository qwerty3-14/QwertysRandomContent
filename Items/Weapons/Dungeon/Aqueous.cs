using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Dungeon
{
    public class Aqueous : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aqueous");
            Tooltip.SetDefault("Shot from your bow alongside normal arrows");

        }
        public override void SetDefaults()
        {
            item.damage = 20;
            item.ranged = true;
            item.knockBack = .5f;
            item.value = Item.sellPrice(silver: 54);
            item.rare = 2;
            item.width = 2;
            item.height = 2;
            item.crit = 20;
            item.shootSpeed = 12f;
            item.useTime = 100;

            item.maxStack = 1;


        }
        public override bool CanUseItem(Player player)
        {
            return false;
        }
        Player player = Main.player[0];
        float closestDistance = 10000;
        public virtual void ReturningDust()
        {
            if (Main.rand.Next(6) == 0)
            {
                Dust.NewDust(item.position, item.width, item.height, 172);
            }
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {

            for (int p = 0; p < 255; p++)
            {
                if (Main.player[p].active && (Main.player[p].Center - item.Center).Length() < closestDistance)
                {

                    closestDistance = (Main.player[p].Center - item.Center).Length();

                    player = Main.player[p];

                }
            }

            Vector2 vectorItemToPlayer = player.Center - item.Center;

            Vector2 v = vectorItemToPlayer.SafeNormalize(default(Vector2)) * 8f * (100 / (float)item.useTime);

            item.velocity = v;
            item.position += v * .1f;

            item.beingGrabbed = true;

            ReturningDust();


            closestDistance = 10000;
        }
        float r = 0f;
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            r += (float)Math.PI / 60f * (100 / (float)item.useTime);
            Texture2D texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    item.Center.X - Main.screenPosition.X,
                    item.Center.Y - Main.screenPosition.Y + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                lightColor,
                r,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
            return false;
        }
    }
    public class AqueousShot : ModPlayer
    {
        protected int arrowID = -1;
        protected int shootID = -1;

        public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (arrowID == -1)
            {
                arrowID = mod.ItemType("Aqueous");
                shootID = mod.ProjectileType("AqueousP");
            }
            if (item.useAmmo == 40)
            {
                for (int i = 0; i < 58; i++)
                {
                    if (player.inventory[i].type == arrowID && player.inventory[i].stack > 0 && Main.LocalPlayer == player)
                    {


                        Projectile p = Main.projectile[Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedByRandom(Math.PI / 32).SafeNormalize(default(Vector2)) * player.inventory[i].shootSpeed, shootID, player.inventory[i].damage, player.inventory[i].knockBack, player.whoAmI)];
                        p.localAI[1] = player.inventory[i].prefix;
                        p.localAI[0] = player.inventory[i].crit;
                        player.inventory[i].stack--;
                    }
                }
            }
            return base.Shoot(item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
    }
    public class AqueousP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aqueous");


        }

        protected int assosiatedItemID = -1;
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.ranged = true;
            projectile.arrow = true;

            projectile.tileCollide = true;
        }
        public override void Kill(int timeLeft)
        {
            if (assosiatedItemID == -1)
            {
                assosiatedItemID = mod.ItemType("Aqueous");
            }
            if (Main.netMode != 1)
            {
                Item i = Main.item[Item.NewItem(projectile.Center, assosiatedItemID)];
                i.Prefix((int)projectile.localAI[1]);
            }

        }
        public override void AI()
        {
            if (Main.rand.Next(6) == 0)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
            }

        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = Main.rand.Next(100) < ((int)projectile.localAI[0] + Main.player[projectile.owner].rangedCrit);
        }



    }

}

