using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Tablero contenedor de todos los tiles
/// Constructor del tablero y controlador del mismo
/// </summary>
public class Board : MonoBehaviour
{
    public const uint BOARD_HEIGHT = 14;
    public const uint BOARD_WIDTH = 11;

    /// <summary>
    /// Array con los prefabs de los diferentes tiles
    /// </summary>
    [SerializeField] private Tile[] tilePrefabs;
    [SerializeField] private SpriteRenderer alertZoneRenderer;

    /// <summary>
    /// Tiles que quedan por destruir
    /// </summary>
    public uint PendingTiles { get; protected set; }

    /// <summary>
    /// Cuando es modificado activa/desactiva la zona de alerta
    /// </summary>
    private bool alert
    {
        get { return alertZoneRenderer.enabled; }
        set
        {
            alertZoneRenderer.enabled = value;

            if (value)
            {
                alertRoutine = AlertRoutine();
                StartCoroutine(alertRoutine);
            }
            else
            {
                if (alertRoutine != null)
                    StopCoroutine(alertRoutine);
            }
        }
    }
    /// <summary>
    /// Referencia a la corrutina
    /// </summary>
    private IEnumerator alertRoutine;

    /// <summary>
    /// Tiles que se han destruido esta ronda
    /// </summary>
    private uint tilesDestroyedThisRound;

    /// <summary>
    /// Puntos que otorga el primerTile destruido
    /// </summary>
    private uint _tilePoints;

    /// <summary>
    /// Duración de la alerta
    /// </summary>
    private float _alertDuration;

    //Ancho y alto del mapa
    private uint width;
    private uint height;

    //Other References
    private LevelManager _levelManager;
    private Tile[,] _board;


    /// <summary>
    /// Construye el tablero del mapa pasado
    /// Se suscribe a eventos de OnRoundStart y OnRoundEnd
    /// </summary>
    /// <param name="levelManager"></param>
    /// <param name="map"></param>
    public void Init(LevelManager levelManager, TextAsset map, uint tilePoints, float alertDuration)
    {
        _levelManager = levelManager;
        _tilePoints = tilePoints;
        _alertDuration = alertDuration;
        PendingTiles = 0;

        //Division por layers
        string[] layers = map.text.Split(new string[] { "[layer]" }, StringSplitOptions.None);

        //Calculo del tamaño del mapa
        width = (uint)layers[1].Split('\n')[3].Split(',').Length - 1;
        height = (uint)layers[1].Split('\n').Length - 4;

        //Tipos de Tile
        int[,] typeMap = DataStringToIntMap(layers[1].Split(new string[] { "data=" }, StringSplitOptions.None)[1]);

        //Vida del tile
        int[,] healthMap = DataStringToIntMap(layers[2].Split(new string[] { "data=" }, StringSplitOptions.None)[1]);

        height += 1; //Guardamos en board también la linea de "Perder"
        _board = new Tile[height, width];

        //Creación del tablero
        for (uint i = 1; i < height; i++)
        {
            for (uint j = 0; j < width; j++)
            {
                //Si hay algún tipo de bloque
                if (typeMap[height - i - 1, j] > 0)
                {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                    //Si el bloque aún no ha sido implementado
                    if (tilePrefabs[typeMap[height - i - 1, j] - 1] == null)
                        Debug.LogError("Tile: " + typeMap[height - i - 1, j] + " no implementado");
#endif
                    if (tilePrefabs[typeMap[height - i - 1, j] - 1] != null)
                    {
                        Tile tile = tilePrefabs[typeMap[height - i - 1, j] - 1];
                        _board[i, j] = Instantiate(tile, transform.position + new Vector3(j, i, 0), tile.transform.rotation, transform);

                        //Es un tile que se tiene que destruir
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

        //Se suscribe a eventos
        levelManager.OnRoundStartCallback += OnRoundStart;
        levelManager.OnRoundEndCallback += OnRoundEnd;

        //Detectamos si hay alerta al inicio
        alert = DetectAlert();
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

        for (uint i = 0; i < height; i++)
            for (uint j = 0; j < width; j++)
                intMap[i, j] = int.Parse(stringMap[i * width + j].Trim());

        return intMap;
    }

    /// <summary>
    /// Es llamado cuando empieza una tirada
    /// Se quita la alerta y se reestablecen los tilesDestroyedThisRound a 0
    /// </summary>
    private void OnRoundStart()
    {
        alert = false;
        tilesDestroyedThisRound = 0;
    }

    /// <summary>
    /// Es llamado cuando un tile es destruido
    /// Lo elimina del tablero y aumenta los puntos
    /// </summary>
    /// <param name="tile"></param>
    public void TileDestroyed(Tile tile)
    {
        _board[tile.Y, tile.X] = null;
        PendingTiles--;

        tilesDestroyedThisRound++;
        _levelManager.Points += tilesDestroyedThisRound * _tilePoints;

        Destroy(tile.gameObject);
    }

    /// <summary>
    /// Es llamado cuando ha acabado una tirada
    /// Baja todos los ladrillos y comprueba el estado de la partida
    /// </summary>
    private void OnRoundEnd()
    {
        //Si todos los bloques han sido destruidos
        if (PendingTiles == 0)
            _levelManager.LevelEnd();

        else
        {
            //Bajamos todos los tiles
            for (uint i = 1; i < height; i++)
            {
                for (uint j = 0; j < width; j++)
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
                                tile.SetPosition(new Vector2Int((int)j, (int)i - 1));
                            }
                        }
                    }
                }
            }

            //Comprobación si se ha perdido
            bool gameOver = false;
            uint k = 0;
            while (!gameOver && k < width)
            {
                if (_board[0, k] != null)
                    gameOver = true;
                k++;
            }

            //Si se ha perdido
            if (gameOver)
                _levelManager.LevelEnd();

            //No se ha perdido, detectamos si hay que activar la alerta
            else
                alert = DetectAlert();
        }

    }

    /// <summary>
    /// Devuelve si hay que activar la alerta
    /// </summary>
    /// <returns></returns>
    private bool DetectAlert()
    {
        int k = 0;
        bool alert = false;

        while (!alert && k < width)
        {
            if (_board[1, k] != null)
                alert = true;

            k++;
        }

        return alert;
    }

    /// <summary>
    /// Rutina de animación de la alerta
    /// </summary>
    /// <returns></returns>
    private IEnumerator AlertRoutine()
    {
        while (true)
        {
            UtilitiesManager.Instance.FadeInFadeOut(alertZoneRenderer, _alertDuration / 2);
            yield return new WaitForSeconds(_alertDuration);
        }
    }

    #region PowerUps

    /// <summary>
    /// PowerUp: Elimina la fila inferior
    /// </summary>
    public void DestroyRow()
    {
        //Buscamos la fila
        bool found = false;
        uint i = 1;
        uint j = 0;

        while (!found && i < height)
        {
            j = 0;
            while (!found && j < width)
            {
                //Si hay tile
                if (_board[i, j] != null && _board[i, j].MustBeDestroyed())
                    found = true;

                j++;
            }
            i++;
        }


        //Destruye la fila
        i--;
        for (j = j-1; j < width; j++)
        {
            if (_board[i, j] != null && _board[i, j].MustBeDestroyed())
                TileDestroyed(_board[i, j]);
        }

        tilesDestroyedThisRound = 0;

        //Comprobamos si ha destruido todos los bloques
        if (PendingTiles == 0)
            _levelManager.LevelEnd();

    }

    #endregion PowerUps
}

