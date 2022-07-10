using System;
using System.IO;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ArtOfRallySplits
{
    public static class SplitsConfigLoader
    {
        private const string PaceNoteConfigPath = "SplitConfigs";

        [CanBeNull]
        public static int[] LoadPaceNoteConfig(string stage)
        {
            var path = Path.Combine(Main.ModEntry.Path, PaceNoteConfigPath, Main.Settings.ConfigSet, $"{stage}.csv");

            if (!File.Exists(path))
            {
                Main.Logger.Warning($"No config file found for stage {stage} (tried set {Main.Settings.ConfigSet})");
                return null;
            }

            try
            {
                return File.ReadLines(path).First().Split(',').Select(int.Parse).ToArray();
            }
            catch (Exception e)
            {
                Main.Logger.Error($"Faulty Split Config: {e.Message}");
            }

            return null;
        }
    }
    
    [HarmonyPatch(typeof(OutOfBoundsManager), nameof(OutOfBoundsManager.Start))]
    public class SplitsLoader
    {
        public static void Postfix(Vector3[] ____cachedWaypoints)
        {
            SplitsState.Waypoints = ____cachedWaypoints;
            
            var stage = GameModeManager.RallyManager.RallyData.GetCurrentStage();
            var stageKey = $"{AreaManager.GetAreaStringNotLocalized(stage.Area)}_{stage.Name}";

            Main.Logger.Log($"Stage key: {stageKey}, WaypointCount: {____cachedWaypoints.Length}");

            SplitsState.SplitsConfig =
                SplitsConfigLoader.LoadPaceNoteConfig(stageKey);
            if (SplitsState.SplitsConfig == null)
            {
                Main.Logger.Warning($"Missing pace note config for stage {stageKey}");
            }
            else
            {
                Main.Logger.Log($"Loaded Splits: [{string.Join(", ", SplitsState.SplitsConfig)}]");
            }
        }
    }
}