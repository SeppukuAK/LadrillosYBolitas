using UnityEngine;

/// <summary>
/// Componente asociado a um GameObject que cuando una pelota colisiona con él, le informa de que ha muerto
/// </summary>
public class DeadZone : MonoBehaviour {

    public float SinkVelocity;
    private Vector3 ballPosition;

    public void OnNextRound()
    {
        ballPosition = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball)
        {
            //Ha chocado la primera pelota
            if (ballPosition == Vector3.zero)
            {
                ballPosition = ball.transform.position;
                ball.SetVelocity(new Vector2(0f, 0f));
            }
            else
            {
                ball.MoveToSink(ballPosition, SinkVelocity);
            }

        }
    }
    
}
