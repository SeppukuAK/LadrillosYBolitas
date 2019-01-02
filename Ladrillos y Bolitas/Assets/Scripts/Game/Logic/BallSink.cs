using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sumidero de las pelotas. Posición a la que se mueven las pelotas cuando colisionan con la DeathZone
/// Muestra un texto con cuantas pelotas han llegado al BallSink
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class BallSink : MonoBehaviour
{
    /// <summary>
    /// Número de pelotas que tienen que llegar al sink esta ronda
    /// </summary>
    private uint roundBalls;

    //Own References
    private Text labelText;
    private SpriteRenderer ballSprite;

    //Other References
    private LevelManager _levelManager;

    /// <summary>
    /// Obtiene referencias
    /// </summary>
    private void Awake()
    {
        ballSprite = GetComponent<SpriteRenderer>();
        labelText = GetComponentInChildren<Text>();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (ballSprite == null)
            Debug.LogError("SpriteRenderer no asociado");

        if (labelText == null)
            Debug.LogError("Text no asociado");
#endif
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
        labelText.text = "x" + _levelManager.CurrentNumBalls.ToString();
        _levelManager.OnRoundStartCallback += OnRoundStart;
        _levelManager.OnRoundEndCallback += OnRoundEnd;
    }

    /// <summary>
    /// Reinicia el contador de pelotas y se oculta
    /// </summary>
    private void OnRoundStart()
    {
        roundBalls = _levelManager.CurrentNumBalls;
        labelText.text = "x0";
        Hide();
    }

    /// <summary>
    /// Muestra el BallSink
    /// </summary>
    private void OnRoundEnd()
    {
        Show(transform.position.x);
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
        transform.position = new Vector3(x, transform.position.y, transform.position.z);

        labelText.enabled = true;
        ballSprite.enabled = true;
    }

    /// <summary>
    /// Actualiza el número de pelotas que han llegado, y si han llegado todas, informa al LevelManager
    /// </summary>
    public void OnBallArrived(Ball ball)
    {
        _levelManager.Balls.Remove(ball);
        Destroy(ball.gameObject);

        labelText.text = "x" + (roundBalls - _levelManager.Balls.Count).ToString();

        if (_levelManager.Balls.Count == 0)
            _levelManager.RoundEnd();
    }
}
