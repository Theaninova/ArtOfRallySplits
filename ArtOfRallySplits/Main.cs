using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;

namespace ArtOfRallySplits
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Main
    {
        public static SplitsSettings Settings;

        public static UnityModManager.ModEntry.ModLogger Logger;
        
        public static UnityModManager.ModEntry ModEntry;

        // ReSharper disable once ArrangeTypeMemberModifiers
        // ReSharper disable once UnusedMember.Local
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            ModEntry = modEntry;
            Logger = modEntry.Logger;
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Settings = UnityModManager.ModSettings.Load<SplitsSettings>(modEntry);
            modEntry.OnGUI = entry => Settings.Draw(entry);
            modEntry.OnSaveGUI = entry => Settings.Save(entry);
            modEntry.OnFixedGUI = SplitsUI.Draw;

            return true;
        }
    }
}