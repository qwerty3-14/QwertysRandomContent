
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Exceptions;
using System.IO;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;

namespace QwertysRandomContent
{
    public class AccessoryPrefix : ModPrefix
    {
        
        private byte damage;
        private byte crit;
        private byte moveSpeed;
        private byte meleeSpeed;
        private byte defense;

        private byte manaReduction;
        private byte ammoReduction;
        private byte throwVel;
        private byte rangedVel;
        private byte dashPower;
        private byte recovery;
        private byte dodgeChance;
        public override float RollChance(Item item)
        {
            return Config.disableModAccesoryPrefixes ? 0 : 1f;
        }
        public override bool CanRoll(Item item)
        {
            return true;
        }
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }

        public AccessoryPrefix()
        {
        }

        public AccessoryPrefix(byte damage, byte crit, byte moveSpeed, byte meleeSpeed, byte defense, byte manaReduction, byte ammoReduction, byte throwVel, byte rangedVel, byte dashPower, byte recovery, byte dodgeChance)
        {
            
            this.damage = damage;
            this.crit = crit;
            this.moveSpeed = moveSpeed;
            this.meleeSpeed = meleeSpeed;
            this.defense = defense;
            this.manaReduction = manaReduction;
            this.ammoReduction = ammoReduction;
            this.throwVel = throwVel;
            this.rangedVel = rangedVel;
            this.dashPower = dashPower;
            this.recovery = recovery;
            this.dodgeChance = dodgeChance;

        }

        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {
                //mod.AddPrefix("name", new AccessoryPrefix(damage, crit, moveSpeed, meleeSpeed, defense, manaReduction, ammoReduction, throwVel, rangedVel, dashPower, recovery));
                mod.AddPrefix("Displacive", new AccessoryPrefix(1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                mod.AddPrefix("Destructive", new AccessoryPrefix(2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                mod.AddPrefix("Buffeted", new AccessoryPrefix(1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1));
                mod.AddPrefix("Agitated", new AccessoryPrefix(0, 0, 0, 0, 0, 0, 0, 4, 4, 0, 0, 0));
                mod.AddPrefix("Mached", new AccessoryPrefix(0, 0, 0, 0, 0, 0, 0, 8, 8, 0, 0, 0));
                mod.AddPrefix("Accelerated", new AccessoryPrefix(0, 0, 2, 0, 0, 0, 0, 4, 4, 0, 0, 0));
                mod.AddPrefix("Strong", new AccessoryPrefix(1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0));
                mod.AddPrefix("Mighty", new AccessoryPrefix(2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0));

                mod.AddPrefix("Meditative", new AccessoryPrefix(0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0));
                mod.AddPrefix("Knowledgeable", new AccessoryPrefix(0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0));
                mod.AddPrefix("Wise", new AccessoryPrefix(0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0));
                mod.AddPrefix("Enlightened", new AccessoryPrefix(0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0));
                mod.AddPrefix("Spectral", new AccessoryPrefix(0, 0, 0, 0, 0, 6, 6, 0, 0, 0, 0, 0));
                mod.AddPrefix("Stable", new AccessoryPrefix(0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0));
                mod.AddPrefix("Quivered", new AccessoryPrefix(0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0));

                mod.AddPrefix("Restless", new AccessoryPrefix(0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0));
                mod.AddPrefix("Energised", new AccessoryPrefix(0, 0, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0));
                mod.AddPrefix("Rushed", new AccessoryPrefix(0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0));
                mod.AddPrefix("Hyper", new AccessoryPrefix(0, 0, 3, 0, 0, 0, 0, 0, 0, 1, 0, 0));
                mod.AddPrefix("Stitched", new AccessoryPrefix(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0));
                mod.AddPrefix("Perscribed", new AccessoryPrefix(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0));
                mod.AddPrefix("Nursing", new AccessoryPrefix(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0));
                mod.AddPrefix("First Aid Trained", new AccessoryPrefix(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0));
                mod.AddPrefix("Ninja-Like", new AccessoryPrefix(0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1));
                mod.AddPrefix("Blurred", new AccessoryPrefix(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1));
                mod.AddPrefix("Evasive", new AccessoryPrefix(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2));
                mod.AddPrefix("Dodgy", new AccessoryPrefix(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3));
                mod.AddPrefix("Quantum", new AccessoryPrefix(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4));
            }
            return false;
        }
        public override void Apply(Item item)
        {
            
            item.GetGlobalItem<QwertyForge>().damage = damage;
            item.GetGlobalItem<QwertyForge>().crit = crit;
            item.GetGlobalItem<QwertyForge>().moveSpeed = moveSpeed;
            item.GetGlobalItem<QwertyForge>().meleeSpeed = meleeSpeed;
            item.GetGlobalItem<QwertyForge>().defense = defense;
            item.GetGlobalItem<QwertyForge>().manaReduction = manaReduction;
            item.GetGlobalItem<QwertyForge>().ammoReduction = ammoReduction;
            item.GetGlobalItem<QwertyForge>().throwVel = throwVel;
            item.GetGlobalItem<QwertyForge>().rangedVel = rangedVel;
            item.GetGlobalItem<QwertyForge>().dashPower = dashPower;
            item.GetGlobalItem<QwertyForge>().recovery = recovery;
            item.GetGlobalItem<QwertyForge>().dodgeChance = dodgeChance;
        }
        public override void ModifyValue(ref float valueMult)
        {
            float multiplier = 1f * (1 + damage * 0.04f) * (1 + crit * 0.04f) * (1 + moveSpeed * 0.04f) * (1 + meleeSpeed * 0.04f) * (1 + defense * 0.04f) * (1 + manaReduction * 0.02f) * (1 + ammoReduction * 0.02f) * (1 + throwVel * 0.01f) * (1 + rangedVel * 0.01f) * (1 + dashPower * 0.12f) * (1 + recovery * 0.04f) * (1 + dodgeChance * 0.04f);
            valueMult *= multiplier;
        }
    }
    public class QwertyForge : GlobalItem
    {
        
        public int damage;
        public int crit;
        public int moveSpeed;
        public int meleeSpeed;
        public int defense;
        public int manaReduction;
        public int ammoReduction;
        public int throwVel;
        public int rangedVel;
        public int dashPower;
        public int recovery;
        public int dodgeChance;
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            QwertyForge myClone = (QwertyForge)base.Clone(item, itemClone);
            
            myClone.damage = damage;
            myClone.crit = crit;
            myClone.moveSpeed = moveSpeed;
            myClone.meleeSpeed = meleeSpeed;
            myClone.defense = defense;
            myClone.manaReduction = manaReduction;
            myClone.ammoReduction = ammoReduction;
            myClone.throwVel = throwVel;
            myClone.rangedVel = rangedVel;
            myClone.dashPower = dashPower;
            myClone.recovery = recovery;
            myClone.dodgeChance = dodgeChance;
            return myClone;
        }
        public override bool NewPreReforge(Item item)
        {
            
            damage = 0;
            crit = 0;
            moveSpeed = 0;
            meleeSpeed = 0;
            defense = 0;
            manaReduction = 0;
            ammoReduction = 0;
            throwVel = 0;
            rangedVel = 0;
            dashPower = 0;
            recovery = 0;
            dodgeChance = 0;
            return base.NewPreReforge(item);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {

            if (damage > 0)
            {
                TooltipLine line = new TooltipLine(mod, "damage", "+" + damage + "% damage");
                line.isModifier = true;
                tooltips.Add(line);
                line.text="+" + damage + Language.GetTextValue("Mods.QwertysRandomContent.Perfixdamage");
            }
            if (crit > 0)
            {
                TooltipLine line = new TooltipLine(mod, "crit", "+" + crit + "% critical strike chance");
                line.isModifier = true;
                tooltips.Add(line);
                line.text="+" + crit + Language.GetTextValue("Mods.QwertysRandomContent.Perfixcriticalstrikechance");
            }
            if (moveSpeed > 0)
            {
                TooltipLine line = new TooltipLine(mod, "moveSpeed", "+" + moveSpeed + "% movement speed");
                line.isModifier = true;
                tooltips.Add(line);
                line.text="+" + moveSpeed + Language.GetTextValue("Mods.QwertysRandomContent.Perfixmovementspeed");
            }
            if (meleeSpeed > 0)
            {
                TooltipLine line = new TooltipLine(mod, "meleeSpeed", "+" + meleeSpeed + "% melee speed");
                line.isModifier = true;
                tooltips.Add(line);
                line.text="+" + meleeSpeed + Language.GetTextValue("Mods.QwertysRandomContent.PerfixmeleeSpeed");
            }
            if (defense > 0)
            {
                TooltipLine line = new TooltipLine(mod, "defense", "+" + defense + " defense");
                line.isModifier = true;
                tooltips.Add(line);
                line.text="+" + defense + Language.GetTextValue("Mods.QwertysRandomContent.Perfixdefense");
            }

            if (manaReduction > 0)
            {
                TooltipLine line = new TooltipLine(mod, "manaReduction", "+" + manaReduction + "% reduced mana usage");
                line.isModifier = true;
                tooltips.Add(line);
                line.text="+" + manaReduction + Language.GetTextValue("Mods.QwertysRandomContent.PerfixmanaReduction");
            }
            if (ammoReduction > 0)
            {
                TooltipLine line = new TooltipLine(mod, "ammoReduction", "+" + ammoReduction + "% reduced ammo usage");
                line.isModifier = true;
                tooltips.Add(line);
                line.text="+" + ammoReduction + Language.GetTextValue("Mods.QwertysRandomContent.PerfixammoReduction");
            }
            if (throwVel > 0)
            {
                TooltipLine line = new TooltipLine(mod, "throwVel", "+" + throwVel + "% throwing velocity");
                line.isModifier = true;
                tooltips.Add(line);
                line.text="+" + throwVel + Language.GetTextValue("Mods.QwertysRandomContent.PerfixthrowVel");
            }
            if (rangedVel > 0)
            {
                TooltipLine line = new TooltipLine(mod, "rangedVel", "+" + rangedVel + "% ranged velocity");
                line.isModifier = true;
                tooltips.Add(line);
                line.text="+" + rangedVel + Language.GetTextValue("Mods.QwertysRandomContent.PerfixrangedVel");
            }
            if (dashPower > 0)
            {
                TooltipLine line = new TooltipLine(mod, "dashPower", "+" + dashPower + " dash power");
                line.isModifier = true;
                tooltips.Add(line);
                line.text="+" + dashPower + Language.GetTextValue("Mods.QwertysRandomContent.PerfixdashPower");
            }
            if (recovery > 0)
            {
                TooltipLine line = new TooltipLine(mod, "recovery", "+" + recovery + " recovery");
                line.isModifier = true;
                tooltips.Add(line);
                line.text="+" + recovery + Language.GetTextValue("Mods.QwertysRandomContent.Perfixrecovery");
            }
            if (dodgeChance > 0)
            {
                TooltipLine line = new TooltipLine(mod, "recovery", "+" + dodgeChance + "% dodge chance");
                line.isModifier = true;
                tooltips.Add(line);
                line.text="+" + dodgeChance + Language.GetTextValue("Mods.QwertysRandomContent.PerfixdodgeChance");
            }





        }
        public override void UpdateEquip(Item item, Player player)
        {
            if (item.prefix > 0)
            {
                player.allDamage += damage * .01f;
                player.meleeCrit += crit;
                player.rangedCrit += crit;
                player.magicCrit += crit;
                player.thrownCrit += crit;
                player.moveSpeed += moveSpeed * .01f;
                player.meleeSpeed += meleeSpeed * .01f;
                player.statDefense += defense;
                player.manaCost -= manaReduction * .01f;
                player.GetModPlayer<QwertyPlayer>().ammoReduction *= (1f - (ammoReduction * .01f));
                player.thrownVelocity += throwVel * .01f;
                player.GetModPlayer<QwertyPlayer>().rangedVelocity += rangedVel * .01f;
                player.GetModPlayer<QwertyPlayer>().customDashBonusSpeed += dashPower;
                player.GetModPlayer<QwertyPlayer>().recovery += recovery;
                player.GetModPlayer<QwertyPlayer>().dodgeChance += dodgeChance;

            }
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            
            writer.Write(damage);
            writer.Write(crit);
            writer.Write(moveSpeed);
            writer.Write(meleeSpeed);
            writer.Write(defense);
            writer.Write(manaReduction);
            writer.Write(ammoReduction);
            writer.Write(throwVel);
            writer.Write(rangedVel);
            writer.Write(dashPower);
            writer.Write(recovery);
            writer.Write(dodgeChance);

        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            damage = reader.ReadInt32();
            crit = reader.ReadInt32();
            moveSpeed = reader.ReadInt32();
            meleeSpeed = reader.ReadInt32();
            defense = reader.ReadInt32();
            manaReduction = reader.ReadInt32();
            ammoReduction = reader.ReadInt32();
            throwVel = reader.ReadInt32();
            rangedVel = reader.ReadInt32();
            dashPower = reader.ReadInt32();
            recovery = reader.ReadInt32();
            dodgeChance = reader.ReadInt32();


        }
        public override bool ConsumeAmmo(Item item, Player player)
        {
            if (Main.rand.NextFloat() > player.GetModPlayer<QwertyPlayer>().ammoReduction)
            {
                return false;
            }
            return true;
        }
        public override bool ConsumeItem(Item item, Player player)
        {
            if (item.thrown)
            {
                return !(Main.rand.NextFloat() > player.GetModPlayer<QwertyPlayer>().throwReduction);
            }
            return base.ConsumeItem(item, player);
        }
       
        public override bool OnPickup(Item item, Player player)
        {
            if (player.GetModPlayer<QwertyPlayer>().recovery > 0)
            {
                if (item.type == ItemID.Heart ||
                    item.type == ItemID.CandyApple ||
                    item.type == ItemID.SugarPlum
                    )
                {
                    Main.PlaySound(SoundID.Grab, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
                    player.statLife += (20 + player.GetModPlayer<QwertyPlayer>().recovery);
                    if (Main.myPlayer == player.whoAmI)
                    {
                        player.HealEffect((20 + player.GetModPlayer<QwertyPlayer>().recovery), true);
                    }
                    if (player.statLife > player.statLifeMax2)
                    {
                        player.statLife = player.statLifeMax2;
                    }
                    
                    return false;
                }

            }
            return base.OnPickup(item, player);
        }
        public override bool UseItem(Item item, Player player)
        {
            if(item.healLife>0 && player.GetModPlayer<QwertyPlayer>().recovery>0)
            {
               player.HealEffect((player.GetModPlayer<QwertyPlayer>().recovery *2) , true);
                player.statLife += (player.GetModPlayer<QwertyPlayer>().recovery * 2) ;
                return true;
            }
            return false;
        }
    }
   
    
 }