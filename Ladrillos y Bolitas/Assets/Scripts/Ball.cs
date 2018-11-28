using UnityEngine;

/// <summary>
/// Controla el comportamiento de la pelota
/// </summary>
public class Ball : MonoBehaviour
{
    public float Speed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(Vector2 vel)
    {
        rb.velocity = new Vector2(vel.x, vel.y);

    }


}
