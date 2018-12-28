using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Estructura de datos que representa el progreso en un nivel
/// </summary>
public class LevelData
{
    public uint Level;
    public uint Points;
    public bool Blocked;
}

/// <summary>
/// Clase persistente entre escenas
/// </summary>
public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    /// <summary>
    /// Fichero con la información de las bolas disponibles en cada nivel
    /// </summary>
    public TextAsset GameData;

    /// <summary>
    /// Array con la información de todos los mapas
    /// </summary>
    public TextAsset[] MapData;

    public uint TotalScore { get; set; }
    public uint Coins { get; set; }
    public List<LevelData> LevelData;

    [Header("Test Attributes")]
    public int MapLevel;//Mapa con la informacion del juego

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadData();
    }

    private void Update()
    {
        Debug.Log("lapis:" + Coins);

        if (Input.GetKeyDown(KeyCode.A))
            SaveData();
        

        if (Input.GetKeyDown(KeyCode.S))
            LoadData();

        if (Input.GetKeyDown(KeyCode.D))
            Coins++;
        if (Input.GetKeyDown(KeyCode.W))
            ResetSaveData();
    }

    public void SaveData()
    {
        SaveSystem.SaveGameData(TotalScore, Coins, LevelData);
    }

    public void LoadData()
    {
        SaveDataGame saveDataGame = SaveSystem.LoadGameDate();

        //Crea nuevo archivo de guardado
        if (saveDataGame == null)
        {
            ResetSaveData();
        }

        //Carga el fichero existente
        else
        {
            TotalScore = saveDataGame.TotalScore;
            Coins = saveDataGame.Coins;

            LevelData = new List<LevelData>();

            for (int i = 0; i < MapData.Length; i++)
            {
                LevelData levelData = new LevelData();
                levelData.Level = saveDataGame.Level[i];
                levelData.Points = saveDataGame.Points[i];
                levelData.Blocked = saveDataGame.Blocked[i];

                LevelData.Add(levelData);
            }
        }
    }

    private void ResetSaveData()
    {
        TotalScore = 0;
        Coins = 0;
        LevelData = new List<LevelData>();

        for (int i = 0; i < MapData.Length; i++)
        {
            LevelData levelData = new LevelData();
            levelData.Points = 0;
            levelData.Level = (uint)i + 1;
            levelData.Blocked = true;

            LevelData.Add(levelData);
        }

        LevelData[0].Blocked = false;

        SaveData();
    }
}
