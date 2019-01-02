using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manager de un nivel de juego
/// Gestiona el flujo de la partida, las rondas de tiro, los bloques, etc.
/// </summary>
public class LevelManager : MonoBehaviour
{
    #region Callbacks
    public delegate void OnRoundStart();
    public OnRoundStart OnRoundStartCallback;

    public delegate void OnRoundEnd();
    public OnRoundStart OnRoundEndCallback;
    #endregion Callbacks

    #region Inspector Attributes
    [Header("Game Attributes")]
    [SerializeField] private uint ballSpawnTickRate;
    [SerializeField] private float ballToSinkTime;
    [SerializeField] private float ballVelocity;
    [SerializeField] private uint maxScoreMultiplier;
    [SerializeField] private float alertDuration;
    [SerializeField] private uint tilePoints;
    [SerializeField] private uint maxTimeScale;
    [SerializeField] private float skipTime;

    [Header("Gameplay References")]
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private BallSpawner ballSpawner;
    [SerializeField] private BallSink ballSink;
    [SerializeField] private DeathZone deathZone;
    [SerializeField] private Board board;
    [SerializeField] private AimController aimController;
    [SerializeField] private PauseUI pauseUIPrefab;
    [SerializeField] private GameOverUI gameOverUIPrefab;

    [Header("UI References")]
    [SerializeField] private Image pointsFillBar;
    [SerializeField] private Text pointsText;
    [SerializeField] private Image[] starsImages;
    [SerializeField] private Button skipButton;
    [SerializeField] private RectTransform powerUpsPanel;

    #endregion Inspector Attributes

    #region Properties
    /// <summary>
    /// Puntuación del jugador.
    /// Actualiza el UI cuando es modificado
    /// </summary>
    public uint Points
    {
        get { return uint.Parse(pointsText.text); }
        set
        {
            uint points = value;

            //Actualiza el UI
            pointsText.text = points.ToString();
            pointsFillBar.fillAmount = Mathf.Clamp01((float)points / maxScore);
            starsImages[0].enabled = points >= 0;
            starsImages[1].enabled = pointsFillBar.fillAmount >= 0.7f;
            starsImages[2].enabled = pointsFillBar.fillAmount == 1;
        }
    }

    /// <summary>
    /// Devuelve si el juego esta pausado.
    /// Cuando es pausado, se muestra el panel de Pausa y se para el juego
    /// </summary>
    public bool Pause
    {
        get { return pause; }
        set
        {
            if (value)
            {
                pause = true;
                Time.timeScale = 0.0f;
                Instantiate(pauseUIPrefab).Init(this);
            }
            else
            {
                Time.timeScale = 1.0f;
                StartCoroutine(ResumeDelay());
            }

        }
    }
    private bool pause;

    /// <summary>
    /// Delay necesario para que no haga input el usuario cuando sale de la pausa
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResumeDelay()
    {
        yield return new WaitForSeconds(0.1f);
        pause = false;
    }

    #endregion Properties

    /// <summary>
    /// Número de pelotas de las que se dispone
    /// </summary>
    public uint CurrentNumBalls { get; set; }

    /// <summary>
    /// Puntuación para obtener 3 estrellas
    /// </summary>
    private uint maxScore;

    /// <summary>
    /// Lista de pelotas en el juego
    /// </summary>
    public List<Ball> Balls { get; set; }

    /// <summary>
    /// Inicializa todos los componentes, pasandole los atributos que necesitan para su funcionamiento en la escena
    /// </summary>
    private void Start()
    {
        CurrentNumBalls = uint.Parse(GameManager.Instance.GameData.text.Split('\n')[GameManager.Instance.MapLevel].Split(' ', ',')[1]);        //Obtiene el numero de pelotas iniciales
        Points = 0;
        pause = false;

        aimController.Init(this, ballSpawner, ballVelocity, maxTimeScale);
        ballSpawner.Init(this, ballPrefab, ballSpawnTickRate);
        deathZone.Init(this, ballSink, ballToSinkTime);
        ballSink.Init(this);
        board.Init(this, GameManager.Instance.MapData[GameManager.Instance.MapLevel], tilePoints, alertDuration);

        //El maxScore es en funcion del número de tiles
        maxScore = board.PendingTiles * maxScoreMultiplier;
    }

    /// <summary>
    /// Detecta el input de "return"
    /// Pausa el juego
    /// </summary>
    private void Update()
    {
        if (!Pause && Input.GetKeyDown(KeyCode.Escape))
            Pause = true;
    }

    /// <summary>
    /// Activa el botón de skip
    /// Informa a los componentes subscritos
    /// Es llamado cuando se inicia el disparo
    /// </summary>
    public void RoundStart()
    {
        skipButton.gameObject.SetActive(true);
        powerUpsPanel.gameObject.SetActive(false);

        if (OnRoundStartCallback != null)
            OnRoundStartCallback.Invoke();
    }

    /// <summary>
    /// Establece la posición del ballSpawner a a la del ballSink e informa a todos los subscritos que ha acabado la ronda
    /// Es llamado cuando todas las pelotas han caido
    /// Desactiva el botón de skip
    /// </summary>
    public void RoundEnd()
    {
        ballSpawner.transform.position = ballSink.transform.position;
        skipButton.gameObject.SetActive(false);
        powerUpsPanel.gameObject.SetActive(true);

        if (OnRoundEndCallback != null)
            OnRoundEndCallback.Invoke();
    }

    /// <summary>
    /// Es llamado cuando acaba la partida
    /// Instancia el panel de GameOver y pausa el juego
    /// </summary>
    public void LevelEnd()
    {
        int stars = 0;
        foreach (Image starImage in starsImages)
        {
            if (starImage.enabled)
                stars++;
        }

        Instantiate(gameOverUIPrefab).Init(board.PendingTiles == 0, stars);
        pause = true;
    }

    /// <summary>
    /// Para la generación de pelotas
    /// Empieza a mover todas las pelotas hacia el sink
    /// </summary>
    public void MoveAllBallsToSink()
    {
        ballSpawner.StopSpawn();
        skipButton.gameObject.SetActive(false);

        foreach (Ball ball in Balls)
        {
            ball.Stop();
            ball.StopMoveRoutine();
            ball.SetTrigger();
            ball.MoveTo(ballSink.transform.position, skipTime, ballSink.OnBallArrived);
        }

    }

    #region PowerUps

    /// <summary>
    /// PowerUp: Elimina la fila inferior
    /// </summary>
    public void DestroyRow()
    {
        board.DestroyRow();
    }
    #endregion PowerUps


}
