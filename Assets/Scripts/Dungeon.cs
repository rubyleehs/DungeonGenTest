using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public struct Cell
    {
        public int genID; //-1 walls, 0 uncheck, 1 room, 2 connect

        public int visualID;
        public int visualMetaID;
    }

    [System.Serializable]
    public struct SpritesDict
    {
        public int key;
        public TileSprites sprites;
    }

    [System.Serializable]
    public struct MeshLayer
    {
        public Transform meshPositioner;
        public MeshFilter meshFilter;
        public Texture2D mainTex;
    }

    public class Dungeon : MonoBehaviour
    {
        public Vector2Int size;
        public Vector2 cellSize;
        public Vector2Int cellPixelSize;
        public Cell[,] cells;

        public SpritesDict[] I_wallSpritesDict;
        public Dictionary<int, TileSprites> wallSpritesDict;

        public SpritesDict[] I_floorSpritesDict;
        public Dictionary<int, TileSprites> floorSpritesDict;

        public static Sprite transparent;
        public Sprite I_transparent;

        public SubArea mainSubArea;

        public MeshLayer wallMeshLayer;
        public MeshLayer floorMeshLayer;

        public void Init(Vector2Int _size, int maxSplitVariance, int minSubAreaSize)
        {
            transparent = I_transparent;

            wallSpritesDict = new Dictionary<int, TileSprites>();
            floorSpritesDict = new Dictionary<int, TileSprites>();
            for (int i = 0; i < I_wallSpritesDict.Length; i++) wallSpritesDict.Add(I_wallSpritesDict[i].key, I_wallSpritesDict[i].sprites);
            for (int i = 0; i < I_floorSpritesDict.Length; i++) floorSpritesDict.Add(I_floorSpritesDict[i].key, I_floorSpritesDict[i].sprites);

            wallMeshLayer.meshFilter.mesh = MeshGen.GridMeshGen.CreateNewGrid(size, cellSize, Vector2.zero, ref wallMeshLayer.meshPositioner);
            floorMeshLayer.meshFilter.mesh = MeshGen.GridMeshGen.CreateNewGrid(size, cellSize, Vector2.zero, ref floorMeshLayer.meshPositioner);

            DungeonGen.GenerateNewDungeon(this, _size,maxSplitVariance, minSubAreaSize);
            DungeonVisualGen.SetupCellVisualElements(ref cells);
            DungeonVisualGen.CreateDungeonTex(ref wallMeshLayer.mainTex, ref wallSpritesDict, ref cells, cellPixelSize);
            DungeonVisualGen.CreateDungeonTex(ref floorMeshLayer.mainTex, ref floorSpritesDict, ref cells, cellPixelSize);
        }
    }
}
