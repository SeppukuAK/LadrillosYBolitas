using UnityEngine;
using UnityEngine.UI;
public class AspectRatioManager : MonoBehaviour {

    public CanvasScaler CanvasScaler;
    public RectTransform PanelTop;
    public RectTransform PanelBot;

    //Unidades de unity
    public float NumTilesHorizontal;
    public float NumTilesVertical;

    private void Update()
    {
        //float TARGET_WIDTH = 960.0f;
        //float TARGET_HEIGHT = 540.0f;
        //int PIXELS_TO_UNITS = 30; // 1:1 ratio of pixels to units

        //float desiredRatio = TARGET_WIDTH / TARGET_HEIGHT;
        //float currentRatio = (float)Screen.width / (float)Screen.height;

        //if (currentRatio >= desiredRatio)
        //{
        //    // Our resolution has plenty of width, so we just need to use the height to determine the camera size
        //    Camera.main.orthographicSize = TARGET_HEIGHT / 4 / PIXELS_TO_UNITS;
        //}
        //else
        //{
        //    // Our camera needs to zoom out further than just fitting in the height of the image.
        //    // Determine how much bigger it needs to be, then apply that to our original algorithm.
        //    float differenceInSize = desiredRatio / currentRatio;
        //    Camera.main.orthographicSize = TARGET_HEIGHT / 4 / PIXELS_TO_UNITS * differenceInSize;
        //}

        AdjustCameraToAspectRatio();
    }

    private void AdjustCameraToAspectRatio() 
    {
        float panelTopRealHeight = (PanelTop.rect.height * Screen.height) / CanvasScaler.referenceResolution.y;
        float panelBotRealHeight = (PanelBot.rect.height * Screen.height) / CanvasScaler.referenceResolution.y;

        float boardRealHeight = Screen.height - panelTopRealHeight - panelBotRealHeight;//Altura en pixeles fisicos

        float boardDesiredAspectRatio = NumTilesVertical / NumTilesHorizontal;
        float boardCurrentActualAspectRatio = boardRealHeight / Screen.width;

        float pixelsPerUnit;

        //Es mas ancho que alto, lo hago más pequeño y lo pongo a los lados
        if (boardCurrentActualAspectRatio < boardDesiredAspectRatio)
            pixelsPerUnit = boardRealHeight / NumTilesVertical;
                                                                                                                        
        else
            pixelsPerUnit = Screen.width / NumTilesHorizontal;        

        Camera.main.orthographicSize = Screen.height / 2 / pixelsPerUnit;

        /***
         * Teniendo el alto del Screen, si sabemos cuantos pixeles reales ocupa una unidad de unity, tenemos que hacer una regla de tres para saber cuantas
         * unidades de unity necesitamos
         * el orthographicSize es toda la pantalla, ha dicho que necesitamos la mitad
         * Puede que entre el ancho de pantalla, si no entra, entonces ajustamos respecto al alto
         * si por lo que sea cabe de sobra, se verán los bordes negros
         * */
    }

}
