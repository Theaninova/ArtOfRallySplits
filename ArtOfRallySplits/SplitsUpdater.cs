// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

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

            if (index != -1 && LastWaypoint < SplitsState.CurrentWaypointIndex)
            {
                SplitsState.PlayerTime = GameEntryPoint.EventManager.stageTimerManager.GetStageTimeMS();
                SplitsState.GhostTime = SplitsState.GhostTimes[index];
                SplitsUI.ResetFade();
            }

            LastWaypoint = SplitsState.CurrentWaypointIndex;
        }
    }
}