using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Tile[,] _board; //Array de Tiles
    public int PendingTiles { get; protected set; }

    [SerializeField] private Tile _tilePrefab;

    private LevelManager _levelManager;
    const int OFFSET = 3;

    /// <summary>
    /// Fichero que queremos leer
    /// </summary>
    public void Init(LevelManager levelManager, int mapLevel)
    {
        _levelManager = levelManager;
        PendingTiles = 0;

        TextAsset map = GameManager.Instance.MapData[mapLevel];
        string[] layers = map.text.Split('[');//Se separa el contenido del mapa por layers

        //Lectura de los tipos de Tile
        string[] layerNumberTiles = layers[1].Split('\n');

        int N = layerNumberTiles.Length - OFFSET - 1; //Tamaño de la matriz de Tiles

        string[] inversedMatrix = new string[N];


        for (int i = 0; i < N; i++)
            inversedMatrix[i] = layerNumberTiles[OFFSET + i];


        string[] realMatrix = new string[N];

        //Invertimos la matriz
        for (int i = 0; i < N; i++)
            realMatrix[i] = inversedMatrix[N - 1 - i];



        //Lectura de la salud de los Tiles
        string[] layerHealthTiles = layers[2].Split('\n');


        string[] inversedHealthMatrix = new string[N];


        for (int i = 0; i < N; i++)
            inversedHealthMatrix[i] = layerHealthTiles[OFFSET + i];


        string[] realHealthMatrix = new string[N];

        //Invertimos la matriz
        for (int i = 0; i < N; i++)
            realHealthMatrix[i] = inversedHealthMatrix[N - 1 - i];

        _board = new Tile[realMatrix[0].Length - 1, N];


        for (int i = 0; i < N; i++)
        {
            string[] matAux;
            string[] matHealthAux;


            //Obtenemos cada tile
            matAux = realMatrix[i].Split(',', '.');
            matHealthAux = realHealthMatrix[i].Split(',', '.');

            //Rellenamos la matriz
            for (int k = 0; k < matAux.Length - 1; k++)
            {
                int tileType = int.Parse(matAux[k]);

                //Caso en el que existe el tile
                if (tileType == 1)
                {
                    uint tileHealth = uint.Parse(matHealthAux[k]);

                    _board[k, i] = Instantiate(_tilePrefab, transform.position + new Vector3(k, i, 0), Quaternion.identity, transform);
                    _board[k, i].Init(_levelManager,this, tileHealth, k, i);

                    PendingTiles++;
                }
            }
        }

        levelManager.OnRoundEndCallback += OnRoundEnd;
    }


    public void TileDestroyed(Tile tile)
    {
        _board[tile.Y, tile.X] = null;
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

