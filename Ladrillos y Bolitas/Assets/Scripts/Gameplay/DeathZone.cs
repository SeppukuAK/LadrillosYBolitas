using UnityEngine;

/// <summary>
/// Componente asociado a um GameObject que cuando una pelota colisiona con él, le informa de que ha muerto
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class DeathZone : MonoBehaviour
{
    //References
    private LevelManager _levelManager;
    private BallSink _ballSink;

    /// <summary>
    /// Tiempo que tarda la pelota en llegar al Sink
    /// </summary>
    private float _ballToSinkTime;

    /// <summary>
    /// Guarda la posición del sumidero
    /// Zero si no ha caido todavia ninguna pelota en esta ronda
    /// </summary>
    private Vector3 ballSinkPos;

    /// <summary>
    /// Inicializa el deathZone desde levelManager. 
    /// Le pasa referencias necesarias
    /// Se subscribe a eventos de rondas
    /// </summary>
    /// <param name="levelManager"></param>
    /// <param name="ballToSinkTime"></param>
    /// <param name="onBallSinkArrivedCallback"></param>
    public void Init(LevelManager levelManager, BallSink ballSink, float ballToSinkTime)
    {
        _levelManager = levelManager;
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
        ballSinkPos = Vector3.zero;
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
            if (ballSinkPos == Vector3.zero)
            {   
                ballSinkPos = new Vector3(ball.transform.position.x, _ballSink.SinkYPos, ball.transform.position.z);
                _ballSink.Show(ballSinkPos);
                _ballSink.OnBallArrived(ball);
            }

            //No es la primera pelota
            else
            {
                //Para la pelota, fuerza su posición en Y y llama a la corrutina de movimiento hacia el sink
                ball.Stop();
                ball.SetYPos(_ballSink.SinkYPos);
                ball.MoveTo(ballSinkPos, _ballToSinkTime, _ballSink.OnBallArrived);
            }
        }
    }


}

