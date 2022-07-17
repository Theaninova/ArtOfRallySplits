using UnityEngine;
using UnityModManagerNet;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertToConstant.Global

namespace ArtOfRallySplits
{
    public class SplitsSettings : UnityModManager.ModSettings, IDrawable
    {
        [Header("General")] [Draw(DrawType.Auto)]
        public string ConfigSet = "myth";

        [Draw(DrawType.Auto)] public string AssetSet = "aor";

        [Header("Debug")] [Draw(DrawType.Toggle)]
        public bool ShowCurrentWaypoint = false;

        [Header("Customization")] [Draw] public float DisplayTime = 3f;

        [Draw] public float FadeTime = 0.1f;

        [Draw] public Color AheadColor = new Color(0.3250f, 0.325f, 1.0f, 0.7f);

        [Draw] public Color BehindColor = new Color(1f, 0.325f, 0.325f, 0.7f);

        [Draw] public Color BackgroundColor = new Color(0.4f, 0.4f, 0.4f, 0.7f);

        [Draw] public Color DeltaTextColor = Color.white;

        [Draw] public Color SplitsTextColor = Color.white;

        [Header("Split Time")] [Draw] public bool ShowSplitTime = true;

        [Draw] public bool AlwaysShowTime = true;

        [Draw(DrawType.Slider, Min = 1, Max = 100)]
        public int TimeFontSize = 22;

        [Draw(DrawType.PopupList)] public FontStyle TimeFontStyle = FontStyle.Normal;

        [Draw(DrawType.PopupList)] public TextAnchor TimeAnchor = TextAnchor.UpperRight;

        [Draw] public Vector2 TimePosition = new Vector2(0, 120);

        [Draw] public Vector2 TimeSize = new Vector2(180, 200);

        [Header("Split Delta")] [Draw] public bool ShowDelta = true;

        [Draw] public bool AlwaysShowDelta = false;

        [Draw(DrawType.Slider, Min = 1, Max = 100)]
        public int DeltaFontSize = 35;

        [Draw(DrawType.PopupList)] public FontStyle DeltaFontStyle = FontStyle.Normal;

        [Draw(DrawType.PopupList)] public TextAnchor DeltaAnchor = TextAnchor.MiddleCenter;

        [Draw] public Vector2 DeltaPosition = new Vector2(0, 300);

        [Draw] public Vector2 DeltaSize = new Vector2(220, 70);


        public void OnChange()
        {
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            // if (HideGhost)
            // {
            //     GhostHider.Hide();
            // }
            // else
            // {
            //     GhostHider.Show();
            // }

            Save(this, modEntry);
        }
    }
}