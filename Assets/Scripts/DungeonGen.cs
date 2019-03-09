using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class DungeonGen : MonoBehaviour
    {
        //-1 walls, 0 uncheck, 1 room, 2 connect

        public static void GenerateNewDungeon(Dungeon d, Vector2Int size, int maxSplitVariance, int minSubAreaSize)
        {
            d.size = size;
            d.cells = new Cell[size.x, size.y];
            d.mainSubArea = new SubArea
            {
                size = new Vector2Int(size.x - 4, size.y - 4),
                anchor = Vector2Int.one * 2,
            };

            MaxSplit(d.mainSubArea, maxSplitVariance, minSubAreaSize);
            Place(ref d, d.mainSubArea);
            Replace(ref d, 1, 0);
            PlaceAll(ref d, d.mainSubArea);
            Replace(ref d, 0, -1);
            ConnectAll(ref d, d.mainSubArea);
        }

        static void MaxSplit(SubArea a0, int maxSplitVariance, int minSubAreaSize)
        {
            if (a0.childsSubArea == null)
            {
                a0.SplitArea(maxSplitVariance, minSubAreaSize);

                for (int i = 0; i < a0.childsSubArea.Count; i++)
                {
                    MaxSplit(a0.childsSubArea[i], maxSplitVariance, minSubAreaSize);
                }
            }
        }

        static void PlaceAll(ref Dungeon d, SubArea a0)
        {
            if (a0.childsSubArea.Count == 0)
            {
                Place(ref d, a0);
            }
            else
            {
                for (int i = 0; i < a0.childsSubArea.Count; i++)
                {
                    PlaceAll(ref d, a0.childsSubArea[i]);
                }
            }

        }

        static void Place(ref Dungeon d, SubArea a0)
        {
            for (int dy = -1; dy < a0.size.y + 1; dy++)
            {
                for (int dx = -1; dx < a0.size.x + 1; dx++)
                {
                    if (a0.isTooSmall)
                    {
                        d.cells[a0.anchor.x + dx, a0.anchor.y + dy].genID = 1;
                    }
                    else
                    {
                        if ((dx == -1) || (dy == -1) || (dx == a0.size.x) || (dy == a0.size.y)) d.cells[a0.anchor.x + dx, a0.anchor.y + dy].genID = -1;
                        else d.cells[a0.anchor.x + dx, a0.anchor.y + dy].genID = 1;
                    }
                }
            }
        }

        static void Replace(ref Dungeon d, int from, int to)
        {
            for (int dy = 0; dy < d.cells.GetLength(1); dy++)
            {
                for (int dx = 0; dx < d.cells.GetLength(0); dx++)
                {
                    if (d.cells[dx, dy].genID == from) d.cells[dx, dy].genID = to;
                }
            }
        }

        static void ConnectAll(ref Dungeon d, SubArea a0)
        {
            for (int i = 0; i < a0.childsSubArea.Count; i++)
            {
                ConnectAll(ref d, a0.childsSubArea[i]);
            }
            Connect(ref d, a0);
        }

        static void Connect(ref Dungeon d, SubArea a)
        {
            a.connection = new List<Vector2Int>();
            if (a.childsSubArea.Count != 2) return;
            SubArea a0 = a.childsSubArea[0];
            SubArea a1 = a.childsSubArea[1];
            int dy = 0;
            int dx = 0;
            int fs = 0;

            if (a0.anchor.y == a1.anchor.y) dy = Random.Range(1, a1.size.y - 1);
            else dx = Random.Range(1, a1.size.x - 1);
            if (!a1.isTooSmall) a.connection.Add(new Vector2Int(a1.anchor.x + dx, a1.anchor.y + dy));

            while (true)
            {
                fs++;
                if (a0.anchor.y == a1.anchor.y) dx--;
                else dy--;

                if (a1.anchor.x + dx == 1 || a1.anchor.y + dy == 1 || (d.cells[a1.anchor.x + dx, a1.anchor.y + dy].genID > 0 && fs > 1)) break;
                else
                {
                    a.connection.Add(new Vector2Int(a1.anchor.x + dx, a1.anchor.y + dy));
                }
            }

            for (int i = 0; i < a.connection.Count; i++)
            {
                d.cells[a.connection[i].x, a.connection[i].y].genID = 2;
            }
        }
    }

    public class SubArea
    {
        public SubArea parentSubArea;
        public List<SubArea> childsSubArea;
        public List<Vector2Int> connection;
        public Vector2Int anchor;
        public Vector2Int size;
        public bool isTooSmall = false;

        public void SplitArea(int maxSplitVariance, int minSubAreaSize)
        {
            childsSubArea = new List<SubArea>();
            if (size.x < 2 * minSubAreaSize || size.y < 2 * minSubAreaSize)
            {
                if (size.x < minSubAreaSize || size.y < minSubAreaSize) isTooSmall = true;
                return;
            }

            bool splitX = Random.Range(0, size.x + size.y) > size.y;
            int variance = Random.Range(-maxSplitVariance, maxSplitVariance + 1);

            childsSubArea.Add(new SubArea());
            childsSubArea.Add(new SubArea());
            childsSubArea[0].parentSubArea = this;
            childsSubArea[1].parentSubArea = this;

            if (splitX)
            {
                childsSubArea[0].size = new Vector2Int(Mathf.FloorToInt(size.x * 0.5f) + variance - 1, size.y);
                childsSubArea[0].anchor = anchor;

                childsSubArea[1].size = new Vector2Int(Mathf.FloorToInt(size.x * 0.5f) - variance - 1, size.y);
                childsSubArea[1].anchor = anchor + Vector2Int.right * (childsSubArea[0].size.x + 2);
            }
            else
            {
                childsSubArea[0].size = new Vector2Int(size.x, Mathf.FloorToInt(size.y * 0.5f) + variance - 1);
                childsSubArea[0].anchor = anchor;

                childsSubArea[1].size = new Vector2Int(size.x, Mathf.FloorToInt(size.y * 0.5f) - variance - 1);
                childsSubArea[1].anchor = anchor + Vector2Int.up * (childsSubArea[0].size.y + 2);
            }
        }
    }
}

