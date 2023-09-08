using System;
using System.IO;
using UnityEngine;

namespace ValheimHitmarker.Utils
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class ModUtils
    {
        public static Texture2D LoadTextureFromFile(string filePath)
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
                    texture = null; //Cleanup
                }
            }
            catch (Exception)
            {
                ValheimHitmarkerPlugin.Log.LogError("Failed to load texture from file: " + filePath);
            }

            return null; // Return null to indicate failure
        }
    }
}