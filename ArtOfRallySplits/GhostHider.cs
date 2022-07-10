using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace ArtOfRallySplits
{
    [HarmonyPatch(typeof(GhostManager), "SetOpaque")]
    public static class GhostHider
    {
        private static readonly MethodInfo SetOpaqueMethod =
            typeof(Renderer).GetMethod("SetOpaque", BindingFlags.Instance | BindingFlags.NonPublic);
        
        private static readonly MethodInfo SetTransparentMethod =
            typeof(Renderer).GetMethod("SetTransparent", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void Hide()
        {
            if (LoadGhostHider.Instance == null || SetTransparentMethod == null) return;
            SetTransparentMethod.Invoke(LoadGhostHider.Instance, null);
        }

        public static void Show()
        {
            if (LoadGhostHider.Instance == null || SetOpaqueMethod == null) return;
            SetOpaqueMethod.Invoke(LoadGhostHider.Instance, null);
        }

        public static bool Prefix()
        {
            // return !Main.Settings.HideGhost;
            return true;
        }
    }

    [HarmonyPatch(typeof(GhostManager), nameof(GhostManager.InitializeGhost))]
    public static class LoadGhostHider
    {
        [CanBeNull] public static GhostManager Instance;
        
        public static void Postfix(GhostManager __instance)
        {
            Instance = __instance;
        }
    }
}