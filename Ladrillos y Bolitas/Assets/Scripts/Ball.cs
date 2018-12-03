using UnityEngine;
using System.Collections;

/// <summary>
/// Controla el comportamiento de la pelota
/// </summary>
public class Ball : MonoBehaviour
{
    public float Speed;
    private Rigidbody2D rb;
    private Collider2D collider2D;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
    }

    public void SetVelocity(Vector2 vel)
    {
        rb.velocity = new Vector2(vel.x, vel.y);
    }

    public void OnFirstDead()
    {
        rb.velocity = new Vector2(0.0f, 0.0f);
        collider2D.isTrigger = true;
    }

    public void MoveToSink(Vector3 pos, float SinkVelocity)
    {
        rb.velocity = new Vector2(0.0f, 0.0f);
        collider2D.isTrigger = true;
        StartCoroutine(MoveToSinkRoutine(pos, SinkVelocity));
    }

    private IEnumerator MoveToSinkRoutine(Vector3 pos, float SinkVelocity)
    {
        //TODO: Ver si acaba
        while (Mathf.Abs((transform.position - pos).magnitude) > 0.1f)
        {
            Vector3 dir = (transform.position - pos).normalized;
            transform.position -= dir * SinkVelocity * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
        yield return null;
    }
}