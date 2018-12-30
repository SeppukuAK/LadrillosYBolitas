using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sumidero de las pelotas. Posición a la que se mueven las pelotas cuando colisionan con la DeathZone
/// Muestra un texto con cuantas pelotas han llegado al BallSink
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class BallSink : MonoBehaviour
{
    #region References
    //Own References
    private Text labelText;
    private SpriteRenderer ballSprite;

    //Other References
    private LevelManager _levelManager;
    #endregion References

    #region Attributes

    /// <summary>
    /// Numero actual de pelotas que han llegado al ballsink durante esta ronda
    /// Modifica el texto cuando es actualizado su valor
    /// </summary>
    private int currentNumBalls { get { return _currentNumBalls; } set { _currentNumBalls = value; labelText.text = "x" + _currentNumBalls; } }
    private int _currentNumBalls;

    /// <summary>
    /// Pelotas que tienen que llegar al BallSink durante esta ronda
    /// </summary>
    private int totalNumBalls;

    #endregion Attributes

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
    public void Init(LevelManager levelManager)
    {
        _levelManager = levelManager;
        currentNumBalls = _levelManager.CurrentNumBalls;
        _levelManager.OnRoundStartCallback += OnRoundStart;
    }

    /// <summary>
    /// Reinicia el contador de pelotas
    /// </summary>
    public void OnRoundStart()
    {
        currentNumBalls = 0;
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
    public void Show(float x)
    {
        transform.position = new Vector3(x,transform.position.y,transform.position.z);

        labelText.enabled = true;
        ballSprite.enabled = true;
    }

    /// <summary>
    /// Actualiza el número de pelotas que han llegado, y si han llegado todas, informa al LevelManager
    /// </summary>
    public void OnBallArrived(Ball ball)
    {
        Destroy(ball.gameObject);
        currentNumBalls++;

        if (currentNumBalls == totalNumBalls)
            _levelManager.RoundEnd();
    }
}
