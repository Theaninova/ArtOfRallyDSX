using System;
using System.Reflection;
using ArtOfRallyDSX.DSX;
using HarmonyLib;
using UnityModManagerNet;

namespace ArtOfRallyDSX
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Main
    {
        public static DsxSettings Settings;

        public static UnityModManager.ModEntry.ModLogger Logger;
        
        public static UnityModManager.ModEntry ModEntry;

        public static DualSenseConnection DualSenseConnection;

        // ReSharper disable once ArrangeTypeMemberModifiers
        // ReSharper disable once UnusedMember.Local
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            ModEntry = modEntry;
            Logger = modEntry.Logger;
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Settings = UnityModManager.ModSettings.Load<DsxSettings>(modEntry);
            modEntry.OnGUI = entry => Settings.Draw(entry);
            modEntry.OnSaveGUI = entry => Settings.Save(entry);
            modEntry.OnFixedGUI = DebugUI.Draw;

            try
            {
                DualSenseConnection = new DualSenseConnection();
            }
            catch (Exception e)
            {
                Logger.Critical(e.Message);
            }

            return true;
        }
    }
}