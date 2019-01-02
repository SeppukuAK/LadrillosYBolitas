
/// <summary>
/// Bloque indestructible que no puede caer
/// </summary>
public class UnbreakableTile : Tile
{
    public override bool MustBeDestroyed() { return false; }
    public override bool CanFall() { return false; }
}
