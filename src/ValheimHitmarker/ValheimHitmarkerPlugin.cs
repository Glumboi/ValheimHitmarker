using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ValheimHitmarker.MonoBehaviours;

namespace ValheimHitmarker
{
    // TODO Review this file and update to your own requirements.

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class ValheimHitmarkerPlugin : BaseUnityPlugin
    {
        // Mod specific details. MyGUID should be unique, and follow the reverse domain pattern
        // e.g.
        // com.mynameororg.pluginname
        // Version should be a valid version string.
        // e.g.
        // 1.0.0
        private const string MyGUID = "com.glumboi.ValheimHitmarker";

        public const string PluginName = "ValheimHitmarker";
        private const string VersionString = "1.1.0";

        // Config entry key strings
        // These will appear in the config file created by BepInEx and can also be used
        // by the OnSettingsChange event to determine which setting has changed.
        public static string HitmarkerSizeKey = "Hitmarker size";

        public static string HitmarkerDisplayDurationKey = "Int Example Key";

        // Configuration entries. Static, so can be accessed directly elsewhere in code via
        // e.g.
        // float myFloat = ValheimHitmarkerPlugin.FloatExample.Value;
        // TODO Change this code or remove the code if not required.
        public static ConfigEntry<float> HitmarkerSize;

        public static ConfigEntry<float> HitmarkerDisplayDuration;

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        public static BasicHitmarker hitMarker;
        public static CriticalHitmarker criticalHitMarker;
        public static KillMessage killMessage;

        /// <summary>
        /// Initialise the configuration settings and patch methods
        /// </summary>
        private void Awake()
        {
            // Float configuration setting example
            // TODO Change this code or remove the code if not required.
            HitmarkerSize = Config.Bind("General",    // The section under which the option is shown
                HitmarkerSizeKey,                            // The key of the configuration option
                35f,                            // The default value
                new ConfigDescription("Changes the size of the Hitmarker"));

            // Int setting example
            // TODO Change this code or remove the code if not required.
            HitmarkerDisplayDuration = Config.Bind("General",
                HitmarkerDisplayDurationKey,
                0.5f,
                new ConfigDescription("Changes the display duration of the Hitmarker"));

            // Add listeners methods to run if and when settings are changed by the player.
            // TODO Change this code or remove the code if not required.
            HitmarkerSize.SettingChanged += ConfigSettingChanged;
            HitmarkerDisplayDuration.SettingChanged += ConfigSettingChanged;

            // Apply all of our patches
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");

            // Sets up our static Log, so it can be used elsewhere in code.
            // .e.g.
            // ValheimHitmarkerPlugin.Log.LogDebug("Debug Message to BepInEx log file");

            Log = Logger;
        }

        /// <summary>
        /// Method to handle changes to configuration made by the player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigSettingChanged(object sender, System.EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

            // Check if null and return
            if (settingChangedEventArgs == null)
            {
                return;
            }

            // Example Float Shortcut setting changed handler
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == HitmarkerSizeKey)
            {
                HitmarkerSize.Value = (float)settingChangedEventArgs.ChangedSetting.BoxedValue;
            }

            // Example Int Shortcut setting changed handler
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == HitmarkerDisplayDurationKey)
            {
                HitmarkerDisplayDuration.Value = (float)settingChangedEventArgs.ChangedSetting.BoxedValue;
            }
        }
    }
}