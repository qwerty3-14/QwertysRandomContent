using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.B4Items
{
    public class B4Bow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Possesing Bow");
            Tooltip.SetDefault("Arrows fired from this will chase your enemies!");
        }
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicOLORD ? base.Texture + "_Old" : base.Texture;
        public override void SetDefaults()
        {
            item.damage = 233;
            item.ranged = true;

            item.useTime = 11;
            item.useAnimation = 33;
            item.reuseDelay = 33;
            item.useStyle = 5;
            item.knockBack = 2f;
            item.value = 750000;
            item.rare = 10;
            item.UseSound = SoundID.Item5;

            item.width = 32;
            item.height = 62;

            item.shoot = 40;
            item.useAmmo = 40;
            item.shootSpeed = 12f;
            item.noMelee = true;
            item.autoReuse = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }

        public override bool ConsumeAmmo(Player player)
        {
            return !(player.itemAnimation < item.useAnimation - 2);
        }

        public Projectile arrow;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 trueSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
            arrow = Main.projectile[Projectile.NewProjectile(position.X, position.Y, trueSpeed.X, trueSpeed.Y, type, damage, knockBack, player.whoAmI)];
            arrow.GetGlobalProjectile<arrowHoming>().B4HomingArrow = true;
            if (Main.netMode == 1)
            {
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)ModMessageType.ArrowMessage); // Message type, you would need to create an enum for this
                packet.Write(arrow.identity); // tells which projectile is being modified by the effect, the effect is then applied on the receiving end
                packet.Write((byte)player.whoAmI); // the player that shot the projectile, will be useful later
                packet.Send();
            }
            return false;
        }
    }

    public class arrowHoming : GlobalProjectile
    {
        public bool B4HomingArrow;

        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            if (B4HomingArrow)
            {
                projectile.netUpdate = true;
                NPC prey = null;
                if (QwertyMethods.ClosestNPC(ref prey, 1000, projectile.Center))
                {
                    float direction = projectile.velocity.ToRotation();
                    direction.SlowRotation((prey.Center - projectile.Center).ToRotation(), MathHelper.ToRadians(4));
                    projectile.velocity = QwertyMethods.PolarVector(projectile.velocity.Length(), direction);
                }
            }
        }
    }
}