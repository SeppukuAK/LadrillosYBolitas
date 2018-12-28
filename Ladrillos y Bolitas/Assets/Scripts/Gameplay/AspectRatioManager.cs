using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Componente asociado a la cámara, hace que se visualice en el aspect ratio del dispositivo actual todo el tablero,
/// ajustando el ortographicSize de la cámara.
/// </summary>
public class AspectRatioManager : MonoBehaviour
{
    [Header("Board Size")]
    [SerializeField] private float boardWidth;       //Número de Tiles que caben en el tablero horizontalmente. 1 Tile = 1 unidad de Unity
    [SerializeField] private float boardHeight;      //Número de Tiles que caben en el tablero verticalmente. 1 Tile = 1 unidad de Unity

    [Header("References")]
    [SerializeField] private CanvasScaler canvasScaler;       //Canvas sobre el que actua
    [SerializeField] private RectTransform panelTop;          //Panel superior
    [SerializeField] private RectTransform panelBot;          //Panel inferior

    private void Start()
    {
        //TODO: AWAKE?
        AdjustCameraToAspectRatio();
    }

    //TODO: ELIMINAR UPDATE. SE HACE EN EL START
    //private void Update()
    //{
    //    float TARGET_WIDTH = 960.0f;
    //    float TARGET_HEIGHT = 540.0f;
    //    int PIXELS_TO_UNITS = 30; // 1:1 ratio of pixels to units

    //    float desiredRatio = TARGET_WIDTH / TARGET_HEIGHT;
    //    float currentRatio = (float)Screen.width / (float)Screen.height;

    //    if (currentRatio >= desiredRatio)
    //    {
    //        // Our resolution has plenty of width, so we just need to use the height to determine the camera size
    //        Camera.main.orthographicSize = TARGET_HEIGHT / 4 / PIXELS_TO_UNITS;
    //    }
    //    else
    //    {
    //        // Our camera needs to zoom out further than just fitting in the height of the image.
    //        // Determine how much bigger it needs to be, then apply that to our original algorithm.
    //        float differenceInSize = desiredRatio / currentRatio;
    //        Camera.main.orthographicSize = TARGET_HEIGHT / 4 / PIXELS_TO_UNITS * differenceInSize;
    //    }

    //    /***
    //     * Teniendo el alto del Screen, si sabemos cuantos pixeles reales ocupa una unidad de unity, tenemos que hacer una regla de tres para saber cuantas
    //     * unidades de unity necesitamos
    //     * el orthographicSize es toda la pantalla, ha dicho que necesitamos la mitad
    //     * Puede que entre el ancho de pantalla, si no entra, entonces ajustamos respecto al alto
    //     * si por lo que sea cabe de sobra, se verán los bordes negros
    //     * */
    //}

    /// <summary>
    /// Ajusta el ortographicSize de la cámara para que se vea el tablero con dimensiones BoardHeight y BoardWidth
    /// </summary>
    private void AdjustCameraToAspectRatio()
    {
        //Calculo del tamaño del canvas en pixeles
        float panelTopRealHeight = (panelTop.rect.height * Screen.height) / canvasScaler.referenceResolution.y;
        float panelBotRealHeight = (panelBot.rect.height * Screen.height) / canvasScaler.referenceResolution.y;

        //AspectRatio deseado para que encaje perfecto el tablero en la cámara
        float desiredAspectRatio = boardHeight / boardWidth;

        //Altura en pixeles disponibles para el tablero
        float boardHeightAvailable = Screen.height - panelTopRealHeight - panelBotRealHeight;

        //AspectRatio que obtendríamos usando boardHeight
        float aspectRatioGivenByBoardHeight = boardHeightAvailable / Screen.width;          

        //Escala a aplicar e los pixeles por unidad de la cámara
        float pixelsPerUnitScale;

        //Es mas ancho que alto respecto al deseado
        if (aspectRatioGivenByBoardHeight < desiredAspectRatio)
            pixelsPerUnitScale = boardHeightAvailable / boardHeight; //Se hace más pequeño y se ponen bordes negros a los lados

        else
            pixelsPerUnitScale = Screen.width / boardWidth;  //Se utiliza todo el ancho de la pantalla

        Camera.main.orthographicSize = Screen.height / 2 / pixelsPerUnitScale;
    }

}
