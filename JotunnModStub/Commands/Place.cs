using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
namespace JotunnModStub.Commands
{
    internal class PlaceClass : BaseUnityPlugin
    {
        public class Place : ConsoleCommand
        {
            public override string Name => "place";
            public override string Help => "Places a custom prefab";
            public override void Run(string[] args)
            {
                if (args.Length == 0)
                    return;

                GameObject prefab = PrefabManager.Instance.GetPrefab(args[0]);
                if(prefab == null)
                {
                    Console.instance.Print($"{args[0]} is not a prefab");
                    return;
                }

                Instantiate(prefab, (Player.m_localPlayer.transform.position + Vector3.forward), Quaternion.identity);
            }
        }
    }
}
