
/*
namespace QwertysRandomContent.Experiments       ///We need item to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class GlassPrism : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glass Prism");
            Tooltip.SetDefault("Wants to be a last prism");
            

        }

        public override void SetDefaults()
        {

            item.useStyle = 5;
            item.useAnimation = 10;
            item.useTime = 10;
            item.reuseDelay = 5;
            item.shootSpeed = 30f;
            item.knockBack = 0f;
            item.width = 16;
            item.height = 16;
            item.damage = 100;
            item.UseSound = SoundID.Item13;
            item.shoot = mod.ProjectileType("GlassPrismP");
            item.mana = 12;
            item.rare = 10;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.noMelee = true;
            item.noUseGraphic = true;
            item.magic = true;
            item.channel = true;

        }
        
    }
    public class GlassPrismP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 18;
           // projectile.aiStyle = 75;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
        }
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            float num = 1.57079637f;
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
           
     
           
                float num26 = 30f;
                if (projectile.ai[0] > 90f)
                {
                    num26 = 15f;
                }
                if (projectile.ai[0] > 120f)
                {
                    num26 = 5f;
                }
                projectile.damage = (int)((float)player.inventory[player.selectedItem].damage * player.magicDamage);
                projectile.ai[0] += 1f;
                projectile.ai[1] += 1f;
                bool flag9 = false;
                if (projectile.ai[0] % num26 == 0f)
                {
                    flag9 = true;
                }
                int num27 = 10;
                bool flag10 = false;
                if (projectile.ai[0] % num26 == 0f)
                {
                    flag10 = true;
                }
                if (projectile.ai[1] >= 1f)
                {
                    projectile.ai[1] = 0f;
                    flag10 = true;
                    if (Main.myPlayer == projectile.owner)
                    {
                        float scaleFactor5 = player.inventory[player.selectedItem].shootSpeed * projectile.scale;
                        Vector2 value12 = vector;
                        Vector2 value13 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - value12;
                        if (player.gravDir == -1f)
                        {
                            value13.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - value12.Y;
                        }
                        Vector2 vector11 = Vector2.Normalize(value13);
                        if (float.IsNaN(vector11.X) || float.IsNaN(vector11.Y))
                        {
                            vector11 = -Vector2.UnitY;
                        }
                        vector11 = Vector2.Normalize(Vector2.Lerp(vector11, Vector2.Normalize(projectile.velocity), 0.92f));
                        vector11 *= scaleFactor5;
                        if (vector11.X != projectile.velocity.X || vector11.Y != projectile.velocity.Y)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.velocity = vector11;
                    }
                }
                projectile.frameCounter++;
                int num28 = (projectile.ai[0] < 120f) ? 4 : 1;
                if (projectile.frameCounter >= num28)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame >= 5)
                    {
                        projectile.frame = 0;
                    }
                }
                if (projectile.soundDelay <= 0)
                {
                    projectile.soundDelay = num27;
                    projectile.soundDelay *= 2;
                    if (projectile.ai[0] != 1f)
                    {
                        Main.PlaySound(SoundID.Item15, projectile.position);
                    }
                }
                if (flag10 && Main.myPlayer == projectile.owner)
                {
                    bool flag11 = !flag9 || player.CheckMana(player.inventory[player.selectedItem].mana, true, false);
                    bool flag12 = player.channel && flag11 && !player.noItems && !player.CCed;
                    if (flag12)
                    {
                        if (projectile.ai[0] == 1f)
                        {
                            Vector2 center3 = projectile.Center;
                            Vector2 vector12 = Vector2.Normalize(projectile.velocity);
                            if (float.IsNaN(vector12.X) || float.IsNaN(vector12.Y))
                            {
                                vector12 = -Vector2.UnitY;
                            }
                            int num29 = projectile.damage;
                            for (int l = 0; l < 6; l++)
                            {
                                Projectile.NewProjectile(center3.X, center3.Y, vector12.X, vector12.Y, mod.ProjectileType("GlassPrismBeam"), num29, projectile.knockBack, projectile.owner, (float)l, (float)projectile.whoAmI);
                            }
                            projectile.netUpdate = true;
                        }
                    }
                    else
                    {
                        projectile.Kill();
                    }
                }
            

            projectile.position = player.RotatedRelativePoint(player.MountedCenter, true) - projectile.Size / 2f;
            projectile.position.X -= 5;
            projectile.rotation = projectile.velocity.ToRotation() + num;
            projectile.spriteDirection = projectile.direction;
            projectile.timeLeft = 2;
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2((double)(projectile.velocity.Y * (float)projectile.direction), (double)(projectile.velocity.X * (float)projectile.direction));
            


        }
    }

    public class GlassPrismBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            //projectile.aiStyle = 84;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 5;
        }
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (float)projectile.width * projectile.scale, new Utils.PerLinePoint(DelegateMethods.CutTiles));
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float num6 = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], 22f * projectile.scale, ref num6))
            {
                return true;
            }
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.velocity == Vector2.Zero)
            {
                return false;
            }
            Texture2D tex = Main.projectileTexture[projectile.type];
            float num228 = projectile.localAI[1];
            
            
            
            Vector2 value26 = projectile.Center.Floor();
            value26 += projectile.velocity * projectile.scale * 10.5f;
            num228 -= projectile.scale * 14.5f * projectile.scale;
            Vector2 vector29 = new Vector2(projectile.scale);
            DelegateMethods.f_1 = 1f;
            DelegateMethods.c_1 = Color.White * 0.75f * projectile.Opacity;
            //projectile.oldPos[0] + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
            Utils.DrawLaser(Main.spriteBatch, tex, value26 - Main.screenPosition, value26 + projectile.velocity * num228 - Main.screenPosition, vector29, new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw));
            DelegateMethods.c_1 = new Microsoft.Xna.Framework.Color(255, 255, 255, 127) * 0.75f * projectile.Opacity;
            Utils.DrawLaser(Main.spriteBatch, tex, value26 - Main.screenPosition, value26 + projectile.velocity * num228 - Main.screenPosition, vector29 / 2f, new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw));
            return false;
        }
        int hi;
        public override void AI()
        {


            Vector2? vector69 = null;
            if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
            {
                projectile.velocity = -Vector2.UnitY;
            }
            if(!Main.player[projectile.owner].channel)
            {
                projectile.Kill();
            }

            float num790 = (float)((int)projectile.ai[0]) - 2.5f;
            Vector2 value35 = Vector2.Normalize(Main.projectile[(int)projectile.ai[1]].velocity);
            Projectile p = Main.projectile[(int)projectile.ai[1]];
            if(!p.active || p.type != mod.ProjectileType("GlassPrismP"))
            {
                projectile.Kill();
            }
            float num791 = num790 * 0.5235988f;
            Vector2 value36 = Vector2.Zero;
            float num792;
            float y;
            float num793;
            float scaleFactor6;
            if (p.ai[0] < 180f)
            {
                num792 = 1f - p.ai[0] / 180f;
                y = 20f - p.ai[0] / 180f * 14f;
                if (p.ai[0] < 120f)
                {
                    num793 = 20f - 4f * (p.ai[0] / 120f);
                    projectile.Opacity = p.ai[0] / 120f * 0.4f;
                }
                else
                {
                    num793 = 16f - 10f * ((p.ai[0] - 120f) / 60f);
                    projectile.Opacity = 0.4f + (p.ai[0] - 120f) / 60f * 0.6f;
                }
                scaleFactor6 = -22f + p.ai[0] / 180f * 20f;
            }
            else
            {
                num792 = 0f;
                num793 = 1.75f;
                y = 6f;
                projectile.Opacity = 1f;
                scaleFactor6 = -2f;
            }
            float num794 = (p.ai[0] + num790 * num793) / (num793 * 6f) * 6.28318548f;
            num791 = Vector2.UnitY.RotatedBy((double)num794, default(Vector2)).Y * 0.5235988f * num792;
            value36 = (Vector2.UnitY.RotatedBy((double)num794, default(Vector2)) * new Vector2(4f, y)).RotatedBy((double)p.velocity.ToRotation(), default(Vector2));
            projectile.position = p.Center + value35 * 16f - projectile.Size / 2f + new Vector2(0f, -Main.projectile[(int)projectile.ai[1]].gfxOffY);
            projectile.position += p.velocity.ToRotation().ToRotationVector2() * scaleFactor6;
            projectile.position += value36;
            projectile.velocity = Vector2.Normalize(p.velocity).RotatedBy((double)num791, default(Vector2));
            projectile.scale = 1.4f * (1f - num792);
            projectile.damage = p.damage;
            if (p.ai[0] >= 180f)
            {
                projectile.damage *= 3;
                vector69 = new Vector2?(p.Center);
            }
            if (!Collision.CanHitLine(Main.player[projectile.owner].Center, 0, 0, p.Center, 0, 0))
            {
                vector69 = new Vector2?(Main.player[projectile.owner].Center);
            }
            projectile.friendly = (p.ai[0] > 30f);

            if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
            {
                projectile.velocity = -Vector2.UnitY;
            }

            float num798 = projectile.velocity.ToRotation();

            projectile.rotation = num798 - 1.57079637f;
            projectile.velocity = num798.ToRotationVector2();
            float num799 = 0f;
            float num800 = 0f;
            Vector2 samplingPoint = projectile.Center;
            if (vector69.HasValue)
            {
                samplingPoint = vector69.Value;
            }


            num799 = 2f;
            num800 = 0f;


            float[] array3 = new float[(int)num799];
            Collision.LaserScan(samplingPoint, projectile.velocity, num800 * projectile.scale, 2400f, array3);
            float num801 = 0f;
            for (int num802 = 0; num802 < array3.Length; num802++)
            {
                num801 += array3[num802];
            }
            num801 /= num799;
            float amount = 0.5f;

            projectile.localAI[1] = MathHelper.Lerp(projectile.localAI[1], num801, amount);

            if (Math.Abs(projectile.localAI[1] - num801) < 100f && projectile.scale > 0.15f)
            {
                //float prismHue = projectile.GetPrismHue(projectile.ai[0]);
                //Color color = Main.hslToRgb(prismHue, 1f, 0.5f);
                Color color = Color.White;
                color.A = 0;
                Vector2 vector78 = projectile.Center + projectile.velocity * (projectile.localAI[1] - 14.5f * projectile.scale);
                float x = Main.rgbToHsl(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB)).X;
                for (int num823 = 0; num823 < 2; num823++)
                {
                    float num824 = projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.57079637f;
                    float num825 = (float)Main.rand.NextDouble() * 0.8f + 1f;
                    Vector2 vector79 = new Vector2((float)Math.Cos((double)num824) * num825, (float)Math.Sin((double)num824) * num825);
                    int num826 = Dust.NewDust(vector78, 0, 0, 267, vector79.X, vector79.Y, 0, default(Color), 1f);
                    Main.dust[num826].color = color;
                    Main.dust[num826].scale = 1.2f;
                    if (projectile.scale > 1f)
                    {
                        Main.dust[num826].velocity *= projectile.scale;
                        Main.dust[num826].scale *= projectile.scale;
                    }
                    Main.dust[num826].noGravity = true;
                    if (projectile.scale != 1.4f)
                    {
                        Dust dust9 = Dust.CloneDust(num826);
                        dust9.color = Color.White;
                        dust9.scale /= 2f;
                    }
                    float hue = (x + Main.rand.NextFloat() * 0.4f) % 1f;
                    //Main.dust[num826].color = Color.Lerp(color, Main.hslToRgb(hue, 1f, 0.75f), projectile.scale / 1.4f);
                }
                if (Main.rand.Next(5) == 0)
                {
                    Vector2 value41 = projectile.velocity.RotatedBy(1.5707963705062866, default(Vector2)) * ((float)Main.rand.NextDouble() - 0.5f) * (float)projectile.width;
                    int num827 = Dust.NewDust(vector78 + value41 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[num827].velocity *= 0.5f;
                    Main.dust[num827].velocity.Y = -Math.Abs(Main.dust[num827].velocity.Y);
                }
                DelegateMethods.v3_1 = color.ToVector3() * 0.3f;
                float value42 = 0.1f * (float)Math.Sin((double)(Main.GlobalTime * 20f));
                Vector2 size = new Vector2(projectile.velocity.Length() * projectile.localAI[1], (float)projectile.width * projectile.scale);
                float num828 = projectile.velocity.ToRotation();
                if (Main.netMode != 2)
                {
                    ((WaterShaderData)Filters.Scene["WaterDistortion"].GetShader()).QueueRipple(projectile.position + new Vector2(size.X * 0.5f, 0f).RotatedBy((double)num828, default(Vector2)), new Color(0.5f, 0.1f * (float)Math.Sign(value42) + 0.5f, 0f, 1f) * Math.Abs(value42), size, RippleShape.Square, num828);
                }
                Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (float)projectile.width * projectile.scale, new Utils.PerLinePoint(DelegateMethods.CastLight));
                return;
            }

        }

        
    }





}

    */