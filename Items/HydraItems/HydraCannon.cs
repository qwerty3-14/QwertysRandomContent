using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
    public class HydraCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Cannon");
            Tooltip.SetDefault("Killing enemies releases a powerful wave of destruction");
        }

        public override void SetDefaults()
        {
            item.damage = 28;
            item.ranged = true;

            item.useAnimation = 12;
            item.useTime = 6;
            item.reuseDelay = 14;

            item.useStyle = 5;
            item.knockBack = 5;
            item.value = 250000;
            item.rare = 5;
            item.UseSound = SoundID.Item11;

            item.width = 54;
            item.height = 64;

            item.shoot = 97;
            item.useAmmo = 97;
            item.shootSpeed = 36;
            item.noMelee = true;
            item.autoReuse = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(8, 4);
        }

        public override void HoldItem(Player player)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            modPlayer.HydraCannon = true;
        }

        public override bool ConsumeAmmo(Player player)
        {
            // We can get the Clockwork Assault Riffle Effect by not consuming ammo when itemAnimation is lower than the first shot.
            return !(player.itemAnimation < item.useAnimation - 2);
        }
    }

    public class DoomBreath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doom Breath");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 96;
            projectile.height = 52;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ranged = true;
            projectile.tileCollide = false;
            projectile.light = 1f;
        }

        private int frameCounter;

        public override void AI()
        {
            frameCounter++;
            if (frameCounter > 20)
            {
                frameCounter = 0;
            }
            else if (frameCounter > 10)
            {
                projectile.frame = 1;
            }
            else
            {
                projectile.frame = 0;
            }
            CreateDust();
        }

        public virtual void CreateDust()
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBreathGlow"));
        }
    }
}