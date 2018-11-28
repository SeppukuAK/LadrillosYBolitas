using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour {

    public int Balls;
    public Vector2 Velocity;
    public Vector2 BallSpawnPosition;

    public Ball BallPrefab;

    // Use this for initialization
    void Start () {
        StartCoroutine(SpawnBalls());
	}
	
    private IEnumerator SpawnBalls()
    {
        for (int i = 0; i < Balls; i++)
        {
            Ball ball = Instantiate(BallPrefab, BallSpawnPosition,Quaternion.identity);
            ball.SetVelocity(Velocity);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

}
