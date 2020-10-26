using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.ShapeShifter
{
    public class EoCShift : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shift: Cthulhu's Stare");
            Tooltip.SetDefault("Breifly turns you into the eye of cthulhu charging toward where you point");
        }

        public const int dmg = 200;
        public const int crt = 0;
        public const float kb = 9f;
        public const int def = -1;

        public override void SetDefaults()
        {
            item.damage = dmg;
            item.crit = crt;
            item.knockBack = kb;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.QuickShiftType;
            item.GetGlobalItem<ShapeShifterItem>().morphCooldown = 30;
            item.noMelee = true;

            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = 5;

            item.value = 10000;
            item.rare = 1;

            item.noUseGraphic = true;
            item.width = 18;
            item.height = 32;

            //item.autoReuse = true;
            item.shoot = mod.ProjectileType("EoCMorph");
            item.shootSpeed = 13f;
            item.channel = true;
        }

        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if ((Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot) || player.HasBuff(mod.BuffType("MorphCooldown")))
                {
                    return false;
                }
            }
            //player.AddBuff(mod.BuffType("MorphCooldown"), (int)(item.GetGlobalItem<ShapeShifterItem>().morphCooldown * 60 * player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration * item.GetGlobalItem<ShapeShifterItem>().PrefixorphCooldownModifier));
            Main.PlaySound(SoundID.Roar, player.position, 0);

            return true;
        }
    }

    public class EoCMorph : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of Cthulu?");
            Main.projFrames[projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 110;
            projectile.height = 110;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            //projectile.tileCollide = false;
            projectile.timeLeft = 60;
            projectile.usesLocalNPCImmunity = true;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter % 7 == 0)
            {
                projectile.frame++;
                if (projectile.frame >= 3)
                {
                    projectile.frame = 0;
                }
            }
            Player player = Main.player[projectile.owner];
            player.Center = projectile.Center;
            player.immune = true;
            player.immuneTime = 2;
            player.statDefense = 0;
            player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
            player.itemTime = 2;
            player.itemAnimation = 2;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.player[projectile.owner].Center.X < target.Center.X)
            {
                hitDirection = 1;
            }
            else
            {
                hitDirection = -1;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle(0, Main.projectileTexture[projectile.type].Height / 3 * projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 3), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }

            return true;
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            for (int k = 0; k < 200; k++)
            {
                projectile.localNPCImmunity[k] = 0;
            }
            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.velocity.X = -velocityChange.X;
            }
            if (projectile.velocity.Y != velocityChange.Y)
            {
                projectile.velocity.Y = -velocityChange.Y;
            }
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }

    public class EoCBagDrop : GlobalItem
    {
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag" && arg == ItemID.EyeOfCthulhuBossBag && Main.rand.Next(4) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("EoCShift"));
            }
        }
    }

    public class EoCDrop : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.EyeofCthulhu && Main.rand.Next(5) == 0 && !Main.expertMode)
            {
                Item.NewItem(npc.Hitbox, mod.ItemType("EoCShift"));
            }
        }
    }
}