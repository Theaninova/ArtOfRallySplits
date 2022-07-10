using System;
using UnityEngine;
using UnityModManagerNet;
using Color = UnityEngine.Color;

namespace ArtOfRallySplits
{
    public static class SplitsUI
    {
        private const float DisplayTime = 2;

        private const float FadeTime = 0.5f;

        private static float _lastTimestamp;

        private static readonly GUIStyle TimeStyle = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.LowerCenter,
        };

        private static readonly GUIStyle DeltaStyle = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperCenter,
        };

        private static readonly GUIStyleState Normal = new GUIStyleState();

        public static void ResetFade()
        {
            _lastTimestamp = Time.time;
        }

        public static void Draw(UnityModManager.ModEntry modEntry)
        {
            var time = Time.time - _lastTimestamp;
            var fade = Mathf.Clamp(time > FadeTime
                    ? 1f - (time + FadeTime - DisplayTime) / FadeTime
                    : time / FadeTime, 0, 1
            );
            if (Main.Settings.AlwaysShowSplit)
            {
                fade = 1;
            }

            if (Main.Settings.ShowCurrentWaypoint)
            {
                GUI.Label(
                    new Rect(0, 0, 200, 200),
                    $"Waypoint {SplitsState.CurrentWaypointIndex}\n"
                    + $"Player Split {SplitsState.PlayerTime:F}s\n"
                    + $"Ghost Split {SplitsState.GhostTime:F}s\n"
                    + $"Fade: {fade}"
                );
            }

            var difference = SplitsState.PlayerTime - SplitsState.GhostTime;
            var sign = Math.Sign(difference) != -1 ? "+" : "-";
            var background = Main.Settings.BackgroundColor;

            var timeText = $"{TimeFormatter.GetCachedFormattedTime(SplitsState.PlayerTime)}";
            var deltaText =          $"{sign}{TimeFormatter.GetCachedFormattedTime(Mathf.Abs(difference))}";

            GUI.color = new Color(1f, 1f, 1f, fade);
            Normal.textColor = Math.Sign(difference) != -1 ? Main.Settings.BehindColor : Main.Settings.AheadColor;
            TimeStyle.normal = Normal;
            TimeStyle.fontSize = Main.Settings.TimeFontSize;
            DeltaStyle.normal = Normal;
            DeltaStyle.fontSize = Main.Settings.DeltaFontSize;

            var rect = new Rect(
                Screen.width * Main.Settings.BackgroundX - Main.Settings.BackgroundWidth / 2f,
                Screen.height * Main.Settings.BackgroundY - Main.Settings.BackgroundHeight / 2f,
                Main.Settings.BackgroundWidth,
                Main.Settings.BackgroundHeight);

            GUI.Box(rect, Texture2D.grayTexture);
            GUI.Label(new Rect(rect.x, rect.y + Main.Settings.TimeOffsetY, rect.width, rect.height), timeText,
                TimeStyle);
            GUI.Label(new Rect(rect.x, rect.y + Main.Settings.DeltaOffsetY, rect.width, rect.height), deltaText,
                DeltaStyle);
        }
    }
}