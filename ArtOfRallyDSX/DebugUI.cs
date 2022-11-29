using UnityEngine;
using UnityModManagerNet;

namespace ArtOfRallyDSX
{
    public static class DebugUI
    {
        public static float Ffb = 0f;
        
        public static void Draw(UnityModManager.ModEntry modEntry)
        {
            if (!Main.Settings.DrawDebugUI) return;

            GUI.Label(new Rect (0, 0, 0, 0), $"FFB: {Ffb}");
        }
    }
}