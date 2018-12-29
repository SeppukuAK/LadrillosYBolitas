using UnityEngine;
using System;

public class Board : MonoBehaviour
{
    public int PendingTiles { get; protected set; }

    /// <summary>
    /// Array con los prefabs de los diferentes tiles
    /// </summary>
    [SerializeField] private Tile[] tilePrefabs;

    private LevelManager _levelManager;

    private Tile[,] _board; //Array de Tiles

    private int width;
    private int height;

    /// <summary>
    /// Fichero que queremos leer
    /// </summary>
    public void Init(LevelManager levelManager, TextAsset map)
    {
        _levelManager = levelManager;
        PendingTiles = 0;

        string[] layers = map.text.Split(new string[] { "[layer]" }, StringSplitOptions.None);

        //Calculo del tamaño del board
        width = layers[1].Split('\n')[3].Split(',').Length - 1;
        height = layers[1].Split('\n').Length - 4;

        //Tipos de Tile
        int[,] typeMap = DataStringToIntMap(layers[1].Split(new string[] { "data=" }, StringSplitOptions.None)[1]);

        //Vida del tile
        int[,] healthMap = DataStringToIntMap(layers[2].Split(new string[] { "data=" }, StringSplitOptions.None)[1]);

        _board = new Tile[height, width];

        //Creación del tablero
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (typeMap[height - i - 1, j] > 0)
                {
                    Tile tilePrefab = tilePrefabs[typeMap[height - i - 1, j] - 1];
                    _board[i, j] = Instantiate(tilePrefab, transform.position + new Vector3(j, i+1, 0), tilePrefab.transform.rotation, transform);
                    _board[i, j].Init(_levelManager, this, healthMap[height - i - 1, j], j, i+1);

                    PendingTiles++;
                }
            }
        }

        levelManager.OnRoundEndCallback += OnRoundEnd;
    }

    /// <summary>
    /// Convierte un DataMap guardado en un string en una matriz de Int
    /// </summary>
    /// <param name="dataMap"></param>
    /// <returns></returns>
    private int[,] DataStringToIntMap(string dataMap)
    {
        string[] stringMap = dataMap.Trim().Split(',', '.');

        int[,] intMap = new int[height, width];

        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
                intMap[i, j] = int.Parse(stringMap[i * width + j].Trim());

        return intMap;
    }

    public void TileDestroyed(Tile tile)
    {
        _board[tile.Y-1, tile.X] = null;
        Destroy(tile.gameObject);

        PendingTiles--;

        if (PendingTiles == 0)
            _levelManager.LevelEnd();
    }

    /// <summary>
    /// Es llamado cuando ha acabado una tirada
    /// Baja todos los ladrillos
    /// </summary>
    public void OnRoundEnd()
    {
        foreach (Tile tile in _board)
            if (tile != null)
                tile.SetPosition(tile.X, tile.Y - 1);
    }
}

