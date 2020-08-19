using Microsoft.Xna.Framework;
using QwertysRandomContent.Items.Accesories;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSpells
{
    public class RingOfGuns : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ring of Guns");
            Tooltip.SetDefault("A ring of guns appears around your cursor shooting inwards");
        }

        public override void SetDefaults()
        {
            item.damage = 24;
            item.magic = true;

            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = 5;
            item.knockBack = 0;
            item.value = 500000;
            item.rare = 9;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.width = 28;
            item.height = 30;
            item.useAmmo = AmmoID.Bullet;
            item.mana = 12;
            item.shoot = mod.ProjectileType("RingGun");
            item.shootSpeed = 25;
            item.noMelee = true;
            item.channel = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CraftingRune"), 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool ConsumeAmmo(Player player)
        {
            return false;
        }

        private Projectile gun;
        private float radius = 125;

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            for (int n = 0; n < 6; n++)
            {
                gun = Main.projectile[Projectile.NewProjectile(Main.MouseWorld.X + (float)Math.Cos((float)(n * Math.PI / 3f)) * radius, Main.MouseWorld.Y + (float)Math.Sin((float)(n * Math.PI / 3f)) * radius, 0, 0, mod.ProjectileType("RingGun"), damage, knockBack, player.whoAmI)];
                gun.ai[1] = n;
            }

            return false;
        }
    }

    public class RingGun : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gun");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 42;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 60 * 15;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
        }

        private float radius = 125;
        private float rotateSpeed = (float)Math.PI / 100;
        private bool runOnce = true;
        private float angleInRing;
        private float trigTimer = 0;
        private float speed = 14f;
        private int reloadTimer;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.itemAnimation = 2;
            player.itemTime = 2;
            bool firing = player.channel && player.HasAmmo(QwertyMethods.MakeItemFromID(ItemID.FlintlockPistol), true) && !player.noItems && !player.CCed;
            int weaponDamage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
            int Ammo = 14;
            float speed = 14f;
            float weaponKnockback = player.inventory[player.selectedItem].knockBack;
            if (runOnce)
            {
                angleInRing = (float)(projectile.ai[1] * Math.PI / 3f);
                runOnce = false;
            }
            trigTimer += (float)Math.PI / 10;
            radius = 125 + 25 * (float)Math.Sin(trigTimer);
            angleInRing += rotateSpeed;
            projectile.rotation = angleInRing + (float)Math.PI;
            projectile.Center = new Vector2(Main.MouseWorld.X + (float)Math.Cos(angleInRing) * radius, Main.MouseWorld.Y + (float)Math.Sin(angleInRing) * radius);
            if (player.channel)
            {
                reloadTimer++;

                projectile.timeLeft = 2;
                if (reloadTimer % 15 == 0)
                {
                    if (projectile.UseAmmo(AmmoID.Bullet, ref Ammo, ref speed, ref weaponDamage, ref weaponKnockback, Main.rand.Next(2) == 0))
                    {
                        if (firing && player.CheckMana((int)((float)player.inventory[player.selectedItem].mana / 6f), !player.GetModPlayer<BloodMedalionEffect>().effect))
                        {
                            if (player.GetModPlayer<BloodMedalionEffect>().effect)
                            {
                                player.statLife -= (int)(player.inventory[player.selectedItem].mana / 6f * player.manaCost);
                                if (player.statLife <= 0)
                                {
                                    player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " madly drained " + (player.Male ? "his " : "her") + " lifeforce!"), (int)(player.inventory[player.selectedItem].mana * player.manaCost), 0);
                                }
                            }
                            player.manaRegenDelay = (int)player.maxRegenDelay;
                            Main.PlaySound(SoundID.Item11, player.position);

                            Projectile bul = Main.projectile[Projectile.NewProjectile(projectile.Center, new Vector2((float)Math.Cos(projectile.rotation) * speed, (float)Math.Sin(projectile.rotation) * speed), Ammo, projectile.damage, projectile.knockBack, player.whoAmI)];
                            bul.magic = true;
                            bul.ranged = false;
                            if (Main.netMode == 1)
                            {
                                QwertysRandomContent.UpdateProjectileClass(bul);
                            }
                        }
                    }
                }
            }
            else
            {
            }
        }
    }
}