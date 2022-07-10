using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityModManagerNet;
using Color = UnityEngine.Color;

namespace ArtOfRallySplits
{
    public static class SplitsUI
    {
        private static float _lastTimestamp;

        public static readonly GUIStyle TimeStyle = new GUIStyle(GUI.skin.box)
        {
            alignment = TextAnchor.MiddleCenter,
            normal =
            {
                background = Texture2D.whiteTexture
            }
        };

        public static readonly GUIStyle DeltaStyle = new GUIStyle(GUI.skin.box)
        {
            alignment = TextAnchor.MiddleCenter,
            normal =
            {
                background = Texture2D.whiteTexture
            }
        };

        public static void ResetFade()
        {
            _lastTimestamp = Time.time;
        }

        public static void Draw(UnityModManager.ModEntry modEntry)
        {
            if (GameEntryPoint.EventManager.status != EventStatusEnums.EventStatus.UNDERWAY &&
                GameEntryPoint.EventManager.status != EventStatusEnums.EventStatus.WAITING_TO_BEGIN) return;

            var time = Time.time - _lastTimestamp;
            var fade = Mathf.Clamp(time > Main.Settings.FadeTime
                    ? 1f - (time + Main.Settings.FadeTime - Main.Settings.DisplayTime) / Main.Settings.FadeTime
                    : time / Main.Settings.FadeTime, 0, 1
            );

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
            var color = Math.Sign(difference) != -1 ? Main.Settings.BehindColor : Main.Settings.AheadColor;

            if (Main.Settings.ShowDelta)
            {
                GUI.color = new Color(1f, 1f, 1f, Main.Settings.AlwaysShowDelta ? 1f : fade);
                GUI.backgroundColor = Main.Settings.BackgroundColor;

                DeltaStyle.normal.textColor = color;
                DeltaStyle.fontSize = Main.Settings.DeltaFontSize;
                DeltaStyle.fontStyle = Main.Settings.DeltaFontStyle;
                GUI.Box(
                    Anchor(Main.Settings.DeltaAnchor, new Rect(
                        Main.Settings.DeltaPosition.x,
                        Main.Settings.DeltaPosition.y,
                        Main.Settings.DeltaSize.x,
                        Main.Settings.DeltaSize.y
                    )),
                    $"{sign}{TimeFormatter.GetCachedFormattedTime(Mathf.Abs(difference))}",
                    DeltaStyle
                );
            }

            if (Main.Settings.ShowSplitTime && SplitsState.PlayerTimes != null)
            {
                GUI.color = new Color(1f, 1f, 1f, Main.Settings.AlwaysShowTime ? 1f : fade);
                GUI.backgroundColor = Main.Settings.BackgroundColor;

                TimeStyle.normal.textColor = Color.white;
                TimeStyle.fontSize = Main.Settings.TimeFontSize;
                TimeStyle.fontStyle = Main.Settings.TimeFontStyle;
                GUI.Box(
                    Anchor(Main.Settings.TimeAnchor, new Rect(
                        Main.Settings.TimePosition.x,
                        Main.Settings.TimePosition.y,
                        Main.Settings.TimeSize.x,
                        Main.Settings.TimeSize.y
                    )),
                    string.Join("\n",
                        SplitsState.PlayerTimes.Select(it =>
                            it < 0 ? "--:--:---" : TimeFormatter.GetCachedFormattedTime(it))
                    ),
                    TimeStyle
                );
            }
        }

        private static Rect Anchor(TextAnchor anchor, Rect position)
        {
            return anchor switch
            {
                TextAnchor.MiddleCenter => new Rect(
                    Screen.width / 2f - position.width / 2f + position.x,
                    Screen.height / 2f - position.height / 2f - position.y,
                    position.width,
                    position.height
                ),
                TextAnchor.UpperRight => new Rect(
                    Screen.width - position.width - position.x,
                    position.y,
                    position.width,
                    position.height
                ),
                _ => throw new ArgumentOutOfRangeException(nameof(anchor), anchor, "Unsupported anchor")
            };
        }
    }

    [HarmonyPatch(typeof(StageTimerManager), "Update")]
    public static class StageTimeDisplay
    {
        public static void Postfix(Text ___TimeDisplay)
        {
            SplitsUI.DeltaStyle.font = ___TimeDisplay.font;
            SplitsUI.TimeStyle.font = ___TimeDisplay.font;
        }
    }
}