using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class DungeonVisualGen : MonoBehaviour
    {
        private static int[] primes = new int[] { 2, 3, 5, 7 };
        private static int[] multiples = new int[] { 1, 2, 3, 5, 6, 7, 10, 14, 15, 21, 30, 35, 42, 70, 105, 210 };

        public static void GenerateNewDungeonVisuals(ref Texture2D tex, ref Cell[,] cells, ref Dictionary<int, TileSprites> dict)
        {
            
        }

        public static void SetupCellVisualElements(ref Cell[,] cells)
        {
            CalculateCellsVisualID(ref cells);
            CalculateCellsVisualMetaID(ref cells);
        }

        protected static void CalculateCellsVisualID(ref Cell[,] cells)//if there are "rooms" change this function
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    cells[x,y].visualID = cells[x, y].genID;
                }
            }
        }

        protected static void CalculateCellsVisualMetaID(ref Cell[,] cells)
        {
            for (int y = 1; y < cells.GetLength(1) - 1; y++)
            {
                for (int x = 1; x < cells.GetLength(0) - 1; x++)
                {
                    int primedID = 1;
                    bool isSolid = cells[x, y].visualID < 0;
                    if ((cells[x + 1, y].visualID < 0) == isSolid) primedID *= primes[0];
                    if ((cells[x, y + 1].visualID < 0) == isSolid) primedID *= primes[1];
                    if ((cells[x - 1, y].visualID < 0) == isSolid) primedID *= primes[2];
                    if ((cells[x, y - 1].visualID < 0) == isSolid) primedID *= primes[3];

                    cells[x, y].visualMetaID = System.Array.IndexOf(multiples, primedID);
                }
            }
        }

        public static void CreateDungeonTex(ref Texture2D tex, ref Dictionary<int, TileSprites> dict, ref Cell[,] cells, Vector2Int cellPixelSize)
        {
            tex.Resize(cells.GetLength(0) * cellPixelSize.x, cells.GetLength(1) * cellPixelSize.y);

            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    int id = cells[x, y].visualID;
                    int metaID = cells[x, y].visualMetaID;
                    Color[] spriteTex = null;

                    if (dict.ContainsKey(id))
                    {
                        if (metaID < dict[id].metaSprites.Length && dict[id].metaSprites[metaID] != null) spriteTex = dict[id].metaSprites[metaID].texture.GetPixels(0, 0, cellPixelSize.x, cellPixelSize.y);
                        else if (dict[id].metaSprites.Length > 0 && dict[id].metaSprites[0] != null) spriteTex = dict[id].metaSprites[0].texture.GetPixels(0, 0, cellPixelSize.x, cellPixelSize.y);
                    }

                    if(spriteTex == null) spriteTex = Dungeon.transparent.texture.GetPixels(0, 0, cellPixelSize.x, cellPixelSize.y);
                    if (spriteTex != null) tex.SetPixels(x * cellPixelSize.x, y * cellPixelSize.y, cellPixelSize.x, cellPixelSize.y, spriteTex);
                }
            }
            tex.Apply();
        }
    }

    [System.Serializable]
    public struct TileSprites
    {
        public Sprite[] metaSprites;
    }
}
