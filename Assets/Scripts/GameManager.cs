using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public Dungeon.Dungeon I_dungeon;
    public Dungeon.DungeonDecoPlacer dungeonDecoPlacer;
    public static Dungeon.Dungeon dungeon;

    [Header("Dungeon Generation")]
    public Vector2Int dungeonSize;
    public int dungeonMaxSplitVariance = 5, dungeonMinSubAreaSize = 6;


    private void Awake()
    {
        dungeon = I_dungeon;
        dungeon.Init(dungeonSize, dungeonMaxSplitVariance, dungeonMinSubAreaSize);
        dungeonDecoPlacer.Init();
    }

}
