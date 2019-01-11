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
        AdjustCameraToAspectRatio();
    }

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
