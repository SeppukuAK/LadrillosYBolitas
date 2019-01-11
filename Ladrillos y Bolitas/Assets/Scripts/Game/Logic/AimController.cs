using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Controlador del input del usuario.
/// Detecta cuando se pulsa en la pantalla para disparar las bolas
/// Renderiza una linea auxiliar de apuntado
/// </summary>
public class AimController : MonoBehaviour
{
    [SerializeField] private float heightOffset;
    [SerializeField] private float fastImageDuration;
    [SerializeField] private Image fastImage;

    private bool canShoot;
    private float _ballVelocity;
    private uint _maxTimeScale;

    //Own References
    private LineRenderer lineRenderer;

    //Other References
    private LevelManager _levelManager;
    private BallSpawner _ballSpawner;


    /// <summary>
    /// Obtiene referencias
    /// </summary>
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (lineRenderer == null)
            Debug.LogError("LineRenderer no asociado");
#endif
    }

    /// <summary>
    /// Inicializa el spawner, se suscribe a los eventos inicio de ronda y fin de ronda 
    /// </summary>
    /// <param name="levelManager"></param>
    /// <param name="ballSpawner"></param>
    /// <param name="ballVelocity"></param>
    /// <param name="maxTimeScale"></param>
    public void Init(LevelManager levelManager, BallSpawner ballSpawner, float ballVelocity, uint maxTimeScale)
    {
        _levelManager = levelManager;
        _ballSpawner = ballSpawner;
        _ballVelocity = ballVelocity;
        _maxTimeScale = maxTimeScale;

        lineRenderer.positionCount = 0;
        levelManager.OnRoundStartCallback += OnRoundStart;
        levelManager.OnRoundEndCallback += OnRoundEnd;


        canShoot = true;
    }

    /// <summary>
    /// Anula el poder disparar
    /// </summary>
    private void OnRoundStart()
    {
        canShoot = false;
    }

    /// <summary>
    /// Permite volver a disparar
    /// </summary>
    private void OnRoundEnd()
    {
        canShoot = true;
        Time.timeScale = 1.0f;
    }

    /// <summary>
    /// Detecta input.
    /// Cuando se suelta el dedo, informa de que ha empezado una nueva ronda y dispara las pelotas
    /// </summary>
	private void Update()
    {
        if (!_levelManager.Pause)
        {
#if UNITY_EDITOR
            //Obtenemos la posición del ratón
            Vector3 mousePos;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
#endif
            //Comprobamos si se puede disparar
            if (canShoot)
            {
#if UNITY_EDITOR
                //Comprobamos el input dentro del tablero teniendo en cuenta el Offset
                if (OnBoard(mousePos))
                {
                    //Se detecta cuando se hace click
                    if (Input.GetMouseButtonDown(0))
                    {
                        lineRenderer.enabled = true;
                        lineRenderer.positionCount = 2;//Se establece el número de vertices
                        lineRenderer.SetPosition(0, _ballSpawner.transform.position); //Posición del primer vert en el Spawner

                        lineRenderer.SetPosition(1, mousePos); //Posición donde ha tocado el usuario
                    }
                    //Se detecta cuando se mantiene el ratón
                    else if (Input.GetMouseButton(0))
                    {
                        lineRenderer.enabled = true;
                        lineRenderer.SetPosition(1, mousePos); 
                    }
                    //Se detecta cuando se suelta el ratón
                    if (Input.GetMouseButtonUp(0))
                    {
                        lineRenderer.positionCount = 0;//Se resetea a 0 el número de vértices

                        //Comprobar si no pulsa demasiado cerca
                        _levelManager.RoundStart();

                        //Se spawnean las bolas
                        Vector2 dir = (mousePos - _ballSpawner.transform.position).normalized;
                        _ballSpawner.SpawnBalls(_levelManager.CurrentNumBalls, _ballVelocity * dir);

                    }
                }
                //Caso en el que el ratón se encuentra fuera del board
                else
                {
                    if (Input.GetMouseButton(0))
                        lineRenderer.enabled = false;
                }
#endif
#if UNITY_ANDROID 
                //Detectamos que solo se está pulsando con un dedo
                if (Input.touchCount == 1)
                {
                    Touch myTouch;
                    myTouch = Input.touches[0];

                    //Se obtiene la posición del dedo
                    Vector3 pos = Camera.main.ScreenToWorldPoint(myTouch.position);
                    pos.z = 0;

                    //Comprobamos el input dentro del tablero teniendo en cuenta el Offset
                    if (OnBoard(pos))
                    {
                        //Se detecta cuándo se hace tap
                        if (myTouch.phase == TouchPhase.Began)
                        {
                            lineRenderer.enabled = true;
                            lineRenderer.positionCount = 2;//Se establece el número de vertices
                            lineRenderer.SetPosition(0, _ballSpawner.transform.position); //Posición del primer vert en el Spawner

                            lineRenderer.SetPosition(1, pos); //Posición donde ha tocado el usuario
                        }

                        //Se detecta cuando se mantiene el dedo
                        else if (myTouch.phase == TouchPhase.Moved)
                        {
                            lineRenderer.enabled = true;
                            lineRenderer.SetPosition(1, pos); 
                        }
                        //Caso en el que se hay un dedo pulsando la pantalla pero no se mueve
                        else if (myTouch.phase == TouchPhase.Stationary)
                            lineRenderer.enabled = true;

                        //Se detecta cuando se suelta el dedo
                        else if (myTouch.phase == TouchPhase.Ended && lineRenderer.enabled)
                        {
                            lineRenderer.positionCount = 0;//Se resetea a 0 el número de vértices
                            _levelManager.RoundStart();

                            Vector2 dir = (pos - _ballSpawner.transform.position).normalized;
                            _ballSpawner.SpawnBalls(_levelManager.CurrentNumBalls, _ballVelocity * dir);

                        }
                    }
                    //Caso en el que el dedo se encuentra fuera del board
                    else
                    {
                        if (myTouch.phase == TouchPhase.Moved)
                            lineRenderer.enabled = false;
                    }
                }

                //Caso en el que hay más de 2 dedos, se desactiva el lineRenderer
                else if (Input.touchCount > 1)
                    lineRenderer.enabled = false;

#endif
            }

            //Caso en el que no se puede disparar
            else
            {
#if UNITY_EDITOR
                //Cuando se hace click, se duplica la velocidad del motor
                if (Input.GetMouseButtonUp(0) && Time.timeScale < _maxTimeScale && OnBoard(mousePos))
                {
                    Time.timeScale++;
                    StartCoroutine(FadeInFadeOut());
                }
#endif
#if UNITY_ANDROID
                Touch myTouch;

                if (Input.touchCount >= 1)
                {
                    myTouch = Input.touches[0];

                    //Se obtiene la posición del tap
                    Vector3 pos = Camera.main.ScreenToWorldPoint(myTouch.position);
                    pos.z = 0;

                    //Cuando se hace tap, se duplica la velocidad del motor
                    if (myTouch.phase == TouchPhase.Ended && Time.timeScale < _maxTimeScale && OnBoard(pos))
                    {
                        Time.timeScale++;
                        StartCoroutine(FadeInFadeOut());
                    }
                }
#endif
            }
        }
    }

    /// <summary>
    /// Devuelve si la posición está dentro del tablero
    /// </summary>
    /// <returns></returns>
    private bool OnBoard(Vector2 pos)
    {
        //Comprobación de si ha pulsado dentro y con un offset
        return Mathf.Abs(pos.y) <= Board.BOARD_HEIGHT / 2.0f && Mathf.Abs(pos.x) <= Board.BOARD_WIDTH / 2.0f && pos.y > -(Board.BOARD_HEIGHT / 2.0f - heightOffset);
    }

    /// <summary>
    /// Corrutina que hace un Fade in y Fade Out para el modo de acelerar las bolas
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInFadeOut()
    {
        Color desiredColor = fastImage.color;
        desiredColor.a = 1.0f;
        StartCoroutine(UtilitiesManager.FadeImage(fastImage, fastImageDuration, desiredColor));
        yield return new WaitForSeconds(fastImageDuration);
        desiredColor.a = 0.0f;
        StartCoroutine(UtilitiesManager.FadeImage(fastImage, fastImageDuration, desiredColor));
    }
}
