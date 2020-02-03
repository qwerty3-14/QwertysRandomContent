using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace QwertysRandomContent.Config
{

    [Label("Sprite Settings")]
    public class SpriteSettings : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(false)]
        [ReloadRequired]
        [Label("$Mods.QwertysRandomContent.ClassicFortressConfigLabel")]
        [Tooltip("$Mods.QwertysRandomContent.ClassicFortressConfigTooltip")]
        public bool ClassicFortress;

        [DefaultValue(false)]
        [ReloadRequired]
        [Label("$Mods.QwertysRandomContent.ClassicRhuthiniumConfigLabel")]
        [Tooltip("$Mods.QwertysRandomContent.ClassicRhuthiniumConfigTooltip")]
        public bool ClassicRhuthinium;

        [DefaultValue(false)]
        [ReloadRequired]
        [Label("$Mods.QwertysRandomContent.ClassicAncientConfigLabel")]
        [Tooltip("$Mods.QwertysRandomContent.ClassicAncientConfigTooltip")]
        public bool ClassicAncient;

        [DefaultValue(false)]
        [ReloadRequired]
        [Label("$Mods.QwertysRandomContent.ClassicOLORDConfigLabel")]
        [Tooltip("$Mods.QwertysRandomContent.ClassicOLORDConfigTooltip")]
        public bool ClassicOLORD;

        [DefaultValue(false)]
        [ReloadRequired]
        [Label("$Mods.QwertysRandomContent.ClassicGunChakramConfigLabel")]
        [Tooltip("$Mods.QwertysRandomContent.ClassicGunChakramConfigTooltip")]
        public bool ClassicGunChakram;

        [DefaultValue(false)]
        [ReloadRequired]
        [Label("$Mods.QwertysRandomContent.ClassicNoehtnapConfigLabel")]
        [Tooltip("$Mods.QwertysRandomContent.ClassicNoehtnapConfigTooltip")]
        public bool ClassicNoehtnap;
    }
}
