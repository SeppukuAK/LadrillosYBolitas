using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour {

    private Vector2 touchPos;

	// Update is called once per frame
	void Update () {

        //Solo si hay un dedo hago cosas
        if (Input.touchCount == 1)
        {
            //Store the first touch detected.
            Touch myTouch = Input.touches[0];

            if (myTouch.phase == TouchPhase.Began)
            {
                   
            }

            //Check if the phase of that touch equals Began
            else if (myTouch.phase == TouchPhase.Moved)
            {
                //If so, set touchOrigin to the position of that touch
                touchPos = myTouch.position;
                Debug.Log("NO ME TOQUES");
            }

            //Disparo si soltamos
            else if (myTouch.phase == TouchPhase.Ended)
            {
                //If so, set touchOrigin to the position of that touch
                touchPos = myTouch.position;
                Debug.Log("GRACIAS");
            }
        }

        //Si hay más de 2 dedos o ninguno, paro el disparo
        else
        {

        }
    }
}
