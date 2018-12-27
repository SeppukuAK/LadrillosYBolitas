using System.Collections.Generic;

/// <summary>
/// Estructura de datos que representa el progreso en un nivel
/// </summary>
public class LevelData
{
    public uint Level;
    public uint Points;
}

/// <summary>
/// Clase que contiene toda la información de la partida guardada
/// </summary>
public class DataManager {

    public uint TotalScore;
    public uint Coins;
    public List<LevelData> LevelData;

}
