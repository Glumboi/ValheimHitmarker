using HarmonyLib;
using UnityEngine;
using ValheimHitmarker.MonoBehaviours;

namespace ValheimHitmarker.Patches
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required

    /// <summary>
    /// Sample Harmony Patch class. Suggestion is to use one file per patched class
    /// though you can include multiple patch classes in one file.
    /// Below is included as an example, and should be replaced by classes and methods
    /// for your mod.
    /// </summary>
    [HarmonyPatch(typeof(Player))]
    internal class PlayerPatches
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void Postfix_Start()
        {
            // Create a new GameObject for hitmarkers
            GameObject hmObj = new GameObject("Hitmarkers");

            // Add the BasicHitmarker and CriticalHitmarker components to the GameObject
            BasicHitmarker bHm = hmObj.AddComponent<BasicHitmarker>();
            CriticalHitmarker cHm = hmObj.AddComponent<CriticalHitmarker>();

            // Initialize the hitmarkers with your mod's values
            bHm.Init(ValheimHitmarkerPlugin.HitmarkerSize.Value,
                ValheimHitmarkerPlugin.HitmarkerDisplayDuration.Value,
                "Basic hitmarker");

            cHm.Init(ValheimHitmarkerPlugin.HitmarkerSize.Value,
                ValheimHitmarkerPlugin.HitmarkerDisplayDuration.Value,
                "Critical hitmarker");

            // Store the hitmarkers in your plugin for later use
            ValheimHitmarkerPlugin.hitMarker = bHm;
            ValheimHitmarkerPlugin.criticalHitMarker = cHm;
        }
    }
}