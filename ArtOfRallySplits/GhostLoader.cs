using System;
using HarmonyLib;
using UnityEngine;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable SuggestBaseTypeForParameter

namespace ArtOfRallySplits
{
    [HarmonyPatch(typeof(GhostManager), nameof(GhostManager.InitializeGhost))]
    public class GhostLoader
    {
        public static int Counter = 0;

        public static bool Initialized;

        public static void Prefix(bool ____createdGhost)
        {
            Initialized = ____createdGhost;
        }
        
        public static void Postfix(GhostManager __instance, GhostManager.GhostData ____currentData)
        {
            if (Initialized) return;
            Main.Logger.Log($"Attempting to load ghost ({Counter}, {__instance.GetHashCode()})");
            Counter++;
            if (SplitsState.SplitsConfig == null || SplitsState.Waypoints == null || ____currentData == null) return;

            var waypointsForReplay = FindWaypointsForReplay(____currentData._positions, SplitsState.Waypoints);

            SplitsState.GhostTimes = new float[SplitsState.SplitsConfig.Length];
            for (var i = 0; i < SplitsState.GhostTimes.Length; i++)
            {
                var index = Array.FindIndex(waypointsForReplay, it => it >= SplitsState.SplitsConfig[i]);
                if (index == -1) continue;
                SplitsState.GhostTimes[i] = ____currentData._timeData[index];
            }

            Main.Logger.Log($"Ghost Times: [{string.Join(", ", SplitsState.GhostTimes)}]");
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static int FindClosestWaypointToTransform(Vector3 transform, Vector3[] waypoints)
        {
            var currentWaypoint = 0;
            var distance = Vector3.Distance(waypoints[0], transform);

            for (var i = 1; i < waypoints.Length; i++)
            {
                if (Vector3.Distance(waypoints[i], transform) >= distance) continue;

                currentWaypoint = i;
                distance = Vector3.Distance(waypoints[currentWaypoint], transform);
            }

            return currentWaypoint;
        }

        private static int[] FindWaypointsForReplay(Vector3[] positions, Vector3[] waypoints)
        {
            var waypointsForReplay = new int[positions.Length];
            for (var i = 0; i < positions.Length; i++)
            {
                waypointsForReplay[i] = FindClosestWaypointToTransform(positions[i], waypoints);
            }

            return waypointsForReplay;
        }
    }
}