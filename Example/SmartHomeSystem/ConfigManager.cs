using System;
using System.IO;
using System.Text.Json;

namespace SmartHomeSystem
{
    class ConfigManager
    {
        static readonly string settingsPath = "config.txt";
        static string programPath = "";
        static JsonDocument document;

        public static void LoadSettings()
        {
            if (programPath == "") programPath = Environment.CurrentDirectory;

            //Combine Two Paths To Form A Path To Config File
            string configFilePath = Path.Combine(programPath, settingsPath);


            if (File.Exists(configFilePath))
            {
                //Read All File Contents To String For Processing
                string configText = File.ReadAllText(configFilePath);

                document = JsonDocument.Parse(configText);

                return;
            }

            throw new FileNotFoundException("Config File Not Found");
        }

        public static string GetSettingText(string setting)
        {
            JsonElement root = document.RootElement;

            string text = root.GetProperty(setting).GetString();

            return text;
        }

        public static int GetSettingInt(string setting)
        {
            JsonElement root = document.RootElement;

            int number = root.GetProperty(setting).GetInt32();

            return number;
        }
    }
}
