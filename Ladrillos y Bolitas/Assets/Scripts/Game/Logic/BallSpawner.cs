using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Spawner de la pelota.
/// Tiene funciones para establecer su posición y empezar la generación de pelotas
/// </summary>
public class BallSpawner : MonoBehaviour
{
    /// <summary>
    /// Cada cuantos ticks se spawnea una pelota
    /// </summary>
    private uint _ballSpawnTickRate;

    //Other References
    private LevelManager _levelManager;
    private Ball _ballPrefab;

    /// <summary>
    /// Inicializa el BallSpawner.
    /// Le da el Prefab de la bola y cada cuantos ticks es spawneada
    /// </summary>
    /// <param name="ball"></param>
    public void Init(LevelManager levelManager, Ball ballPrefab, uint ballSpawnTickRate)
    {
        _levelManager = levelManager;
        _ballPrefab = ballPrefab;
        _ballSpawnTickRate = ballSpawnTickRate;

    }

    /// <summary>
    /// Inicia la corrutina que instancia numBalls pelotas con ballVelocity de velocidad
    /// </summary>
    /// <param name="numBalls"></param>
    /// <param name="ballVelocity"></param>
    public void SpawnBalls(uint numBalls, Vector2 ballVelocity)
    {
        StartCoroutine(SpawnBallsCoroutine(numBalls, ballVelocity));
    }

    /// <summary>
    /// Corrutina que instancia "numBalls" pelotas con "ballVelocity" de velocidad en función de _ballSpawnTickRate
    /// Guarda las pelotas en _levelManager.Balls
    /// </summary>
    /// <param name="numBalls"></param>
    /// <param name="ballVelocity"></param>
    /// <returns></returns>
    private IEnumerator SpawnBallsCoroutine(uint numBalls, Vector2 ballVelocity)
    {
        _levelManager.Balls = new List<Ball>();

        for (int i = 0; i < numBalls; i++)
        {
            Ball ball = Instantiate(_ballPrefab, transform.position, Quaternion.identity);
            _levelManager.Balls.Add(ball);

            //Espera "ticks" veces llamadas a la fisica
            for (uint j = 0; j < _ballSpawnTickRate; j++)
                yield return new WaitForFixedUpdate();

            ball.StartMoving(ballVelocity);
        }
        yield return null;
    }

    /// <summary>
    /// Para la generación de pelotas
    /// </summary>
    public void StopSpawn()
    {
        StopAllCoroutines();
    }
}
