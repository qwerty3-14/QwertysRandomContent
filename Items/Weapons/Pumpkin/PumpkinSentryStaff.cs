using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.World.Generation;

namespace QwertysRandomContent.Items.Weapons.Pumpkin      ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class PumpkinSentryStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jack-o'-lantern Staff");
			Tooltip.SetDefault("Summons a stationary Jack-o'-lantern to burn nearbly enemies!");
			
		}
 
        public override void SetDefaults()
        {

            item.damage = 10;  //The damage stat for the Weapon.
            item.mana = 20;      //this defines how many mana this weapon use
            item.width = 38;    //The size of the width of the hitbox in pixels.
            item.height = 40;     //The size of the height of the hitbox in pixels.
            item.useTime = 25;   //How fast the Weapon is used.
            item.useAnimation = 25;    //How long the Weapon is used for.
            item.useStyle = 1;  //The way your Weapon will be used, 1 is the regular sword swing for example
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = .4f;  //The knockback stat of your Weapon.
            item.value = 1000;
			item.rare = 1;   
            item.UseSound = SoundID.Item44;   //The sound played when using your Weapon
            item.autoReuse = true;   //Weather your Weapon will be used again after use while holding down, if false you will need to click again after use to use it again.
            item.shoot = mod.ProjectileType("PumpkinSentry");   //This defines what type of projectile this weapon will shot
            item.summon = true;    //This defines if it does Summon damage and if its effected by Summon increasing Armor/Accessories.
            item.sentry = true; //tells the game that this is a sentry

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
            position.Y -= 16;
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Pumpkin, 30);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool UseItem(Player player)
		{
			if(player.altFunctionUse == 2)
			{
				player.MinionNPCTargetAim();
			}
			return base.UseItem(player);
		}
    }

    public class PumpkinSentry : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jack-o'-lantern");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
            Main.projFrames[projectile.type] = 3; 
        }

        public override void SetDefaults()
        {

            projectile.sentry = true;
            projectile.width = 60; //Set the hitbox width
            projectile.height = 36;   //Set the hitbox heinght
            projectile.hostile = false;    //tells the game if is hostile or not.
            projectile.friendly = true;   //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
            projectile.timeLeft = Projectile.SentryLifeTime;
            
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            projectile.tileCollide = true; //Tells the game whether or not it can collide with tiles/ terrain
            projectile.sentry = true; //tells the game that this is a sentry
            projectile.usesLocalNPCImmunity = true;
        }
        NPC target;
        NPC possibleTarget;
        bool foundTarget;
        float maxDistance = 10000f;
        float distance;
        float flameRange = 400f;
        int faceDirection;
        int frameTimer;
        float frameposition;
        bool attacking;
        float CP;
        int d;
        public override void AI()
        {
            flameRange = 400f;
            frameTimer++;
            
            Player player = Main.player[projectile.owner];
            player.UpdateMaxTurrets();
            if (player.MinionAttackTargetNPC != -1)
            {
                target = Main.npc[player.MinionAttackTargetNPC];
                foundTarget = true;

            }
            else
            {
                for (int k = 0; k < 200; k++)
                {
                    possibleTarget = Main.npc[k];
                    distance = Math.Abs(possibleTarget.Center.X - projectile.Center.X);
                    if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0) && Collision.CheckAABBvLineCollision(possibleTarget.position, possibleTarget.Size, new Vector2(projectile.Center.X-flameRange, projectile.Center.Y+projectile.height/4), new Vector2(projectile.Center.X + flameRange, projectile.Center.Y + projectile.height / 4)))
                    {
                        target = Main.npc[k];
                        foundTarget = true;


                        maxDistance = Math.Abs(target.Center.X - projectile.Center.X);
                    }

                }
            }
            if(frameTimer % 10 ==0)
            {
                if(frameposition < faceDirection)
                {
                    frameposition += .5f;
                }
                else if(frameposition > faceDirection)
                {
                    frameposition -= .5f;
                }
            }
            if(frameposition < 0)
            {
                d = -1;
            }
            else
            {
                d = 1;
            }
            if(Math.Abs(frameposition) == 1)
            {
                projectile.frame = 2;
            }
            else if(Math.Abs(frameposition) == .5f)
            {
                projectile.frame = 1;
            }
            else
            {
                projectile.frame = 0;
            }
            if(foundTarget)
            {
                if(target.Center.X < projectile.Center.X)
                {
                    faceDirection = -1;
                }
                else if (target.Center.X > projectile.Center.X)
                {
                    faceDirection = 1;
                }

                if(faceDirection == frameposition && faceDirection !=0)
                {
                    attacking = true;
                    Main.PlaySound(SoundID.Item34, projectile.position);
                    flameRange = 0;
                    for (int f =0; f <400; f++)
                    {
                        if(Collision.CanHit(projectile.Center, 0, 0, new Vector2(projectile.Center.X+ f* faceDirection, projectile.Center.Y), 0, 0))
                        {
                            flameRange++;
                        }
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(new Vector2(projectile.Center.X + (Main.rand.Next((int)flameRange-100)) * faceDirection , projectile.Center.Y + projectile.height/4), DustID.Fire, new Vector2(10 * faceDirection, 0).RotatedByRandom((float)Math.PI / 16));
                        dust.scale = 1.5f;
                        dust.noGravity = true;
                    }
                    
                }
                else
                {
                    attacking = false;
                }
            }
            else
            {
                faceDirection = 0;
                attacking = false;
            }
            maxDistance = 10000f;
            foundTarget = false;
            
            //Main.NewText(frameposition);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(mod.GetTexture("Items/Weapons/Pumpkin/PumpkinSentry"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), drawColor, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, d == 1 ? SpriteEffects.None: SpriteEffects.FlipHorizontally, 0f);
            
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if(attacking)
            {
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, new Vector2(projectile.Center.X + flameRange * d, projectile.Center.Y), projectile.height, ref CP);
            }
           
            return false;
        }
        
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
            projectile.localNPCImmunity[target.whoAmI] = 10;
            target.immune[projectile.owner] = 0;
        }

    }
}