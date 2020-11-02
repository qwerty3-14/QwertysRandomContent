using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.NPCs.RuneGhost.RuneBuilder;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSummons       ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class RunicMinionStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leech Rune Staff");
            Tooltip.SetDefault("Summons an leech rune to fight for you!" + "\nchance to steal life");
        }

        public override void SetDefaults()
        {
            item.damage = 50;  //The damage stat for the Weapon.
            item.mana = 20;      //this defines how many mana this weapon use
            item.width = 72;    //The size of the width of the hitbox in pixels.
            item.height = 72;     //The size of the height of the hitbox in pixels.
            item.useTime = 25;   //How fast the Weapon is used.
            item.useAnimation = 25;    //How long the Weapon is used for.
            item.useStyle = 1;  //The way your Weapon will be used, 1 is the regular sword swing for example
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1f;  //The knockback stat of your Weapon.
            item.value = 500000;
            item.rare = 9;
            item.UseSound = SoundID.Item8;   //The sound played when using your Weapon
            item.shoot = mod.ProjectileType("RunicMinionFreindly");   //This defines what type of projectile this weapon will shot
            item.summon = true;    //This defines if it does Summon damage and if its effected by Summon increasing Armor/Accessories.
            item.buffType = mod.BuffType("AncientMinion");  //The buff added to player after used the item
            item.buffTime = 3600;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Weapons/MiscSummons/RunicMinionStaff_Glow");
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("AncientMinionStaff"));
            recipe.AddIngredient(mod.ItemType("CraftingRune"), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/Weapons/MiscSummons/RunicMinionStaff_Glow");
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

    public class RunicMinionFreindly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leech Rune");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
        }

        public override void SetDefaults()
        {
            projectile.width = 40; //Set the hitbox width
            projectile.height = 40;   //Set the hitbox height
            projectile.hostile = false;    //tells the game if is hostile or not.
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
        public bool charging;
        public NPC target;

        private int waitTime = 20;
        private int chargeTime = 40;
        private Vector2 moveTo;
        private bool justTeleported;
        private float chargeSpeed = 12;
        private bool runOnce = true;

        private float maxDistance = 1000f;
        private int noTargetTimer = 0;
        float rot = 0;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (player.GetModPlayer<MinionManager>().AncientMinion)
            {
                projectile.timeLeft = 2;
            }

            if (runOnce)
            {
                if (Main.netMode != 2)
                {
                    moveTo = projectile.Center;
                    projectile.netUpdate = true;
                }
                runOnce = false;
            }

            if (QwertyMethods.ClosestNPC(ref target, maxDistance, player.Center, player.MinionAttackTargetNPC != -1, player.MinionAttackTargetNPC))
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

                        Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(minionRingRadius, theta), mod.DustType("LeechRuneDeath"), QwertyMethods.PolarVector(-minionRingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                    if (Main.netMode != 2)
                    {
                        projectile.ai[1] = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                        projectile.netUpdate = true;
                    }
                    moveTo = new Vector2(target.Center.X + (float)Math.Cos(projectile.ai[1]) * 120, target.Center.Y + (float)Math.Sin(projectile.ai[1]) * 180);
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
                            Dust dust = Dust.NewDustPerfect(projectile.Center, mod.DustType("LeechRuneDeath"), QwertyMethods.PolarVector(minionRingRadius / 10, theta));
                            dust.noGravity = true;
                        }
                    }
                    charging = false;
                }
                if (charging)
                {
                    projectile.velocity = new Vector2((float)Math.Cos( rot), (float)Math.Sin( rot)) * chargeSpeed;
                }
                else
                {
                    projectile.Center = new Vector2(moveTo.X, moveTo.Y);
                    projectile.velocity = new Vector2(0, 0);
                    float targetAngle = new Vector2(target.Center.X - projectile.Center.X, target.Center.Y - projectile.Center.Y).ToRotation();
                     rot = targetAngle;
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
                        Dust dust = Dust.NewDustPerfect(projectile.Center, mod.DustType("LeechRuneDeath"), QwertyMethods.PolarVector(minionRingRadius / 10, theta));
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

                        Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(minionRingRadius, theta), mod.DustType("LeechRuneDeath"), QwertyMethods.PolarVector(-minionRingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                    noTargetTimer = 0;
                    moveTo = new Vector2(player.Center.X + (float)Math.Cos(projectile.ai[1]) * 100, player.Center.Y + (float)Math.Sin(projectile.ai[1]) * 100);
                    justTeleported = true;
                }

                projectile.Center = moveTo;

                float targetAngle = new Vector2(player.Center.X - projectile.Center.X, player.Center.Y - projectile.Center.Y).ToRotation();
                 rot = targetAngle;
            }
            if (justTeleported)
            {
                projectile.frameCounter=0;
                justTeleported = false;
            }

            projectile.frameCounter++;
            projectile.rotation += Math.Sign(projectile.velocity.X) * (float)Math.PI / 60f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.immortal && !target.SpawnedFromStatue && Main.rand.Next(5) == 0)
            {
                Player player = Main.player[projectile.owner];
                player.statLife++;
                player.HealEffect(1, true);
            }
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float c = (projectile.frameCounter / 20f);
            if (c > 1f)
            {
                c = 1f;
            }
            int frame = projectile.frameCounter ;
            if (frame > 19)
            {
                frame = 19;
            }
            spriteBatch.Draw(RuneSprites.runeTransition[(int)Runes.Leech][frame], projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), projectile.rotation, new Vector2(10, 10), Vector2.One * 2, 0, 0);
            return false;
        }
    }
}