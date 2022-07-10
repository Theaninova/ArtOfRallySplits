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

        [Draw(DrawType.Toggle)] public bool HideGhost = true;

        [Header("Debug")] [Draw(DrawType.Toggle)]
        public bool ShowCurrentWaypoint = false;

        [Header("Customization")] [Draw] public float DisplayTime = 4f;

        [Draw] public float FadeTime = 0.3f;

        [Draw] public Color AheadColor = new Color(0.71f, 1f, 0.74f);

        [Draw] public Color BehindColor = new Color(1f, 0.63f, 0.61f);

        [Draw] public Color BackgroundColor = Color.grey;

        [Header("Split Time")] [Draw] public bool ShowSplitTime = true;

        [Draw] public bool AlwaysShowTime = true;

        [Draw(DrawType.Slider, Min = 1, Max = 100)]
        public int TimeFontSize = 38;

        [Draw(DrawType.PopupList)] public FontStyle TimeFontStyle = FontStyle.Bold;

        [Draw(DrawType.PopupList)] public TextAnchor TimeAnchor = TextAnchor.MiddleCenter;

        [Draw] public Vector2 TimePosition = new Vector2();

        [Draw] public Vector2 TimeSize = new Vector2();

        [Header("Split Delta")] [Draw] public bool ShowDelta = true;

        [Draw] public bool AlwaysShowDelta = false;

        [Draw(DrawType.Slider, Min = 1, Max = 100)]
        public int DeltaFontSize = 38;

        [Draw(DrawType.PopupList)] public FontStyle DeltaFontStyle = FontStyle.Bold;

        [Draw(DrawType.PopupList)] public TextAnchor DeltaAnchor = TextAnchor.MiddleCenter;

        [Draw] public Vector2 DeltaPosition = new Vector2();

        [Draw] public Vector2 DeltaSize = new Vector2();


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