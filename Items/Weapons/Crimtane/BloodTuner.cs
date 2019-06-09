using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.Weapons.Crimtane       ///We need projectile to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class BloodTuner : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Tuner");
			Tooltip.SetDefault("");
			Item.staff[item.type] = true; //projectile makes the useStyle animate as a staff instead of as a gun
			
		}
 
        public override void SetDefaults()
        {

            item.damage = 10;
            item.crit = 5;
            item.mana = 4;      
            item.width = 24;    
            item.height = 24;    
            item.useTime = 10;  
            item.useAnimation = 10;    
            item.useStyle = 5; 
            item.noMelee = true;
            item.knockBack = 1f; 
            item.value = Item.sellPrice(silver: 27);
            item.rare = 1;
            item.UseSound = SoundID.Item43;  
            item.autoReuse = true;   
            item.shoot = mod.ProjectileType("BloodP");   
            item.magic = true;
            item.channel = true;
            item.shootSpeed = 8;
            
        }
		
        
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrimtaneBar, 10);
			
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for(int i =0; i<1000; i++)
            {
                if(Main.projectile[i].type == type && Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI&& Main.projectile[i].ai[1] != -1)
                {
                    Main.projectile[i].ai[0] += damage;
                    Main.projectile[i].ai[1] = 1;
                    return false;
                }
            }
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 17f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, damage, 1f);
            return false;
        }


    }
    public class BloodP : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.height = 8;
            projectile.width = 8;
            projectile.magic = true;
            projectile.friendly = true;
        }
        float trigCounter = 0f;
        public List<Vector2> bloodSplatters = new List<Vector2>();
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (player.channel && projectile.ai[1] != -1)
            {
                projectile.damage = (int)projectile.ai[0];
                if (projectile.ai[1] == 1f)
                {
                    bloodSplatters.Add(new Vector2((float)Math.PI * Main.rand.NextFloat(-1, 1), (float)Math.PI * Main.rand.NextFloat(-1, 1)));
                    projectile.ai[1] = 0;
                }
                projectile.velocity = 10f * (QwertysRandomContent.LocalCursor[projectile.owner] - projectile.Center).SafeNormalize(-Vector2.UnitY);
                Vector2 offset = QwertyMethods.PolarVector(20f, player.itemRotation);
                offset *= player.direction;
                projectile.Center = player.Center + offset;
            }
            else
            {
                projectile.ai[1] = -1;
            }
            for (int i = 0; i < bloodSplatters.Count; i++)
            {
                bloodSplatters[i] += new Vector2((float)Math.PI / 30f, (float)Math.PI / 30f);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D blood = Main.projectileTexture[projectile.type];
            for (int i = 0; i < bloodSplatters.Count; i++)
            {
                float Major = 20f;
                float Minor = 5f;
                Vector2 BloodCenter = projectile.Center + QwertyMethods.PolarVector((Major*Minor)/(float)Math.Sqrt((Minor*(float)Math.Cos(bloodSplatters[i].X))* (Minor * (float)Math.Cos(bloodSplatters[i].X)) + (Major * (float)Math.Sin(bloodSplatters[i].X)) * (Major * (float)Math.Sin(bloodSplatters[i].X))), bloodSplatters[i].Y);
                spriteBatch.Draw(blood, BloodCenter - Main.screenPosition,
                            blood.Frame(), Lighting.GetColor((int)BloodCenter.X / 16, (int)BloodCenter.Y / 16), 0f,
                            new Vector2(blood.Width/2, blood.Height/2), .5f, SpriteEffects.None, 0f);
            }
                
            return false;
        }
    }

    
}