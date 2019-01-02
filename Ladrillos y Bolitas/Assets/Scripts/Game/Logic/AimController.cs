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
            if (canShoot)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = 0;

                    //Teniendo en cuenta el Offset
                    if (OnBoard(mousePos))
                    {
                        //Comprobar si no pulsa demasiado cerca
                        _levelManager.RoundStart();

                        Vector2 dir = (mousePos - _ballSpawner.transform.position).normalized;
                        _ballSpawner.SpawnBalls(_levelManager.CurrentNumBalls, _ballVelocity * dir);
                    }
                }

                //Solo si hay un dedo hago cosas
                if (Input.touchCount == 1)
                {
                    //Store the first touch detected.
                    Touch myTouch = Input.touches[0];

                    if (myTouch.phase == TouchPhase.Began)
                    {
                        lineRenderer.positionCount = 2;//Se establece el número de vertices
                        lineRenderer.SetPosition(0, _ballSpawner.transform.position); //Posición del primer vert en el Spawner

                        Vector3 pos = Camera.main.ScreenToWorldPoint(myTouch.position);
                        pos.z = 0;
                        lineRenderer.SetPosition(1, pos); //Posición donde ha tocado el usuario
                    }

                    //Check if the phase of that touch equals Began
                    else if (myTouch.phase == TouchPhase.Moved)
                    {
                        Vector3 pos = Camera.main.ScreenToWorldPoint(myTouch.position);
                        pos.z = 0;
                        lineRenderer.SetPosition(1, pos); //Posición donde ha tocado el usuario

                        RaycastHit hit;
                        ////Creación del rayo
                        //Ray ray = new Ray(_ballSpawner.transform.position, myTouch.position);
                        //if(Physics.Raycast(ray, out hit, 10))
                        //{
                        //    if(hit.collider.name == "Top" || hit.collider.name == "Bot" || hit.collider.name == "Left" || hit.collider.name == "Right")
                        //    {
                        //        lineRenderer.SetPosition(2, hit.transform.position); //Posición del primer vert en el Spawner
                        //        lineRenderer.SetPosition(3, ); //Posición del primer vert en el Spawner

                        //    }
                        //}

                        //If so, set touchOrigin to the position of that touch
                    }

                    //Disparo si soltamos
                    else if (myTouch.phase == TouchPhase.Ended)
                    {
                        lineRenderer.positionCount = 0;//Se establece el número de vertices
                        _levelManager.RoundStart();
                        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        mousePos.z = 0;

                        Vector2 dir = (mousePos - _ballSpawner.transform.position).normalized;
                        _ballSpawner.SpawnBalls(_levelManager.CurrentNumBalls, _ballVelocity * dir);

                    }
                }

                //Si hay más de 2 dedos o ninguno, paro el disparo
                else
                {

                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0) && Time.timeScale < _maxTimeScale && OnBoard(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                {
                    Time.timeScale++;

                    StartCoroutine(FadeInFadeOut());
                }
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

    private IEnumerator FadeInFadeOut()
    {
        Color desiredColor = fastImage.color;
        desiredColor.a = 1.0f;
        StartCoroutine(MenuCanvas.FadeImage(fastImage, fastImageDuration, desiredColor));
        yield return new WaitForSeconds(fastImageDuration);
        desiredColor.a = 0.0f;
        StartCoroutine(MenuCanvas.FadeImage(fastImage, fastImageDuration, desiredColor));
    }
}
