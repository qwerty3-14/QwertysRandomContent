using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using QwertysRandomContent.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Shroomite
{
    public class ShroomiteJavelin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroomite Javelin");

        }
        public override void SetDefaults()
        {
            // Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
            item.shootSpeed = 12f;
            item.damage = 136;
            item.knockBack = 0f;
            item.useStyle = 1;
            item.useAnimation = 26;
            item.useTime = 26;
            item.width = 68;
            item.height = 68;
            item.maxStack = 999;
            item.rare = 8;
            item.value = Item.sellPrice(copper: 50);
            item.consumable = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.thrown = true;

            item.UseSound = SoundID.Item1;

            item.shoot = mod.ProjectileType("ShroomiteJavelinP");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ShroomiteBar, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 150);
            recipe.AddRecipe();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach(TooltipLine line  in tooltips)
            {
                if (line.mod == "Terraria" && line.Name == "Knockback") //this checks if it's the line we're interested in
                {
                    line.text = "Rocket powered knockback";//change tooltip
                }
            }
        }
    }
    public class ShroomiteJavelinP : Javelin
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroomite Javelin");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.thrown = true;
            projectile.penetrate = 1;
            projectile.GetGlobalProjectile<ImplaingProjectile>().CanImpale = true;
            projectile.GetGlobalProjectile<ImplaingProjectile>().damagePerImpaler = 40;
            maxStickingJavelins = 3;
            dropItem = mod.ItemType("ShroomiteJavelin");
            rotationOffset = (float)Math.PI / 4;
            maxTicks = 70f;
        }
        int flameCounter = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y + 2),
                        new Rectangle(0, 0, texture.Width, texture.Height), lightColor, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            if (projectile.ai[0] == 1f)
            {
                flameCounter++;
                texture = mod.GetTexture("Items/Weapons/Shroomite/ShroomiteJavelinFlame");
                spriteBatch.Draw(texture, new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y + 2),
                            new Rectangle(0, (flameCounter%10 > 5 ? 1 : 0) *  texture.Height / 2, texture.Width, texture.Height/2), Color.White, projectile.rotation,
                            new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void StuckEffects(NPC victim)
        {
            if (victim.active && victim.chaseable && !victim.dontTakeDamage && !victim.friendly && victim.lifeMax > 5 && !victim.immortal)
            {
                Vector2 instaVel = QwertyMethods.PolarVector(victim.boss ? .4f : .6f, projectile.rotation - rotationOffset - (float)Math.PI / 2);
                if (!victim.noTileCollide)
                {
                    instaVel = Collision.TileCollision(victim.position, instaVel, victim.width, victim.height);
                }
                victim.position += instaVel;


            }
            
        }
    }
}
