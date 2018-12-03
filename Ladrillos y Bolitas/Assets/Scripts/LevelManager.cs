using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Vector2 SpawnerStartPosition;
    public BallSpawner BallSpawner;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnLevelStart();
    }

    private void OnLevelStart()
    {
        BallSpawner.transform.position = SpawnerStartPosition;
        BallSpawner.SpawnBalls(30);
    }


}
