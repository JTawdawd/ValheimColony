using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using HarmonyLib;
using Jotunn.Utils;

namespace JotunnModStub
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class ValheimColony : BaseUnityPlugin
    {
        public const string PluginGUID = "com.jotunn.ValheimColony";
        public const string PluginName = "ValheimColony";
        public const string PluginVersion = "0.0.1";
        private readonly Harmony harmony = new Harmony("ValheimColony");

        AssetBundle testBundle;
        GameObject newhoneyPrefab;
        CustomItem newhoney;

        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            // Jotunn comes with MonoMod Detours enabled for hooking Valheim's code
            // https://github.com/MonoMod/MonoMod
            On.FejdStartup.Awake += FejdStartup_Awake;
            
            // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
            Jotunn.Logger.LogInfo("ModStub has landed");

            harmony.PatchAll();

            LoadAssets();

            // To learn more about Jotunn's features, go to
            // https://valheim-modding.github.io/Jotunn/tutorials/overview.html
        }

        private void FejdStartup_Awake(On.FejdStartup.orig_Awake orig, FejdStartup self)
        {
            // This code runs before Valheim's FejdStartup.Awake
            Jotunn.Logger.LogInfo("FejdStartup is going to awake");

            // Call this method so the original game method is invoked
            orig(self);

            // This code runs after Valheim's FejdStartup.Awake
            Jotunn.Logger.LogInfo("FejdStartup has awoken");
        }

        void LoadAssets()
        {
            testBundle = AssetUtils.LoadAssetBundle("JotunnModStub/Assets/newhoney");
            newhoneyPrefab = testBundle.LoadAsset<GameObject>("Honey 1");
            newhoney = new CustomItem(newhoneyPrefab, fixReference: false, new Jotunn.Configs.ItemConfig
            {
                Amount =1,
                Name = "New Honey",
                Description = "Jordan Stinks"
            });

            ItemManager.Instance.AddItem(newhoney);
        }

        void OnDestroy()
        {
            harmony.UnpatchSelf();
        }

        [HarmonyPatch(typeof(Bed), nameof(Bed.Interact))]
        class Sleep_Patch
        {
            static bool Prefix(Humanoid human, bool repeat, bool alt, Transform ___m_spawnPoint)
            {
                human.AttachStart(___m_spawnPoint, null, hideWeapons: true, isBed: true, onShip: false, "attach_bed", new Vector3(0f, 0.5f, 0f));
                Instantiate(ItemManager.Instance.GetItem("Honey 1").ItemPrefab, ___m_spawnPoint.position, Quaternion.identity);
                return false;
            }
        }
    }
}