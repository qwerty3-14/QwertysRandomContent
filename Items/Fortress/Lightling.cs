using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress
{
	public class Lightling : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lightling");
			Tooltip.SetDefault("Emits light");
		}

        public override void SetDefaults()
        {
            item.damage = 0;
            item.useStyle = 1;
            item.shoot = mod.ProjectileType("LightlingP");
            item.width = 16;
            item.height = 30;
            item.UseSound = SoundID.Item2;
            item.useAnimation = 20;
            item.useTime = 20;
            item.rare = 8;
            item.noMelee = true;
            item.value = Item.sellPrice(0, 5, 50, 0);
            item.buffType = mod.BuffType("LightlingBuff");
        }
        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }





    }
    public class LightlingP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightling");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
            
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 20;
            projectile.penetrate = -1;
            projectile.netImportant = true;
            projectile.timeLeft *= 5;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            
            projectile.tileCollide = false;
            projectile.light = 1f;
        }


        int shader = 0;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            shader = player.miscDyes[1].dye;
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (!player.active)
            {
                projectile.active = false;
                return;
            }
            if (player.dead)
            {
                modPlayer.Lightling = false;
            }
            if (modPlayer.Lightling)
            {
                projectile.timeLeft = 2;
            }
            Vector2 flyTo = player.Center - projectile.Center;
            //float dir = flyTo.ToRotation();
            //flyTo = (player.Center +QwertyMethods.PolarVector(-200, dir)) - projectile.Center;
           // Main.NewText(flyTo.Length());
            if (flyTo.Length() < 120)
            {
                projectile.velocity = Vector2.Zero;
            }
            else
            {
                projectile.velocity = flyTo * .01f;
            }
            projectile.frameCounter++;
            if(projectile.frameCounter%10 ==0)
            {
                projectile.frame = 1;
            }
            else if(projectile.frameCounter % 5==0)
            {
                projectile.frame = 0;
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            // As mentioned above, be sure not to forget this step.
            Player player = Main.player[projectile.owner];
            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];

            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
            }
            return true;
        }
    }
    public class LightlingBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Lightling");
            Description.SetDefault("Emits light");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<QwertyPlayer>().Lightling = true;
            player.buffTime[buffIndex] = 18000;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("LightlingP")] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.Center.X, player.Center.Y , 0f, 0f, mod.ProjectileType("LightlingP"), 0, 0f, player.whoAmI, 0f, 0f);
            }
           
        }
    }
}
