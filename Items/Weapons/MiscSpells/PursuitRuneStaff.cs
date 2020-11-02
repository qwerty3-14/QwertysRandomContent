using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.NPCs.RuneGhost.RuneBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSpells
{
    public class PursuitRuneStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pursuit Rune Staff");
            Tooltip.SetDefault("Fires explosive Pursuit Runes!");
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }


        public override void SetDefaults()
        {
            item.damage = 160;
            item.magic = true;

            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 5;
            item.knockBack = 2;
            item.value = 500000;
            item.rare = 9;
            item.autoReuse = true;
            item.width = 72;
            item.height = 72;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Weapons/MiscSpells/PursuitRuneStaff_Glow");
            }
            item.mana = 7;
            item.shoot = mod.ProjectileType("PursuitRuneMissile");
            item.shootSpeed = 9;
            item.noMelee = true;
            //item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/AncientItems/AncientWave_Glow");
        }

        public override bool CanUseItem(Player player)
        {
            if (player.statMana > item.mana)
            {
                Main.PlaySound(25, player.position, 0);
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            
            return true;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/Weapons/MiscSpells/PursuitRuneStaff_Glow");
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
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("AncientMissileStaff"));
            recipe.AddIngredient(mod.ItemType("CraftingRune"), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    public class PursuitRuneMissile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.alpha = 0;
            projectile.tileCollide = true;
            projectile.timeLeft = 720;
            projectile.magic = true;
            projectile.usesLocalNPCImmunity = true;
        }

        public int runeTimer;
        public NPC target;

        public float runeSpeed = 10;
        public float runeDirection;
        public float runeTargetDirection;
        public bool runOnce = true;
        public int f;
        int timer = 0;
        public override void AI()
        {
            timer++;
            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                projectile.rotation = (projectile.velocity).ToRotation();
                runOnce = false;
            }
            if (QwertyMethods.ClosestNPC(ref target, 1000, projectile.Center))
            {
                projectile.rotation.SlowRotation((target.Center - projectile.Center).ToRotation(), MathHelper.ToRadians(1));
            }
            projectile.velocity = new Vector2((float)(Math.Cos(projectile.rotation) * runeSpeed), (float)(Math.Sin(projectile.rotation) * runeSpeed));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float c = (timer / 40f);
            if (c > 1f)
            {
                c = 1f;
            }
            int frame = timer / 2;
            if (frame > 19)
            {
                frame = 19;
            }
            spriteBatch.Draw(RuneSprites.runeTransition[(int)Runes.Pursuit][frame], projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), projectile.rotation, new Vector2(10, 5), Vector2.One * 2, 0, 0);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
            Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("PursuitRuneBlast"), projectile.damage, projectile.knockBack, projectile.owner, 1f)];
            e.localNPCImmunity[target.whoAmI] = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("PursuitRuneBlast"), projectile.damage, projectile.knockBack, projectile.owner);
            return true;
        }
    }
    public class PursuitRuneBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rune Blast");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 225;
            projectile.height = 225;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 2;
            projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.width = 225;
            projectile.height = 225;

            Main.PlaySound(SoundID.Item62, projectile.position);

            for (int i = 0; i < 600; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(projectile.Center, mod.DustType("PursuitRuneDeath"), QwertyMethods.PolarVector(Main.rand.Next(2, 30), theta));
                dust.noGravity = true;
            }
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
