
/// <summary>
/// Controlador del UI de la pausa
/// </summary>
public class PauseUI : OverlayUI
{
    private LevelManager _levelManager;

    /// <summary>
    /// Inicializacion con las referencias necesarias
    /// </summary>
    /// <param name="levelManager"></param>
    public void Init(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    /// <summary>
    /// Sale de la pausa
    /// </summary>
    public override void Exit()
    {
        _levelManager.Pause = false;
        Destroy(gameObject);
    }

    /// <summary>
    /// Resetea este nivel
    /// </summary>
    public void ResetLevel()
    {
        _levelManager.Pause = false;
        MenuCanvas.Instance.PlayLevelOptions("GameScene", true);
        Destroy(gameObject);
    }

    /// <summary>
    /// Va al menu
    /// </summary>
    public void GoMenu()
    {
        _levelManager.Pause = false;
        MenuCanvas.Instance.PlayLevelOptions("MenuScene", true);
        Destroy(gameObject);
    }
}
