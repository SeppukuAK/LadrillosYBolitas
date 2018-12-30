using UnityEngine;
using System;

/// <summary>
/// Tablero contenedor de todos los tiles
/// </summary>
public class Board : MonoBehaviour
{
    public const int VISIBLEHEIGHT = 13;
    /// <summary>
    /// Array con los prefabs de los diferentes tiles
    /// </summary>
    [SerializeField] private Tile[] tilePrefabs;

    /// <summary>
    /// Tiles que quedan por destruir
    /// </summary>
    public int PendingTiles { get; protected set; }

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

        height += 1; //Guardamos en board también la linea de "Perder"
        _board = new Tile[height, width];

        //Creación del tablero
        for (int i = 1; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (typeMap[height - i - 1, j] > 0)
                {
                    //TODO: DEBUG
                    if (tilePrefabs[typeMap[height - i - 1, j] - 1] == null)
                        Debug.LogError("Tile: " + typeMap[height - i - 1, j] + " no implementado");

                    else
                    {
                        Tile tile = tilePrefabs[typeMap[height - i - 1, j] - 1];
                        _board[i, j] = Instantiate(tile, transform.position + new Vector3(j, i, 0), tile.transform.rotation, transform);

                        if (tile.MustBeDestroyed())
                        {
                            _board[i, j].Init(healthMap[height - i - 1, j], TileDestroyed);
                            PendingTiles++;
                        }
                        else
                            _board[i, j].Init(healthMap[height - i - 1, j]);
                    }

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

    /// <summary>
    /// Es llamado cuando un tile es destruido
    /// </summary>
    /// <param name="tile"></param>
    public void TileDestroyed(Tile tile)
    {
        _board[tile.Y, tile.X] = null;
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
        //Bajamos todos los tiles
        for (int i = 1; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //Si hay tile
                if (_board[i, j] != null)
                {
                    Tile tile = _board[i, j];

                    //Si es un tipo de tile que se puede mover
                    if (tile.CanFall())
                    {
                        //Comprobamos si no hay tile abajo para moverse
                        if (_board[i - 1, j] == null)
                        {
                            _board[i, j] = null;
                            _board[i - 1, j] = tile;
                            tile.SetPosition(new Vector2Int(j, i - 1));
                        }
                    }
                }
            }
        }

        //Comprobación si se ha perdido
        int k = 0;
        bool fail = false;
        while (!fail && k < width)
        {
            if (_board[0, k] != null)
                fail = true;
            k++;
        }

        if (fail)
            _levelManager.LevelEnd();

    }
}

