using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Rhuthinium
{
    public class RhuthiniumRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Rod");
            if (ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium)
            {
                Main.itemTexture[item.type] = mod.GetTexture("Items/Weapons/Rhuthinium/RhuthiniumRod_Old");
            }
        }
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.GoldenFishingRod);  //This defines the fishing pole you want to clone

            item.fishingPole = 29; //this defines the fishing pole fishing power

            item.value = 25000;
            item.rare = 3;    //The color the title of your item when hovering over it ingame .
            item.shoot = mod.ProjectileType("RhuthiniumBobber");  //This defines what type of projectile this item will shot
            item.shootSpeed = 9f; //this defines the the projectile speed when shot. for fishing pole also increases the fishing line length/range
        }
        public override void AddRecipes()   //This defines the crafting recepe for this item
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 8); //this is an example of how to add a modded item as an ingredient
            recipe.AddTile(TileID.Anvils);   //this is where to craft the item ,WorkBenches = all WorkBenches    Anvils = all anvils , MythrilAnvil = Mythril Anvil and Orichalcum Anvil, Furnaces = all furnaces , DemonAltar = Demon Altar and Crimson Altar , TinkerersWorkbench = Tinkerer's Workbench
            recipe.SetResult(this);   //this defines the resultat of hte crafting, so 15 bombs + 1 customtileitem = 15 Custom Explosive
            recipe.AddRecipe();
        }
        /* The code below makes the pole shoot two additional bobbers
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y+10f, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Projectile.NewProjectile(position.X, position.Y-10f, speedX, speedY, type, damage, knockBack, player.whoAmI);
			return true;
		}
		*/
    }


    public class RhuthiniumBobber : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Bobber");
            if (ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium)
            {
                Main.projectileTexture[projectile.type] = mod.GetTexture("Items/Weapons/Rhuthinium/RhuthiniumBobber_Old");
            }
        }
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BobberGolden);   //so we are going to clone a bobber from vanilla terraria
        }

        public override bool PreDrawExtras(SpriteBatch spriteBatch)      //this draws the fishing line correctly
        {

            Player player = Main.player[projectile.owner];
            if (projectile.bobber && Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].holdStyle > 0)
            {
                float pPosX = player.MountedCenter.X;
                float pPosY = player.MountedCenter.Y;
                pPosY += Main.player[projectile.owner].gfxOffY;
                int type = Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].type;
                float gravDir = Main.player[projectile.owner].gravDir;

                if (type == mod.ItemType("RhuthiniumRod"))
                {
                    pPosX += (float)(50 * Main.player[projectile.owner].direction);
                    if (Main.player[projectile.owner].direction < 0)
                    {
                        pPosX -= 13f;
                    }
                    pPosY -= 30f * gravDir;
                }

                if (gravDir == -1f)
                {
                    pPosY -= 12f;
                }
                Vector2 value = new Vector2(pPosX, pPosY);
                value = Main.player[projectile.owner].RotatedRelativePoint(value + new Vector2(8f), true) - new Vector2(8f);
                float projPosX = projectile.position.X + (float)projectile.width * 0.5f - value.X;
                float projPosY = projectile.position.Y + (float)projectile.height * 0.5f - value.Y;
                Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
                float rotation2 = (float)Math.Atan2((double)projPosY, (double)projPosX) - 1.57f;
                bool flag2 = true;
                if (projPosX == 0f && projPosY == 0f)
                {
                    flag2 = false;
                }
                else
                {
                    float projPosXY = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
                    projPosXY = 12f / projPosXY;
                    projPosX *= projPosXY;
                    projPosY *= projPosXY;
                    value.X -= projPosX;
                    value.Y -= projPosY;
                    projPosX = projectile.position.X + (float)projectile.width * 0.5f - value.X;
                    projPosY = projectile.position.Y + (float)projectile.height * 0.5f - value.Y;
                }
                while (flag2)
                {
                    float num = 12f;
                    float num2 = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
                    float num3 = num2;
                    if (float.IsNaN(num2) || float.IsNaN(num3))
                    {
                        flag2 = false;
                    }
                    else
                    {
                        if (num2 < 20f)
                        {
                            num = num2 - 8f;
                            flag2 = false;
                        }
                        num2 = 12f / num2;
                        projPosX *= num2;
                        projPosY *= num2;
                        value.X += projPosX;
                        value.Y += projPosY;
                        projPosX = projectile.position.X + (float)projectile.width * 0.5f - value.X;
                        projPosY = projectile.position.Y + (float)projectile.height * 0.1f - value.Y;
                        if (num3 > 12f)
                        {
                            float num4 = 0.3f;
                            float num5 = Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y);
                            if (num5 > 16f)
                            {
                                num5 = 16f;
                            }
                            num5 = 1f - num5 / 16f;
                            num4 *= num5;
                            num5 = num3 / 80f;
                            if (num5 > 1f)
                            {
                                num5 = 1f;
                            }
                            num4 *= num5;
                            if (num4 < 0f)
                            {
                                num4 = 0f;
                            }
                            num5 = 1f - projectile.localAI[0] / 100f;
                            num4 *= num5;
                            if (projPosY > 0f)
                            {
                                projPosY *= 1f + num4;
                                projPosX *= 1f - num4;
                            }
                            else
                            {
                                num5 = Math.Abs(projectile.velocity.X) / 3f;
                                if (num5 > 1f)
                                {
                                    num5 = 1f;
                                }
                                num5 -= 0.5f;
                                num4 *= num5;
                                if (num4 > 0f)
                                {
                                    num4 *= 2f;
                                }
                                projPosY *= 1f + num4;
                                projPosX *= 1f - num4;
                            }
                        }
                        rotation2 = (float)Math.Atan2((double)projPosY, (double)projPosX) - 1.57f;
                        Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)value.X / 16, (int)(value.Y / 16f), new Microsoft.Xna.Framework.Color(38, 126, 126));    //this is the fishing line color in RGB, 200 is red, 12 is green, 50 blue

                        Main.spriteBatch.Draw(Main.fishingLineTexture, new Vector2(value.X - Main.screenPosition.X + (float)Main.fishingLineTexture.Width * 0.5f, value.Y - Main.screenPosition.Y + (float)Main.fishingLineTexture.Height * 0.5f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.fishingLineTexture.Width, (int)num)), color2, rotation2, new Vector2((float)Main.fishingLineTexture.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
                    }
                }
            }
            return false;
        }
    }
}
