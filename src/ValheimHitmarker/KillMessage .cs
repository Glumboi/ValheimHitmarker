﻿using System.Collections.Generic;
using BepInEx;
using System.IO;
using UnityEngine;
using ValheimHitmarker.Utils;
using System.Collections;

namespace ValheimHitmarker.MonoBehaviours
{
    public class KillMessage : MonoBehaviour
    {
        public Texture2D killTexture; // The texture to represent a kill
        private List<KillEntry> killEntries = new List<KillEntry>();
        private int maxKillCount = 10; // Maximum number of kills to display
        private float displayDuration = 4.5f;
        private float fadeDuration = 2.0f;
        private float killSpacing = 40f; // Adjust this for spacing between kill textures

        private GUIStyle textStyle;

        private void Start()
        {
            // Load the kill texture from a file
            killTexture = ModUtils.LoadTextureFromFile(Path.Combine(Path.GetDirectoryName(Paths.PluginPath), $"plugins/{ValheimHitmarkerPlugin.PluginName}/kill.png"));
        }

        private void OnGUI()
        {
            // Initialize the GUI style for the text
            textStyle = new GUIStyle(GUI.skin.label);
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.alignment = TextAnchor.MiddleCenter;
            textStyle.normal.textColor = Color.white;

            float centerY = Screen.height * 0.7f; // Adjust the Y position for a lower placement

            List<KillEntry> entriesToRemove = new List<KillEntry>();
            float totalWidth = 0;

            foreach (KillEntry entry in killEntries)
            {
                // Calculate the alpha based on the elapsed time
                float alpha = 1f - Mathf.Clamp01((Time.time - entry.displayStartTime) / fadeDuration);

                // Apply the alpha to the kill texture
                Color originalColor = GUI.color;
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

                // Calculate the position to draw the kill image centered
                float startX = (Screen.width - totalWidth) / 2;

                // Display each kill texture
                GUI.DrawTexture(new Rect(startX, centerY, 40, 40), killTexture);

                /*Display a small bold text at the bottom
                 string labelText = entry.killText; Use the text from the entry
                 float labelX = startX;
                 float labelY = centerY + 40;

                GUI.Label(new Rect(labelX, labelY, killSpacing, 20), labelText, textStyle);*/

                // Restore the original GUI color
                GUI.color = originalColor;

                totalWidth += 45; // Adjust the spacing between kills
                totalWidth += killSpacing; // Add fixed spacing between kill textures

                // Check if the entry is ready to be removed
                if (Time.time - entry.displayStartTime >= displayDuration)
                {
                    entriesToRemove.Add(entry);
                }
            }

            // Remove entries that have faded out completely
            foreach (KillEntry entry in entriesToRemove)
            {
                killEntries.Remove(entry);
            }

            // Ensure layout updates
            GUILayout.FlexibleSpace();
        }

        private IEnumerator ExecuteAfterTime(float time)
        {
            yield return new WaitForSeconds(time);

            // Code to execute after the delay
            ShowKill("[A LONG NAME HERE]"); // Default text
        }

        public void DebugShow()
        {
            StartCoroutine(ExecuteAfterTime(0.2f));
            StartCoroutine(ExecuteAfterTime(0.4f));
            StartCoroutine(ExecuteAfterTime(0.5f));
        }

        public void ShowKill(string killText)
        {
            if (killEntries.Count >= maxKillCount)
            {
                // Remove the oldest kill if the list is full
                killEntries.RemoveAt(0);
            }

            // Add a new kill entry with the specified text
            KillEntry entry = new KillEntry();
            entry.killText = killText;
            entry.displayStartTime = Time.time;
            killEntries.Add(entry);
        }

        private class KillEntry
        {
            public string killText;
            public float displayStartTime;
        }
    }
}