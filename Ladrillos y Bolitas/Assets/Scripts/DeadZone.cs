using UnityEngine;

/// <summary>
/// Componente asociado a um GameObject que cuando una pelota colisiona con él, le informa de que ha muerto
/// </summary>
public class DeadZone : MonoBehaviour
{

    public float SinkVelocity;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball)
        {
            //Ha chocado la primera pelota
            if (!LevelManager.Instance.BallSpawner.FirstRoundCollision)
            {
                LevelManager.Instance.BallSpawner.FirstRoundCollision = true;
                LevelManager.Instance.BallSpawner.transform.position = ball.transform.position;
                ball.OnFirstDead();
            }
            else
            {
                ball.MoveToSink(LevelManager.Instance.BallSpawner.transform.position, SinkVelocity);
            }

        }
    }
}

