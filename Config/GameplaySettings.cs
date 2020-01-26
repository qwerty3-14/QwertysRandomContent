using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace QwertysRandomContent.Config
{
    public class GameplaySettings : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(false)]
        [Label("$Mods.QwertysRandomContent.DisableModdedPrefixesLabel")]
        [Tooltip("$Mods.QwertysRandomContent.DisableModdedPrefixesTooltip")]
        public bool DisableModdedPrefixes;
    }
}
