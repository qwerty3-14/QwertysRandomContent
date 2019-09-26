using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
 
 
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

            item.damage = 24;  //The damage stat for the Weapon.
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
            item.autoReuse = true;   //Weather your Weapon will be used again after use while holding down, if false you will need to click again after use to use it again.
            item.shoot = mod.ProjectileType("RunicMinionFreindly");   //This defines what type of projectile this weapon will shot
            item.summon = true;    //This defines if it does Summon damage and if its effected by Summon increasing Armor/Accessories.
            item.buffType = mod.BuffType("AncientMinion");	//The buff added to player after used the item
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
			if(player.altFunctionUse == 2)
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


        public int timer;
        public int Pos = 1;
        public int damage = 30;
        public int switchTime = 300;
        public int moveCount = 0;
        public int fireCount = 0;
        public int attackType = 1;
        public bool charging;
        public NPC target;
        public NPC possibleTarget;
        int waitTime = 5;
        int chargeTime = 15;
        Vector2 moveTo;
        bool justTeleported;
        float chargeSpeed = 12;
        bool runOnce = true;
        bool foundTarget;
        float maxDistance = 1000f;
        float distance;
        float targetAngle;
        public override void AI()
        {
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 255 / 5;
            }
            Player player = Main.player[projectile.owner];
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            if (modPlayer.AncientMinion)
            {
                projectile.timeLeft = 2;
            }
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
                    distance = (possibleTarget.Center - player.Center).Length();
                    if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal)
                    {
                        target = Main.npc[k];
                        foundTarget = true;


                        maxDistance = (target.Center - player.Center).Length();
                    }

                }
            }
            maxDistance = 1000f;
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

            if (foundTarget)
            {
                timer++;
                if (timer > waitTime + chargeTime)
                {
                    for(int k =0; k<200; k++)
                    {
                        projectile.localNPCImmunity[k] = 0;
                    }
                    for (int i = 0; i < 100; i++)
                    {
                        Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("LeechRuneDeath"));

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
                }
                else if (timer > waitTime)
                {
                    charging = true;
                }
                else
                {
                    charging = false;
                }
                if (charging)
                {
                    projectile.rotation += MathHelper.ToRadians(3);
                    projectile.velocity = new Vector2((float)Math.Cos(targetAngle) * chargeSpeed, (float)Math.Sin(targetAngle) * chargeSpeed);
                }
                else
                {
                    projectile.Center = new Vector2(moveTo.X, moveTo.Y);
                    projectile.velocity = new Vector2(0, 0);
                    targetAngle = new Vector2(target.Center.X - projectile.Center.X, target.Center.Y - projectile.Center.Y).ToRotation();
                    //projectile.rotation = targetAngle;
                }



                
            }
            else
            {
                if ( (projectile.Center-player.Center).Length()> 300)
                {
                    if (Main.netMode != 2)
                    {



                        projectile.ai[1] = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                        projectile.netUpdate = true;
                    }
                    for (int d = 0; d < 300; d++)
                    {
                        Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("LeechRuneDeath"));
                    }
                    moveTo = new Vector2(player.Center.X + (float)Math.Cos(projectile.ai[1]) * 100, player.Center.Y + (float)Math.Sin(projectile.ai[1]) * 100);
                    justTeleported = true;
                }
                
                projectile.Center = moveTo;

                targetAngle = new Vector2(player.Center.X - projectile.Center.X, player.Center.Y - projectile.Center.Y).ToRotation();
                //projectile.rotation = targetAngle;
            }
            if (justTeleported)
            {
                projectile.alpha = 255;
                Main.PlaySound(SoundID.Item8, projectile.position);
                for (int d = 0; d < 300; d++)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("LeechRuneDeath"));
                }
                justTeleported = false;
            }
            foundTarget = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.immortal && !target.SpawnedFromStatue && Main.rand.Next(0, 3) == 0)
            {
                Player player = Main.player[projectile.owner];
                player.statLife += damage / 10;
                CombatText.NewText(player.getRect(), Color.Green, damage / 10, false, false);
            }
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }
        

    }
	
}