using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectB
{

    /// <summary>
    /// This class handles anything related to storing picked seats and accessing them
    /// </summary>
    internal static class SettingsHandler
    {
	public struct Settings
	{
            public bool CoronaCheck { get; set; }

            public Settings ToggleCoronaCheck()
	    {
                this.CoronaCheck = this.CoronaCheck ? false : true;
                SettingsHandler.Save(this);
                return this;
            }
        }

	static string SettingsJsonName = @$"Data{Path.DirectorySeparatorChar}Settings.json";       


        /// <summary>
        /// Creates a Settings object from the Settings.json file if it exists. If it doesn't exist,
	/// then a new json is created with default settings
        /// </summary>
        public static Settings Load()
        {
            if (File.Exists(SettingsJsonName))
            {
                string json = File.ReadAllText(SettingsJsonName);
                return JsonSerializer.Deserialize<Settings>(json);
            }
	    else
	    {
		Settings settings = new Settings();
		settings.CoronaCheck = false;
                Save(settings);
                return settings;
            }
        }

        /// <summary>
	/// Saves any changes to the SeatsDict variable to the json file
	/// </summary>
        public static void Save(Settings settings)
        {
	    var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            File.WriteAllText(SettingsJsonName, JsonSerializer.Serialize(settings, options: options));
        }
    }

}
