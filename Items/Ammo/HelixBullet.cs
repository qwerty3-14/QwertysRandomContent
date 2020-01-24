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

namespace QwertysRandomContent.Items.Ammo
{
    public class HelixBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Helix Bullet");
            Tooltip.SetDefault("Fires a pair of bullets flying in a helix pattern. \nEach bullet does 80% normal damage.");
        }
        public override void SetDefaults()
        {
            item.damage = 9;
            item.rare = 7;
            item.shootSpeed = 1;
            item.knockBack = 2;
            item.value = 1;
            item.consumable = true;
            item.shoot = mod.ProjectileType("HelixBulletP");
            item.ammo = 97;
            item.maxStack = 999;
            item.ranged = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ShroomiteBar, 1);
            recipe.AddIngredient(ItemID.SpectreBar, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 200);
            recipe.AddRecipe();
        }
    }
    public class HelixBulletP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Helix Bullet");
            Main.projFrames[projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 6;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.extraUpdates = 4;
        }
        bool runOnce = true;
        float trigCounter = 0;
        float initialDirection = 0;
        float period = 60;
        float amplitude = 20;
        float previusR = 0;
        Projectile partner = null;
        public override void AI()
        {
            if(runOnce)
            {
                initialDirection = projectile.velocity.ToRotation();
                if(projectile.ai[0]==0)
                {
                    projectile.velocity *= .3f;
                    partner = Main.projectile[Projectile.NewProjectile(projectile.Center, projectile.velocity, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, 1, projectile.whoAmI)];
                }
                else
                {
                    partner = Main.projectile[(int)projectile.ai[1]];
                }
                runOnce = false;
            }
            projectile.frame = (int)projectile.ai[0];
            trigCounter += 2 * (float)Math.PI / period;
            float r = amplitude * (float)Math.Sin(trigCounter) * (projectile.ai[0] == 0 ? 1 : -1);
            Vector2 instaVel = QwertyMethods.PolarVector(r - previusR, initialDirection + (float)Math.PI / 2);
            projectile.position += instaVel;
            previusR = r;
            projectile.rotation = (projectile.velocity + instaVel).ToRotation() + (float)Math.PI / 2;
        }
        static void Draw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
        {
            //Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(QwertysRandomContent.Instance.GetTexture("Items/Ammo/HelixBulletTrail"), drawPos, new Rectangle(0, 6 * projectile.frame, 6, 6), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, 16 * projectile.frame, 8, 16), lightColor, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if( projectile.ai[0] == 0 && (float)Math.Cos(trigCounter) >0)
            {
                Draw(projectile, spriteBatch, lightColor);
                if (partner != null && partner.active && partner.type == projectile.type && (partner.Center-projectile.Center).Length() < amplitude*3)
                {
                    Draw(partner, spriteBatch, lightColor);
                }
            }
            else if(projectile.ai[0] == 0)
            {
                if (partner != null && partner.active && partner.type == projectile.type && (partner.Center - projectile.Center).Length() < amplitude * 3)
                {
                    Draw(partner, spriteBatch, lightColor);
                }
                Draw(projectile, spriteBatch, lightColor);
            }
            else if(partner == null || !partner.active || partner.type != projectile.type || (partner.Center - projectile.Center).Length() > amplitude * 3)
            {
                Draw(projectile, spriteBatch, lightColor);
            }
            return false;
        }
       
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (int)(damage * .8f);
        }
    }
}
