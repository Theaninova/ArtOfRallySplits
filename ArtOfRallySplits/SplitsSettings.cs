using UnityEngine;
using UnityModManagerNet;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertToConstant.Global

namespace ArtOfRallySplits
{
    public class SplitsSettings : UnityModManager.ModSettings, IDrawable
    {
        [Header("General")]
        [Draw] public string ConfigSet = "myth";
        
        [Draw(DrawType.Toggle)] public bool HideGhost = true;

        [Header("Debug")]
        [Draw(DrawType.Toggle)] public bool ShowCurrentWaypoint = false;

        [Draw(DrawType.Toggle)] public bool AlwaysShowSplit = false;

        [Header("Customization")] [Draw]
        public float DisplayTime = 4f;

        [Draw] public Color AheadColor = new Color(0.71f, 1f, 0.74f);
        
        [Draw] public Color BehindColor = new Color(1f, 0.63f, 0.61f);

        [Draw] public Color BackgroundColor = Color.grey;

        [Header("Background")]
        
        [Draw(DrawType.Slider, Min = 0, Max = 600)] public int BackgroundWidth = 250;
        
        [Draw(DrawType.Slider, Min = 0, Max = 600)] public int BackgroundHeight = 100;
        
        [Draw(DrawType.Slider, Min = 0f, Max = 1f)]
        public float BackgroundY = 0.25f;

        [Draw(DrawType.Slider, Min = 0f, Max = 1f)]
        public float BackgroundX = 0.5f;

        [Header("Split Time")]

        [Draw(DrawType.Slider, Min = 1, Max = 100)]
        public int TimeFontSize = 38;
        
        [Draw(DrawType.Slider, Min = -100, Max = 100)]
        public int TimeOffsetY = 10;

        [Header("Split Delta")]
        
        [Draw(DrawType.Slider, Min = 1, Max = 100)]
        public int DeltaFontSize = 38;
        
        [Draw(DrawType.Slider, Min = -100, Max = 100)]
        public int DeltaOffsetY = -10;
        

        public void OnChange()
        {
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            if (HideGhost)
            {
                GhostHider.Hide();
            }
            else
            {
                GhostHider.Show();
            }

            Save(this, modEntry);
        }
    }
}