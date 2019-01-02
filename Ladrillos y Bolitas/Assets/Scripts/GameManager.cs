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

    public uint TotalStars{ get; set; }
    public uint Gems { get; set; }
    public List<LevelData> LevelData;

    [Header("Test Attributes")]
    public uint MapLevel;//Mapa con la informacion del juego

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            SaveData();

        if (Input.GetKeyDown(KeyCode.B))
            SaveSystem.DeleteData();


        if (Input.GetKeyDown(KeyCode.S))
            LoadData();

        if (Input.GetKeyDown(KeyCode.D))
            Gems++;
        if (Input.GetKeyDown(KeyCode.W))
            ResetSaveData();
    }

    public void SaveData()
    {
        SaveSystem.SaveGameData(TotalStars, Gems, LevelData);
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
            TotalStars = saveDataGame.TotalStars;
            Gems = saveDataGame.Gems;

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

    private void ResetSaveData()
    {
        TotalStars = 0;
        Gems = 0;
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
