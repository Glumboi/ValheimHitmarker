using System;
using HarmonyLib;

namespace ValheimHitmarker.Patches
{
    [HarmonyPatch(typeof(Character))]
    internal static class CharacterPatches
    {
        [HarmonyPatch("ApplyDamage", new Type[] {
            typeof(HitData),
            typeof(bool),
            typeof(bool),
            typeof(HitData.DamageModifier)})]

        [HarmonyPostfix]
        private static void Postfix_ApplyDamage(Character __instance,
            HitData hit,
            bool showDamageText,
            bool triggerEffects,
            HitData.DamageModifier mod = HitData.DamageModifier.Normal)
        {
            try
            {
                if (hit.GetAttacker() != Player.m_localPlayer) return;

#if DEBUG

                ValheimHitmarkerPlugin.Log.LogInfo($"Attacker ID: {hit.GetAttacker().GetZDOID().UserID}");

                ValheimHitmarkerPlugin.Log.LogInfo("Hit something...");
                ValheimHitmarkerPlugin.Log.LogInfo($"Target dead? {__instance.IsDead()}");
                ValheimHitmarkerPlugin.Log.LogInfo($"Target health: {__instance.GetHealth()}\n");
#endif

                if (__instance.GetHealth() <= 0)//hit.GetTotalDamage())
                {
                    ValheimHitmarkerPlugin.criticalHitMarker.ShowHitMarker();
#if DEBUG
                    ValheimHitmarkerPlugin.killMessage.DebugShow();
#else
                    ValheimHitmarkerPlugin.killMessage.ShowKill($"[{__instance.GetHoverName()}]");
#endif
                    return;
                }

                ValheimHitmarkerPlugin.hitMarker.ShowHitMarker();
            }
            catch (NullReferenceException)
            {
                return;
            }
        }
    }
}