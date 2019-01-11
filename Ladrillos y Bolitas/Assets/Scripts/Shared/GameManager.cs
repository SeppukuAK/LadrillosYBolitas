using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Estructura de datos que representa el progreso en un nivel
/// </summary>
public class LevelData
{
    public uint Level;
    public uint Stars;
    public bool Blocked;
}

/// <summary>
/// Clase persistente entre escenas
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum PowerUpType { DestroyRow };

    /// <summary>
    /// Fichero con la información de las bolas disponibles en cada nivel
    /// </summary>
    public TextAsset GameData;

    /// <summary>
    /// Array con la información de todos los mapas
    /// </summary>
    public TextAsset[] MapData;

    public uint Gems
    {
        get { return gems; }
        set
        {
            gems = value;
            SaveData();
        }
    }
    private uint gems;

    public uint TotalStars { get; set; }
    public List<LevelData> LevelData;

    public uint[] PowerUps { get; set; }

    public uint SelectedMapLevel { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();

        }
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Establece el número de powerUps de los que se dispone de un tipo.
    /// Guarda el progreso cuando se establece
    /// </summary>
    /// <param name="powerUpType"></param>
    /// <param name="value"></param>
    public void SetNumPowerUp(PowerUpType powerUpType, uint value)
    {
        PowerUps[(int)powerUpType] = value;
        SaveData();
    }

    public void SaveData()
    {
        SaveSystem.SaveGameData(TotalStars, gems, PowerUps, LevelData);
    }

    public void LoadData()
    {
        SaveDataGame saveDataGame = SaveSystem.LoadGameDate();

        //Crea nuevo archivo de guardado
        if (saveDataGame == null)
            ResetSaveData();

        //Carga el fichero existente
        else
        {
            TotalStars = saveDataGame.TotalStars;
            gems = saveDataGame.Gems;
            PowerUps = saveDataGame.PowerUps;
            //TODO: Comprobar que se guardan bien los powerUps
            LevelData = new List<LevelData>();

            for (int i = 0; i < MapData.Length; i++)
            {
                LevelData levelData = new LevelData();
                levelData.Level = saveDataGame.Level[i];
                levelData.Stars = saveDataGame.Stars[i];
                levelData.Blocked = saveDataGame.Blocked[i];

                LevelData.Add(levelData);
            }
        }
    }

    public void ResetSaveData()
    {
        TotalStars = 0;
        gems = 0;

        uint numPowerUps = (uint)System.Enum.GetValues(typeof(PowerUpType)).Length;
        PowerUps = new uint[numPowerUps];
        for (int i = 0; i < PowerUps.Length; i++)
            PowerUps[i] = 2;

        LevelData = new List<LevelData>();

        for (int i = 0; i < MapData.Length; i++)
        {
            LevelData levelData = new LevelData();
            levelData.Stars = 0;
            levelData.Level = (uint)i + 1;
            levelData.Blocked = true;

            LevelData.Add(levelData);
        }

        LevelData[0].Blocked = false;

        SaveData();
    }
}
