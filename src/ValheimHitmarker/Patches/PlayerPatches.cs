using HarmonyLib;
using UnityEngine;
using ValheimHitmarker.MonoBehaviours;

namespace ValheimHitmarker.Patches
{
    [HarmonyPatch(typeof(Player))]
    internal static class PlayerPatches
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void Postfix_Start()
        {
            InstantiateHitmarkers();
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void Postfix_Update()
        {
            //TODO: Update hotkey keys

            //KeyCode.F7
            if (Input.GetKeyDown(ValheimHitmarkerPlugin.RealodHitmarkerTexturesShortcut.Value))
            {
                ReloadHitmarkerTextures();
            }

            //KeyCode.F8
            if (Input.GetKeyDown(ValheimHitmarkerPlugin.RealodHitmarkersShortcut.Value))
            {
                InstantiateHitmarkers();
            }
        }

        private static void ReloadHitmarkerTextures()
        {
            GameObject hmObj = GameObject.Find("Hitmarkers");
            hmObj.GetComponent<BasicHitmarker>().ReloadTexture();
            hmObj.GetComponent<CriticalHitmarker>().ReloadTexture();
        }

        private static void InstantiateHitmarkers()
        {
            GameObject hmObj = GameObject.Find("Hitmarkers");

            if (hmObj != null)
                GameObject.Destroy(hmObj);

            // Create a new GameObject for hitmarkers
            hmObj = new GameObject("Hitmarkers");

            // Add the BasicHitmarker and CriticalHitmarker components to the GameObject
            BasicHitmarker bHm = hmObj.AddComponent<BasicHitmarker>();
            CriticalHitmarker cHm = hmObj.AddComponent<CriticalHitmarker>();
            KillMessage kM = hmObj.AddComponent<KillMessage>();

            // Initialize the hitmarkers
            bHm.Init(ValheimHitmarkerPlugin.HitmarkerSize.Value,
                ValheimHitmarkerPlugin.HitmarkerDisplayDuration.Value,
                "Basic hitmarker");

            cHm.Init(ValheimHitmarkerPlugin.HitmarkerSize.Value,
                ValheimHitmarkerPlugin.HitmarkerDisplayDuration.Value,
                "Critical hitmarker");

            ValheimHitmarkerPlugin.hitMarker = bHm;
            ValheimHitmarkerPlugin.criticalHitMarker = cHm;
            ValheimHitmarkerPlugin.killMessage = kM;
        }
    }
}