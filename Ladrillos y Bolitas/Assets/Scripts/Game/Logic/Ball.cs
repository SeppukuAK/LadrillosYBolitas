using UnityEngine;
using System.Collections;

/// <summary>
/// Controla el comportamiento de la pelota
/// Tiene métodos para contrar su velocidad y para moverle a un punto
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    //Other References
    private Rigidbody2D rb;

    /// <summary>
    /// Obtiene referencias
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (rb == null)
            Debug.LogError("Rigidbody no asociado");
#endif
    }

    /// <summary>
    /// Da una velocidad a la pelota
    /// </summary>
    /// <param name="vel"></param>
    public void StartMoving(Vector2 vel)
    {
        rb.velocity = new Vector2(vel.x, vel.y);
    }

    /// <summary>
    /// Para el rigidbody de la pelota
    /// </summary>
    public void Stop()
    {
        rb.velocity = Vector2.zero;
    }

    /// <summary>
    /// Empieza la corrutina de movimiento a una posición en el tiempo establecido
    /// Llamará a endMoveCallback cuando haya llegado al destino
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="vel"></param>
    /// <param name="endMove"></param>
    public void MoveTo(Vector3 pos, float time, System.Action<Ball> endMoveCallback = null)
    {
        StartCoroutine(MoveToCoroutine(pos, time, endMoveCallback));
    }

    /// <summary>
    /// Corrutina que mueve la pelota hasta un punto en el tiempo establecido
    /// Llamará a endMoveCallback cuando haya llegado al destino
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="vel"></param>
    /// <param name="endMove"></param>
    /// <returns></returns>
    private IEnumerator MoveToCoroutine(Vector3 pos, float time, System.Action<Ball> endMoveCallback = null)
    {
        //Calculo de ticks y de direccion
        float totalTicks = time / Time.fixedDeltaTime;
        float distancePerTick = (pos - transform.position).magnitude / totalTicks;
        Vector2 dir = (pos - transform.position).normalized;

        //Movimiento hasta la posición
        for (int i = 0; i < totalTicks; i++)
        {
            rb.position += dir * distancePerTick;
            yield return new WaitForFixedUpdate();
        }

        //Llama al callback
        if (endMoveCallback != null)
            endMoveCallback(this);

        yield return null;
    }
}