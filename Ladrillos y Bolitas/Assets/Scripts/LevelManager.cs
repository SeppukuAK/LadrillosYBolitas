using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private const float MAGIC = 6.1875F;
    public static LevelManager Instance;

    public Vector2 SpawnerStartPosition;
    public BallSpawner BallSpawner;

    private void Awake()
    {
        Instance = this;
        AdjustCameraToAspectRatio();
    }

    private void AdjustCameraToAspectRatio()
    {
        float aspectRatio = (float) Screen.height / Screen.width;
        Debug.Log(aspectRatio);
        Camera.main.orthographicSize = aspectRatio * MAGIC;
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
