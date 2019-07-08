using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.TankCommander
{
    [AutoloadEquip(EquipType.Head)]
    public class TankCommanderHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tank Commander Helmet");
            Tooltip.SetDefault("+1 max minions \n When morphed you'll occasionally get air support!");
        }


        public override void SetDefaults()
        {

            item.value = 100000;
            item.rare = 1;


            item.width = 22;
            item.height = 14;
            item.defense = 5;
            item.GetGlobalItem<ShapeShifterItem>().equipedMorphDefense = 8;


        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("TankCommanderJacket") && legs.type == mod.ItemType("TankCommanderPants");

        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.TankSet");
            player.GetModPlayer<TankComHelmEffects>().setBonus = true;
        }
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;

        }
        public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
            player.GetModPlayer<TankComHelmEffects>().effect = true;
        }







    }
    public class TankComHelmEffects : ModPlayer
    {
        public bool effect;
        public bool setBonus;
        int bomberDelay = 300;
        int[] selected = new int[3];
        int bombingCounter = 0;
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            
            int maxAgeToStun = 300;
            if(proj.GetGlobalProjectile<MorphProjectile>().morph && setBonus && target.GetGlobalNPC<QwertyGloabalNPC>().age < maxAgeToStun)
            {
                damage = (int)(damage * 1.2f);
                target.AddBuff(mod.BuffType("Stunned"), maxAgeToStun - target.GetGlobalNPC<QwertyGloabalNPC>().age);
            }
            if(setBonus && proj.minion && target.HasBuff(mod.BuffType("Stunned")))
            {
                damage = (int)(damage * 1.3f);
            }
            
        }
        public override void PreUpdate()
        {

            if (effect && player.GetModPlayer<ShapeShifterPlayer>().morphed)
            {
                bomberDelay--;
                if (bomberDelay <= 0)
                {

                    if(bomberDelay % 30 ==0)
                    {
                        Deck<int> targets = new Deck<int>();
                        for (int n = 0; n < 200; n++)
                        {
                            if (Main.npc[n].active && !Main.npc[n].friendly && !Main.npc[n].dontTakeDamage && !Main.npc[n].immortal && (player.Center - Main.npc[n].Center).Length() < 1000)
                            {
                                targets.Add(n);
                            }
                        }
                        if (targets.Count > 0)
                        {
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/BombDrop"));
                            float rotation = (float)Math.PI / 2;

                            Projectile.NewProjectile(new Vector2(Main.npc[targets[Main.rand.Next(targets.Count)]].Center.X, player.Center.Y) + QwertyMethods.PolarVector(-500, rotation), QwertyMethods.PolarVector(12, rotation), mod.ProjectileType("MiniBomb"), (int)(140 * player.minionDamage), 0f, player.whoAmI);

                        }
                    }
                    if(bomberDelay < -60)
                    {
                        bomberDelay = 260 + Main.rand.Next(120);
                    }
                    
                   




                }
                

            }
        }
        public override void ResetEffects()
        {
            effect = false;
            setBonus = false;
        }
    }
    public class MiniBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Bomb");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.minion = true;

        }
        public bool runOnce = true;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
            Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MiniBombBlast"), projectile.damage, projectile.knockBack, projectile.owner)];
            e.localNPCImmunity[target.whoAmI] = -1;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MiniBombBlast"), projectile.damage, projectile.knockBack, projectile.owner)];
            return true;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];


            Main.PlaySound(SoundID.Item62, projectile.position);
            for (int i = 0; i < 10; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dustIndex = Dust.NewDustPerfect(projectile.Center, 31, QwertyMethods.PolarVector(Main.rand.NextFloat() * 4f, theta));

            }
            // Fire Dust spawn
            for (int i = 0; i < 20; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dustIndex = Dust.NewDustPerfect(projectile.Center, 6, QwertyMethods.PolarVector(Main.rand.NextFloat() * 4f, theta));
                dustIndex.noGravity = true;
                theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                dustIndex = Dust.NewDustPerfect(projectile.Center, 6, QwertyMethods.PolarVector(Main.rand.NextFloat() * 4f, theta), Scale: 2f);
            }
        }
        

    }
    public class MiniBombBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Bomb");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            
            projectile.width = 80;
            projectile.height = 80;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 2;
            projectile.usesLocalNPCImmunity = true;
            projectile.minion = true;

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

    }
}

