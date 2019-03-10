using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public Dungeon.Dungeon I_dungeon;
    public static Dungeon.Dungeon dungeon;
    public CamCG camCG;

    [Header("Dungeon Generation")]
    public Vector2Int dungeonSize;
    public int dungeonMaxSplitVariance = 5, dungeonMinSubAreaSize = 6;


    private void Awake()
    {
        dungeon = I_dungeon;
        dungeon.Init(dungeonSize, dungeonMaxSplitVariance, dungeonMinSubAreaSize);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dungeon.Init(dungeonSize, dungeonMaxSplitVariance, dungeonMinSubAreaSize);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            camCG.enabled = !camCG.enabled;
        }
    }

}
