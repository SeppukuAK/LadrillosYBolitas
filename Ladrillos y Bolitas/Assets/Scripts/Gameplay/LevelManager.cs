using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    [SerializeField] private int ballSpawnTickRate;
    [SerializeField] private float ballToSinkTime;
    [SerializeField] private float ballVelocity;
    [SerializeField] private float alertDuration;

    [Header("Gameplay References")]
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private BallSpawner ballSpawner;
    [SerializeField] private BallSink ballSink;
    [SerializeField] private DeathZone deathZone;
    [SerializeField] private Board board;
    [SerializeField] private AimController aimController;

    [Header("UI References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image pointsFillBar;
    [SerializeField] private Text pointsText;
    [SerializeField] private Image[] starsImages;
    [SerializeField] private GameOverUI gameOverUIPrefab;
    [SerializeField] private PauseUI pauseUIPrefab;

    [SerializeField] private SpriteRenderer alertZoneRenderer;
    #endregion Inspector Attributes

    public int Points
    {
        get { return points; }
        set
        {
            points = value;
            pointsText.text = points.ToString();
            pointsFillBar.fillAmount = Mathf.Clamp01((float)points / maxScore);

            starsImages[0].enabled = points >= 0;
            starsImages[1].enabled = pointsFillBar.fillAmount >= 0.7f;
            starsImages[2].enabled = pointsFillBar.fillAmount == 1;
        }
    }
    private int points;

    public bool Pause
    {
        get { return pause; }
        set
        {
            if (value)
            {
                pause = true;
                Time.timeScale = 0.0f;
                PauseUI pauseUI = Instantiate(pauseUIPrefab, canvas.transform);
                pauseUI.Init(this);
            }
            else
            {
                Time.timeScale = 1.0f;
                StartCoroutine(ResumeDelay());
            }

        }
    }
    private bool pause;

    private IEnumerator ResumeDelay()
    {
        yield return new WaitForSeconds(1.0f);
        pause = false;
    }


    /// <summary>
    /// Cuando es modificado activa/desactiva la zona de alerta
    /// TODO: LLEVARLO A BOARD
    /// </summary>
    public bool Alert
    {
        get { return alertZoneRenderer.enabled; }
        set
        {
            alertZoneRenderer.enabled = value;

            if (value)
            {
                alertRoutine = AlertRoutine();
                StartCoroutine(alertRoutine);
            }
            else
            {
                if (alertRoutine != null)
                    StopCoroutine(alertRoutine);
            }
        }
    }
    private IEnumerator alertRoutine;


    public int CurrentNumBalls { get; set; }

    private int maxScore;
    /// <summary>
    /// Inicializa todos los componentes, pasandole los atributos que necesitan para su funcionamiento en la escena
    /// </summary>
    private void Start()
    {
        CurrentNumBalls = int.Parse(GameManager.Instance.GameData.text.Split('\n')[GameManager.Instance.MapLevel].Split(' ', ',')[1]);
        Points = 0;

        aimController.Init(this, ballSpawner, ballVelocity);
        ballSpawner.Init(ballPrefab, ballSpawnTickRate);
        deathZone.Init(this, ballSink, ballToSinkTime);
        ballSink.Init(this);
        board.Init(this, GameManager.Instance.MapData[GameManager.Instance.MapLevel]);

        maxScore = board.PendingTiles * 50;
        Pause = false;
        Alert = board.DetectAlert();
    }

    /// <summary>
    /// Detecta el input de "return"
    /// </summary>
    private void Update()
    {
        if (!Pause && Input.GetKeyDown(KeyCode.Escape))     
            Pause = true;
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
    /// Establece la posición del ballSpawner a a la del ballSink e informa a todos los subscritos que ha acabado la ronda
    /// Es llamado cuando todas las pelotas han caido
    /// </summary>
    public void RoundEnd()
    {
        ballSpawner.transform.position = ballSink.transform.position;

        if (OnRoundEndCallback != null)
            OnRoundEndCallback.Invoke();
    }

    public void LevelEnd()
    {
        int stars = 0;
        foreach (Image starImage in starsImages)
        {
            if (starImage.enabled)
                stars++;
        }

        GameOverUI gameOverUI = Instantiate(gameOverUIPrefab, canvas.transform);
        gameOverUI.Init(board.PendingTiles == 0, stars);
        pause = true;
    }

    private IEnumerator AlertRoutine()
    {
        while (true)
        {
            UtilitiesManager.Instance.FadeInFadeOut(alertZoneRenderer, alertDuration / 2);
            yield return new WaitForSeconds(alertDuration);
        }
    }

    public void AllBallsGoToSink()
    {

    }
}
