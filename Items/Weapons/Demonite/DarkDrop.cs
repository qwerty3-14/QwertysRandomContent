using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.Weapons.Demonite       ///We need projectile to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class DarkDrop : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Drop");
            Tooltip.SetDefault("");
            Item.staff[item.type] = true; //projectile makes the useStyle animate as a staff instead of as a gun

        }

        public override void SetDefaults()
        {

            item.damage = 26;
            item.mana = 7;
            item.width = 44;
            item.height = 44;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1f;
            item.value = Item.sellPrice(silver: 27);
            item.rare = 1;
            item.UseSound = SoundID.Item43;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("DarkDropP");
            item.magic = true;
            item.shootSpeed = 8;

        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DemoniteBar, 10);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 62f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, (Main.MouseWorld - position).Length());
            return false;
        }


    }
    public class DarkDropP : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.magic = true;
            projectile.friendly = true;
            projectile.hostile = false;
        }
        float trigCounter;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void AI()
        {
            trigCounter += (float)Math.PI / 30;

            if (projectile.ai[0] > 0)
            {
                projectile.ai[0] -= projectile.velocity.Length();
                float rad = 10;
                if (projectile.ai[0] / 24f < rad)
                {
                    rad = projectile.ai[0] / 24f;
                }
                for (int i = 0; i < 3; i++)
                {
                    //Dust p = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector((float)Math.Sin(trigCounter + ((float)Math.PI*2*(i/3f))) * 10, projectile.velocity.ToRotation() + (float)Math.PI / 2), 27, Vector2.Zero);
                    Dust p = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(rad, trigCounter + ((float)Math.PI * 2 * (i / 3f))), 27, Vector2.Zero);
                    p.noGravity = true;
                }

            }
            else
            {
                projectile.velocity = new Vector2(0, 10f);
                Dust p = Dust.NewDustPerfect(projectile.Center, 27, Vector2.Zero, Scale: 1.5f);
                p.noGravity = true;
            }



        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (int)(damage * 1.5f);

        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Dust p = Dust.NewDustPerfect(projectile.Center, 27, QwertyMethods.PolarVector(8, (float)Math.PI * Main.rand.NextFloat(-1, 1)));
                p.noGravity = true;
            }
        }
    }


}