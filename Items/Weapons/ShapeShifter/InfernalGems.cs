namespace QwertysRandomContent.Items.Weapons.ShapeShifter
{
    /*
    public class InfernalGems : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(" (UNFINNISED)");
            Tooltip.SetDefault("Gain flight and the ability to shoot fire! /nThis form require constant feeding from either attacking your foes or your own life!");

        }
        public const int dmg = 43;
        public const int crt = 0;
        public const float kb = 1f;
        public const int def = 13;
        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 42;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.value = 200;
            item.rare = 1;
            item.UseSound = SoundID.Item79;
            item.noMelee = true;
            item.mountType = mod.MountType("Gems");
            item.damage = dmg;
            item.crit = crt;
            item.knockBack = kb;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;

        }
        public override bool CanUseItem(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().justStableMorphed();
            if (player.GetModPlayer<ShapeShifterPlayer>().EyeBlessing)
            {

                player.GetModPlayer<ShapeShifterPlayer>().EyeBlessing = false;
            }
            else
            {
                player.AddBuff(mod.BuffType("MorphSickness"), 180);
            }

            return base.CanUseItem(player);
        }
        
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    public class GemsB : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Infernal Gems");
            Description.SetDefault("");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType<Gems>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
    public class Gems : ModMountData
    {
        public override void SetDefaults()
        {

            mountData.buff = mod.BuffType("GemsB");
            mountData.spawnDust = 15;

            mountData.heightBoost = 0;
            mountData.flightTimeMax = 0;
            mountData.fallDamage = 0f;
            mountData.runSpeed = 0f;
            mountData.dashSpeed = 0f;
            mountData.acceleration = 0f;
            mountData.jumpHeight = 0;
            mountData.jumpSpeed = 0f;
            mountData.totalFrames = 1;
            mountData.constantJump = false;

            int[] array = new int[mountData.totalFrames];
            for (int l = 0; l < array.Length; l++)
            {
                array[l] = 0;
            }
            mountData.playerYOffsets = array;
            mountData.xOffset = 0;
            mountData.bodyFrame = 1;
            mountData.yOffset = 2;
            mountData.playerHeadOffset = 0;
            mountData.standingFrameCount = 1;
            mountData.standingFrameDelay = 10;
            mountData.standingFrameStart = 0;
            mountData.runningFrameCount = 1;
            mountData.runningFrameDelay = 10;
            mountData.runningFrameStart = 0;
            mountData.flyingFrameCount = 1;
            mountData.flyingFrameDelay = 0;
            mountData.flyingFrameStart = 0;
            mountData.inAirFrameCount = 1;
            mountData.inAirFrameDelay = 12;
            mountData.inAirFrameStart = 0;
            mountData.idleFrameCount = 1;
            mountData.idleFrameDelay = 12;
            mountData.idleFrameStart = 0;
            mountData.idleFrameLoop = true;
            mountData.swimFrameCount = mountData.inAirFrameCount;
            mountData.swimFrameDelay = mountData.inAirFrameDelay;
            mountData.swimFrameStart = mountData.inAirFrameStart;

            if (Main.netMode != 2)
            {

                mountData.textureWidth = mountData.backTexture.Width;
                mountData.textureHeight = mountData.backTexture.Height;
            }
        }
      
        public override void UpdateEffects(Player player)
        {
            player.GetModPlayer<GemControl>().controlled = true;
            
        }

    }
    
    public class GemControl : ModPlayer
    {
        float flySpeed = 5f;
        int shotCooldown = 0;
        public bool controlled = false;
        public override void ResetEffects()
        {
            controlled = false;
        }
        
        public override void PostUpdateMiscEffects()
        {
            if (controlled)
            {
                player.noFallDmg = true;
                player.gravity = 0;
                player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
                
                Mount mount = player.mount;
                player.GetModPlayer<ShapeShifterPlayer>().morphed = true;
                //player.GetModPlayer<ShapeShifterPlayer>().overrideWidth = 40;
                player.noItems = true;
                player.statDefense = InfernalGems.def + player.GetModPlayer<ShapeShifterPlayer>().morphDef;
                player.velocity = Vector2.Zero;
                if(player.controlUp)
                {
                    player.velocity.Y += -1;
                }
                if (player.controlDown)
                {
                    player.velocity.Y += 1;
                }
                if (player.controlLeft)
                {
                    player.velocity.X += -1;
                }
                if (player.controlRight)
                {
                    player.velocity.X += 1;
                }
                if (shotCooldown > 0)
                {
                    shotCooldown--;
                }
                if (player.whoAmI == Main.myPlayer && Main.mouseLeft && !player.HasBuff(mod.BuffType("MorphSickness")) && shotCooldown == 0)
                {
                    shotCooldown = 20;
                    Projectile p = Main.projectile[Projectile.NewProjectile(player.Center, QwertyMethods.PolarVector(12, (QwertysRandomContent.LocalCursor[player.whoAmI]-player.Center).ToRotation()), 34, (int)(InfernalGems.dmg * player.GetModPlayer<ShapeShifterPlayer>().morphDamage), InfernalGems.kb, player.whoAmI)];
                    p.magic = false;
                    p.GetGlobalProjectile<MorphProjectile>().morph = true;
                    p.penetrate = 1;
                    p.alpha = 0;
                    if (Main.netMode == 1)
                    {
                        QwertysRandomContent.UpdateProjectileClass(p);
                    }
                    Main.PlaySound(SoundID.Item20, player.Center);
                }
                if (player.velocity.Length() > 0)
                {
                    player.velocity = player.velocity.SafeNormalize(-Vector2.UnitY);
                    player.velocity *= flySpeed;
                }

            }
        }
    }
    */
}
