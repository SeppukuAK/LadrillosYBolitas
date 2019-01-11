using UnityEngine;

/// <summary>
/// Clase base para todos los paneles que se superponen al juego
/// Permite establecer una función a la que llamar cuando es destruido
/// </summary>
public class OverlayUI : MonoBehaviour {

    /// <summary>
    /// Función a la que tiene que llamar cuando es destruido
    /// </summary>
    private System.Action _destroyCallback;

    /// <summary>
    /// Establece la función a la que tiene que llamar cuando es destruido
    /// </summary>
    /// <param name="destroyCallback"></param>
    public void SetCallbackOnDestroy(System.Action destroyCallback)
    {
        _destroyCallback = destroyCallback;
    }

    /// <summary>
    /// Cuando se pulsa la 'x' de salir. Se destruye el canvas y se llama al destroyCallback
    /// </summary>
    public virtual void Exit()
    {
        if (_destroyCallback != null)
            _destroyCallback.Invoke();

        Destroy(gameObject);
    }
}
