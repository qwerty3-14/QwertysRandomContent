using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.B4Items       ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class DreadnoughtStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rod of Command");
            Tooltip.SetDefault("Used by Ur-Quan lords to issue commands");
        }

        public override void SetDefaults()
        {
            item.damage = 80;
            item.mana = 20;
            item.width = 54;
            item.height = 54;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 1f;
            item.value = 750000;
            item.rare = 10;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("Dreadnought");
            item.summon = true;
            item.buffType = mod.BuffType("UrQuan");
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

    public class Dreadnought : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ur-Quan Dreadnought");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
        }

        public override void SetDefaults()
        {
            projectile.width = 80; //Set the hitbox width
            projectile.height = 66;   //Set the hitbox height
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

        private NPC target;
        private const float maxSpeed = 6f;
        private int shotCounter = 0;
        private int fighterCounter = 0;
        private List<Projectile> fighters = new List<Projectile>();

        private void Thrust()
        {
            projectile.velocity += QwertyMethods.PolarVector(-.08f, projectile.velocity.ToRotation());
            projectile.velocity += QwertyMethods.PolarVector(.15f, projectile.rotation);
            Dust d = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(-40, projectile.rotation), 6);
            d.noGravity = true;
            d.noLight = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            shotCounter++;
            fighterCounter--;
            if (player.GetModPlayer<MinionManager>().Dreadnought)
            {
                projectile.timeLeft = 2;
            }
            if (QwertyMethods.ClosestNPC(ref target, 1000, projectile.Center, false, player.MinionAttackTargetNPC) && (player.Center - projectile.Center).Length() < 1000)
            {
                projectile.rotation = QwertyMethods.SlowRotation(projectile.rotation, (target.Center - projectile.Center).ToRotation(), 4);
                if (fighterCounter <= 0 && fighters.Count < 6)
                {
                    fighterCounter = 60;
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Ur-Quan/UrQuan-Launch").WithVolume(.4f));
                    fighters.Add(Main.projectile[Projectile.NewProjectile(projectile.Center + QwertyMethods.PolarVector(-40, projectile.rotation) + QwertyMethods.PolarVector(10, projectile.rotation + (float)Math.PI / 2), QwertyMethods.PolarVector(4, projectile.rotation + 3 * (float)Math.PI / 4), mod.ProjectileType("Fighter"), (int)(projectile.damage / 6f), 0, projectile.owner, projectile.whoAmI)]);
                    fighters.Add(Main.projectile[Projectile.NewProjectile(projectile.Center + QwertyMethods.PolarVector(-40, projectile.rotation) + QwertyMethods.PolarVector(10, projectile.rotation - (float)Math.PI / 2), QwertyMethods.PolarVector(4, projectile.rotation - 3 * (float)Math.PI / 4), mod.ProjectileType("Fighter"), (int)(projectile.damage / 6f), 0, projectile.owner, projectile.whoAmI)]);
                }
                if ((target.Center - projectile.Center).Length() < 300)
                {
                    if (shotCounter >= 20)
                    {
                        shotCounter = 0;
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Ur-Quan/UrQuan-Fusion").WithVolume(.1f));
                        Projectile l = Main.projectile[Projectile.NewProjectile(projectile.Center + QwertyMethods.PolarVector(40f, projectile.rotation), QwertyMethods.PolarVector(12f, projectile.rotation), mod.ProjectileType("Fusion"), projectile.damage, projectile.knockBack, projectile.owner)];
                    }
                }
                else
                {
                    Thrust();
                }
            }
            else
            {
                projectile.rotation = QwertyMethods.SlowRotation(projectile.rotation, (player.Center - projectile.Center).ToRotation(), 6);
                if ((player.Center - projectile.Center).Length() < 300)
                {
                }
                else
                {
                    Thrust();
                }
            }
            for (int k = 0; k < 1000; k++)
            {
                if (Main.projectile[k].type == projectile.type && k != projectile.whoAmI)
                {
                    if (Collision.CheckAABBvAABBCollision(projectile.position + new Vector2(projectile.width / 4, projectile.height / 4), new Vector2(projectile.width / 2, projectile.height / 2), Main.projectile[k].position + new Vector2(Main.projectile[k].width / 4, Main.projectile[k].height / 4), new Vector2(Main.projectile[k].width / 2, Main.projectile[k].height / 2)))
                    {
                        projectile.velocity += new Vector2((float)Math.Cos((projectile.Center - Main.projectile[k].Center).ToRotation()) * .1f, (float)Math.Sin((projectile.Center - Main.projectile[k].Center).ToRotation()) * .1f);
                    }
                }
            }
            if (projectile.velocity.Length() > maxSpeed)
            {
                projectile.velocity = projectile.velocity.SafeNormalize(-Vector2.UnitY) * maxSpeed;
            }
            for (int i = 0; i < fighters.Count; i++)
            {
                if (!fighters[i].active || fighters[i].type != mod.ProjectileType("Fighter"))
                {
                    fighters.RemoveAt(i);
                }
                else if (fighters[i].ai[1] == 0)
                {
                    fighters[i].timeLeft = 2;
                }
            }
            if ((player.Center - projectile.Center).Length() > 2000)
            {
                fighters.Clear();
                projectile.rotation = (player.Center - projectile.Center).ToRotation();
                projectile.Center = player.Center;
            }
        }
    }

    public class Fighter : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fighter");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
        }

        public override void SetDefaults()
        {
            projectile.width = 2; //Set the hitbox width
            projectile.height = 2;   //Set the hitbox height
            projectile.hostile = false;    //tells the game if is hostile or not.
            projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
            Main.projFrames[projectile.type] = 1;  //this is where you add how many frames u'r projectile has to make the animation
            projectile.knockBack = 10f;
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            projectile.tileCollide = false; //Tells the game whether or not it can collide with tiles/ terrain
            projectile.minion = true;
            projectile.timeLeft = 2;
        }

        private NPC target;
        private float speed = 16f;
        private int counter;
        private Projectile parent;
        private int startTime = 0;

        public override void AI()
        {
            parent = Main.projectile[(int)projectile.ai[0]];
            counter--;
            Player player = Main.player[projectile.owner];
            if (startTime > 20)
            {
                if (QwertyMethods.ClosestNPC(ref target, 2000, projectile.Center, false, player.MinionAttackTargetNPC))
                {
                    projectile.rotation = (target.Center - projectile.Center).ToRotation() + (float)Math.PI / 2;
                    Vector2 offSpot = target.Center + QwertyMethods.PolarVector(-40, (target.Center - projectile.Center).ToRotation());
                    projectile.velocity = (offSpot - projectile.Center);
                    if (projectile.velocity.Length() > speed)
                    {
                        projectile.velocity = projectile.velocity.SafeNormalize(-Vector2.UnitY) * speed;
                    }
                    else
                    {
                        if (counter <= 0)
                        {
                            counter = 30;
                            QwertyMethods.PokeNPC(player, target, projectile.damage, 0, summon: true).GetGlobalProjectile<QwertyGlobalProjectile>().ignoresArmor = true;
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Ur-Quan/UrQuan-Fighter").WithVolume(.15f));
                        }
                    }
                }
                else
                {
                    projectile.rotation = (parent.Center - projectile.Center).ToRotation() + (float)Math.PI / 2;
                    projectile.velocity = (parent.Center - projectile.Center);
                    if (projectile.velocity.Length() > speed)
                    {
                        projectile.velocity = projectile.velocity.SafeNormalize(-Vector2.UnitY) * speed;
                    }
                    if (Collision.CheckAABBvAABBCollision(projectile.position, projectile.Size, parent.position, parent.Size))
                    {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Ur-Quan/UrQuan-Recover").WithVolume(.8f));
                        projectile.ai[1] = 1;
                    }
                }
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;
                startTime++;
            }
            for (int k = 0; k < 1000; k++)
            {
                if (Main.projectile[k].type == projectile.type && k != projectile.whoAmI)
                {
                    if ((projectile.Center - Main.projectile[k].Center).Length() < 10)
                    {
                        projectile.velocity += new Vector2((float)Math.Cos((projectile.Center - Main.projectile[k].Center).ToRotation()) * .1f, (float)Math.Sin((projectile.Center - Main.projectile[k].Center).ToRotation()) * .1f);
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (counter > 25)
            {
                spriteBatch.Draw(mod.GetTexture("Items/B4Items/FighterShot"), projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, new Vector2(1, 39), new Vector2(1, 1), 0, 0);
            }
            return true;
        }
    }

    public class Fusion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fusion Blast");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.minion = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 60;
            projectile.tileCollide = false;
            projectile.GetGlobalProjectile<QwertyGlobalProjectile>().ignoresArmor = true;
        }

        public override void AI()
        {
        }
    }
}