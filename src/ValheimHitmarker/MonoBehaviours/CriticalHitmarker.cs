using UnityEngine;

namespace ValheimHitmarker.MonoBehaviours
{
    public class CriticalHitmarker : HitMarkerDisplay
    {
        public override void StyleHitmarker()
        {
            if (isShowingHitMarker && hitMarkerTexture != null)
            {
                float timeSinceDisplayStart = Time.time - displayStartTime;

                // Calculate the alpha value based on time to achieve fade-out effect
                float alpha = Mathf.Lerp(1f, 0f, timeSinceDisplayStart / displayDuration);

                // Apply the alpha value to the hit marker color
                Color critColor = Color.red;
                GUI.color = new Color(critColor.r, critColor.g, critColor.b, alpha);

                // Calculate the position to draw the hit marker at the center of the screen
                float centerX = Screen.width * 0.5f;
                float centerY = Screen.height * 0.5f;

                // Draw the hit marker using the loaded texture
                GUI.DrawTexture(
                    new Rect(centerX - hitMarkerSize * 0.5f, centerY - hitMarkerSize * 0.5f, hitMarkerSize, hitMarkerSize),
                    hitMarkerTexture);

                // Restore the original GUI color
                GUI.color = critColor;

                // Check if it's time to hide the hit marker
                if (timeSinceDisplayStart >= displayDuration)
                {
                    isShowingHitMarker = false;
                }
            }
        }
    }
}