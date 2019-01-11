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
/// Tipos de powerUp
/// </summary>
public enum PowerUpType { DestroyRow ,Earthquake};

/// <summary>
/// Clase persistente entre escenas
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Initial Attributes")]
    /// <summary>
    /// Gemas con las que empieza el jugador al inicio
    /// </summary>
    [SerializeField] private uint initialGems;

    /// <summary>
    /// Número de powerUps con los que empieza el jugador al inicio
    /// </summary>
    [SerializeField] private uint initialPowerUps;

    [Header("TextAssets")]
    /// <summary>
    /// Fichero con la información de las bolas disponibles en cada nivel
    /// </summary>
    public TextAsset GameData;

    /// <summary>
    /// Array con la información de todos los mapas
    /// </summary>
    public TextAsset[] MapData;

    /// <summary>
    /// Cada vez que es modificado, guarda los datos
    /// </summary>
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

    /// <summary>
    /// Número total de estrellas conseguidas
    /// </summary>
    public uint TotalStars { get; set; }

    /// <summary>
    /// Array con los powerUps disponibles
    /// </summary>
    public uint[] PowerUps { get; set; }

    /// <summary>
    /// Lista con la información de los niveles
    /// </summary>
    public List<LevelData> LevelData;

    /// <summary>
    /// Nivel seleccionado actualmente
    /// </summary>
    public uint SelectedMapLevel { get; set; }

    /// <summary>
    /// Carga los datos de guardado
    /// </summary>
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

    /// <summary>
    /// Guarda los datos en el archivo .data
    /// </summary>
    public void SaveData()
    {
        SaveSystem.SaveGameData(TotalStars, gems, PowerUps, LevelData);
    }

    /// <summary>
    /// Carga los archivos de guardado
    /// </summary>
    private void LoadData()
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

    /// <summary>
    /// Resetea el archivo de guardado al por defecto y lo guarda
    /// </summary>
    public void ResetSaveData()
    {
        TotalStars = 0;
        gems = initialGems;

        uint numPowerUps = (uint)System.Enum.GetValues(typeof(PowerUpType)).Length;
        PowerUps = new uint[numPowerUps];

        for (int i = 0; i < PowerUps.Length; i++)
            PowerUps[i] = initialPowerUps;

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
