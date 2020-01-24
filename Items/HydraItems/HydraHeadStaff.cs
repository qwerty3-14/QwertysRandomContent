using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.HydraItems       ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class HydraHeadStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Head Staff");
            Tooltip.SetDefault("Summons a hydra head to shoot towards your cursor" + "\nThe hydra head will automatically summon more heads if there are empty minion slots");

        }

        public override void SetDefaults()
        {

            item.damage = 30;  //The damage stat for the Weapon.
            item.mana = 20;      //this defines how many mana this weapon use
            item.width = 80;    //The size of the width of the hitbox in pixels.
            item.height = 80;     //The size of the height of the hitbox in pixels.
            item.useTime = 25;   //How fast the Weapon is used.
            item.useAnimation = 25;    //How long the Weapon is used for.
            item.useStyle = 1;  //The way your Weapon will be used, 1 is the regular sword swing for example
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1f;  //The knockback stat of your Weapon.
            item.value = 250000;
            item.rare = 5;
            item.UseSound = SoundID.Item44;   //The sound played when using your Weapon
            item.autoReuse = true;   //Weather your Weapon will be used again after use while holding down, if false you will need to click again after use to use it again.
            item.shoot = mod.ProjectileType("MinionHead");   //This defines what type of projectile this weapon will shot
            item.summon = true;    //This defines if it does Summon damage and if its effected by Summon increasing Armor/Accessories.
            item.buffType = mod.BuffType("HydraHead");  //The buff added to player after used the item
            item.buffTime = 3600;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 SPos = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);   //this make so the projectile will spawn at the mouse cursor position
            position = SPos;

            return true;
        }


        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }

    }

    public class MinionHead : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Head");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting

        }

        public override void SetDefaults()
        {


            projectile.width = 42; //Set the hitbox width
            projectile.height = 36;   //Set the hitbox height
            projectile.hostile = false;    //tells the game if is hostile or not.
            projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
            Main.projFrames[projectile.type] = 1;  //this is where you add how many frames u'r projectile has to make the animation
            projectile.knockBack = 10f;
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            projectile.tileCollide = false; //Tells the game whether or not it can collide with tiles/ terrain
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.timeLeft = 2;

        }


        public int varTime;
        public int Yvar = 0;
        public int YvarOld = 0;
        public int Xvar = 0;
        public int XvarOld = 0;
        public int f = 1;
        public float targetAngle = 90;
        public float s = 1;
        public float tarX;
        public float tarY;
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (modPlayer.HydraHeadMinion)
            {
                projectile.timeLeft = 2;
            }
            if (Main.LocalPlayer == player)
            {
                projectile.ai[0] = Main.MouseWorld.X;
                projectile.ai[1] = Main.MouseWorld.Y;
                projectile.netUpdate = true;

                //projectile.netUpdate = true;
            }
            projectile.rotation = (new Vector2(projectile.ai[0], projectile.ai[1]) - projectile.Center).ToRotation();
            tarX = (float)Math.Cos(projectile.rotation) * 10;
            tarY = (float)Math.Sin(projectile.rotation) * 10;

            //  Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 24);



            if (player.maxMinions - player.numMinions >= 1 && Main.netMode != 2 && modPlayer.HydraHeadMinion && Main.myPlayer == projectile.owner)
            {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.Center.X, projectile.Center.Y, mod.ProjectileType("MinionHead"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
            }






            varTime++;
            if (varTime == 30 && Main.netMode != 2 && Main.myPlayer == projectile.owner)
            {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, tarX, tarY, mod.ProjectileType("MinionBreath"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
            }
            if (varTime >= 60)
            {
                if (Main.netMode != 2 && Main.myPlayer == projectile.owner)
                {
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, tarX, tarY, mod.ProjectileType("MinionBreath"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                }
                YvarOld = Yvar;

                XvarOld = Xvar;

                varTime = 0;
                if (Main.netMode != 2)
                {
                    Yvar = Main.rand.Next(0, 80);
                    Xvar = Main.rand.Next(-80, 80);
                }
            }

            Vector2 moveTo = new Vector2(player.Center.X + Xvar, player.Center.Y - Yvar) - projectile.Center;
            projectile.velocity = (moveTo) * .04f;

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 playerCenter = Main.player[projectile.owner].MountedCenter;
            Vector2 center = projectile.Center;
            Vector2 distToProj = playerCenter - projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            for (int i = 0; i < 1000; i++)
            {
                if (distance > 4f && !float.IsNaN(distance))
                {
                    distToProj.Normalize();                 //get unit vector
                    distToProj *= 8f;                      //speed = 12
                    center += distToProj;                   //update draw position
                    distToProj = playerCenter - center;    //update distance
                    distance = distToProj.Length();
                    Color drawColor = lightColor;

                    //Draw chain
                    spriteBatch.Draw(mod.GetTexture("Items/HydraItems/HydraHookChain"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 14, 8), drawColor, projRotation,
                        new Vector2(14 * 0.5f, 8 * 0.5f), 1f, SpriteEffects.None, 0f);
                }
            }
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(mod.GetTexture("Items/HydraItems/MinionHead_Glow"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, projectile.width, projectile.height), Color.White, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
        }
    }
    public class MinionBreath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Minion Breath");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 14;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.minion = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 600;



        }
        public override void AI()
        {
            CreateDust();
        }
        public virtual void CreateDust()
        {

            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBreathGlow"));



        }


    }
}