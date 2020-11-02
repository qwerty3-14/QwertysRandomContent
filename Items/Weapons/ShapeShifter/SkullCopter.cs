using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.ShapeShifter
{
    public class SkullCopter : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shift: Skull Copter");

        }
        public override void SetDefaults()
        {

            item.damage = 90;
            item.knockBack = 1;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = 26;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.value = Item.sellPrice(gold: 7);
            item.rare = 5;
            item.UseSound = SoundID.Item79;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("SkullCopterMorph");
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(mod.BuffType("SkullCopterB"), 2);
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override bool UseItem(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().justStableMorphed();
            return base.UseItem(player);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofFright, 20);
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
    public class SkullCopterB : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Skull Copter");
            Description.SetDefault("BWHAHAHAHAHA");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.GetModPlayer<ShapeShifterPlayer>().delayThing <= 0)
            {
                player.buffTime[buffIndex] = 2;
            }
        }
    }
    public class SkullCopterMorph : StableMorph
    {
        public override void SetSafeDefaults()
        {
            projectile.width = 106;
            projectile.height = 120;
            projectile.timeLeft = 2;
            buffName = "SkullCopterB";
            itemName = "SkullCopter";
            Main.projFrames[projectile.type] = 4;
        }

        private float flySpeed = 6.2f;
        private int shotCooldown = 20;

        public override void Effects(Player player)
        {

        }
        int ascentSpeed = 30;
        int angel = 0;
        int ascentRange = 60;
        int angelRange = 15;
        public override void Movement(Player player)
        {
            
            projectile.frameCounter++;
            projectile.frame = projectile.frameCounter % 8 < 4 ? 0 : 1;
            player.gravity = 0f;
            player.accRunSpeed = 0;

            Vector2 LocalCursor = QwertysRandomContent.GetLocalCursor(player.whoAmI);
            
            projectile.velocity = Vector2.Zero;
            if (player.controlUp && ascentSpeed < ascentRange)
            {
                ascentSpeed++;
            }
            if (player.controlDown && ascentSpeed >0)
            {
                ascentSpeed--;
            }
            if (player.controlLeft && angel > -angelRange)
            {
                angel--;
            }
            if (player.controlRight && angel < angelRange)
            {
                angel++;
            }
            if (shotCooldown > 0 )
            {
                shotCooldown--;
                if(shotCooldown > 10)
                {
                    projectile.frame += 2;
                }
            }
            projectile.rotation = ((float)angel / angelRange) * (float)Math.PI / 3;
            projectile.velocity = QwertyMethods.PolarVector(((float)ascentSpeed / ascentRange) * 10f, projectile.rotation - (float)Math.PI/2);
            projectile.velocity.Y += (float)Math.Sqrt((5f*5f)/2);
            player.direction = projectile.spriteDirection = Math.Sign(projectile.rotation);
            if (player.whoAmI == Main.myPlayer && Main.mouseLeft && !player.HasBuff(mod.BuffType("MorphSickness")) && shotCooldown == 0)
            {
                shotCooldown = 20;
                for(int i = 0; i < 2 + Main.rand.Next(2); i++)
                {
                    Projectile.NewProjectile(projectile.Center + QwertyMethods.PolarVector(24*player.direction, projectile.rotation) + QwertyMethods.PolarVector(38, projectile.rotation + (float)Math.PI / 2), QwertyMethods.PolarVector((Main.rand.NextFloat(3f)+7f)* player.direction, projectile.rotation + Main.rand.NextFloat(-(float)Math.PI / 8f, (float)Math.PI / 8f)), mod.ProjectileType("BoomBone"), projectile.damage, projectile.knockBack, projectile.owner, projectile.rotation);
                }
                
            }
           
        }
    }
    public class BoomBone : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boom Bone");
        }
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 24;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 600;
            projectile.usesLocalNPCImmunity = true;
        }
        public override void AI()
        {
            projectile.rotation += .1f * projectile.ai[0];
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
            Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("BoneBlast"), projectile.damage, projectile.knockBack, projectile.owner)];
            e.localNPCImmunity[target.whoAmI] = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("BoneBlast"), projectile.damage, projectile.knockBack, projectile.owner)];
            return true;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];

            Main.PlaySound(SoundID.Item62, projectile.position);
            for (int i = 0; i < 5; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(projectile.Center, 31, QwertyMethods.PolarVector(Main.rand.NextFloat() * 2f, theta));
                dust.noGravity = true;
            }
            // Fire Dust spawn
            for (int i = 0; i < 10; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dustIndex = Dust.NewDustPerfect(projectile.Center, 6, QwertyMethods.PolarVector(Main.rand.NextFloat() * 2f, theta));
                dustIndex.noGravity = true;
                theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                dustIndex = Dust.NewDustPerfect(projectile.Center, 6, QwertyMethods.PolarVector(Main.rand.NextFloat() * 2f, theta), Scale: 2f);
                dustIndex.noGravity = true;
            }
        }
    }
    public class BoneBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boom Bone");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 80;
            projectile.height = 80;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 2;
            projectile.usesLocalNPCImmunity = true;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
    }

}
