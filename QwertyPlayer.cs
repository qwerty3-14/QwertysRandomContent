using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent
{
    public class QwertyPlayer : ModPlayer
    {
        public bool ninjaSabatoge = false;
        public bool minionIchor = false;
        public bool siphon = false;
        public bool meleeSiphon = false;
        public bool circletSetBonus = false;
        public bool meleeCircletSetBonus = false;
        public bool RhuthiniumMagic = false;
        public bool RhuthiniumMight = false;

        public bool DinoPox = false;
        public bool HydraSetBonus = false;
        public bool HydraCannon = false;
        public bool HydraCannonConfirm = false;
        public bool Metronome = false;
        public int killCount = 0;
        public bool runOnce = true;
        public bool usingVulcan = false;
        public bool TheAbstract = false;
        public bool hydraCharm = false;
        public Item heldItemOld = new Item();
        public int RhuthiniumCharge = 0;
        public bool minionFang = false;
        public bool gemRegen = false;
        public int regenTimer = 301;
        public bool iceScroll = false;
        public bool pursuitScroll = false;
        public int iceScrollCounter = 10;
        public bool leechScroll = false;
        public bool aggroScroll = false;
        public int charge;
        public bool stormEnchantment = false;
        public int shotNumber = 0;
        public float customDashSpeed = 0;
        public float customDashBonusSpeed = 0;
        public int customDashRam = 0;
        public int hyperRuneTimer = 0;
        public bool hyperRune = false;
        public bool noRegen = false;

        public bool cantUse = false;
        public bool blessedMedalion = false;
        public Projectile signalRune;
        public float rangedVelocity = 1f;
        public bool grappleBoost = false;
        public float ammoReduction = 1f;
        public float throwReduction = 1f;
        public int recovery;
        public int dodgeChance = 0;
        public bool dodgeImmuneBoost = false;
        public bool dodgeDamageBoost = false;
        public bool damageBoostFromDodge = false;
        public bool Lightling = false;
        public int forcedAntiGravity = 0;
        public int ArmorFrameCounter = 0;
        public float PincusionMultiplier = 1f;
        public float TopFrictionMultiplier = 1f;
        public float FlechetteDropAcceleration = 1f;
        public float GrenadeExplosionModifier = 1f;
        public int deflectCooldown = 0;

        public override void ResetEffects()
        {
            ninjaSabatoge = false;
            minionIchor = false;
            siphon = false;
            meleeSiphon = false;
            circletSetBonus = false;
            meleeCircletSetBonus = false;
            RhuthiniumMagic = false;
            RhuthiniumMight = false;

            DinoPox = false;
            HydraSetBonus = false;
            HydraCannon = false;
            HydraCannonConfirm = false;
            Metronome = false;
            hydraCharm = false;
            minionFang = false;
            gemRegen = false;
            iceScroll = false;
            pursuitScroll = false;
            leechScroll = false;
            aggroScroll = false;
            stormEnchantment = false;
            customDashSpeed = 0;
            customDashRam = 0;
            customDashBonusSpeed = 0;
            hyperRune = false;
            noRegen = false;

            cantUse = false;
            blessedMedalion = false;
            rangedVelocity = 1f;
            grappleBoost = false;
            ammoReduction = 1f;
            throwReduction = 1f;
            recovery = 0;
            dodgeChance = 0;
            dodgeImmuneBoost = false;
            dodgeDamageBoost = false;
            Lightling = false;
            if (!player.channel)
            {
                shotNumber = 0;
            }
            PincusionMultiplier = 1f;
            TopFrictionMultiplier = 1f;
            FlechetteDropAcceleration = 1f;
            GrenadeExplosionModifier = 1f;
        }

        public override void UpdateDead()
        {
            RhuthiniumMagic = false;
            RhuthiniumMight = false;
            usingVulcan = false;
            RhuthiniumCharge = 0;
            iceScroll = false;
            pursuitScroll = false;
            leechScroll = false;
            aggroScroll = false;
            shotNumber = 0;
        }

        public override void UpdateBadLifeRegen()
        {
            if (DinoPox)//Dino Pox
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }
                player.lifeRegenTime = 0;
                player.lifeRegen -= 20;
            }
            if (noRegen)
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }
                player.lifeRegenTime = 0;
            }
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            damage += (int)(damage * .02f * killCount);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += (int)(damage * .02f * killCount);
        }

        public override void ModifyDrawHeadLayers(List<PlayerHeadLayer> layers)
        {
            base.ModifyDrawHeadLayers(layers);
        }

        public void PickRandomAmmo(Item sItem, ref int shoot, ref float speed, ref bool canShoot, ref int Damage, ref float KnockBack, bool dontConsume = false)
        {
            Item item = new Item();
            List<int> possibleAmmo = new List<int>();

            for (int i = 0; i < 58; i++)
            {
                if (player.inventory[i].ammo == sItem.useAmmo && player.inventory[i].stack > 0)
                {
                    //item = player.inventory[i];

                    possibleAmmo.Add(i);

                    canShoot = true;
                }
            }

            if (canShoot)
            {
                item = player.inventory[possibleAmmo[Main.rand.Next(possibleAmmo.Count)]];
                speed += item.shootSpeed;
                if (item.ranged)
                {
                    if (item.damage > 0)
                    {
                        Damage += (int)((float)item.damage * player.rangedDamage);
                    }
                }
                else
                {
                    Damage += item.damage;
                }
                if (sItem.useAmmo == AmmoID.Arrow && player.archery)
                {
                    if (speed < 20f)
                    {
                        speed *= 1.2f;
                        if (speed > 20f)
                        {
                            speed = 20f;
                        }
                    }
                    Damage = (int)((double)((float)Damage) * 1.2);
                }
                KnockBack += item.knockBack;
                shoot = item.shoot;
                if (!dontConsume && item.maxStack > 1)
                {
                    item.stack--;
                }
                ItemLoader.PickAmmo(sItem, item, player, ref shoot, ref speed, ref Damage, ref KnockBack);
                bool flag2 = dontConsume;

                if (player.magicQuiver && sItem.useAmmo == AmmoID.Arrow && Main.rand.Next(5) == 0)
                {
                    flag2 = true;
                }
                if (player.ammoBox && Main.rand.Next(5) == 0)
                {
                    flag2 = true;
                }
                if (player.ammoPotion && Main.rand.Next(5) == 0)
                {
                    flag2 = true;
                }

                if (player.ammoCost80 && Main.rand.Next(5) == 0)
                {
                    flag2 = true;
                }
                if (player.ammoCost75 && Main.rand.Next(4) == 0)
                {
                    flag2 = true;
                }
                if (Main.rand.NextFloat() > ammoReduction)
                {
                    flag2 = true;
                }
                if (shoot == 85 && player.itemAnimation < player.itemAnimationMax - 6)
                {
                    flag2 = true;
                }

                if (!PlayerHooks.ConsumeAmmo(player, sItem, item))
                {
                    flag2 = true;
                }
                if (!ItemLoader.ConsumeAmmo(sItem, item, player))
                {
                    flag2 = true;
                }
            }
        }

        public override void PreUpdate()
        {
            deflectCooldown--;
            if (deflectCooldown == 1)
            {
                Main.PlaySound(25, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
                for (int num71 = 0; num71 < 5; num71++)
                {
                    int num72 = Dust.NewDust(player.position, player.width, player.height, 45, 0f, 0f, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
                    Main.dust[num72].noLight = true;
                    Main.dust[num72].noGravity = true;
                    Main.dust[num72].velocity *= 0.5f;
                }
            }
            ArmorFrameCounter++;
            if (grappleBoost)
            {
                //Main.NewText("double??");
            }
            //WorldGen.PlaceTile((int)player.Center.X/16, (int)player.Center.Y/16 - 4, TileID.Dirt);
            if (hyperRune)
            {
                if (hyperRuneTimer == 120)
                {
                    signalRune = Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("SignalRune"), 0, 0, player.whoAmI)];
                    signalRune.timeLeft = 2;
                    //CombatText.NewText(player.getRect(), new Color(39, 219, 219), "Recharged!", true, false);
                }
                if (hyperRuneTimer > 120)
                {
                    signalRune.timeLeft = 2;
                }
            }

            if (gemRegen)
            {
                regenTimer++;
                if (regenTimer == 300)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);

                        Dust dust = Dust.NewDustPerfect(player.Center + QwertyMethods.PolarVector(200, theta), mod.DustType("AncientGlow"), QwertyMethods.PolarVector(-200 / 10, theta));
                        dust.noGravity = true;
                    }
                    //CombatText.NewText(player.getRect(), Color.Green, "Reconstructed", true, false);
                    player.statLife += 999;
                }
            }

            if (usingVulcan)
            {
                player.accRunSpeed = 0f;
                player.moveSpeed = 0f;
                player.rangedDamage += 10;
            }

            if (Main.mouseRight)
            {
                while (RhuthiniumCharge > 0)
                {
                    float angle = (Main.MouseWorld - player.Center).ToRotation() + MathHelper.ToRadians(Main.rand.Next(-100, 101) * .05f);

                    Projectile.NewProjectile(player.Center.X, player.Center.Y, (float)Math.Cos(angle) * 12f, (float)Math.Sin(angle) * 12f, mod.ProjectileType("RhuthiniumCharge"), 20, 2f, player.whoAmI);
                    RhuthiniumCharge--;
                }
            }

            if (iceScroll)
            {
                if (iceScrollCounter >= (int)(2 * Math.PI * 10))
                {
                    float startDistance = 100;
                    Projectile.NewProjectile(player.Center.X + (float)Math.Cos(0) * startDistance, player.Center.Y + (float)Math.Sin(0) * startDistance, 0, 0, mod.ProjectileType("IceRuneFreindly"), (int)(300 * player.meleeDamage), 3f, Main.myPlayer);
                    Projectile.NewProjectile(player.Center.X + (float)Math.Cos(Math.PI) * startDistance, player.Center.Y + (float)Math.Sin(Math.PI) * startDistance, 0, 0, mod.ProjectileType("IceRuneFreindly"), (int)(300 * player.meleeDamage), 3f, Main.myPlayer);
                    iceScrollCounter = 0;
                }
                iceScrollCounter++;
            }

            if (heldItemOld != player.inventory[player.selectedItem])
            {
                if (Metronome && killCount != 0)
                {
                    CombatText.NewText(player.getRect(), Color.DarkRed, "Reset!", true, false);
                }
                killCount = 0;
            }
            heldItemOld = player.inventory[player.selectedItem];
        }

        public override void PostUpdateEquips()
        {
            if (forcedAntiGravity > 0)
            {
                player.gravDir = -1f;
                player.gravControl2 = true;
                forcedAntiGravity--;

                if (forcedAntiGravity == 1)
                {
                    player.velocity.Y = 0;
                }
            }
            if (forcedAntiGravity < 0)
            {
                forcedAntiGravity = 0;
            }
            if (damageBoostFromDodge)
            {
                if (player.immuneTime > 0)
                {
                    player.allDamage += .35f;
                }
                else
                {
                    damageBoostFromDodge = false;
                }
            }
            if (player.grappling[0] == -1 && !player.tongued)
            {
                customDashSpeedMovement();
            }
            if (player.pulley)
            {
                customDashSpeedMovement();
            }
        }

        public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
        {
            if (liquidType == 0 && (Main.rand.Next(4) == 0 || fishingRod.type == mod.ItemType("RhuthiniumRod")) && caughtType == ItemID.WoodenCrate && NPC.downedBoss3)
            {
                caughtType = mod.ItemType("RhuthiniumCrate");
            }
            if (liquidType == 0 && caughtType == ItemID.FloatingIslandFishingCrate && player.GetModPlayer<FortressBiome>().TheFortress)
            {
                caughtType = mod.ItemType("FortressCrate");
            }
            if (liquidType == 0 && caughtType == ItemID.Bass && Main.rand.Next(2) == 0 && player.GetModPlayer<FortressBiome>().TheFortress)
            {
                caughtType = mod.ItemType("EnchantedSwimmer");
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (blessedMedalion && damage > player.statLife && Main.rand.Next(100) < 30)
            {
                player.NinjaDodge();
                return false;
            }
            int dodgeRng = Main.rand.Next(100);
            //Main.NewText(dodgeChance);
            if (dodgeRng < dodgeChance && dodgeRng < 80)
            {
                player.immune = true;
                player.immuneTime = 80;
                if (player.longInvince)
                {
                    player.immuneTime += 40;
                }
                if (dodgeDamageBoost)
                {
                    damageBoostFromDodge = true;
                }
                if (dodgeImmuneBoost)
                {
                    player.immuneTime += 60;
                }
                for (int i = 0; i < player.hurtCooldowns.Length; i++)
                {
                    player.hurtCooldowns[i] = player.immuneTime;
                }
                for (int j = 0; j < 100; j++)
                {
                    int num = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 31, 0f, 0f, 100, default(Color), 2f);
                    Dust expr_A4_cp_0 = Main.dust[num];
                    expr_A4_cp_0.position.X = expr_A4_cp_0.position.X + (float)Main.rand.Next(-20, 21);
                    Dust expr_CB_cp_0 = Main.dust[num];
                    expr_CB_cp_0.position.Y = expr_CB_cp_0.position.Y + (float)Main.rand.Next(-20, 21);
                    Main.dust[num].velocity *= 0.4f;
                    Main.dust[num].scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                    Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(player.cWaist, player);
                    if (Main.rand.Next(2) == 0)
                    {
                        Main.dust[num].scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                        Main.dust[num].noGravity = true;
                    }
                }
                int num2 = Gore.NewGore(new Vector2(player.position.X + (float)(player.width / 2) - 24f, player.position.Y + (float)(player.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num2].scale = 1.5f;
                Main.gore[num2].velocity.X = (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity.Y = (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity *= 0.4f;
                num2 = Gore.NewGore(new Vector2(player.position.X + (float)(player.width / 2) - 24f, player.position.Y + (float)(player.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num2].scale = 1.5f;
                Main.gore[num2].velocity.X = 1.5f + (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity.Y = 1.5f + (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity *= 0.4f;
                num2 = Gore.NewGore(new Vector2(player.position.X + (float)(player.width / 2) - 24f, player.position.Y + (float)(player.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num2].scale = 1.5f;
                Main.gore[num2].velocity.X = -1.5f - (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity.Y = 1.5f + (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity *= 0.4f;
                num2 = Gore.NewGore(new Vector2(player.position.X + (float)(player.width / 2) - 24f, player.position.Y + (float)(player.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num2].scale = 1.5f;
                Main.gore[num2].velocity.X = 1.5f + (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity.Y = -1.5f - (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity *= 0.4f;
                num2 = Gore.NewGore(new Vector2(player.position.X + (float)(player.width / 2) - 24f, player.position.Y + (float)(player.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num2].scale = 1.5f;
                Main.gore[num2].velocity.X = -1.5f - (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity.Y = -1.5f - (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity *= 0.4f;
                if (player.whoAmI == Main.myPlayer)
                {
                    NetMessage.SendData(62, -1, -1, null, player.whoAmI, 1f, 0f, 0f, 0, 0, 0);
                }
                return false;
            }

            if (damageSource.SourceProjectileType == mod.ProjectileType("SnowFlake"))
            {
                damageSource = PlayerDeathReason.ByCustomReason(player.name + " was driven to extintion by climate change!"); // change death message
            }
            if (damageSource.SourceProjectileType == mod.ProjectileType("DinoBomb") || damageSource.SourceProjectileType == mod.ProjectileType("DinoBombExplosion"))
            {
                damageSource = PlayerDeathReason.ByCustomReason(player.name + " was driven to extintion by dino bomb"); // change death message
            }
            if (damageSource.SourceProjectileType == mod.ProjectileType("TankCannonBall") || damageSource.SourceProjectileType == mod.ProjectileType("TankCannonBallExplosion"))
            {
                damageSource = PlayerDeathReason.ByCustomReason(player.name + " was driven to extintion by triceratank's cannon"); // change death message
            }
            if (damageSource.SourceProjectileType == mod.ProjectileType("MeteorFall"))
            {
                damageSource = PlayerDeathReason.ByCustomReason(player.name + " was driven to extintion by meteor!"); // change death message
            }
            if (damageSource.SourceProjectileType == mod.ProjectileType("MeteorLaunch"))
            {
                damageSource = PlayerDeathReason.ByCustomReason(player.name + " was driven to extintion by meteor!"); // change death message
            }
            if (damageSource.SourceNPCIndex >= 0 && Main.npc[damageSource.SourceNPCIndex].type == mod.NPCType("TheGreatTyrannosaurus"))
            {
                damageSource = PlayerDeathReason.ByCustomReason(player.name + " was driven to extintion by The Great Tyrannosaurus!");
            }
            if (damageSource.SourceNPCIndex >= 0 && Main.npc[damageSource.SourceNPCIndex].type == mod.NPCType("Triceratank"))
            {
                damageSource = PlayerDeathReason.ByCustomReason(player.name + " was driven to extintion by Triceratank");
            }
            if (damageSource.SourceNPCIndex >= 0 && Main.npc[damageSource.SourceNPCIndex].type == mod.NPCType("Utah"))
            {
                damageSource = PlayerDeathReason.ByCustomReason(player.name + " was driven to extintion by Utah");
            }
            if (damageSource.SourceNPCIndex >= 0 && Main.npc[damageSource.SourceNPCIndex].type == mod.NPCType("Velocichopper"))
            {
                damageSource = PlayerDeathReason.ByCustomReason(player.name + " was driven to extintion by Velocichopper");
            }

            if (Metronome && killCount != 0)
            {
                CombatText.NewText(player.getRect(), Color.DarkRed, "Reset!", true, false);
            }
            if (gemRegen)
            {
                regenTimer = 0;
            }
            killCount = 0;
            return true;
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (player.HasBuff(mod.BuffType("DinoPox")))
            {
                damageSource = PlayerDeathReason.ByCustomReason(player.name + " was driven to extintion by diseaese!");
            }

            return true;
        }

        public int runeRate;

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (target.life <= 0 && !target.SpawnedFromStatue && stormEnchantment)
            {
                if (modPlayer.charge >= 20)
                {
                    modPlayer.charge = 20;
                    CombatText.NewText(target.getRect(), Color.Cyan, "MAX!", true, false);
                }
                else
                {
                    modPlayer.charge++;
                    CombatText.NewText(target.getRect(), Color.Cyan, modPlayer.charge, true, false);
                }
            }

            if (aggroScroll && !target.immortal && target.life <= 0 && !target.SpawnedFromStatue && proj.magic && proj.type != mod.ProjectileType("AggroRuneFreindly"))
            {
                Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("AggroRuneFreindly"), (int)(420 * player.magicDamage), 3f, Main.myPlayer);
            }
            if (leechScroll && proj.ranged && proj.type != mod.ProjectileType("LeechRuneFreindly"))
            {
                runeRate = damage;

                while (runeRate > 200)
                {
                    float theta = MathHelper.ToRadians(Main.rand.Next(0, 360));
                    Projectile.NewProjectile(target.Center.X + (float)Math.Cos(theta) * 150, target.Center.Y + (float)Math.Sin(theta) * 150, -(float)Math.Cos(theta) * 10, -(float)Math.Sin(theta) * 10, mod.ProjectileType("LeechRuneFreindly"), (int)(25 * player.rangedDamage), 3f, Main.myPlayer);
                    runeRate -= 200;
                }
                if (runeRate >= Main.rand.Next(0, 199))
                {
                    float theta = MathHelper.ToRadians(Main.rand.Next(0, 360));
                    Projectile.NewProjectile(target.Center.X + (float)Math.Cos(theta) * 150, target.Center.Y + (float)Math.Sin(theta) * 150, -(float)Math.Cos(theta) * 10, -(float)Math.Sin(theta) * 10, mod.ProjectileType("LeechRuneFreindly"), (int)(25 * player.rangedDamage), 3f, Main.myPlayer);
                }
            }

            if (Metronome && !target.immortal && target.life <= 0 && !target.SpawnedFromStatue)
            {
                killCount++;
                if (killCount >= 100)
                {
                    killCount = 100;
                    CombatText.NewText(player.getRect(), Color.Purple, "Max!", true, false);
                }
                else
                {
                    CombatText.NewText(player.getRect(), Color.Purple, killCount, false, false);
                }
            }
            if (HydraCannon && !target.immortal && target.life <= 0 && proj.ranged == true && proj.type != mod.ProjectileType("DoomBreath") && !target.SpawnedFromStatue)
            {
                Main.PlaySound(SoundID.Roar, player.position, 0);

                Projectile.NewProjectile(player.Center.X, player.Center.Y, target.Center.X - player.Center.X, target.Center.Y - player.Center.Y, mod.ProjectileType("DoomBreath"), damage * 5, knockback * 3, player.whoAmI);

                Main.rand.NextFloat(player.width);
            }
            if (ninjaSabatoge)
            {
                if (proj.thrown == true)
                {
                    target.AddBuff(20, 999999);
                    target.AddBuff(31, 999999);
                }
            }
            if (minionIchor)
            {
                if (proj.minion)
                {
                    target.AddBuff(69, 120);
                }
            }
            if (siphon)
            {
                if (!target.immortal && !target.SpawnedFromStatue)
                {
                    if (proj.melee == true)
                    {
                        player.statMana += (damage / 2);
                        CombatText.NewText(player.getRect(), Color.Blue, damage / 2, false, false);
                    }
                }
            }

            if (circletSetBonus)
            {
                if (!target.immortal && !target.SpawnedFromStatue)
                {
                    if (proj.melee == true)
                    {
                        player.AddBuff(mod.BuffType("RhuthiniumMagic"), 600);
                    }
                    if (proj.magic == true)
                    {
                        player.AddBuff(mod.BuffType("RhuthiniumMight"), 600);
                    }
                }
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (Metronome && !target.immortal && target.life <= 0 && !target.SpawnedFromStatue)
            {
                killCount++;
                if (killCount >= 100)
                {
                    killCount = 100;
                    CombatText.NewText(player.getRect(), Color.Purple, "Max!", true, false);
                }
                else
                {
                    CombatText.NewText(player.getRect(), Color.Purple, killCount, false, false);
                }
            }
            if (meleeSiphon)
            {
                if (!target.immortal && !target.SpawnedFromStatue)
                {
                    if (item.melee == true)
                    {
                        player.statMana += (damage);
                        CombatText.NewText(player.getRect(), Color.Blue, damage / 2, false, false);
                    }
                }
            }

            if (meleeCircletSetBonus)
            {
                if (!target.immortal && !target.SpawnedFromStatue)
                {
                    if (item.melee == true)
                    {
                        player.AddBuff(mod.BuffType("RhuthiniumMagic"), 600);
                    }
                    if (item.magic == true)
                    {
                        player.AddBuff(mod.BuffType("RhuthiniumMight"), 600);
                    }
                }
            }
        }

        public void customDashSpeedMovement()
        {
            if (hyperRune)
            {
                hyperRuneTimer++;
            }
            if (customDashRam > 0 && player.eocDash > 0)
            {
                if (player.eocHit < 0)
                {
                    Rectangle rectangle = new Rectangle((int)((double)player.position.X + (double)player.velocity.X * 0.5 - 4.0), (int)((double)player.position.Y + (double)player.velocity.Y * 0.5 - 4.0), player.width + 8, player.height + 8);
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].active && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly)
                        {
                            NPC nPC = Main.npc[i];
                            Rectangle rect = nPC.getRect();
                            if (rectangle.Intersects(rect) && (nPC.noTileCollide || player.CanHit(nPC) && nPC.type != mod.NPCType("Hydra")))
                            {
                                float num = customDashRam * player.meleeDamage;
                                float num2 = 9f;
                                bool crit = false;
                                if (player.kbGlove)
                                {
                                    num2 *= 2f;
                                }
                                if (player.kbBuff)
                                {
                                    num2 *= 1.5f;
                                }
                                if (Main.rand.Next(100) < player.meleeCrit)
                                {
                                    crit = true;
                                }
                                int num3 = player.direction;
                                if (player.velocity.X < 0f)
                                {
                                    num3 = -1;
                                }
                                if (player.velocity.X > 0f)
                                {
                                    num3 = 1;
                                }
                                if (player.whoAmI == Main.myPlayer)
                                {
                                    player.ApplyDamageToNPC(nPC, (int)num, num2, num3, crit);
                                }
                                player.eocDash = 10;
                                player.dashDelay = 30;
                                player.velocity.X = -(float)num3 * 9f;
                                player.velocity.Y = -4f;
                                player.immune = true;
                                player.immuneNoBlink = true;
                                player.immuneTime = 4;
                                player.eocHit = i;
                            }
                        }
                    }
                }
            }

            if (player.dash < 1)
            {
                if (player.dashDelay > 0)
                {
                    if (player.eocDash > 0)
                    {
                        player.eocDash--;
                    }
                    if (player.eocDash == 0)
                    {
                        player.eocHit = -1;
                    }
                    player.dashDelay--;
                    return;
                }
                if (player.dashDelay < 0)
                {
                    if (hyperRune && hyperRuneTimer > 120)
                    {
                        player.immune = true;
                    }
                    float num7 = 12f;
                    float num8 = 0.992f;
                    float num9 = Math.Max(player.accRunSpeed, player.maxRunSpeed);
                    float num10 = 0.96f;
                    int num11 = 20;
                    if ((customDashSpeed > 0 || customDashBonusSpeed > 0) && player.dash < 1)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            int num12;
                            if (player.velocity.Y == 0f)
                            {
                                num12 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (float)player.height - 4f), player.width, 8, 31, 0f, 0f, 100, default(Color), 1.4f);
                            }
                            else
                            {
                                num12 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (float)(player.height / 2) - 8f), player.width, 16, 31, 0f, 0f, 100, default(Color), 1.4f);
                            }
                            Main.dust[num12].velocity *= 0.1f;
                            Main.dust[num12].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                            Main.dust[num12].shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, player);
                        }
                    }

                    if ((customDashSpeed > 0 || customDashBonusSpeed > 0) && player.dash < 1)
                    {
                        player.vortexStealthActive = false;
                        if (player.velocity.X > num7 || player.velocity.X < -num7)
                        {
                            player.velocity.X = player.velocity.X * num8;
                            return;
                        }
                        if (player.velocity.X > num9 || player.velocity.X < -num9)
                        {
                            player.velocity.X = player.velocity.X * num10;
                            return;
                        }
                        hyperRuneTimer = 0;
                        player.dashDelay = num11;
                        //player.immune = false;
                        if (player.velocity.X < 0f)
                        {
                            player.velocity.X = -num9;
                            return;
                        }
                        if (player.velocity.X > 0f)
                        {
                            player.velocity.X = num9;
                            return;
                        }
                    }
                }
                else if (player.dash < 1 && (customDashSpeed > 0 || customDashBonusSpeed > 0) && !player.mount.Active)
                {
                    if ((customDashSpeed > 0 || customDashBonusSpeed > 0))
                    {
                        int num16 = 0;
                        bool flag = false;
                        if (player.dashTime > 0)
                        {
                            player.dashTime--;
                        }
                        if (player.dashTime < 0)
                        {
                            player.dashTime++;
                        }
                        if (player.controlRight && player.releaseRight)
                        {
                            if (player.dashTime > 0)
                            {
                                num16 = 1;
                                flag = true;
                                player.dashTime = 0;
                            }
                            else
                            {
                                player.dashTime = 15;
                            }
                        }
                        else if (player.controlLeft && player.releaseLeft)
                        {
                            if (player.dashTime < 0)
                            {
                                num16 = -1;
                                flag = true;
                                player.dashTime = 0;
                            }
                            else
                            {
                                player.dashTime = -15;
                            }
                        }
                        if (flag)
                        {
                            player.velocity.X = (customDashSpeed + 10f + customDashBonusSpeed) * (float)num16;

                            Point point = (player.Center + new Vector2((float)(num16 * player.width / 2 + 2), player.gravDir * -(float)player.height / 2f + player.gravDir * 2f)).ToTileCoordinates();
                            Point point2 = (player.Center + new Vector2((float)(num16 * player.width / 2 + 2), 0f)).ToTileCoordinates();
                            if (WorldGen.SolidOrSlopedTile(point.X, point.Y) || WorldGen.SolidOrSlopedTile(point2.X, point2.Y))
                            {
                                player.velocity.X = player.velocity.X / 2f;
                            }

                            player.dashDelay = -1;
                            if (customDashRam > 0)
                            {
                                player.eocDash = 15;
                            }
                            for (int num17 = 0; num17 < 20; num17++)
                            {
                                int num18 = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 31, 0f, 0f, 100, default(Color), 2f);
                                Dust dust = Main.dust[num18];
                                dust.position.X = dust.position.X + (float)Main.rand.Next(-5, 6);
                                Dust dust2 = Main.dust[num18];
                                dust2.position.Y = dust2.position.Y + (float)Main.rand.Next(-5, 6);
                                Main.dust[num18].velocity *= 0.2f;
                                Main.dust[num18].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                                Main.dust[num18].shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, player);
                            }
                            int num19 = Gore.NewGore(new Vector2(player.position.X + (float)(player.width / 2) - 24f, player.position.Y + (float)(player.height / 2) - 34f), default(Vector2), Main.rand.Next(61, 64), 1f);
                            Main.gore[num19].velocity.X = (float)Main.rand.Next(-50, 51) * 0.01f;
                            Main.gore[num19].velocity.Y = (float)Main.rand.Next(-50, 51) * 0.01f;
                            Main.gore[num19].velocity *= 0.4f;
                            num19 = Gore.NewGore(new Vector2(player.position.X + (float)(player.width / 2) - 24f, player.position.Y + (float)(player.height / 2) - 14f), default(Vector2), Main.rand.Next(61, 64), 1f);
                            Main.gore[num19].velocity.X = (float)Main.rand.Next(-50, 51) * 0.01f;
                            Main.gore[num19].velocity.Y = (float)Main.rand.Next(-50, 51) * 0.01f;
                            Main.gore[num19].velocity *= 0.4f;
                            return;
                        }
                    }
                }
            }
        }
    }
}