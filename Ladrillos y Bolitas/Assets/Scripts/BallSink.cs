using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sumidero de las pelotas. Posición a la que se mueven las pelotas cuando colisionan con la DeathZone
/// Muestra un texto con cuantas pelotas han llegado al BallSink
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class BallSink : MonoBehaviour
{
    //References
    private Text labelText;
    private SpriteRenderer ballSprite;
    private LevelManager _levelManager;

    //TODO: Yo esto no se si esta bien. Consultar
    public delegate void OnBallSinkArrived(Ball ball);

    /// <summary>
    /// Numero actual de pelotas que han llegado al ballsink durante esta ronda
    /// Modifica el texto cuando es actualizado su valor
    /// </summary>
    private uint CurrentNumBalls { get { return currentNumBalls; } set { currentNumBalls = value; labelText.text = "x" + currentNumBalls; } }
    private uint currentNumBalls;

    /// <summary>
    /// Pelotas que tienen que llegar al BallSink durante esta ronda
    /// </summary>
    private uint totalNumBalls;

    /// <summary>
    /// Obtiene referencias
    /// </summary>
    private void Awake()
    {
        ballSprite = GetComponent<SpriteRenderer>();
        labelText = GetComponentInChildren<Text>();
    }

    /// <summary>
    /// Inicialización desde el LevelManager
    /// Establece las pelotas iniciales
    /// Se subscribe a eventos de rondas
    /// </summary>
    /// <param name="levelManager"></param>
    /// <param name="numBalls"></param>
    public void Init(LevelManager levelManager, uint numBalls)
    {
        _levelManager = levelManager;
        CurrentNumBalls = numBalls;
        levelManager.OnRoundStartCallback += OnRoundStart;
    }

    /// <summary>
    /// Reinicia el contador de pelotas
    /// </summary>
    public void OnRoundStart()
    {
        CurrentNumBalls = 0;
        totalNumBalls = _levelManager.CurrentNumBalls;
        Hide();
    }

    /// <summary>
    /// Oculta el sprite y el texto
    /// </summary>
    private void Hide()
    {
        ballSprite.enabled = false;
        labelText.enabled = false;
    }

    /// <summary>
    /// Lo muestra en una posición, activando el sprite y el texto
    /// </summary>
    /// <param name="pos"></param>
    public void Show(Vector3 pos)
    {
        transform.position = pos;

        labelText.enabled = true;
        ballSprite.enabled = true;
    }

    /// <summary>
    /// Actualiza el número de pelotas que han llegado, y si han llegado todas, informa al LevelManager
    /// </summary>
    public void OnBallArrived(Ball ball)
    {
        Destroy(ball.gameObject);
        CurrentNumBalls++;

        if (CurrentNumBalls == totalNumBalls)
            _levelManager.RoundEnd();

    }
}
