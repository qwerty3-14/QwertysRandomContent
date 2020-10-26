using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Bionic
{
    public class BionicEffects : ModPlayer
    {
        public bool eyeEquiped = false;
        public bool setBonus = false;
        public override void ResetEffects()
        {
            eyeEquiped = false;
            setBonus = false;
        }
        public override void ProcessTriggers(TriggersSet triggersSet) //runs hotkey effects
        {
            if (QwertysRandomContent.YetAnotherSpecialAbility.JustPressed) //hotkey is pressed
            {
                if (setBonus && !player.HasBuff(mod.BuffType("OverrdriveCooldown")))
                {
                    player.AddBuff(mod.BuffType("OverrdriveCooldown"), 60 * 40);
                    player.AddBuff(mod.BuffType("Overrdrive"), 60 * 10);
                }
            }
        }

        public static readonly PlayerLayer Eye = LayerDrawing.DrawHeadSimple("BionicEye", "Items/Armor/Bionic/BionicEye_HeadSimple", glowmask: false);
        public static readonly PlayerLayer EyeGlow = LayerDrawing.DrawHeadSimple("BionicEye", "Items/Armor/Bionic/BionicEye_HeadSimple_Glow", glowmask: true);

        public static readonly PlayerLayer Body = LayerDrawing.DrawOnBodySimple("BionicImplants", "Items/Armor/Bionic/BionicImplants_BodySimple", "Items/Armor/Bionic/BionicImplants_FemaleBodySimple", "BionicImplants", false);
        public static readonly PlayerLayer BodyGlow = LayerDrawing.DrawOnBodySimple("BionicImplants", "Items/Armor/Bionic/BionicImplants_BodySimple_Glow", "Items/Armor/Bionic/BionicImplants_FemaleBodySimple_Glow", "BionicImplants", true);
        public static readonly PlayerLayer Arm = LayerDrawing.DrawOnArms("BionicImplants", "Items/Armor/Bionic/BionicImplants_Arms_Glow", "BionicImplants", true);

        public static readonly PlayerLayer Limbs = LayerDrawing.DrawOnLegs("BionicLimbs_Legs", "Items/Armor/Bionic/BionicLimbs_Legs_Glow", "BionicLimbs_FemaleLegs", "Items/Armor/Bionic/BionicLimbs_FemaleLegs_Glow", "BionicLimbs", true);
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int headLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Face"));

            if (headLayer != -1)
            {
                Eye.visible = true;
                layers.Insert(headLayer + 1, Eye);
                layers.Insert(headLayer + 2, EyeGlow);
            }

            int bodyLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Body"));
            if (bodyLayer != -1)
            {
                Body.visible = true;
                layers.Insert(bodyLayer + 1, Body);
                layers.Insert(bodyLayer + 2, BodyGlow);
            }
            int armLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Arms"));
            if (armLayer != -1)
            {
                Arm.visible = true;
                layers.Insert(armLayer + 1, Arm);
            }

            int legLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Legs"));
            if (legLayer != -1)
            {
                Limbs.visible = true;
                layers.Insert(legLayer + 1, Limbs);
            }
        }
    }
}
