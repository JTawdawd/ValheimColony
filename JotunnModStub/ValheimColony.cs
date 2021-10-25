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

            //Loads new items
            LoadAssets();

            //Load new give command for new honey
            CommandManager.Instance.AddConsoleCommand(new NewItemCommand());
            CommandManager.Instance.AddConsoleCommand(new ResetHungerCommand());

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

        //currently creates new honey item from a side load location
        void LoadAssets()
        {
            testBundle = AssetUtils.LoadAssetBundle("JotunnModStub/Assets/newhoney");
            newhoneyPrefab = testBundle.LoadAsset<GameObject>("newHoney");

            //Makes the item into a custom item with some details
            newhoney = new CustomItem(newhoneyPrefab, fixReference: true, new Jotunn.Configs.ItemConfig
            {
                Amount =1,
                Name = "New Honey",
                Description = "Jordan Stinks"
            });

            //Adds item to item manager
            ItemManager.Instance.AddItem(newhoney);
        }

        void OnDestroy()
        {
            harmony.UnpatchSelf();
        }

        //creates a new command to give new honey 
        public class NewItemCommand : ConsoleCommand
        {
            public override string Name => "give";

            public override string Help => "Gives you custom items";

            public override void Run(string[] args)
            {
                //if there is nothing typed after give 
                if (args.Length == 0)
                    return;

                //Attempts to get the item from item manager
                GameObject prefab = ItemManager.Instance.GetItem(args[0]).ItemPrefab;
                if(prefab == null)
                {
                    Console.instance.Print($"{args[0]} is not an item");
                    return;
                }

                //checks if there should be more than 1 given and gives that amount
                int count = args.Length < 2 ? 1 : int.Parse(args[1]);
                for(int i = 0; i < count; i++)
                {
                    Instantiate(prefab, Player.m_localPlayer.transform.position, Quaternion.identity);
                }
            }
        }

        //creating a commmand to clear hunger to test new foods
        public class ResetHungerCommand : ConsoleCommand
        {
            public override string Name => "ResetHunger";

            public override string Help => "Clears hunger";

            public override void Run(string[] args)
            {
                Console.instance.Print("Hunger cleared");
                Player.m_localPlayer.ClearFood();
            }
        }

        //Patch for bed to allow daytime sleeping in any bed and spawn some sweet sweet honey
        [HarmonyPatch(typeof(Bed), nameof(Bed.Interact))]
        class Sleep_Patch
        {
            static bool Prefix(Humanoid human, bool repeat, bool alt, Transform ___m_spawnPoint)
            {
                human.AttachStart(___m_spawnPoint, null, hideWeapons: true, isBed: true, onShip: false, "attach_bed", new Vector3(0f, 0.5f, 0f));
                Instantiate(ItemManager.Instance.GetItem("newHoney").ItemPrefab, ___m_spawnPoint.position, Quaternion.identity);
                return false;
            }
        }
    }
}