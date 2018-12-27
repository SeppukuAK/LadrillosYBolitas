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
    [SerializeField] private Board board;
    [SerializeField] private AimController aimController;

    [Header("Test Attributes")]
    [SerializeField] private TextAsset map;//Mapa con la informacion del juego

    public uint CurrentNumBalls { get; set; }

    public delegate void OnRoundStart();
    public OnRoundStart OnRoundStartCallback;

    /// <summary>
    /// Inicializa todos los componentes, pasandole los atributos que necesitan para su funcionamiento en la escena
    /// </summary>
    private void Start()
    {
        ballSpawner.Init(ballPrefab, ballSpawnTickRate);
        CurrentNumBalls = 30;
        ballSink.Init(this, CurrentNumBalls);
        deathZone.Init(this,ballSink, ballToSinkTime);
        board.Init(this, map);
        aimController.Init(this, ballSpawner);
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


    public void TileDestroyed(Tile tile)
    {
        board.OnTileDestroyed(tile);
    }

    /// <summary>
    /// Establece la posición del ballSpawner a a la del ballSink e informa al tablero para que baje
    /// Es llamado cuando todas las pelotas han caido
    /// </summary>
    public void RoundEnd()
    {
        ballSpawner.SetPosition(ballSink.transform.position);
        board.OnRoundEnd();
    }

    public void LevelEnd()
    {
        Debug.Log("has ganado:" + (board.PendingTiles == 0));
    }
}
