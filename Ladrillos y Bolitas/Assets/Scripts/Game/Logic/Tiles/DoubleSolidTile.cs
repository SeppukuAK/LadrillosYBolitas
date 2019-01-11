
/// <summary>
/// Solid Tile que duplica el número de golpes que hay que darle
/// </summary>
public class DoubleSolidTile : SolidTile {

    /// <summary>
    /// Inicializa el Tile
    /// Establece su posicion y guarda la vida y callback
    /// La vida es duplicada
    /// </summary>
    /// <param name="health"></param>
    /// <param name="onDestroyCallBack"></param>
    public override void Init(int value, System.Action<Tile> onDestroyCallBack = null)
    {
        base.Init(value, onDestroyCallBack);
        Value *= 2;
    }
}
