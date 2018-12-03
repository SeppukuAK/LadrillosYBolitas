using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{

    [Header("Atributtes")]
    public float SpawnRate;
    public Vector2 Velocity;

    [Header("References")]
    public Ball BallPrefab;

    public bool FirstRoundCollision { get; set; }

    public void SpawnBalls(int numBalls)
    {
        StartCoroutine(SpawnBallsRoutine(numBalls));
    }

    private IEnumerator SpawnBallsRoutine(int numBalls)
    {
        for (int i = 0; i < numBalls; i++)
        {
            Ball ball = Instantiate(BallPrefab, transform.position, Quaternion.identity);
            ball.SetVelocity(Velocity);
            yield return new WaitForSeconds(SpawnRate);
        }
        yield return null;
    }

}
