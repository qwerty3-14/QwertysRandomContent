using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.Weapons.MiscBoomerangs
{
    public class GoldBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gold Boomerang");
            Tooltip.SetDefault("This boomerang can be thrown notably far but is slower than most boomerangs");


        }
        public override void SetDefaults()
        {
            item.damage = 14;
            item.melee = true;
            item.noMelee = true;

            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = 5;
            item.knockBack = 0;
            item.value = 9000;
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.noUseGraphic = true;
            item.width = 18;
            item.height = 32;

            //item.autoReuse = true;
            item.shoot = mod.ProjectileType("GoldBoomerangP");
            item.shootSpeed = 13f;
            item.channel = true;




        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldBar, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class GoldBoomerangP : ModProjectile
    {
        public override void SetDefaults()
        {
            //projectile.aiStyle = ProjectileID.WoodenBoomerang;
            //aiType = 52;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.width = 18;
            projectile.height = 32;
            projectile.melee = true;
            projectile.usesLocalNPCImmunity = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Boomerang");

        }
        float speed;
        float maxSpeed;
        bool runOnce = true;
        float decceleration = 1f / 7f;
        int spinDirection;
        bool returnToPlayer;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                spinDirection = player.direction;
                speed = projectile.velocity.Length();
                maxSpeed = speed;
                runOnce = false;
            }
            projectile.rotation += MathHelper.ToRadians(maxSpeed * spinDirection);
            if (returnToPlayer)
            {



                if (Collision.CheckAABBvAABBCollision(player.position, player.Size, projectile.position, projectile.Size))
                {
                    projectile.Kill();
                }
                projectile.tileCollide = false;
                //projectile.friendly = false;
                projectile.velocity = QwertyMethods.PolarVector(speed, (player.Center - projectile.Center).ToRotation());
                speed += decceleration;
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
            }
            else
            {

                projectile.velocity = projectile.velocity.SafeNormalize(-Vector2.UnitY) * speed;
                speed -= decceleration;
                if (speed < 1f)
                {
                    returnToPlayer = true;
                }
            }

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
            returnToPlayer = true;

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            returnToPlayer = true;
            return false;
        }
    }


}

