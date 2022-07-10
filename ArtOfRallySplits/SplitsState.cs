using JetBrains.Annotations;
using UnityEngine;

namespace ArtOfRallySplits
{
    public static class SplitsState
    {
        public static string Stage;

        public static int CurrentWaypointIndex = 0;
        
        public static float GhostTime = 0;
        
        public static float PlayerTime = 0;

        [CanBeNull] public static int[] SplitsConfig;

        [CanBeNull] public static float[] GhostTimes;

        [CanBeNull] public static float[] PlayerTimes;

        [CanBeNull] public static Vector3[] Waypoints;
    }
}