using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.BladeBossItems
{
    public class SwordMinionStaff : ModItem
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicImperious ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Longsword Staff");
            Tooltip.SetDefault("Who needs a horde of minions when you have a giant longsword?");
        }

        public override void SetDefaults()
        {
            item.summon = true;
            item.mana = 4;
            item.damage = 19;
            item.rare = 7;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.knockBack = 2f;
            item.useStyle = 5;
            item.useAnimation = item.useTime = 8;
            item.shootSpeed = 24f;
            item.width = item.height = 44;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.shoot = mod.ProjectileType("SwordMinion");
            item.UseSound = SoundID.Item44;
            item.noMelee = true;
            item.autoReuse = true;
            item.buffType = mod.BuffType("SwordMinionBuff");
            item.buffTime = 3600;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float minionCount = 0;
            //Main.NewText(minionCount + ", " + player.maxMinions);
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && projectile.owner == player.whoAmI)
                {
                    minionCount += projectile.minionSlots;
                }
            }
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && projectile.type == type && projectile.owner == player.whoAmI)
                {
                    if (player.maxMinions - minionCount >= 1)
                    {
                        projectile.minionSlots++;
                    }
                    return false;
                }
            }
            player.AddBuff(mod.BuffType("SwordMinionBuff"), 3600); //Idk why but the item.buffType didn't work for this
            position = Main.MouseWorld;
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

    public class SwordMinion : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicImperious ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Longsword");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
        }

        public override void SetDefaults()
        {
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.width = projectile.height = 10;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
            projectile.tileCollide = false;
            projectile.aiStyle = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 2;
        }

        private float yetAnotherTrigCounter;
        private NPC target;
        private bool returningToPlayer = false;
        private float turnOffset = 3 * (float)Math.PI / 4;
        private int counter = 0;
        private float bladeLength = 10;
        float? toward = null;
        public override void AI()
        {
            bool spinAttack = false;
            bladeLength = 24 + 16 + 14 * projectile.minionSlots;
            counter++;
            if (counter % projectile.localNPCHitCooldown == 0)
            {
                turnOffset *= -1;
            }
            yetAnotherTrigCounter += (float)Math.PI / 120;
            Player player = Main.player[projectile.owner];
            if (player.GetModPlayer<MinionManager>().SwordMinion)
            {
                projectile.timeLeft = 2;
            }
            if ((player.Center - projectile.Center).Length() > 1000)
            {
                returningToPlayer = true;
            }
            if ((player.Center - projectile.Center).Length() < 300)
            {
                returningToPlayer = false;
            }
            Vector2 flyTo = player.Center + new Vector2(-50 * player.direction, -50 - 14 * projectile.minionSlots) + Vector2.UnitY * (float)Math.Sin(yetAnotherTrigCounter) * 20;
            float turnTo = (float)Math.PI / 2;
            float speed = 12f;
            if (returningToPlayer)
            {
                speed = (player.Center - projectile.Center).Length() / 30f;
            }
            if (QwertyMethods.ClosestNPC(ref target, 1000, projectile.Center, false, player.MinionAttackTargetNPC) && !returningToPlayer)
            {
                Vector2 difference2 = projectile.Center - target.Center;
                flyTo = target.Center + QwertyMethods.PolarVector(bladeLength / 2, difference2.ToRotation());
                toward = turnTo = (target.Center - projectile.Center).ToRotation();
                int nerabyEnemies = 0;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && npc.chaseable && !npc.dontTakeDamage && !npc.friendly && npc.lifeMax > 5 && !npc.immortal && (npc.Center - projectile.Center).Length() < bladeLength)
                    {
                        nerabyEnemies++;
                    }
                }
                if (nerabyEnemies > 2)
                {
                    spinAttack = true;
                }
                if (difference2.Length() < bladeLength)
                {
                    turnTo += turnOffset;
                }
            }
            else
            {
                toward = null;
            }
            
            if (spinAttack)
            {
                projectile.rotation += ((float)Math.PI * 2) / projectile.localNPCHitCooldown;
            }
            else
            {
                if(toward == null)
                {
                    projectile.rotation.SlowRotation(turnTo, ((float)Math.PI * 2) / projectile.localNPCHitCooldown);
                }
                else
                {
                    projectile.rotation.SlowRotWhileAvoid(turnTo, ((float)Math.PI * 2) / projectile.localNPCHitCooldown, (float)toward + (float)Math.PI);
                }
            }
            Vector2 difference = flyTo - projectile.Center;
            if (difference.Length() < speed)
            {
                projectile.Center = flyTo;
                projectile.velocity = Vector2.Zero;
            }
            else
            {
                projectile.velocity = difference.SafeNormalize(Vector2.UnitY) * speed;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + QwertyMethods.PolarVector(bladeLength, projectile.rotation), 14f, ref point) || Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projHitbox.TopLeft(), projHitbox.Size());
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
            target.immune[projectile.owner] = 0;
        }
        
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition,
                       new Rectangle(0, 0, 31, 21), lightColor, projectile.rotation,
                       new Vector2(9f, 11f), projectile.scale, SpriteEffects.None, 0f);
            for (int b = 0; b < projectile.minionSlots; b++)
            {
                spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + QwertyMethods.PolarVector(22 + b * 14, projectile.rotation),
                       new Rectangle(34, 0, 14, 21), lightColor, projectile.rotation,
                       new Vector2(0, 11f), projectile.scale, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + QwertyMethods.PolarVector(22 + projectile.minionSlots * 14, projectile.rotation),
                       new Rectangle(50, 0, 16, 21), lightColor, projectile.rotation,
                       new Vector2(0, 11f), projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (int)projectile.minionSlots * damage;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            if (player.GetModPlayer<MinionManager>().SwordMinion)
            {
                Projectile p = Main.projectile[Projectile.NewProjectile(projectile.Center, projectile.velocity, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1])];
                p.minionSlots += player.maxMinions - player.slotsMinions - 1;
                p.rotation = projectile.rotation;
            }
        }
    }
}