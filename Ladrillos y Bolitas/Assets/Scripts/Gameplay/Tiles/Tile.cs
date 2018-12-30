using UnityEngine;

public class Tile : MonoBehaviour
{
    public virtual bool MustBeDestroyed() {return true; }
    public virtual bool CanFall() { return true; }

    public int X { get; set; }
    public int Y { get; set; }

    protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// Cuando es modificado activa/desactiva su renderizado
    /// </summary>
    protected virtual bool visible
    {
        get { return _visible; }
        set
        {
            _visible = value;
            spriteRenderer.enabled = _visible;
        }
    }
    protected bool _visible;

    /// <summary>
    /// Obtiene referencias
    /// </summary>
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Inicializa el Tile
    /// </summary>
    /// <param name="health"></param>
    /// <param name="onDestroyCallBack"></param>
    public virtual void Init(int value, System.Action<Tile> onDestroyCallBack = null)
    {
        SetPosition(new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y));
    }

    /// <summary>
    /// Establece la nueva posición del tile
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetPosition(Vector2Int newPos)
    {
        X = newPos.x;
        Y = newPos.y;

        visible = Y < Board.VISIBLEHEIGHT;

        transform.localPosition = new Vector3(X, Y, 0);
    }

}
