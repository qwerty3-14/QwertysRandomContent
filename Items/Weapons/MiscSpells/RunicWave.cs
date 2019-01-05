using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSpells
{
    public class RunicWave : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Runic Wave");
            Tooltip.SetDefault("Cast a wave that draws runes in fligt" + "\nRight click to switch runes");

        }
        public override void SetDefaults()
        {
            item.damage = 100;
            item.magic = true;

            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = 5;
            item.knockBack = 100;
            item.value = 500000;
            item.rare = 9;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.width = 28;
            item.height = 30;

            item.mana = 12;
            item.shoot = mod.ProjectileType("RunicWavep");
            item.shootSpeed = 9;
            item.noMelee = true;




        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("AncientWave"));
            recipe.AddIngredient(mod.ItemType("CraftingRune"), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        int runeMode = 0;
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useStyle = 5;
                item.noUseGraphic = true;
                item.mana = 0;
                item.useTime = 10;
                item.useAnimation = 10;
                item.autoReuse = false;
                runeMode++;
                if (runeMode >= 4)
                {
                    runeMode = 0;
                }
                item.shoot = 0;
            }
            else
            {
                item.useStyle = 5;
                item.noUseGraphic = false;
                item.mana = 12;
                item.useTime = 40;
                item.useAnimation = 40;
                item.autoReuse = true;
                item.shoot = mod.ProjectileType("RunicWaveP");
            }
            return base.CanUseItem(player);
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = mod.GetTexture("Items/Weapons/MiscSpells/RunicWave_Glow");
            if (runeMode != 0)
            {
                spriteBatch.Draw
                (
                    texture,
                    new Vector2
                    (

                        position.X + item.width * 0.5f,
                        position.Y + item.height - (texture.Height / 4) * 0.5f
                    ),
                    new Rectangle(0, runeMode * (texture.Height / 4), texture.Width, (texture.Height / 4)),
                    Color.White,
                    0,
                    new Vector2(texture.Width / 2, (texture.Height / 8)) + origin,
                    scale,
                    SpriteEffects.None,
                    0f
                );
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/Weapons/MiscSpells/RunicWave_Glow");
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - (texture.Height / 4) * 0.5f + 2f
                ),
                new Rectangle(0, runeMode * (texture.Height / 4), texture.Width, (texture.Height / 4)),
                Color.White,
                rotation,
                new Vector2(texture.Width / 2, (texture.Height / 8)),
                scale,
                SpriteEffects.None,
                0f
            );
        }
        Projectile wave;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {


            wave = Main.projectile[Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI)];
            wave.ai[1] = runeMode;


            return false;
        }

    }
    public class RunicWaveP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Runic Wave");
            Main.projFrames[projectile.type] = 4;

        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 48;
            projectile.height = 48;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 60 * 3;



        }
        public int dustTimer;
        public int timer;
        bool runOnce = true;
        float iceRuneSpeed = 10;
        Projectile ice1;
        Projectile ice2;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.frame = (int)projectile.ai[1];
            dustTimer++;
            timer++;
            if (projectile.ai[1] == 0)
            {
                if (timer > 10)
                {
                    Projectile.NewProjectile(projectile.position.X + Main.rand.Next(projectile.width), projectile.position.Y + Main.rand.Next(projectile.height), 0, 0, mod.ProjectileType("AggroRuneTome"), 3 * projectile.damage, projectile.knockBack, projectile.owner);
                    timer = 0;
                }
                if (dustTimer > 5)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("AggroRuneLash"), 0, 0, 0, default(Color), .2f);
                    dustTimer = 0;
                }
            }
            if (projectile.ai[1] == 1)
            {
                if(runOnce)
                {
                    float startDistance = 100;
                    
                    ice1 = Main.projectile[Projectile.NewProjectile(projectile.Center.X + (float)Math.Cos(0) * startDistance, projectile.Center.Y + (float)Math.Sin(0) * startDistance, 0, 0, mod.ProjectileType("IceRuneTome"), projectile.damage, 3f, Main.myPlayer)];
                    ice2 = Main.projectile[Projectile.NewProjectile(player.Center.X + (float)Math.Cos(Math.PI) * startDistance, player.Center.Y + (float)Math.Sin(Math.PI) * startDistance, 0, 0, mod.ProjectileType("IceRuneTome"), (int)(300 * player.meleeDamage), 3f, Main.myPlayer)];
                    runOnce = false;
                }
                ice1.rotation += (float)((2 * Math.PI) / (Math.PI * 2 * 100 / iceRuneSpeed));
                ice1.velocity.X = iceRuneSpeed * (float)Math.Cos(ice1.rotation) + projectile.velocity.X;
                ice1.velocity.Y = iceRuneSpeed * (float)Math.Sin(ice1.rotation) + projectile.velocity.Y;
                
                ice2.rotation += (float)((2 * Math.PI) / (Math.PI * 2 * 100 / iceRuneSpeed));
                ice2.velocity.X = iceRuneSpeed * (float)Math.Cos(ice2.rotation) + projectile.velocity.X;
                ice2.velocity.Y = iceRuneSpeed * (float)Math.Sin(ice2.rotation) + projectile.velocity.Y;
                
                if (dustTimer > 5)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("IceRuneDeath"), 0, 0, 0, default(Color), .2f);
                    dustTimer = 0;
                }
            }
            if (projectile.ai[1] == 2)
            {
                if (timer > 10)
                {
                    Projectile.NewProjectile(projectile.position.X + Main.rand.Next(projectile.width), projectile.position.Y + Main.rand.Next(projectile.height), 0, 0, mod.ProjectileType("LeechRuneTome"),  projectile.damage/4, projectile.knockBack, projectile.owner);
                    timer = 0;
                }
                if (dustTimer > 5)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("LeechRuneDeath"), 0, 0, 0, default(Color), .2f);
                    dustTimer = 0;
                }
            }
            if (projectile.ai[1] == 3)
            {
                if (timer > 10)
                {
                    Projectile.NewProjectile(projectile.position.X + Main.rand.Next(projectile.width), projectile.position.Y + Main.rand.Next(projectile.height), -projectile.velocity.X, -projectile.velocity.Y, mod.ProjectileType("PursuitRuneTome"), projectile.damage / 2, projectile.knockBack, projectile.owner);
                    timer = 0;
                }
                if (dustTimer > 5)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("PursuitRuneDeath"), 0, 0, 0, default(Color), .2f);
                    dustTimer = 0;
                }
            }


        }


    }
    class IceRuneTome : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.tileCollide = false;
            projectile.timeLeft = 60 * 3;
            projectile.melee = true;


        }
        public int runeTimer;
        public float startDistance = 200f;
        public float direction;
        public float runeSpeed = 10;
        public bool runOnce = true;
        public float aim;

        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            if (runOnce)
            {
                projectile.rotation = (player.Center - projectile.Center).ToRotation() - (float)Math.PI / 2;
                runOnce = false;
            }



           



        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 1200);
        }

    }
    class PursuitRuneTome : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 20;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.alpha = 0;
            projectile.tileCollide = true;
            projectile.timeLeft = 720;
            projectile.magic = true;


        }
        public int runeTimer;
        public NPC target;

        public float runeSpeed = 10;
        public float runeDirection;
        public float runeTargetDirection;
        public bool runOnce = true;
        public int f;
        bool foundTarget;
        float maxDistance =1000;
        float distance;
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                projectile.rotation = (projectile.velocity).ToRotation();
                runOnce = false;
            }
            if (projectile.alpha > 0)
                projectile.alpha -= 5;
            else
                projectile.alpha = 0;

            for (int k = 0; k < 200; k++)
            {
                distance = (Main.npc[k].Center - projectile.Center).Length();
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && !Main.npc[k].immortal && Main.npc[k].lifeMax > 5)
                {

                    target = Main.npc[k];
                    foundTarget = true;
                    maxDistance = (target.Center - projectile.Center).Length();



                }
                

            }
            if (foundTarget)
            {
                projectile.rotation = QwertyMethods.SlowRotation(projectile.rotation, (target.Center - projectile.Center).ToRotation(), 1);
            }
            
            projectile.velocity = new Vector2((float)(Math.Cos(projectile.rotation) * runeSpeed), (float)(Math.Sin(projectile.rotation) * runeSpeed));

            foundTarget = false;
            maxDistance = 1000;


        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 1200);
        }
        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 100; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("PursuitRuneDeath"));
            }
        }

    }
    class LeechRuneTome : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;

            projectile.tileCollide = true;
            projectile.timeLeft = 60;
            projectile.magic = true;
            projectile.alpha = 255;

        }
        public int runeTimer;
        public float startDistance = 200f;
        public float direction;
        public float runeSpeed = 10;
        public bool runOnce = true;
        public float aim;
        NPC possibleTarget;
        NPC target;
        float distance;
        float maxDistance = 1000;
        bool foundTarget = false;
        public override void AI()
        {

            if (projectile.alpha > 0)
                projectile.alpha -= 25;
            else
                projectile.alpha = 0;
            if(projectile.timeLeft ==30)
            {
                for (int k = 0; k < 200; k++)
                {
                    possibleTarget = Main.npc[k];
                    distance = (possibleTarget.Center - projectile.Center).Length();
                    if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0))
                    {
                        target = Main.npc[k];
                        foundTarget = true;


                        maxDistance = (target.Center - projectile.Center).Length();
                    }

                }
                if(foundTarget)
                {
                    projectile.velocity = new Vector2((float)Math.Cos((target.Center - projectile.Center).ToRotation()) * runeSpeed, (float)Math.Sin((target.Center - projectile.Center).ToRotation()) * runeSpeed);
                }
            }
            if (projectile.timeLeft <= 30 && foundTarget)
            {
                projectile.rotation += MathHelper.ToRadians(3);
            }


        }
        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 100; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("LeechRuneDeath"));
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.immortal && !target.SpawnedFromStatue && Main.rand.Next(0, 3) == 0)
            {
                Player player = Main.player[projectile.owner];
                player.statLife += damage / 10;
                CombatText.NewText(player.getRect(), Color.Green, damage / 10, false, false);
            }

        }

    }
    class AggroRuneTome : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 62;
            projectile.height = 62;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.tileCollide = false;
            projectile.timeLeft = 30;
            projectile.magic = true;
            projectile.usesLocalNPCImmunity = true;

        }
        public int runeTimer;
        public float startDistance = 200f;
        public float direction;
        public float runeSpeed = 10;
        public bool runOnce = true;
        public float aim;

        public override void AI()
        {

            if (projectile.alpha > 0)
                projectile.alpha-=25;
            else
                projectile.alpha = 0;

            if (projectile.timeLeft <= 2)
            {
                projectile.alpha = 255;

                projectile.friendly = true;
                for (int d = 0; d <= 100; d++)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("AggroRuneLash"));
                }
            }




        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }


    }

}

