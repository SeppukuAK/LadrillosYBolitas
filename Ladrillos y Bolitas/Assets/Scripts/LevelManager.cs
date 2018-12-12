using UnityEngine;

/// <summary>
/// Manager de un nivel de juego
/// Gestiona el flujo de la partida, las rondas de tiro, los bloques, etc.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("Game Attributes")]
    [SerializeField] private uint ballSpawnTickRate;
    [SerializeField] private float ballToSinkTime;

    [Header("References")]
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private BallSpawner ballSpawner;
    [SerializeField] private BallSink ballSink;
    [SerializeField] private DeathZone deathZone;

    [Header("Test Attributes")]
    [SerializeField] private Vector2 ballVelocity;

    public uint CurrentNumBalls { get; set; }

    public delegate void OnRoundStart();
    public OnRoundStart OnRoundStartCallback;

    private float sinkYPos; //Coordenada y del sumidero

    /// <summary>
    /// Inicializa todos los componentes, pasandole los atributos que necesitan para su funcionamiento en la escena
    /// </summary>
    private void Start()
    {
        sinkYPos = ballSpawner.transform.position.y;
        ballSpawner.Init(ballPrefab, ballSpawnTickRate);
        ballSink.Init(this,30);
        deathZone.Init(this, ballToSinkTime, ballSink.OnBallArrived);
    }

    /// <summary>
    /// Informa a los componentes subscritos
    /// Es llamado cuando se inicia el disparo
    /// </summary>
    public void RoundStart()
    {
        if (OnRoundStartCallback != null)
            OnRoundStartCallback.Invoke();

    }

    /// <summary>
    /// Es llamado cuando la primera pelota ha llegado a la deathZone
    /// Muestra el BallSink y le informa de que ha llegado una pelota
    /// Devuelve la posición del sumidero
    /// </summary>
    /// <param name="ball"></param>
    /// <returns></returns>
    public Vector3 FirstBallDeath(Ball ball)
    {
        Vector3 ballSinkPos = new Vector3(ball.transform.position.x, sinkYPos, ball.transform.position.z);
        ballSink.Show(ballSinkPos);
        ballSink.OnBallArrived(ball);
        return ballSinkPos;
    }

    /// <summary>
    /// Establece la posición del ballSpawner a a la del ballSink
    /// Es llamado cuando todas las pelotas han caido
    /// </summary>
    public void RoundEnd()
    {
        ballSpawner.SetPosition(ballSink.transform.position);
    }

    //TODO: TEST
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            RoundStart();
            ballSpawner.SpawnBalls(30, ballVelocity);
        }

    }

}
