using JetBrains.Annotations;
using UnityEngine;

namespace ArtOfRallySplits
{
    public static class SplitsState
    {
        public static string Stage;

        public static int CurrentWaypointIndex = 0;
        
        public static int TimeSplitIndex = 0;

        public static float FinalGhostTime = -1;

        public static float FinalPlayerTime = 0;

        [CanBeNull] public static int[] SplitsConfig;

        [CanBeNull] public static float[] GhostTimes;

        [CanBeNull] public static float[] PlayerTimes;

        [CanBeNull] public static Vector3[] Waypoints;
    }
}