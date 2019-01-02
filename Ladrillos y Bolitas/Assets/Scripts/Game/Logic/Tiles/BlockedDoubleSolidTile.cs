
/// <summary>
/// Double Solid Tile que no puede moverse del sitio
/// </summary>
public class BlockedDoubleSolidTile : DoubleSolidTile {

    public override bool CanFall() { return false; }
}
