using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.DinoItems
{
    public class DinoVulcan : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'As heavy as a dinosaur'" + "\nSlows you down when used... unless you're riding a dinosaur" + "\nBuilds up in speed while used, up to 100 rounds a second!" + "\n50% chance not to consume ammo");

        }

        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.useAnimation = 2;
            item.useTime = 38;
            item.shootSpeed = 20f;
            item.knockBack = 2f;
            item.width = 50;
            item.height = 18;
            item.damage = 20;

            item.shoot = mod.ProjectileType("DinoVulcanP");
            item.rare = 6;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.noMelee = true;
            item.noUseGraphic = true;
            item.ranged = true;
            item.channel = true;
            item.useAmmo = AmmoID.Bullet;

            item.autoReuse = true;
        }
        public override void HoldItem(Player player)
        {
            player.accRunSpeed *= .5f;
            player.maxRunSpeed *= .5f;
            player.jumpSpeedBoost *= .5f;
        }
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = mod.ProjectileType("DinoVulcanP");
            position = player.Center;
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
        public override bool ConsumeAmmo(Player player)
        {
            return false;
        }








        /*
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(40));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			
			
			Vector2 muzzleOffset = new Vector2(speedX, speedY).SafeNormalize(-Vector2.UnitY);
			position += new Vector2(muzzleOffset.Y * player.direction, muzzleOffset.X * -player.direction) * 5.5f; // change 5.5f to change the height
			muzzleOffset *= 50f; // change 38f to change the offset from the player
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
			
		}
        */

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, -8);
        }


    }
    public class DinoVulcanP : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;

            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.hide = false;
            projectile.ranged = true;
            projectile.ignoreWater = true;

        }


        public int timer = 0;
        public int reloadTime;
        public float direction;
        public float VarA;
        public float VarB;
        public float VarC;
        public float VarD;
        public float VarE;
        public float SVarA;
        public float SVarB;
        public float SVarC;
        public float SVarD;
        public float SVarE;
        public float Radd;
        public bool runOnce = true;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                reloadTime = player.inventory[player.selectedItem].useTime;
                runOnce = false;

            }
            projectile.timeLeft = 10;
            timer++;
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            bool firing = player.channel && player.HasAmmo(QwertyMethods.MakeItemFromID(ItemID.FlintlockPistol), true) && !player.noItems && !player.CCed;

            int Ammo = 14;
            float speed = 14f;

            int weaponDamage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
            direction = (Main.MouseWorld - player.Center).ToRotation();
            float weaponKnockback = player.inventory[player.selectedItem].knockBack;
            if (firing)
            {

                ///////////////////////////////////// copied from vanilla drill/chainsaw AI
                Vector2 vector24 = Main.player[projectile.owner].RotatedRelativePoint(Main.player[projectile.owner].MountedCenter, true);
                if (Main.myPlayer == projectile.owner)
                {
                    if (Main.player[projectile.owner].channel)
                    {
                        float num264 = Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].shootSpeed * projectile.scale;
                        Vector2 vector25 = vector24;
                        float num265 = (float)Main.mouseX + Main.screenPosition.X - vector25.X;
                        float num266 = (float)Main.mouseY + Main.screenPosition.Y - vector25.Y;
                        if (Main.player[projectile.owner].gravDir == -1f)
                        {
                            num266 = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector25.Y;
                        }
                        float num267 = (float)Math.Sqrt((double)(num265 * num265 + num266 * num266));
                        num267 = (float)Math.Sqrt((double)(num265 * num265 + num266 * num266));
                        num267 = num264 / num267;
                        num265 *= num267;
                        num266 *= num267;
                        if (num265 != projectile.velocity.X || num266 != projectile.velocity.Y)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.velocity.X = num265;
                        projectile.velocity.Y = num266;
                    }
                    else
                    {
                        projectile.Kill();
                    }
                }
                if (projectile.velocity.X > 0f)
                {
                    Main.player[projectile.owner].ChangeDir(1);
                }
                else if (projectile.velocity.X < 0f)
                {
                    Main.player[projectile.owner].ChangeDir(-1);
                }
                projectile.spriteDirection = projectile.direction;
                Main.player[projectile.owner].ChangeDir(projectile.direction);
                Main.player[projectile.owner].heldProj = projectile.whoAmI;
                Main.player[projectile.owner].itemTime = 2;
                Main.player[projectile.owner].itemAnimation = 2;
                projectile.position.X = vector24.X - (float)(projectile.width / 2);
                projectile.position.Y = vector24.Y - (float)(projectile.height / 2);
                projectile.rotation = (float)(Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.5700000524520874);
                if (Main.player[projectile.owner].direction == 1)
                {
                    Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(projectile.velocity.Y * (float)projectile.direction), (double)(projectile.velocity.X * (float)projectile.direction));
                }
                else
                {
                    Main.player[projectile.owner].itemRotation = (float)Math.Atan2((double)(projectile.velocity.Y * (float)projectile.direction), (double)(projectile.velocity.X * (float)projectile.direction));
                }
                projectile.velocity.X = projectile.velocity.X * (1f + (float)Main.rand.Next(-3, 4) * 0.01f);
                if (Main.rand.Next(6) == 0)
                {
                    int num268 = Dust.NewDust(projectile.position + projectile.velocity * (float)Main.rand.Next(6, 10) * 0.1f, projectile.width, projectile.height, 31, 0f, 0f, 80, default(Color), 1.4f);
                    Dust dust51 = Main.dust[num268];
                    dust51.position.X = dust51.position.X - 4f;
                    Main.dust[num268].noGravity = true;
                    Dust dust3 = Main.dust[num268];
                    dust3.velocity *= 0.2f;
                    Main.dust[num268].velocity.Y = -(float)Main.rand.Next(7, 13) * 0.15f;
                    return;
                }
                ///////////////////////////////

                if (!player.HasBuff(mod.BuffType("TrexMountB")))
                {
                    player.velocity.X = 0;
                    float VelYOld = player.velocity.Y;
                    player.velocity.Y = (float)Math.Abs(VelYOld);
                }



                if (timer >= reloadTime)
                {
                    VarA = direction + MathHelper.ToRadians(Main.rand.Next(-100, 101) / 10);
                    VarB = direction + MathHelper.ToRadians(Main.rand.Next(-100, 101) / 10);
                    VarC = direction + MathHelper.ToRadians(Main.rand.Next(-100, 101) / 10);
                    VarD = direction + MathHelper.ToRadians(Main.rand.Next(-100, 101) / 10);
                    VarE = direction + MathHelper.ToRadians(Main.rand.Next(-100, 101) / 10);

                    float shellShift = MathHelper.ToRadians(-50);
                    SVarA = shellShift + MathHelper.ToRadians(Main.rand.Next(-100, 301) / 10);
                    SVarB = shellShift + MathHelper.ToRadians(Main.rand.Next(-100, 301) / 10);
                    SVarC = shellShift + MathHelper.ToRadians(Main.rand.Next(-100, 301) / 10);
                    SVarD = shellShift + MathHelper.ToRadians(Main.rand.Next(-100, 301) / 10);
                    SVarE = shellShift + MathHelper.ToRadians(Main.rand.Next(-100, 301) / 10);
                    float SspeedA = .05f * Main.rand.Next(15, 41);
                    float SspeedB = .05f * Main.rand.Next(15, 41);
                    float SspeedC = .05f * Main.rand.Next(15, 41);
                    float SspeedD = .05f * Main.rand.Next(15, 41);
                    float SspeedE = .05f * Main.rand.Next(15, 41);

                    Main.PlaySound(SoundID.Item11, projectile.Center);
                    Item sItem = QwertyMethods.MakeItemFromID(ItemID.FlintlockPistol);
                    sItem.damage = weaponDamage;
                    player.PickAmmo(sItem, ref Ammo, ref speed, ref firing, ref weaponDamage, ref weaponKnockback, Main.rand.Next(0, 2) == 0);

                    if (player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(VarA) * speed, (float)Math.Sin(VarA) * speed, Ammo, weaponDamage, weaponKnockback, Main.myPlayer);
                        Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(SVarA) * SspeedA * -player.direction, (float)Math.Sin(SVarA) * SspeedA, mod.ProjectileType("Shell"), 0, 0, Main.myPlayer);
                        if (reloadTime < 32)
                        {
                            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(VarB) * speed, (float)Math.Sin(VarB) * speed, Ammo, weaponDamage, weaponKnockback, Main.myPlayer);
                            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(SVarB) * SspeedB * -player.direction, (float)Math.Sin(SVarB) * SspeedB, mod.ProjectileType("Shell"), 0, 0, Main.myPlayer);
                        }
                        if (reloadTime < 28)
                        {
                            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(VarC) * speed, (float)Math.Sin(VarC) * speed, Ammo, weaponDamage, weaponKnockback, Main.myPlayer);
                            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(SVarC) * SspeedC * -player.direction, (float)Math.Sin(SVarC) * SspeedC, mod.ProjectileType("Shell"), 0, 0, Main.myPlayer);
                        }
                        if (reloadTime < 24)
                        {
                            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(VarD) * speed, (float)Math.Sin(VarD) * speed, Ammo, weaponDamage, weaponKnockback, Main.myPlayer);
                            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(SVarD) * SspeedD * -player.direction, (float)Math.Sin(SVarD) * SspeedD, mod.ProjectileType("Shell"), 0, 0, Main.myPlayer);
                        }
                        if (reloadTime < 20)
                        {
                            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(VarE) * speed, (float)Math.Sin(VarE) * speed, Ammo, weaponDamage, weaponKnockback, Main.myPlayer);
                            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(SVarE) * SspeedE * -player.direction, (float)Math.Sin(SVarE) * SspeedE, mod.ProjectileType("Shell"), 0, 0, Main.myPlayer);
                        }
                    }

                    if (reloadTime > 3)
                    {
                        reloadTime -= 1;

                    }
                    timer = 0;
                }


            }
            else
            {
                modPlayer.usingVulcan = false;
                reloadTime = player.inventory[player.selectedItem].useTime;
                projectile.Kill();
            }
        }
    }
    public class Shell : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shell");


        }

        public override void SetDefaults()
        {


            projectile.width = 6; //Set the hitbox width
            projectile.height = 10;   //Set the hitbox height
            projectile.hostile = false;    //tells the game if is hostile or not.
            projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
            projectile.aiStyle = 1;

            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            projectile.tileCollide = true; //Tells the game whether or not it can collide with tiles/ terrain

            projectile.timeLeft = 90;

        }
        public bool runOnce = true;
        public float rotationSpeed;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                rotationSpeed = player.direction * Main.rand.Next(0, 241);
                runOnce = false;
            }

            projectile.rotation += MathHelper.ToRadians(rotationSpeed);
            if (projectile.timeLeft <= 20)
            {
                projectile.alpha = (int)(255f - ((float)projectile.timeLeft / 20f) * 255f);
            }



        }
        public override bool OnTileCollide(Vector2 velocityChange)
        {
            projectile.velocity.X /= 2;
            return false;


        }

    }
}
