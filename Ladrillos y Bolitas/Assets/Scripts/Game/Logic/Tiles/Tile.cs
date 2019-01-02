using UnityEngine;

/// <summary>
/// Clase virtual que representa un tile del juego
/// Todas los tipos de Tiles heredan de esta clase base
/// </summary>
public class Tile : MonoBehaviour
{
    //Propiedades especificas del tile
    public virtual bool MustBeDestroyed() { return true; }
    public virtual bool CanFall() { return true; }

    //Posicion
    public uint X { get; set; }
    public uint Y { get; set; }

    protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// Cuando es modificado activa/desactiva su renderizado
    /// </summary>
    protected virtual bool visible
    {
        get { return spriteRenderer.enabled; }
        set
        {
            spriteRenderer.enabled = value;
        }
    }

    /// <summary>
    /// Obtiene referencias
    /// </summary>
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer no asociado");
#endif
    }

    /// <summary>
    /// Inicializa el Tile
    /// Establece su posicion
    /// </summary>
    /// <param name="health"></param>
    /// <param name="onDestroyCallBack"></param>
    public virtual void Init(int value, System.Action<Tile> onDestroyCallBack = null)
    {
        SetPosition(new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y));
    }

    /// <summary>
    /// Establece la nueva posición del tile
    /// Establece si es visible
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetPosition(Vector2Int newPos)
    {
        X = (uint)newPos.x;
        Y = (uint)newPos.y;

        visible = Y < (Board.BOARD_HEIGHT - 1);

        transform.localPosition = new Vector3(X, Y, 0);
    }

}
