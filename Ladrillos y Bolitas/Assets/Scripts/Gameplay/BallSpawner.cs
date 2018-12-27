﻿using System.Collections;
using UnityEngine;

/// <summary>
/// Spawner de la pelota.
/// Tiene funciones para establecer su posición y empezar la generación de pelotas
/// </summary>
public class BallSpawner : MonoBehaviour
{
    private Ball _ballPrefab;
    private uint _ballSpawnTickRate;

    /// <summary>
    /// Inicializa el BallSpawner.
    /// Le da el Prefab de la bola y cada cuantos ticks es spawneada
    /// </summary>
    /// <param name="ball"></param>
    public void Init(Ball ballPrefab, uint ballSpawnTickRate)
    {
        _ballPrefab = ballPrefab;
        _ballSpawnTickRate = ballSpawnTickRate;
    }

    /// <summary>
    /// Establece su posición
    /// </summary>
    /// <param name="pos"></param>
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
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
    /// </summary>
    /// <param name="numBalls"></param>
    /// <param name="ballVelocity"></param>
    /// <returns></returns>
    private IEnumerator SpawnBallsCoroutine(uint numBalls, Vector2 ballVelocity)
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