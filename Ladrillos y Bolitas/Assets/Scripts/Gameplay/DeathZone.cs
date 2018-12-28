﻿using UnityEngine;

/// <summary>
/// Componente asociado a um GameObject que cuando una pelota colisiona con él, le informa de que ha muerto
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class DeathZone : MonoBehaviour
{
    #region References

    private BallSink _ballSink;

    #endregion References

    #region Attributes

    /// <summary>
    /// Tiempo que tarda la pelota en llegar al Sink
    /// </summary>
    private float _ballToSinkTime;

    /// <summary>
    /// Guarda si ha colisionado la primera pelota con la DeathZOne
    /// </summary>
    private bool firstBallCollided;

    #endregion Attributes

    /// <summary>
    /// Inicializa el deathZone desde levelManager. 
    /// Le pasa referencias necesarias
    /// Se subscribe a eventos de rondas
    /// </summary>
    /// <param name="levelManager"></param>
    /// <param name="ballToSinkTime"></param>
    /// <param name="onBallSinkArrivedCallback"></param>
    public void Init(LevelManager levelManager,BallSink ballSink, float ballToSinkTime)
    {
        _ballSink = ballSink;
        _ballToSinkTime = ballToSinkTime;
        levelManager.OnRoundStartCallback += OnRoundStart;
    }

    /// <summary>
    /// Es llamado cuando se hace un nuevo tiro
    /// Reestablece valores
    /// </summary>
    public void OnRoundStart()
    {
        firstBallCollided = false;
    }

    /// <summary>
    /// Detecta la colisión con la pelota
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball)
        {
            //Ha chocado la primera pelota
            if (!firstBallCollided)
            {
                //Se muestra el sink
                firstBallCollided = true;
                _ballSink.Show(ball.transform.position.x);
                _ballSink.OnBallArrived(ball);
            }

            //No es la primera pelota
            else
            {
                //Para la pelota, fuerza su posición en Y y llama a la corrutina de movimiento hacia el sink
                ball.Stop();
                ball.MoveTo(_ballSink.transform.position, _ballToSinkTime, _ballSink.OnBallArrived);
            }
        }
    }


}

