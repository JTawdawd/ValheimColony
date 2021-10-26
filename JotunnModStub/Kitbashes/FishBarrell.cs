using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using Jotunn.Utils;
using Jotunn.Configs;
using System.Collections.Generic;

namespace JotunnModStub.Kitbashes
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]

    class FishBarrell : BaseUnityPlugin
    {
        public const string PluginGUID = "com.jotunn.ValheimColony";
        public const string PluginName = "ValheimColony";
        public const string PluginVersion = "0.0.1";

        public void Load()
        {
            //load icon tetxure
            var iconTexture = AssetUtils.LoadTexture("JotunnModStub/Assets/ReferenceTexture.png");
            var iconSprite = Sprite.Create(iconTexture, new Rect(0f, 0f, iconTexture.width, iconTexture.height), Vector2.zero);

            //create the kitbash
            CustomPiece fishBarrellPiece = new CustomPiece("fishBarrell", true, "Hammer");
            KitbashManager.Instance.AddKitbash(fishBarrellPiece.PiecePrefab, new KitbashConfig
            {
                Layer = "piece",
                KitbashSources = new List<KitbashSourceConfig>
                {
                    new KitbashSourceConfig
                    {
                        Name = "Barrell",
                        SourcePrefab = "barrell",
                        SourcePath = "default",
                        Position = Vector3.zero,
                        Rotation = Quaternion.identity,
                        Scale = Vector3.one
                    },
                    new KitbashSourceConfig
                    {
                        Name = "Fish1",
                        SourcePrefab = "Fish1",
                        SourcePath = "fi_vil_food_fish01_01",
                        Position = new Vector3(-0.223f, 0.352f, 0.086f),
                        Rotation = Quaternion.Euler(15.272f, 5.778f, -68.986f),
                        Scale = new Vector3(1.5f, 1.5f, 1.5f)
                    },
                    new KitbashSourceConfig
                    {
                        Name = "Fish2",
                        SourcePrefab = "Fish2",
                        SourcePath = "fi_vil_food_fish01_01",
                        Position = new Vector3(-0.257f, 0.2522257f, -0.001f),
                        Rotation = Quaternion.Euler(-7.461f, -2.694f, -70.08f),
                        Scale = new Vector3(2.5f, 2f, 2f)
                    },
                    new KitbashSourceConfig
                    {
                        Name = "Fish3",
                        SourcePrefab = "Fish3",
                        SourcePath = "fi_vil_food_fish01_01",
                        Position = new Vector3(0.1786368f, 0.3770158f, -0.018f),
                        Rotation = Quaternion.Euler(-173.518f, 8.303986f, 71.522f),
                        Scale = new Vector3(2f, 2f, 2f)
                    },
                    new KitbashSourceConfig
                    {
                        Name = "Fish4",
                        SourcePrefab = "Fish4_cave",
                        SourcePath = "fi_vil_food_fish01_01",
                        Position = new Vector3(0.08f, 0.4033244f, -0.101f),
                        Rotation = Quaternion.Euler(-10.431f, -26.964f, -69.44f),
                        Scale = new Vector3(1f, 1f, 1f)
                    },
                    new KitbashSourceConfig
                    {
                        Name = "Fish5",
                        SourcePrefab = "Fish1",
                        SourcePath = "fi_vil_food_fish01_01",
                        Position = new Vector3(0.07018328f, 0.291f, 0.046f),
                        Rotation = Quaternion.Euler(28.458f, 28.366f, -67.481f),
                        Scale = new Vector3(1.5f, 1.5f, 1.5f)
                    },
                    new KitbashSourceConfig
                    {
                        Name = "Fish6",
                        SourcePrefab = "Fish4_cave",
                        SourcePath = "fi_vil_food_fish01_01",
                        Position = new Vector3(-0.1162857f, 0.316f, -0.2257529f),
                        Rotation = Quaternion.Euler(-10.348f, -46.036f, -80.57301f),
                        Scale = new Vector3(1f, 1f, 1f)
                    }
                }
            });

            //Remove default cube assets
            Destroy(fishBarrellPiece.PiecePrefab.GetComponent<MeshRenderer>());
            Destroy(fishBarrellPiece.PiecePrefab.GetComponent<BoxCollider>());

            //add to piece manager
            Piece piece = fishBarrellPiece.Piece;
            piece.m_icon = iconSprite;
            fishBarrellPiece.FixReference = true;
            PieceManager.Instance.AddPiece(fishBarrellPiece);
        }
    }
}
