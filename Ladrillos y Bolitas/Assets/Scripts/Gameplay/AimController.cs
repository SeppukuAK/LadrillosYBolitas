using UnityEngine;

/// <summary>
/// Controlador del input del usuario.
/// Detecta cuando se pulsa en la pantalla para disparar las bolas
/// Renderiza una linea auxiliar de apuntado
/// </summary>
public class AimController : MonoBehaviour
{
    //Own Components
    private LineRenderer lineRenderer;

    //References
    private LevelManager _levelManager;
    private BallSpawner _ballSpawner;

    private bool canShoot;
    private float _ballVelocity;

    /// <summary>
    /// Obtiene referencias
    /// </summary>
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Init(LevelManager levelManager, BallSpawner ballSpawner, float ballVelocity)
    {
        _levelManager = levelManager;
        _ballSpawner = ballSpawner;
        _ballVelocity = ballVelocity;

        lineRenderer.positionCount = 0;
        levelManager.OnRoundStartCallback += OnRoundStart;
        levelManager.OnRoundEndCallback += OnRoundEnd;

        canShoot = true;
    }

    /// <summary>
    /// Anula el poder disparar
    /// </summary>
    public void OnRoundStart()
    {
        canShoot = false;
    }

    /// <summary>
    /// Permite volver a disparar
    /// </summary>
    public void OnRoundEnd()
    {
        canShoot = true;
    }

    /// <summary>
    /// Detecta input.
    /// Cuando se suelta el dedo, informa de que ha empezado una nueva ronda y dispara las pelotas
    /// </summary>
	private void Update()
    {
        if (canShoot)
        {
            if (Input.GetMouseButtonUp(0))
            {
                _levelManager.RoundStart();
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                Vector2 dir = (mousePos - _ballSpawner.transform.position).normalized;
                _ballSpawner.SpawnBalls(_levelManager.CurrentNumBalls, _ballVelocity * dir);
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
    }
}
