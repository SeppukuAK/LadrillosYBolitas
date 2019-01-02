using System.Collections.Generic;

/// <summary>
/// Clase que contiene toda la información de la partida guardada
/// </summary>
[System.Serializable]   //Permite guardar la clase en un fichero
public class SaveDataGame
{
    public uint TotalScore; //Puntuacion total
    public uint Coins;  //Gemas

    //Guardamos tipos básicos porque no podemos convertir nuestras propias estructuras (ni de Unity) a binario
    public uint[] Level;
    public uint[] Points;
    public bool[] Blocked;

    public SaveDataGame(uint totalScore, uint coins, List<LevelData> LevelData)
    {
        TotalScore = totalScore;
        Coins = coins;

        Level = new uint[LevelData.Count];
        Points = new uint[LevelData.Count];
        Blocked = new bool[LevelData.Count];

        for (int i = 0; i < 0; i++)
        {
            Level[i] = LevelData[i].Level;
            Points[i] = LevelData[i].Points;
            Blocked[i] = LevelData[i].Blocked;
        }
    }

    //TODO: PowerUps
}
