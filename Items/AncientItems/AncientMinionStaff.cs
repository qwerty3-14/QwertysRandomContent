using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.AncientItems       ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class AncientMinionStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Minion Staff");
            Tooltip.SetDefault("Summons an ancient minion to fight for you!");

        }

        public override void SetDefaults()
        {

            item.damage = 12;  //The damage stat for the Weapon.
            item.mana = 20;      //this defines how many mana this weapon use
            item.width = 72;    //The size of the width of the hitbox in pixels.
            item.height = 72;     //The size of the height of the hitbox in pixels.
            item.useTime = 25;   //How fast the Weapon is used.
            item.useAnimation = 25;    //How long the Weapon is used for.
            item.useStyle = 1;  //The way your Weapon will be used, 1 is the regular sword swing for example
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1f;  //The knockback stat of your Weapon.
            item.value = 150000;
            item.rare = 3;
            item.UseSound = SoundID.Item8;   //The sound played when using your Weapon
            item.autoReuse = true;   //Weather your Weapon will be used again after use while holding down, if false you will need to click again after use to use it again.
            item.shoot = mod.ProjectileType("AncientMinionFreindly");   //This defines what type of projectile this weapon will shot
            item.summon = true;    //This defines if it does Summon damage and if its effected by Summon increasing Armor/Accessories.
            item.buffType = mod.BuffType("AncientMinion");  //The buff added to player after used the item
            item.buffTime = 3600;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/AncientItems/AncientMinionStaff_Glow");
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/AncientItems/AncientMinionStaff_Glow");
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
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

    public class AncientMinionFreindly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Minion");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {


            projectile.width = 42; //Set the hitbox width
            projectile.height = 56;   //Set the hitbox height
            projectile.hostile = false;    //Tells the game if is hostile or not.
            projectile.friendly = true;   //Tells the game whether it is friendly to players/friendly projectiles or not
            projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
            Main.projFrames[projectile.type] = 1;  //this is where you add how many frames u'r projectile has to make the animation
            projectile.knockBack = 10f;
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            projectile.tileCollide = false; //Tells the game whether or not it can collide with tiles/ terrain
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.timeLeft = 2;
            projectile.usesLocalNPCImmunity = true;
        }

        public const int minionRingRadius = 50;
        public const int minionRingDustQty = 50;
        public int timer;
        public int Pos = 1;
        public int damage = 30;
        public int switchTime = 300;
        public int moveCount = 0;
        public int fireCount = 0;
        public int attackType = 1;
        public bool charging;
        public NPC target;

        int waitTime = 5;
        int chargeTime = 15;
        Vector2 moveTo;
        bool justTeleported;
        float chargeSpeed = 12;
        bool runOnce = true;

        float maxDistance = 1000f;
        int frame;
        int noTargetTimer = 0;
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (modPlayer.AncientMinion)
            {
                projectile.timeLeft = 2;
            }


            if (runOnce)
            {
                //Main.PlaySound(SoundID.Item8);

                if (Main.netMode != 2)
                {
                    moveTo.X = projectile.Center.X;
                    moveTo.Y = projectile.Center.Y;
                    projectile.netUpdate = true;
                }
                runOnce = false;
            }

            if (QwertyMethods.ClosestNPC(ref target, maxDistance, player.Center, true, player.MinionAttackTargetNPC))
            {
                timer++;
                if (timer > waitTime + chargeTime)
                {
                    for (int k = 0; k < 200; k++)
                    {
                        projectile.localNPCImmunity[k] = 0;
                    }
                    for (int i = 0; i < minionRingDustQty; i++)
                    {
                        float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);

                        Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(minionRingRadius, theta), mod.DustType("AncientGlow"), QwertyMethods.PolarVector(-minionRingRadius / 10, theta));
                        dust.noGravity = true;

                    }
                    if (Main.netMode != 2)
                    {



                        projectile.ai[1] = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                        projectile.netUpdate = true;
                    }
                    moveTo = new Vector2(target.Center.X + (float)Math.Cos(projectile.ai[1]) * 100, target.Center.Y + (float)Math.Sin(projectile.ai[1]) * 100);
                    if (Main.netMode != 2)
                    {

                        projectile.netUpdate = true;
                    }
                    justTeleported = true;
                    timer = 0;
                    noTargetTimer = 0;
                }
                else if (timer > waitTime)
                {
                    charging = true;
                }
                else
                {
                    if (timer == 2)
                    {
                        Main.PlaySound(SoundID.Item8, projectile.Center);
                        for (int i = 0; i < minionRingDustQty; i++)
                        {
                            float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                            Dust dust = Dust.NewDustPerfect(projectile.Center, mod.DustType("AncientGlow"), QwertyMethods.PolarVector(minionRingRadius / 10, theta));
                            dust.noGravity = true;
                        }
                    }
                    charging = false;
                }
                if (charging)
                {
                    projectile.velocity = new Vector2((float)Math.Cos(projectile.rotation) * chargeSpeed, (float)Math.Sin(projectile.rotation) * chargeSpeed);
                }
                else
                {
                    projectile.Center = new Vector2(moveTo.X, moveTo.Y);
                    projectile.velocity = new Vector2(0, 0);
                    float targetAngle = new Vector2(target.Center.X - projectile.Center.X, target.Center.Y - projectile.Center.Y).ToRotation();
                    projectile.rotation = targetAngle;
                }




            }
            else
            {
                noTargetTimer++;
                if (noTargetTimer == 2)
                {
                    Main.PlaySound(SoundID.Item8);
                    for (int i = 0; i < minionRingDustQty; i++)
                    {
                        float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                        Dust dust = Dust.NewDustPerfect(projectile.Center, mod.DustType("AncientGlow"), QwertyMethods.PolarVector(minionRingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                }
                if ((projectile.Center - player.Center).Length() > 300)
                {
                    if (Main.netMode != 2)
                    {



                        projectile.ai[1] = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                        projectile.netUpdate = true;
                    }
                    for (int i = 0; i < minionRingDustQty; i++)
                    {
                        float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);

                        Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(minionRingRadius, theta), mod.DustType("AncientGlow"), QwertyMethods.PolarVector(-minionRingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                    noTargetTimer = 0;
                    moveTo = new Vector2(player.Center.X + (float)Math.Cos(projectile.ai[1]) * 100, player.Center.Y + (float)Math.Sin(projectile.ai[1]) * 100);
                    justTeleported = true;
                }

                projectile.Center = moveTo;

                float targetAngle = new Vector2(player.Center.X - projectile.Center.X, player.Center.Y - projectile.Center.Y).ToRotation();
                projectile.rotation = targetAngle;
            }
            if (justTeleported)
            {

                justTeleported = false;
            }

            projectile.frameCounter++;

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(mod.GetTexture("NPCs/AncientMachine/AncientMinion"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), drawColor, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("NPCs/AncientMachine/AncientMinion_Glow"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), Color.White, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }

    }

}