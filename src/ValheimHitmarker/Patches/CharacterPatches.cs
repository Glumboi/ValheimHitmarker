using System;
using HarmonyLib;

namespace ValheimHitmarker.Patches
{
    [HarmonyPatch(typeof(Character))]
    internal static class CharacterPatches
    {
        [HarmonyPatch("RPC_Damage", new Type[] { typeof(long), typeof(HitData) })]
        [HarmonyPostfix]
        private static void Postfix_RPC_Damage(Character __instance, long sender, HitData hit)
        {
            //Check if the attacking gameobject is the player and if so, display the hitmarker
            if (hit.GetAttacker() == Player.m_localPlayer ||
                hit.GetAttacker().GetZDOID().UserID == sender)
            {
#if DEBUG
                ValheimHitmarkerPlugin.Log.LogInfo($"Sender ID: {sender}");
                ValheimHitmarkerPlugin.Log.LogInfo($"Attacker ID: {hit.GetAttacker().GetZDOID().UserID}");

                ValheimHitmarkerPlugin.Log.LogInfo("Hit something...");
                ValheimHitmarkerPlugin.Log.LogInfo($"Target dead? {__instance.IsDead()}");
                ValheimHitmarkerPlugin.Log.LogInfo($"Target health: {__instance.GetHealth()}");
#endif

                if (__instance.GetHealth() <= 0)//hit.GetTotalDamage())
                {
                    ValheimHitmarkerPlugin.criticalHitMarker.ShowHitMarker();
                    ValheimHitmarkerPlugin.killMessage.ShowKill();

                    return;
                }

                ValheimHitmarkerPlugin.hitMarker.ShowHitMarker();
            }
        }
    }
}