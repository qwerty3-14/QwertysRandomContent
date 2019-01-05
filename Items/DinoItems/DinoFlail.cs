using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.DinoItems
{
	public class DinoFlail : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ankylosaurus Tail");
            Tooltip.SetDefault("Critical hits stun enemies");
        }
		public override void SetDefaults()
		{
			item.damage = 90;
			item.melee = true;
			item.noMelee = true;
			item.scale = 1f;
			item.noUseGraphic = true;
			item.width = 30;
			item.height = 32;
			item.useTime = 44;
			item.useAnimation = 44;
			item.useStyle = 5;
			item.knockBack = 3;
            item.rare = 6;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.channel = true;
			item.shoot = mod.ProjectileType("AnkylosaurusTail");
			item.shootSpeed = 15f;
		}
		

	}
    public class AnkylosaurusTail : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ankylosaurus Tail");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            projectile.width = 34;
            projectile.height = 34;
            //projectile.aiStyle = 15;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.timeLeft = 2400;
            projectile.penetrate = -1;
            //projectile.extraUpdates = 1;
        }
        public override void AI()
        {
            //copied from vanilla flail AI
            if (Main.player[projectile.owner].dead)
            {
                projectile.Kill();
                return;
            }
            Main.player[projectile.owner].itemAnimation = 10;
            Main.player[projectile.owner].itemTime = 10;
            if (projectile.position.X + (float)(projectile.width / 2) > Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2))
            {
                Main.player[projectile.owner].ChangeDir(1);
                projectile.direction = 1;
            }
            else
            {
                Main.player[projectile.owner].ChangeDir(-1);
                projectile.direction = -1;
            }
            Vector2 mountedCenter2 = Main.player[projectile.owner].MountedCenter;
            Vector2 vector18 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
            float num204 = mountedCenter2.X - vector18.X;
            float num205 = mountedCenter2.Y - vector18.Y;
            float num206 = (float)Math.Sqrt((double)(num204 * num204 + num205 * num205));
            if (projectile.ai[0] == 0f)
            {
                float num207 = 400f;
                
                projectile.tileCollide = true;
                if (num206 > num207)
                {
                    projectile.ai[0] = 1f;
                    projectile.netUpdate = true;
                }
                else if (!Main.player[projectile.owner].channel)
                {
                    if (projectile.velocity.Y < 0f)
                    {
                        projectile.velocity.Y = projectile.velocity.Y * 0.9f;
                    }
                    projectile.velocity.Y = projectile.velocity.Y + 1f;
                    projectile.velocity.X = projectile.velocity.X * 0.9f;
                }
            }
            else if (projectile.ai[0] == 1f)
            {
                float num208 = 14f / Main.player[projectile.owner].meleeSpeed;
                float num209 = 0.9f / Main.player[projectile.owner].meleeSpeed;
                float num210 = 600f;
                
                Math.Abs(num204);
                Math.Abs(num205);
                if (projectile.ai[1] == 1f)
                {
                    projectile.tileCollide = false;
                }
                if (!Main.player[projectile.owner].channel || num206 > num210 || !projectile.tileCollide)
                {
                    projectile.ai[1] = 1f;
                    if (projectile.tileCollide)
                    {
                        projectile.netUpdate = true;
                    }
                    projectile.tileCollide = false;
                    if (num206 < 20f)
                    {
                        projectile.Kill();
                    }
                }
                if (!projectile.tileCollide)
                {
                    num209 *= 2f;
                }
                int num211 = 60;
                if (projectile.type == 247)
                {
                    num211 = 100;
                }
                if (num206 > (float)num211 || !projectile.tileCollide)
                {
                    num206 = num208 / num206;
                    num204 *= num206;
                    num205 *= num206;
                    new Vector2(projectile.velocity.X, projectile.velocity.Y);
                    float num212 = num204 - projectile.velocity.X;
                    float num213 = num205 - projectile.velocity.Y;
                    float num214 = (float)Math.Sqrt((double)(num212 * num212 + num213 * num213));
                    num214 = num209 / num214;
                    num212 *= num214;
                    num213 *= num214;
                    projectile.velocity.X = projectile.velocity.X * 0.98f;
                    projectile.velocity.Y = projectile.velocity.Y * 0.98f;
                    projectile.velocity.X = projectile.velocity.X + num212;
                    projectile.velocity.Y = projectile.velocity.Y + num213;
                }
                else
                {
                    if (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) < 6f)
                    {
                        projectile.velocity.X = projectile.velocity.X * 0.96f;
                        projectile.velocity.Y = projectile.velocity.Y + 0.2f;
                    }
                    if (Main.player[projectile.owner].velocity.X == 0f)
                    {
                        projectile.velocity.X = projectile.velocity.X * 0.96f;
                    }
                }
            }
            
                projectile.rotation = (float)Math.Atan2((double)num205, (double)num204) - projectile.velocity.X * 0.1f;
                return;
            
            
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.boss && crit)
            {
                target.AddBuff(mod.BuffType("Stunned"), 240);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bool flag11 = false;
            if (oldVelocity.X != projectile.velocity.X)
            {
                if (Math.Abs(oldVelocity.X) > 4f)
                {
                    flag11 = true;
                }
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X * 0.2f;
            }
            if (oldVelocity.Y != projectile.velocity.Y)
            {
                if (Math.Abs(oldVelocity.Y) > 4f)
                {
                    flag11 = true;
                }
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y * 0.2f;
            }
            projectile.ai[0] = 1f;
            if (flag11)
            {
                projectile.netUpdate = true;
                Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
                Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0f);
            }
            if (projectile.wet)
            {
                oldVelocity = projectile.velocity;
            }
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            Vector2 playerCenter = Main.player[projectile.owner].MountedCenter;
            Vector2 center = projectile.Center;
            Vector2 distToProj = playerCenter - projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            for (int i = 0; i < 1000; i++)
            {
                if (distance > 4f && !float.IsNaN(distance))
                {
                    distToProj.Normalize();                 
                    distToProj *= 8f;
                    center += distToProj;                   
                    distToProj = playerCenter - center;    
                    distance = distToProj.Length();
                    Color drawColor = lightColor;

                    //Draw chain
                    spriteBatch.Draw(mod.GetTexture("Items/DinoItems/DinoFlailChain"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 18, 12), drawColor, projRotation,
                        new Vector2(18 * 0.5f, 12 * 0.5f), 1f, SpriteEffects.None, 0f);
                }
            }

            return true;
        }
    }
}

