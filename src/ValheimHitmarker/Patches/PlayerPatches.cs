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
            if (Input.GetKeyDown(KeyCode.F8))
            {
                InstantiateHitmarkers();
            }
        }

        private static void InstantiateHitmarkers()
        {
            GameObject hmObj = GameObject.Find("Hitmarkers");

            if (hmObj == null)
            {
                // Create a new GameObject for hitmarkers
                hmObj = new GameObject("Hitmarkers");

                // Add the BasicHitmarker and CriticalHitmarker components to the GameObject
                BasicHitmarker bHm = hmObj.AddComponent<BasicHitmarker>();
                CriticalHitmarker cHm = hmObj.AddComponent<CriticalHitmarker>();

                // Initialize the hitmarkers
                bHm.Init(ValheimHitmarkerPlugin.HitmarkerSize.Value,
                    ValheimHitmarkerPlugin.HitmarkerDisplayDuration.Value,
                    "Basic hitmarker");

                cHm.Init(ValheimHitmarkerPlugin.HitmarkerSize.Value,
                    ValheimHitmarkerPlugin.HitmarkerDisplayDuration.Value,
                    "Critical hitmarker");

                ValheimHitmarkerPlugin.hitMarker = bHm;
                ValheimHitmarkerPlugin.criticalHitMarker = cHm;

                return;
            }

            GameObject.Destroy(hmObj);
            InstantiateHitmarkers();
        }
    }
}