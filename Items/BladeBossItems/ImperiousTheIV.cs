using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.BladeBossItems
{
    public class ImperiousTheIV : ModItem
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicImperious ? base.Texture + "_Old" : base.Texture;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Imperious The IV");
            Tooltip.SetDefault("Hitting enemies launches richoching swords");

        }
        public override void SetDefaults()
        {
            item.damage = 64;
            item.melee = true;

            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = 1;
            item.knockBack = 3;
            item.value = Item.sellPrice(gold: 10);
            item.rare = 7;
            item.UseSound = SoundID.Item1;

            item.width = 40;
            item.height = 40;
            item.crit = 20;
            item.autoReuse = true;
            //item.scale = 5;



        }



        public override void MeleeEffects(Player player, Rectangle hitbox)
        {

        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (player.whoAmI == Main.myPlayer && !target.immortal && player.ownedProjectileCounts[mod.ProjectileType("ImperiousTheV")] < 40)
            {
                Projectile.NewProjectile(target.Center, Vector2.Zero, mod.ProjectileType("ImperiousTheV"), (int)(item.damage*player.meleeDamage), item.knockBack, player.whoAmI, target.whoAmI);
            }
        }
    }

    public class ImperiousTheV : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicImperious ? base.Texture + "_Old" : base.Texture;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Imperious The V");


        }
        public override void SetDefaults()
        {


            projectile.width = 22;
            projectile.height = 22;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            Main.projFrames[projectile.type] = 1;
            projectile.knockBack = 10f;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.melee = true;
            //projectile.minionSlots = 1;
            projectile.timeLeft = 120;
            projectile.aiStyle = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.extraUpdates = 1;

        }
        NPC target = null;
        bool runOnce = true;
        public override void AI()
        {
            if(runOnce)
            {
                projectile.localNPCImmunity[(int)projectile.ai[0]] = -1;
                runOnce = false;
            }
            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI/2;
            if (QwertyMethods.ClosestNPC(ref target, 400, projectile.Center, true, specialCondition: delegate (NPC possibleTarget) { return projectile.localNPCImmunity[possibleTarget.whoAmI] == 0; }))
            {
                projectile.velocity = (target.Center - projectile.Center).SafeNormalize(-Vector2.UnitY) * 10f;
            }
            else
            {
                projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }



    }


}

