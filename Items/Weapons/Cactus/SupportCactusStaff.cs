using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Buffs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace QwertysRandomContent.Items.Weapons.Cactus
{
    public class SupportCactusStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Support Cactus Staff");
            Tooltip.SetDefault("Use this when you're short on friends");
        }
        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 38;
            item.mana = 10;
            
            item.shoot = mod.ProjectileType("SupportCactus");
            item.shootSpeed = 0f;
            item.useTime = 20;
            item.useAnimation = 20;
           
            
            item.useStyle = 1;
            item.knockBack = 0f;
            item.value = Item.sellPrice(silver: 3, copper: 60) ;
            item.rare = 1;
            item.UseSound = SoundID.Item44;
            item.sentry = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.summon = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            Point point;
            Point origin = position.ToTileCoordinates();
            while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                {
                                            new Conditions.IsSolid()
                }), out point))
            {
                position.Y++;
                origin = position.ToTileCoordinates();
            }
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cactus, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
    public class SupportCactus : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.sentry = true;
            projectile.minion = true;
            projectile.timeLeft = Projectile.SentryLifeTime;
        }
        int cactusHeight = 0;
        int maxCactusHeight = 82;
        int branchOut = 0;
        int maxBranchOut = 10;
        float flowering = 0f;
        Vector2 flowerPos = new Vector2(4, -76);
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.UpdateMaxTurrets();
            if (cactusHeight < maxCactusHeight)
            {
                cactusHeight++;
            }
            else if (branchOut < maxBranchOut)
            {
                branchOut++;
            }
            else if (flowering < 1f)
            {
                flowering += (float)(1f / 60f);
            }
            else
            {
                flowering = 0f;

                flowerPos = new Vector2(Main.rand.Next(-6, 7), -76 - Main.rand.NextFloat(4f));
                Projectile.NewProjectile(projectile.Bottom + flowerPos, QwertyMethods.PolarVector(8f, (player.Center - (projectile.Bottom + flowerPos)).ToRotation()), mod.ProjectileType("CactusFlower"), 0, 0, projectile.owner);
                projectile.netUpdate = true;


            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(flowerPos);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            flowerPos = reader.ReadVector2();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            
            if(branchOut>0)
            {
                Texture2D arm = mod.GetTexture("Items/Weapons/Cactus/RightCactusArm");
                spriteBatch.Draw(arm, projectile.BottomRight - Main.screenPosition - new Vector2(0, 44),
                       new Rectangle(arm.Width - branchOut, 0, branchOut, arm.Height), lightColor, projectile.rotation,
                       new Vector2(0, arm.Height), 1f, 0, 0f);
                arm = mod.GetTexture("Items/Weapons/Cactus/LeftCactusArm");
                spriteBatch.Draw(arm, projectile.BottomLeft - Main.screenPosition - new Vector2(0, 32),
                           new Rectangle(0, 0, branchOut, arm.Height), lightColor, projectile.rotation,
                           new Vector2(branchOut, arm.Height), 1f, 0, 0f);
            }
            
            Texture2D body = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(body, projectile.Bottom-Main.screenPosition - new Vector2(0, 4),
                       new Rectangle(0, 0, body.Width, cactusHeight), lightColor, projectile.rotation,
                       new Vector2(6, cactusHeight), 1f, 0, 0f);
            if (flowering > 0f)
            {
                Texture2D flower = mod.GetTexture("Items/Weapons/Cactus/CactusFlower");
                spriteBatch.Draw(flower, projectile.Bottom - Main.screenPosition + flowerPos,
                      new Rectangle(0, 0, flower.Width, flower.Height), lightColor, projectile.rotation - ((float)Math.PI / 2 * flowering),
                      new Vector2(flower.Width, flower.Height) * .5f, flowering, 0, 0f);
            }
            return false;
        }
    }
    public class CactusFlower : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
        }
        public override void AI()
        {
            projectile.rotation += (float)Math.PI / 120f * (projectile.velocity.X > 0 ? 1 : -1);
            Player player = Main.player[projectile.owner];
            if(Collision.CheckAABBvAABBCollision(projectile.position, projectile.Size, player.position, player.Size))
            {
                player.HealEffect(2);
                projectile.Kill();
                player.statLife += 2;

            }
        }
    }
    
} 
