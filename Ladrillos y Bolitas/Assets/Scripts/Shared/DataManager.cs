using System.Collections.Generic;

/// <summary>
/// Clase que contiene toda la información de la partida guardada
/// </summary>
[System.Serializable]   //Permite guardar la clase en un fichero
public class SaveDataGame
{
    public uint TotalStars; //Puntuacion total
    public uint Gems;  //Gemas

    //Guardamos tipos básicos porque no podemos convertir nuestras propias estructuras (ni de Unity) a binario
    public uint[] Level;
    public uint[] Stars;
    public bool[] Blocked;

    public SaveDataGame(uint totalStars, uint gems, List<LevelData> LevelData)
    {
        TotalStars = totalStars;
        Gems = gems;

        Level = new uint[LevelData.Count];
        Stars = new uint[LevelData.Count];
        Blocked = new bool[LevelData.Count];

        for (int i = 0; i < LevelData.Count; i++)
        {
            Level[i] = LevelData[i].Level;
            Stars[i] = LevelData[i].Stars;
            Blocked[i] = LevelData[i].Blocked;
        }
    }

    //TODO: PowerUps
}
