// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

using System.Linq;
using FluffyUnderware.DevTools.Extensions;
using HarmonyLib;

namespace ArtOfRallySplits
{
    [HarmonyPatch(typeof(OutOfBoundsManager), "FixedUpdate")]
    public class SplitsUpdater
    {
        private static int LastWaypoint;

        // ReSharper disable twice InconsistentNaming
        public static void Postfix(int ___CurrentWaypointIndex)
        {
            SplitsState.CurrentWaypointIndex = ___CurrentWaypointIndex;

            if (SplitsState.SplitsConfig == null || SplitsState.GhostTimes == null) return;

            var index = SplitsState.SplitsConfig.IndexOf(SplitsState.CurrentWaypointIndex);
            if (___CurrentWaypointIndex < SplitsState.SplitsConfig.First())
            {
                SplitsState.PlayerTime = -1;
                SplitsState.GhostTime = -1;
                SplitsState.PlayerTimes = new float[SplitsState.SplitsConfig.Length];
                for (var i = 0; i < SplitsState.PlayerTimes.Length; i++)
                {
                    SplitsState.PlayerTimes[i] = -1;
                }
            }
            
            if (index != -1 && SplitsState.PlayerTimes != null && LastWaypoint < SplitsState.CurrentWaypointIndex)
            {
                SplitsState.PlayerTimes[index] = GameEntryPoint.EventManager.stageTimerManager.GetStageTimeMS();

                SplitsState.PlayerTime = SplitsState.PlayerTimes[index];
                SplitsState.GhostTime = SplitsState.GhostTimes[index];
                SplitsUI.ResetFade();
            }

            LastWaypoint = SplitsState.CurrentWaypointIndex;
        }
    }
}