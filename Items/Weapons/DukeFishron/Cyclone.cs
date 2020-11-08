using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.DukeFishron
{
    public class Cyclone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cyclone");
            Tooltip.SetDefault("<3");
        }
        public override void SetDefaults()
        {
            item.damage = 240;
            item.melee = true;
            item.knockBack = 5;
            item.value = Item.sellPrice(gold: 5);
            item.rare = 8;
            item.width = 34;
            item.height = 38;
            item.useStyle = 1;
            item.shootSpeed = 3f;
            item.useTime = 45;
            item.useAnimation = 45;
            item.shoot = mod.ProjectileType("CycloneP");
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
        }
    }
    public class CycloneP : Top
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cyclone");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;

            projectile.width = 34;
            projectile.height = 38;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;

            projectile.tileCollide = true;
            friction = .001f;
            enemyFriction = 0f;
            frameDelay = 4;
        }
        int maxSegments = 48;
        int segments = 0;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (hitGround && projectile.friendly)
            {
                projHitbox.Height += (segments * 8);
                projHitbox.Y -= (segments * 8);
                return Collision.CheckAABBvAABBCollision(projHitbox.TopLeft(), projHitbox.Size(), targetHitbox.TopLeft(), targetHitbox.Size());
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
        float trigCounter = 0;
        public override void ExtraTopNonesense()
        {
            if(hitGround && projectile.friendly)
            {
                if(projectile.frameCounter % 2 == 0 && segments < maxSegments)
                {
                    segments++;
                }
                trigCounter += (float)Math.PI / 30f;
            }
        }
        public override void TopHit(NPC target)
        {
            target.velocity.Y -= 1f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (hitGround && projectile.friendly)
            {
                Texture2D texture = mod.GetTexture("Items/Weapons/DukeFishron/CycloneSpout");
                int height = texture.Height / 6;
                float spoutRadius = 10f;
                for (int i = 0; i < segments; i++)
                {
                    int frame = ((projectile.frameCounter/5) + i) % 6;
                    Vector2 pos = projectile.Center - Vector2.UnitY * i * height - Vector2.UnitY + Vector2.UnitX * (spoutRadius * (float)Math.Sin(trigCounter + ((float)Math.PI/3f * i)));
                    spriteBatch.Draw(texture, pos - Main.screenPosition, new Rectangle(0, height * frame, texture.Width, height), lightColor, 0, new Vector2(texture.Width, height) * .5f, 1, 0, 0);
                }
            }
            return base.PreDraw(spriteBatch, lightColor);
        }
    }
}
