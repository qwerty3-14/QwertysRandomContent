using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Vitallum
{
    [AutoloadEquip(EquipType.Head)]
    public class VitallumHeadress : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vitallum Headress");
            Tooltip.SetDefault("Increases max life by 80 \n9% increased damage \nAttacks poison enemies. \nHealth nearby enemies lose from debuffs heals you.");
        }

        public override void SetDefaults()
        {
            item.rare = 8;
            item.value = Item.sellPrice(gold: 6);
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 80;
            player.allDamage += .09f;
            player.GetModPlayer<HeadressEffects>().poisonHeal = true;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
            recipe.AddIngredient(ItemID.LifeCrystal, 4);
            recipe.AddIngredient(mod.ItemType("VitallumCoreCharged"));
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("VitallumLifeguard") && legs.type == mod.ItemType("VitallumJeans");
        }

        public override void UpdateArmorSet(Player player)
        {
            String s = "Please go to conrols and bind the 'Yet another special ability key'";
            foreach (String key in QwertysRandomContent.YetAnotherSpecialAbility.GetAssignedKeys()) //get's the string of the hotkey's name
            {
                s = "Set Bonus: You generate Vitallum hearts over time" + "\nEach active heart grants 4% damage." + "\nPress " + key + " to consume the hearts for health.";
            }
            player.setBonus = s;
            player.GetModPlayer<HeadressEffects>().setBonus = true;
        }

        public override void OnCraft(Recipe recipe)
        {
            Main.player[item.owner].QuickSpawnItem(mod.ItemType("VitallumCoreUncharged"), 1);
        }
    }

    public class HeadressEffects : ModPlayer
    {
        public bool poisonHeal = false;
        private int counter = 0;
        private List<Dust> leechDusts = new List<Dust>();
        public bool setBonus = false;
        private int heartCount = 0;
        private int maxHearts = 5;
        private int heartReplenishTime = 300;
        private int heartCounter = 0;
        private int heartRadius = 60;
        private float trigCounter = 0;

        public override void ResetEffects()
        {
            poisonHeal = false;
            setBonus = false;
        }

        public override void ProcessTriggers(TriggersSet triggersSet) //runs hotkey effects
        {
            if (QwertysRandomContent.YetAnotherSpecialAbility.JustPressed) //hotkey is pressed
            {
                if (setBonus && heartCount > 0 && heartCounter > 0)
                {
                    heartCounter = -heartRadius;
                }
            }
        }

        public override void PreUpdate()
        {
            if (setBonus)
            {
                trigCounter += (float)Math.PI / 60;
                heartCounter++;
                if (heartCounter < 0)
                {
                    heartRadius = -heartCounter;
                }
                else if (heartCounter == 0)
                {
                    for (int i = 0; i < heartCount; i++)
                    {
                        int amt = 20 + player.GetModPlayer<QwertyPlayer>().recovery;
                        player.statLife += amt;
                        player.HealEffect(amt, true);
                    }
                    if (player.statLife > player.statLifeMax2)
                    {
                        player.statLife = player.statLifeMax2;
                    }
                    heartCount = 0;
                    heartRadius = 60;
                }
                else if (heartCount < maxHearts)
                {
                    if (heartCounter > heartReplenishTime + 1)
                    {
                        heartCount++;
                        heartCounter = 1;
                    }
                }
            }
            else
            {
                heartCount = 0;
                heartCounter = 0;
            }
        }

        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
        {
            player.allDamage += .04f * heartCount;
            if (poisonHeal)
            {
                for (int d = 0; d < leechDusts.Count; d++)
                {
                    if ((player.Center - leechDusts[d].position).Length() < 11)
                    {
                        leechDusts.RemoveAt(d);
                    }
                    else
                    {
                        leechDusts[d].scale = 1;
                        leechDusts[d].velocity = QwertyMethods.PolarVector(10f, (player.Center - leechDusts[d].position).ToRotation());
                    }
                }
                counter++;
                int regenBoost = 0;
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].immortal && !Main.npc[i].dontTakeDamage && Main.npc[i].lifeRegen < 0 && (Main.npc[i].Center - player.Center).Length() < 400)
                    {
                        regenBoost -= Main.npc[i].lifeRegen;
                        Dust dust = Dust.NewDustPerfect(Main.npc[i].position + new Vector2(Main.rand.Next(Main.npc[i].width), Main.rand.Next(Main.npc[i].height)), mod.DustType("VitallumDust"));
                        dust.frame.Y = 0;
                        leechDusts.Add(dust);
                    }
                }
                if (regenBoost > 20)
                {
                    regenBoost = 20;
                }
                if (counter % 30 == 0 && regenBoost / 4 > 0)
                {
                    player.HealEffect(regenBoost / 4);
                }
                player.lifeRegen += regenBoost;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (poisonHeal)
            {
                target.AddBuff(BuffID.Poisoned, 600);
            }
        }

        public PlayerLayer HeartRing = new PlayerLayer("QwertysRandomContent", "HeartRing", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            HeadressEffects modPlayer = drawPlayer.GetModPlayer<HeadressEffects>();
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            int horizontalFrame = 0;
            int horizontalFrames = 4;
            if (horizontalFrames > 1)
            {
                int frameTimer = drawPlayer.GetModPlayer<QwertyPlayer>().ArmorFrameCounter % ((2 * horizontalFrames - 2) * 10);
                horizontalFrame = frameTimer / 10;

                if (horizontalFrame > horizontalFrames - 1)
                {
                    horizontalFrame = (horizontalFrames - 1) - (horizontalFrame - (horizontalFrames - 1));
                }
            }
            for (int e = 0; e < modPlayer.heartCount; e++)
            {
                Vector2 heartPos = drawPlayer.Center + QwertyMethods.PolarVector(modPlayer.heartRadius, modPlayer.trigCounter + ((float)Math.PI * 2 * e) / (float)modPlayer.heartCount) - Main.screenPosition;
                Texture2D heartTexture = mod.GetTexture("Items/Armor/Vitallum/VitallumCoreUncharged");
                DrawData data = new DrawData(heartTexture, heartPos, null, Color.White, 0, heartTexture.Size() * .5f, 1f, 0, 0);
                data.shader = drawInfo.bodyArmorShader;
                Main.playerDrawData.Add(data);

                Texture2D veinTexture = mod.GetTexture("Items/Armor/Vitallum/VitallumHeartVein");
                data = new DrawData(veinTexture, heartPos, new Rectangle(0, 22 * horizontalFrame, 26, 22), Color.White, 0, heartTexture.Size() * .5f, 1f, 0, 0);
                data.shader = drawPlayer.dye[3].dye;
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer Mask = LayerDrawing.DrawHeadSimple("VitallumHeadress", "Items/Armor/Vitallum/VitallumHeadress_HeadSimple", glowmask: false);
        public static readonly PlayerLayer MaskVien = LayerDrawing.DrawHeadSimple("VitallumHeadress", "Items/Armor/Vitallum/VitallumHeadress_HeadSimpleVein", "VitallumHeadressVein", false, 3, 4, true);

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Head"));

            if (headLayer != -1)
            {
                Mask.visible = true;
                layers.Insert(headLayer + 1, Mask);
                layers.Insert(headLayer + 2, MaskVien);
                layers.Insert(headLayer + 3, HeartRing);
            }
        }

        public static readonly PlayerHeadLayer MapMask = LayerDrawing.DrawHeadLayer("VitallumHeadress", "Items/Armor/Vitallum/VitallumHeadress_HeadSimple");
        public static readonly PlayerHeadLayer MapMaskVein = LayerDrawing.DrawHeadLayer("VitallumHeadress", "Items/Armor/Vitallum/VitallumHeadress_HeadSimpleVein", "Vein", 3, 4, true);

        public override void ModifyDrawHeadLayers(List<PlayerHeadLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerHeadLayer => PlayerHeadLayer.Name.Equals("Armor"));
            if (headLayer != -1)
            {
                MapMask.visible = true;
                layers.Insert(headLayer + 1, MapMask);
                layers.Insert(headLayer + 2, MapMaskVein);
            }
        }
    }
}