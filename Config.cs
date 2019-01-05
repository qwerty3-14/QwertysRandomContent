using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace QwertysRandomContent
{
    public static class Config
    {
        static string filename = "Qwerty's Bosses and Items.json";
        public static bool alternateFortressLook = false;
        static string ConfigPath = Path.Combine(Main.SavePath, "Mod Configs", filename);

        static Preferences Configuration = new Preferences(ConfigPath);
        static Config()
        {
            Load();
        }
        public static void Load()
        {
            bool success = ReadConfig();

            if (!success)
            {
                ErrorLogger.Log("Failed to read the config file for Qwerty's Bosses and Items! Recreating config...");
                CreateConfig();
            }
        }

        static bool ReadConfig()
        {
            if (Configuration.Load())
            {
                Configuration.Get("Alternate Fortress Look", ref alternateFortressLook);
                return true;
            }
            else
            {
                return false;
            }
        }
        static void CreateConfig()
        {
            Configuration.Clear();
            Configuration.Put("Alternate Fortress Look", alternateFortressLook);
            Configuration.Save();
        }
    }
}
