using UnityEngine;

/// <summary>
/// Manejador del panel de Exit
/// </summary>
public class ExitUI : OverlayUI
{
    /// <summary>
    /// Sale del juego si se pulsa el botón
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
