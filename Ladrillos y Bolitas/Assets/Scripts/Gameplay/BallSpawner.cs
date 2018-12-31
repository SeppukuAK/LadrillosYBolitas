using System.Collections;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Spawner de la pelota.
/// Tiene funciones para establecer su posición y empezar la generación de pelotas
/// </summary>
public class BallSpawner : MonoBehaviour
{
    private Ball _ballPrefab;
    private int _ballSpawnTickRate;

    private List<Ball> balls;

    /// <summary>
    /// Inicializa el BallSpawner.
    /// Le da el Prefab de la bola y cada cuantos ticks es spawneada
    /// </summary>
    /// <param name="ball"></param>
    public void Init(Ball ballPrefab, int ballSpawnTickRate)
    {
        _ballPrefab = ballPrefab;
        _ballSpawnTickRate = ballSpawnTickRate;
    }

    /// <summary>
    /// Inicia la corrutina que instancia numBalls pelotas con ballVelocity de velocidad
    /// </summary>
    /// <param name="numBalls"></param>
    /// <param name="ballVelocity"></param>
    public void SpawnBalls(int numBalls, Vector2 ballVelocity)
    {
        StartCoroutine(SpawnBallsCoroutine(numBalls, ballVelocity));
    }

    /// <summary>
    /// Corrutina que instancia "numBalls" pelotas con "ballVelocity" de velocidad en función de _ballSpawnTickRate
    /// </summary>
    /// <param name="numBalls"></param>
    /// <param name="ballVelocity"></param>
    /// <returns></returns>
    private IEnumerator SpawnBallsCoroutine(int numBalls, Vector2 ballVelocity)
    {
        for (int i = 0; i < numBalls; i++)
        {
            Ball ball = Instantiate(_ballPrefab, transform.position, Quaternion.identity);

            //Espera "ticks" veces llamadas a la fisica
            for (uint j = 0; j < _ballSpawnTickRate; j++)
                yield return new WaitForFixedUpdate();

            ball.StartMoving(ballVelocity);
        }
        yield return null;
    }

}
