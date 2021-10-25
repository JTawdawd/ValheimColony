using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using HarmonyLib;
using Jotunn.Utils;

namespace JotunnModStub.Commands
{
    internal class CustomCommand : BaseUnityPlugin {
        //creates a new command to give
        public class Give : ConsoleCommand
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
                if (prefab == null)
                {
                    Console.instance.Print($"{args[0]} is not an item");
                    return;
                }

                //checks if there should be more than 1 given and gives that amount
                int count = args.Length < 2 ? 1 : int.Parse(args[1]);
                for (int i = 0; i < count; i++)
                {
                    Instantiate(prefab, Player.m_localPlayer.transform.position, Quaternion.identity);

                }
            }
        }
    }
}
