using UnityEngine;
using UnityModManagerNet;

namespace ArtOfRallySplits
{
    public class DebugUI
    {
        public static void Draw(UnityModManager.ModEntry modEntry, float fade)
        {
            
            if (Main.Settings.ShowCurrentWaypoint)
            {
                GUI.Label(
                    new Rect(0, 0, 200, 200),
                    $"Waypoint {SplitsState.CurrentWaypointIndex}\n"
                    + $"Stage {SplitsState.Stage} (using \"{Main.Settings.ConfigSet}\")\n"
                    + $"Player Split {SplitsState.PlayerTime:F}s\n"
                    + $"Ghost Split {SplitsState.GhostTime:F}s\n"
                    + $"Fade: {fade}"
                );
            }
        }
    }
}