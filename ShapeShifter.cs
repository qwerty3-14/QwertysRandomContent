using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using QwertysRandomContent.Items.Etims;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace QwertysRandomContent
{
    // This class stores necessary player info for our custom damage class, such as damage multipliers and additions to knockback and crit.
    public class ShapeShifterPlayer : ModPlayer
    {
        public static ShapeShifterPlayer ModPlayer(Player player)
        {
            return player.GetModPlayer<ShapeShifterPlayer>();
        }
        public float morphDamage = 1f;
        public bool morphed = false;
        public int morphCrit = 4;
        public int morphDef = 0;
        public float coolDownDuration = 1f;
        public bool noDraw = false;
        public int overrideWidth = -1;
        public bool drawTankCannon = false;
        public float tankCannonRotation = 0f;
        public int morphTime = 0;
        public bool EyeBlessing = false;
        public bool EyeEquiped = false;
        public bool glassCannon = false;
        public bool hovercraft = false;
        public bool TwistedDarkSetBonus = false;
        bool healMe = false;
        public bool drawGodOfBlasphemy = false;
        public float pulseCounter = 0f;
        public bool Phase = false;
        bool justMorphed = false;
        bool noSick = false;
        public override void ResetEffects()
        {
            ResetVariables();
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }
        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
        {
            if (!player.miscEquips[3].IsAir)
            {
                morphDef += player.miscEquips[3].GetGlobalItem<ShapeShifterItem>().prefixMorphDef;
                morphDamage += player.miscEquips[3].GetGlobalItem<ShapeShifterItem>().prefixMorphDamage * .01f;
                morphCrit += player.miscEquips[3].GetGlobalItem<ShapeShifterItem>().prefixMorphCrit;
            }
            for (int i = 0; i < 10; i++)
            {
                if (!player.armor[i].IsAir)
                {
                    morphDef += player.armor[i].GetGlobalItem<ShapeShifterItem>().equipedMorphDefense;
                }
            }
        }
        public void justStableMorphed(bool noSick = false)
        {
            
            justMorphed = true;
            this.noSick = noSick;
        }
        void PostJustMorphed()
        {
            if (!noSick)
            {
                if (EyeBlessing && TwistedDarkSetBonus)
                {
                    player.statLifeMax2 += 100;
                    healMe = true;
                }
                if (EyeBlessing)
                {
                    EyeBlessing = false;
                    if (Phase)
                    {
                        player.immuneTime += 180;
                        player.immune = true;
                        
                    }
                }
                else
                {
                    player.AddBuff(mod.BuffType("MorphSickness"), 180);
                }
            }
        }
        private void ResetVariables()
        {
            if (!drawTankCannon && !glassCannon && !hovercraft)
            {

                tankCannonRotation = (player.direction == -1 ? (float)Math.PI : 0f);
            }
            glassCannon = false;
            drawTankCannon = false;
            drawGodOfBlasphemy = false;
            hovercraft = false;
            morphDamage = 1f;
            morphed = false;
            morphCrit = 4;
            morphDef = 0;
            coolDownDuration = 1f;
            noDraw = false;
            overrideWidth = -1;
            if (!EyeEquiped)
            {
                EyeBlessing = false;
            }
            Phase = false;
            EyeEquiped = false;
            TwistedDarkSetBonus = false;
        }
        public override void PreUpdate()
        {
           
            pulseCounter += (float)Math.PI / 30;
            //player.gravControl2 = true;
            if (healMe)
            {
                player.statLife += 100;
                player.HealEffect(100, true);
                healMe = false;
            }
            if (!player.HeldItem.IsAir)
            {
                // Main.NewText(player.HeldItem.GetGlobalItem<ShapeShifterItem>().PrefixorphCooldownModifier);
            }

            if (overrideWidth != -1)
            {
                player.width = overrideWidth;
            }
            else
            {
                player.width = 20;
            }
            if (morphed)
            {
                morphTime++;
            }
            else
            {
                morphTime = 0;
            }
            if (EyeBlessing)
            {
                player.AddBuff(mod.BuffType("EyeBless"), 2);
            }
        }
        public override void PostUpdateEquips()
        {
            if(justMorphed)
            {
                PostJustMorphed();
                justMorphed = false;
            }

            if (player.meleeCrit > 4 && player.magicCrit > 4 && player.rangedCrit > 4 && player.thrownCrit > 4)
            {
                int[] crits = { player.meleeCrit, player.magicCrit, player.rangedCrit };
                int smallest = 0;
                for (int d = 0; d < crits.Length; d++)
                {
                    if (crits[d] < crits[smallest])
                    {
                        smallest = d;
                    }
                }
                morphCrit += crits[smallest] - 4;
            }
        }

        public static readonly PlayerLayer TankCannon = new PlayerLayer("QwertysRandomContent", "TankCannon", PlayerLayer.MountBack, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>();
            if (drawPlayer.GetModPlayer<ShapeShifterPlayer>().hovercraft)
            {
                //Main.NewText("Tank!!");
                Texture2D texture = mod.GetTexture("Items/Weapons/Meteor/Hovercraft_Cannon");

                DrawData value = new DrawData(texture,
                    new Vector2(drawInfo.position.X + 20, drawInfo.position.Y + 8) - Main.screenPosition,
                    new Rectangle(0, 0, 24, 10),
                    color12,
                    drawPlayer.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation,
                    new Vector2(5, 5),
                    1f,
                    0,
                    0);
                value.shader = drawPlayer.miscDyes[3].dye;
                Main.playerDrawData.Add(value);

            }
            else if (drawPlayer.GetModPlayer<ShapeShifterPlayer>().drawTankCannon)
            {
                //Main.NewText("Tank!!");
                Texture2D texture = mod.GetTexture("Items/Weapons/ShapeShifter/TankMorph_Cannon");

                DrawData value = new DrawData(texture,
                    new Vector2(drawInfo.position.X + 75, drawInfo.position.Y - 4) - Main.screenPosition,
                    new Rectangle(0, 0, 130, 34),
                    color12,
                    drawPlayer.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation,
                    new Vector2(18, 18),
                    1f,
                    0,
                    0);
                value.shader = drawPlayer.miscDyes[3].dye;
                Main.playerDrawData.Add(value);

            }
            else if (drawPlayer.GetModPlayer<ShapeShifterPlayer>().glassCannon)
            {
                Texture2D texture = mod.GetTexture("Items/Weapons/Glass/GlassCannon" + (ModContent.GetInstance<SpriteSettings>().ClassicGlass ? "_Old" : ""));

                DrawData value = new DrawData(texture,
                    new Vector2(drawInfo.position.X + 15, drawInfo.position.Y) - Main.screenPosition,
                    new Rectangle(0, 0, 30, 8),
                    color12,
                    drawPlayer.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation,
                    new Vector2(4, 4),
                    1f,
                    0,
                    0);
                value.shader = drawPlayer.miscDyes[3].dye;
                Main.playerDrawData.Add(value);
            }
            else if (drawPlayer.GetModPlayer<ShapeShifterPlayer>().drawGodOfBlasphemy)
            {
                
                
                Texture2D texture = mod.GetTexture("Items/Etims/Back" + (ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? "_Old" : ""));
                DrawData value = new DrawData(texture,
                    drawPlayer.Center - Main.screenPosition,
                    null,
                    color12,
                    0,
                    texture.Size()*.5f,
                    drawPlayer.GetModPlayer<MorphFlightControl>().scale,
                    0,
                    0);
                value.shader = drawPlayer.miscDyes[3].dye;
                Main.playerDrawData.Add(value);


                texture = mod.GetTexture("Items/Etims/Pupil" + (ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? "_Old" : ""));
                value = new DrawData(texture,
                    drawPlayer.Center + drawPlayer.GetModPlayer<MorphFlightControl>().pupilPosition - Main.screenPosition ,
                    null,
                    color12,
                    0,
                    texture.Size() * .5f,
                    drawPlayer.GetModPlayer<MorphFlightControl>().scale,
                    0,
                    0);
                value.shader = drawPlayer.miscDyes[3].dye;
                Main.playerDrawData.Add(value);
            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            if (noDraw)
            {
                foreach (PlayerLayer l in layers)
                {
                    //Main.NewText(l.Name);

                    if (l.Name.Equals("MountBack") || l.Name.Equals("cl"))
                    {
                        l.visible = true;

                    }
                    else
                    {
                        l.visible = false;
                    }
                }
            }
            int mountLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("MountBack"));
            if (mountLayer != -1)
            {
                TankCannon.visible = true;
                if (hovercraft)
                {
                    layers.Insert(mountLayer + 1, TankCannon);
                }
                else
                {
                    layers.Insert(mountLayer - 1, TankCannon);
                }

            }

        }
        public override void PostUpdate()
        {

        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (proj.GetGlobalProjectile<MorphProjectile>().morph && Main.rand.Next(100) < morphCrit)
            {
                crit = true;
            }
        }
        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (glassCannon)
            {
                damage *= 3;
            }
        }
        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if (glassCannon)
            {
                damage *= 3;
            }
        }
    }
    public class MorphProjectile : GlobalProjectile
    {
       public override bool InstancePerEntity => true;
       public override bool CloneNewInstances => true;
        public bool morph = false;

    }
    public class ShapeShifterItem : GlobalItem
    {
        public const int StableShiftType = 1;
        public const int QuickShiftType = 2;

       public override bool InstancePerEntity => true;
       public override bool CloneNewInstances => true;
        public bool morph;
        public int morphDef = 0;
        public int morphType = 0;
        public int morphCooldown;
        public float PrefixorphCooldownModifier = 1f;
        public int prefixMorphDef;
        public int prefixMorphCrit;
        public float prefixMorphDamage = 0;
        public int equipedMorphDefense = 0;
        public override bool CanUseItem(Item item, Player player)
        {
            if (morphType == QuickShiftType && !item.IsAir)
            {
                if (player.HasBuff(mod.BuffType("MorphCooldown")))
                {
                    return false;
                }
                if (player.GetModPlayer<ShapeShifterPlayer>().EyeEquiped)
                {
                    player.GetModPlayer<ShapeShifterPlayer>().EyeBlessing = true;
                }
                //Main.NewText("Totals: " + (int)((item.GetGlobalItem<ShapeShifterItem>().morphCooldown * player.HeldItem.GetGlobalItem<ShapeShifterItem>().PrefixorphCooldownModifier * player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration) * 60f));
                //Main.NewText("Cooldown stat: " + player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration);
                //Main.NewText("Item Cooldown Multiplier: " + player.HeldItem.GetGlobalItem<ShapeShifterItem>().PrefixorphCooldownModifier);
                //Main.NewText("Base Cooldown: " + item.GetGlobalItem<ShapeShifterItem>().morphCooldown);
                player.AddBuff(mod.BuffType("MorphCooldown"), (int)((item.GetGlobalItem<ShapeShifterItem>().morphCooldown * PrefixorphCooldownModifier * player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration) * 60f));
            }
            return base.CanUseItem(item, player);
        }
        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {

            if (item.GetGlobalItem<ShapeShifterItem>().morphType == ShapeShifterItem.StableShiftType)
            {
                int random = rand.Next(100);
                if (random == 0)
                {
                    return mod.PrefixType("Pathetic");
                }
                else if (random >= 1 && random <= 5)
                {
                    return mod.PrefixType("Weak");
                }
                else if (random >= 6 && random <= 10)
                {
                    return mod.PrefixType("Vulnerable");
                }
                else if (random >= 11 && random <= 15)
                {
                    return mod.PrefixType("Flimsey");
                }
                else if (random >= 16 && random <= 25)
                {
                    return mod.PrefixType("Sturdy");
                }
                else if (random >= 26 && random <= 35)
                {
                    return mod.PrefixType("Damaging");
                }
                else if (random >= 36 && random <= 45)
                {
                    return mod.PrefixType("Hunting");
                }
                else if (random >= 46 && random <= 53)
                {
                    return mod.PrefixType("Plated");
                }
                else if (random >= 54 && random <= 61)
                {
                    return mod.PrefixType("Shredding");
                }
                else if (random >= 62 && random <= 69)
                {
                    return mod.PrefixType("Predatory");
                }
                else if (random >= 70 && random <= 79)
                {
                    return mod.PrefixType("Feral");
                }
                else if (random >= 80 && random <= 89)
                {
                    return mod.PrefixType("Stiff");
                }
                else if (random >= 80 && random <= 88)
                {
                    return mod.PrefixType("Glass");
                }
                else if (random == 99)
                {
                    return mod.PrefixType("Excellent");
                }
                else
                {
                    return mod.PrefixType("Sturdy");
                }



                /*
                 // mod.AddPrefix("Sturdy", new StableMorphPrefix(0, 0, 5, 0, 0));
                //mod.AddPrefix("Plated", new StableMorphPrefix(0, 0, 10, 0, 0));
                //mod.AddPrefix("Damaging", new StableMorphPrefix(10, 0, 0, 0, 0));
                //mod.AddPrefix("Shredding", new StableMorphPrefix(15, 0, 0, 0, 0));
                //mod.AddPrefix("Feral", new StableMorphPrefix(15, 5, 0, 0, 5));
                mod.AddPrefix("Glass", new StableMorphPrefix(20, 0, 0, 0, 10));
                //mod.AddPrefix("Stiff", new StableMorphPrefix(0, 0, 15, 10, 0));
                //mod.AddPrefix("Hunting", new StableMorphPrefix(0, 5, 0, 0, 0));
               // mod.AddPrefix("Predatory", new StableMorphPrefix(0, 10, 0, 0, 0));
                mod.AddPrefix("Excellent", new StableMorphPrefix(15, 5, 10, 0, 0));

                //mod.AddPrefix("Weak", new StableMorphPrefix(0, 0, 0, 10, 5));
               // mod.AddPrefix("Pathetic", new StableMorphPrefix(0, 0, 0, 30, 15));
                //mod.AddPrefix("Vulnerable", new StableMorphPrefix(0, 0, 0, 0, 10));
                //mod.AddPrefix("Flimsey", new StableMorphPrefix(0, 0, 0, 20, 0));*/
            }
            else if (item.GetGlobalItem<ShapeShifterItem>().morphType == ShapeShifterItem.QuickShiftType)
            {
                int random = rand.Next(31);
                if (random == 0 || random == 1)
                {
                    return PrefixID.Keen;
                }
                if (random == 2 || random == 3)
                {
                    return PrefixID.Forceful;
                }
                if (random == 4 || random == 5)
                {
                    return PrefixID.Damaged;
                }
                if (random == 6 || random == 7)
                {
                    return PrefixID.Hurtful;
                }
                if (random == 8 || random == 9)
                {
                    return PrefixID.Strong;
                }
                if (random == 10 || random == 11)
                {
                    return PrefixID.Weak;
                }
                if (random == 12 || random == 13)
                {
                    return PrefixID.Ruthless;
                }
                if (random == 14 || random == 15)
                {
                    return PrefixID.Zealous;
                }
                if (random == 16)
                {
                    return PrefixID.Superior;
                }
                if (random == 17)
                {
                    return PrefixID.Broken;
                }
                if (random == 18)
                {
                    return PrefixID.Shoddy;
                }
                if (random == 19)
                {
                    return PrefixID.Unpleasant;
                }
                if (random == 20)
                {
                    return PrefixID.Godly;
                }
                if (random == 21)
                {
                    return PrefixID.Demonic;
                }
                if (random == 22 || random == 23)
                {
                    return mod.PrefixType("Refreshing");
                }
                if (random == 24 || random == 25)
                {
                    return mod.PrefixType("Clumsy");
                }
                if (random == 26)
                {
                    return mod.PrefixType("Rejuvenating");
                }
                if (random == 27)
                {
                    return mod.PrefixType("Frenzied");
                }
                if (random == 28)
                {
                    return mod.PrefixType("Powerful");
                }
                if (random == 29)
                {
                    return mod.PrefixType("Aggressive");
                }

                return rand.NextBool() ? mod.PrefixType("Rigorous") : mod.PrefixType("Clumsy");


                /*
                mod.AddPrefix("Refreshing", new QuickMorphPrefix(1f, 0, 1f, .9f));
                mod.AddPrefix("Rejuvenating", new QuickMorphPrefix(1f, 0, 1f, .8f));
                mod.AddPrefix("Frenzied", new QuickMorphPrefix(.5f, 0, 1f, .5f));
                mod.AddPrefix("Powerful", new QuickMorphPrefix(1.5f, 0, 1f, 1.5f));
                mod.AddPrefix("Aggressive", new QuickMorphPrefix(1f, 5, 1.15f, .9f));
                mod.AddPrefix("Rigorous", new QuickMorphPrefix(1.15f, 5, 1.15f, .8f));

                mod.AddPrefix("Clumsy", new QuickMorphPrefix(1f, 0, 1f, 1.1f));*/
            }
            return -1;
        }
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            ShapeShifterItem myClone = (ShapeShifterItem)base.Clone(item, itemClone);

            myClone.prefixMorphDamage = prefixMorphDamage;
            myClone.prefixMorphCrit = prefixMorphCrit;

            myClone.prefixMorphDef = prefixMorphDef;

            return myClone;
        }
        public override bool NewPreReforge(Item item)
        {
            PrefixorphCooldownModifier = 1f;
            prefixMorphDamage = 0;
            prefixMorphDef = 0;
            prefixMorphCrit = 0;
            return base.NewPreReforge(item);
        }
        public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult)
        {
            if (item.GetGlobalItem<ShapeShifterItem>().morph)
            {
                mult *= player.GetModPlayer<ShapeShifterPlayer>().morphDamage;
            }
        }
        /*
        [Obsolete]
        public override void GetWeaponDamage(Item item, Player player, ref int damage)
        {
            if (item.GetGlobalItem<ShapeShifterItem>().morph)
            {
                damage = (int)(damage * ShapeShifterPlayer.ModPlayer(player).morphDamage + 5E-06f);
            }
        }*/
        public override void GetWeaponCrit(Item item, Player player, ref int crit)
        {
            if (item.GetGlobalItem<ShapeShifterItem>().morph)
            {
                crit = crit + ShapeShifterPlayer.ModPlayer(player).morphCrit;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.GetGlobalItem<ShapeShifterItem>().morph)
            {
                TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
                if (tt != null)
                {
                    // We want to grab the last word of the tooltip, which is the translated word for 'damage' (depending on what langauge the player is using)
                    // So we split the string by whitespace, and grab the last word from the returned arrays to get the damage word, and the first to get the damage shown in the tooltip
                    string[] splitText = tt.text.Split(' ');
                    string damageValue = splitText.First();
                    string damageWord = splitText.Last();
                    // Change the tooltip text
                    tt.text = damageValue + Language.GetTextValue("Mods.QwertysRandomContent.morph") + damageWord;
                }
            }
            if (equipedMorphDefense > 0)
            {
                int Index = tooltips.FindIndex(TooltipLine => TooltipLine.Name.Equals("Defense"));
                TooltipLine line = new TooltipLine(mod, "MorphDefense", equipedMorphDefense + " defense when morphed");
                {

                }
                if (Index != -1)
                {
                    tooltips.Insert(Index + 1, line);
                }
                line.text = equipedMorphDefense + Language.GetTextValue("Mods.QwertysRandomContent.morphDefense");
            }
            if (item.GetGlobalItem<ShapeShifterItem>().morphType != 0)
            {
                int KBIndex = tooltips.FindIndex(TooltipLine => TooltipLine.Name.Equals("Knockback"));
                TooltipLine line = new TooltipLine(mod, "MorphType", "Morph Type: Stable");
                {
                    line.overrideColor = Color.Orange;
                }
                line.text = Language.GetTextValue("Mods.QwertysRandomContent.MorphTypeStable");
                if (item.GetGlobalItem<ShapeShifterItem>().morphType == ShapeShifterItem.QuickShiftType)
                {
                    line.text = Language.GetTextValue("Mods.QwertysRandomContent.MorphTypeQuick");
                }
                tooltips.Insert(KBIndex + 1, line);
                if (item.GetGlobalItem<ShapeShifterItem>().morphDef == -1)
                {

                    line = new TooltipLine(mod, "MorphDef", "Invulnerable when morphed");
                    {
                        line.overrideColor = Color.Orange;
                        tooltips.Insert(KBIndex + 2, line);
                    }
                    line.text = Language.GetTextValue("Mods.QwertysRandomContent.MorphTypeInvulnerable");
                }
                else
                {

                    line = new TooltipLine(mod, "MorphDef", (item.GetGlobalItem<ShapeShifterItem>().morphDef + Main.player[item.owner].GetModPlayer<ShapeShifterPlayer>().morphDef) + " defense when morphed");
                    {
                        line.overrideColor = Color.Orange;
                        tooltips.Insert(KBIndex + 2, line);
                    }
                    line.text = (item.GetGlobalItem<ShapeShifterItem>().morphDef + Main.player[item.owner].GetModPlayer<ShapeShifterPlayer>().morphDef) + Language.GetTextValue("Mods.QwertysRandomContent.morphDefense");
                }

                if (item.GetGlobalItem<ShapeShifterItem>().morphCooldown != 0)
                {
                    line = new TooltipLine(mod, "MorphCool", (item.GetGlobalItem<ShapeShifterItem>().morphCooldown * PrefixorphCooldownModifier * Main.player[item.owner].GetModPlayer<ShapeShifterPlayer>().coolDownDuration) + " second cooldown");
                    {
                        line.overrideColor = Color.Orange;
                        tooltips.Insert(KBIndex + 3, line);
                    }
                    line.text = (item.GetGlobalItem<ShapeShifterItem>().morphCooldown * PrefixorphCooldownModifier * Main.player[item.owner].GetModPlayer<ShapeShifterPlayer>().coolDownDuration) + Language.GetTextValue("Mods.QwertysRandomContent.Morphcooldown");
                }
            }
            if (prefixMorphDamage > 0)
            {
                TooltipLine line = new TooltipLine(mod, "morphDamage", "+" + prefixMorphDamage + "% morph damage");
                line.isModifier = true;
                tooltips.Add(line);

                line.text = "+" + prefixMorphDamage + Language.GetTextValue("Mods.QwertysRandomContent.prefixMorphDamage");
            }
            else if (prefixMorphDamage < 0)
            {
                TooltipLine line = new TooltipLine(mod, "morphDamage", prefixMorphDamage + "% morph damage");
                line.isModifierBad = true;
                line.overrideColor = Color.Red;
                tooltips.Add(line);
                line.text = prefixMorphDamage + Language.GetTextValue("Mods.QwertysRandomContent.prefixMorphDamage");
            }
            if (prefixMorphDef > 0)
            {
                TooltipLine line = new TooltipLine(mod, "morphDefense", "+" + prefixMorphDef + " defense when morphed");
                line.isModifier = true;
                tooltips.Add(line);
                line.text = "+" + prefixMorphDef + Language.GetTextValue("Mods.QwertysRandomContent.prefixMorphDef");
            }
            else if (prefixMorphDef < 0)
            {
                TooltipLine line = new TooltipLine(mod, "morphDefense", prefixMorphDef + " defense when morphed");
                line.isModifierBad = true;
                line.overrideColor = Color.Red;
                tooltips.Add(line);
                line.text = prefixMorphDef + Language.GetTextValue("Mods.QwertysRandomContent.prefixMorphDef");
            }
            if (prefixMorphCrit > 0)
            {
                TooltipLine line = new TooltipLine(mod, "morphCrit", "+" + prefixMorphCrit + "% morph critical strike chance");
                line.isModifier = true;
                tooltips.Add(line);
                line.text = "+" + prefixMorphCrit + Language.GetTextValue("Mods.QwertysRandomContent.prefixMorphCrit");
            }
            if (PrefixorphCooldownModifier > 1f)
            {
                TooltipLine line = new TooltipLine(mod, "PrefixorphCooldownModifier", (int)(PrefixorphCooldownModifier * 100f) - 100 + "% longer cooldown");
                line.isModifier = true;
                line.overrideColor = Color.Red;
                tooltips.Add(line);
                line.text = (int)(PrefixorphCooldownModifier * 100f) - 100 + Language.GetTextValue("Mods.QwertysRandomContent.PrefixorphCooldownModifierLonger");
            }
            else if (PrefixorphCooldownModifier < 1f)
            {
                TooltipLine line = new TooltipLine(mod, "PrefixorphCooldownModifier", 100 - (int)(PrefixorphCooldownModifier * 100f) + "% shorter cooldown");
                line.isModifier = true;
                tooltips.Add(line);
                line.text = 100 - (int)(PrefixorphCooldownModifier * 100f) + Language.GetTextValue("Mods.QwertysRandomContent.PrefixorphCooldownModifierShorter");
            }

        }
        public override void NetSend(Item item, BinaryWriter writer)
        {

            writer.Write(prefixMorphDamage);
            writer.Write(prefixMorphDef);
            writer.Write(prefixMorphCrit);
            writer.Write(PrefixorphCooldownModifier);

        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            prefixMorphDamage = reader.ReadSingle();
            prefixMorphDef = reader.ReadInt32();
            prefixMorphCrit = reader.ReadInt32();
            PrefixorphCooldownModifier = reader.ReadSingle();


        }

    }
    public class StableMorphPrefix : ModPrefix
    {
        private byte damage;
        private byte crit;
        private byte defense;
        private byte negetiveDefense;
        private byte negetiveDamage;
        public override float RollChance(Item item)
        {
            return 1f;
        }
        public override bool CanRoll(Item item)
        {
            return true;
        }
        public StableMorphPrefix()
        {

        }
        public StableMorphPrefix(byte damage, byte crit, byte defense, byte negetiveDamage, byte negetiveDefense)
        {
            this.damage = damage;
            this.crit = crit;
            this.defense = defense;
            this.negetiveDefense = negetiveDefense;
            this.negetiveDamage = negetiveDamage;
        }
        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {

                mod.AddPrefix("Sturdy", new StableMorphPrefix(0, 0, 5, 0, 0));
                mod.AddPrefix("Plated", new StableMorphPrefix(0, 0, 10, 0, 0));
                mod.AddPrefix("Damaging", new StableMorphPrefix(10, 0, 0, 0, 0));
                mod.AddPrefix("Shredding", new StableMorphPrefix(15, 0, 0, 0, 0));
                mod.AddPrefix("Feral", new StableMorphPrefix(15, 5, 0, 0, 5));
                mod.AddPrefix("Glass", new StableMorphPrefix(20, 0, 0, 0, 10));
                mod.AddPrefix("Stiff", new StableMorphPrefix(0, 0, 15, 10, 0));
                mod.AddPrefix("Hunting", new StableMorphPrefix(0, 5, 0, 0, 0));
                mod.AddPrefix("Predatory", new StableMorphPrefix(0, 10, 0, 0, 0));
                mod.AddPrefix("Excellent", new StableMorphPrefix(15, 5, 10, 0, 0));

                mod.AddPrefix("Weak", new StableMorphPrefix(0, 0, 0, 10, 5));
                mod.AddPrefix("Pathetic", new StableMorphPrefix(0, 0, 0, 30, 15));
                mod.AddPrefix("Vulnerable", new StableMorphPrefix(0, 0, 0, 0, 10));
                mod.AddPrefix("Flimsy", new StableMorphPrefix(0, 0, 0, 20, 0));
            }
            return false;
        }

        public override void Apply(Item item)
        {

            if (negetiveDamage > 0)
            {
                item.GetGlobalItem<ShapeShifterItem>().prefixMorphDamage = -negetiveDamage;
            }
            else
            {
                item.GetGlobalItem<ShapeShifterItem>().prefixMorphDamage = damage;
            }
            if (negetiveDefense > 0)
            {
                item.GetGlobalItem<ShapeShifterItem>().prefixMorphDef = -negetiveDefense;
            }
            else
            {
                item.GetGlobalItem<ShapeShifterItem>().prefixMorphDef = defense;
            }
            item.GetGlobalItem<ShapeShifterItem>().prefixMorphCrit = crit;

        }
        public override void ModifyValue(ref float valueMult)
        {
            float multiplier = 1f * (1 + damage * 0.04f) * (1 + crit * 0.04f) * (1 + defense * 0.04f) * (1 - negetiveDefense * 0.04f) * (1 - negetiveDamage * 0.04f);
            valueMult *= multiplier;
        }
    }
    public class QuickMorphPrefix : ModPrefix
    {
        float damage = 1f;
        int crit = 0;
        float kb = 1f;
        float cooldown = 1f;



        public QuickMorphPrefix() { }

        public QuickMorphPrefix(float damage = 1f, int crit = 0, float kb = 1f, float cooldown = 1f)
        {
            this.damage = damage;
            this.crit = crit;
            this.kb = kb;
            this.cooldown = cooldown;

        }

        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {
                mod.AddPrefix("Refreshing", new QuickMorphPrefix(1f, 0, 1f, .9f));
                mod.AddPrefix("Rejuvenating", new QuickMorphPrefix(1f, 0, 1f, .8f));
                mod.AddPrefix("Frenzied", new QuickMorphPrefix(.5f, 0, 1f, .5f));
                mod.AddPrefix("Powerful", new QuickMorphPrefix(1.5f, 0, 1f, 1.5f));
                mod.AddPrefix("Aggressive", new QuickMorphPrefix(1f, 5, 1.15f, .9f));
                mod.AddPrefix("Rigorous", new QuickMorphPrefix(1.15f, 5, 1.15f, .8f));

                mod.AddPrefix("Clumsy", new QuickMorphPrefix(1f, 0, 1f, 1.1f));

            }
            return false;
        }
        public override void Apply(Item item)
        {
            item.GetGlobalItem<ShapeShifterItem>().PrefixorphCooldownModifier = cooldown;
        }


        public override bool CanRoll(Item item)
        {
            return true;
        }
        public override float RollChance(Item item)
        {
            return 1f;
        }
        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= (1f - (cooldown - 1f));
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = this.damage;
            knockbackMult = this.kb;
            critBonus = this.crit;
        }


    }
}
