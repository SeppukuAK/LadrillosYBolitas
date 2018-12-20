using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour {

    private LineRenderer lineRenderer;
    private BallSpawner _ballSpawner;
    private RaycastHit hit;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();    
    }

    public void Init(BallSpawner ballSpawner)
    {
        _ballSpawner = ballSpawner;
        lineRenderer.positionCount = 0;
    }

	void Update () {

        //Solo si hay un dedo hago cosas
        if (Input.touchCount == 1)
        {
            //Store the first touch detected.
            Touch myTouch = Input.touches[0];

           

            if (myTouch.phase == TouchPhase.Began)
            {

                //Creación del rayo
                Ray ray = new Ray(_ballSpawner.transform.position, myTouch.position);
                if(Physics.Raycast(ray, out hit, 10))
                {
                    if(hit.collider.name == "Top" || hit.collider.name == "Bot" || hit.collider.name == "Left" || hit.collider.name == "Right")
                    {
                        lineRenderer.SetPosition(2, hit.transform.position); //Posición del primer vert en el Spawner
                        lineRenderer.SetPosition(3, ); //Posición del primer vert en el Spawner

                    }
                }


                lineRenderer.positionCount = 2;//Se establece el número de vertices
                lineRenderer.SetPosition(0, _ballSpawner.transform.position); //Posición del primer vert en el Spawner

                Vector3 pos = Camera.main.ScreenToWorldPoint(myTouch.position);
                pos.z = 0;
                lineRenderer.SetPosition(1, pos) ; //Posición donde ha tocado el usuario

            }

            //Check if the phase of that touch equals Began
            else if (myTouch.phase == TouchPhase.Moved)
            {

                Vector3 pos = Camera.main.ScreenToWorldPoint(myTouch.position);
                pos.z = 0;
                lineRenderer.SetPosition(1, pos); //Posición donde ha tocado el usuario

                //If so, set touchOrigin to the position of that touch
                Debug.Log("NO ME TOQUES");
            }

            //Disparo si soltamos
            else if (myTouch.phase == TouchPhase.Ended)
            {
                lineRenderer.positionCount = 0;//Se establece el número de vertices

                Debug.Log("GRACIAS");
            }
        }

        //Si hay más de 2 dedos o ninguno, paro el disparo
        else
        {

        }
    }
}
