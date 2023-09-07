using System;
using System.IO;
using BepInEx;
using UnityEngine;

namespace ValheimHitmarker.MonoBehaviours
{
    public abstract class HitMarkerDisplay : MonoBehaviour
    {
        private readonly string hitMarkerImageRelativePath = $"plugins/{ValheimHitmarker.ValheimHitmarkerPlugin.PluginName}/hitmarker.png"; // Relative path to the hit marker image within the plugin folder
        public float hitMarkerSize; // Size of the hit marker in pixels
        public float displayDuration; // How long the hit marker should be visible
        public bool isShowingHitMarker = false;
        public float displayStartTime;
        public string hitMarkerName;
        public Texture2D hitMarkerTexture;

        public bool HitmarkerShowingCurrently
        {
            get => isShowingHitMarker;
        }

        public void Init(float hitSize, float displayTime, string markerName)
        {
            hitMarkerSize = hitSize;
            displayDuration = displayTime;
            hitMarkerName = markerName;
        }

        private void Start()
        {
            ValheimHitmarkerPlugin.Log.LogInfo("Initializing hitmarker instance...");

            ReloadTexture();

            if (hitMarkerTexture == null)
            {
                // Handle the failure to load the texture
                ValheimHitmarkerPlugin.Log.LogError("Hit marker texture could not be loaded.");
            }

            ValheimHitmarkerPlugin.Log.LogInfo("Done initializing hitmarker instance!");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F7))
            {
                ReloadTexture();
            }

            if (isShowingHitMarker)
            {
                // Check if it's time to hide the hit marker
                if (Time.time - displayStartTime >= displayDuration)
                {
                    isShowingHitMarker = false;
                }
            }
        }

        public void ShowHitMarker()
        {
            ValheimHitmarkerPlugin.Log.LogInfo($"Showing {hitMarkerName}...");
            isShowingHitMarker = true;
            displayStartTime = Time.time;
        }

        /// <summary>
        /// Gets calledi in the OnGUI loop and is responsible to show/style the hitmarker
        /// </summary>
        public virtual void StyleHitmarker()
        {
            if (isShowingHitMarker && hitMarkerTexture != null)
            {
                float timeSinceDisplayStart = Time.time - displayStartTime;

                // Calculate the alpha value based on time to achieve fade-out effect
                float alpha = Mathf.Lerp(1f, 0f, timeSinceDisplayStart / displayDuration);

                // Apply the alpha value to the hit marker color
                Color originalColor = GUI.color;
                GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

                // Calculate the position to draw the hit marker at the center of the screen
                float centerX = Screen.width * 0.5f;
                float centerY = Screen.height * 0.5f;

                // Draw the hit marker using the loaded texture
                GUI.DrawTexture(
                    new Rect(centerX - hitMarkerSize * 0.5f, centerY - hitMarkerSize * 0.5f, hitMarkerSize, hitMarkerSize),
                    hitMarkerTexture);

                // Restore the original GUI color
                GUI.color = originalColor;

                // Check if it's time to hide the hit marker
                if (timeSinceDisplayStart >= displayDuration)
                {
                    isShowingHitMarker = false;
                }
            }
        }

        public void OnGUI()
        {
            StyleHitmarker();
        }

        public void ReloadTexture()
        {
            string pluginPath = Path.GetDirectoryName(Paths.PluginPath);
            string hitMarkerImagePath = Path.Combine(pluginPath, hitMarkerImageRelativePath);

            hitMarkerTexture = LoadTextureFromFile(hitMarkerImagePath);

            ValheimHitmarkerPlugin.Log.LogInfo($"Loading {hitMarkerName} texture from: {hitMarkerImagePath}");
        }

        // Load a texture from a file path
        private Texture2D LoadTextureFromFile(string filePath)
        {
            Texture2D texture = new Texture2D(2, 2);
            byte[] imageData = File.ReadAllBytes(filePath);

            try
            {
                if (ImageConversion.LoadImage(texture, imageData, false))
                {
                    return texture;
                }
                else
                {
                    ValheimHitmarkerPlugin.Log.LogError("Failed to load texture from file: " + filePath);
                    Destroy(texture); // Clean up the texture if loading fails
                }
            }
            catch (Exception e)
            {
                ValheimHitmarkerPlugin.Log.LogError("Failed to load texture from file: " + filePath);
            }

            return null; // Return null to indicate failure
        }
    }
}