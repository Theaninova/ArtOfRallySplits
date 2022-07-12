using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
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
            var time = Time.time - _lastTimestamp;
            var fade = Mathf.Clamp(time > Main.Settings.FadeTime
                    ? 1f - (time + Main.Settings.FadeTime - Main.Settings.DisplayTime) / Main.Settings.FadeTime
                    : time / Main.Settings.FadeTime, 0, 1
            );

            DebugUI.Draw(modEntry, fade);

            if (GameEntryPoint.EventManager.status == EventStatusEnums.EventStatus.IN_PRE_STAGE_SCREEN) return;

            var relativePlayerTimes = MakeRelativeTimes(SplitsState.PlayerTimes, SplitsState.FinalPlayerTime);
            var relativeGhostTimes = MakeRelativeTimes(SplitsState.GhostTimes, SplitsState.FinalGhostTime);

            var difference = relativePlayerTimes[SplitsState.TimeSplitIndex] -
                             relativeGhostTimes[SplitsState.TimeSplitIndex];
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
                    string.Join("\n", relativePlayerTimes.Select(MakeTimeLabel)),
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

        private static string MakeTimeLabel(float time)
        {
            return time < 0
                ? "--:--:---"
                : TimeFormatter.GetCachedFormattedTime(time);
        }

        private static float[] MakeRelativeTimes(float[] times, float last)
        {
            var result = new float[times.Length + 1];
            for (var i = 0; i < times.Length; i++)
            {
                result[i] = times[i] - Mathf.Abs(i == 0 ? 0 : times[i - 1]);
            }

            result[result.Length - 1] = last - Mathf.Abs(times.Length == 0 ? 0 : times[times.Length - 1]);
            return result;
        }
    }

    [HarmonyPatch(typeof(StageTimerManager), "Update")]
    public static class StageTimeDisplay
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(Text ___TimeDisplay)
        {
            SplitsUI.DeltaStyle.font = ___TimeDisplay.font;
            SplitsUI.TimeStyle.font = ___TimeDisplay.font;
        }
    }
}