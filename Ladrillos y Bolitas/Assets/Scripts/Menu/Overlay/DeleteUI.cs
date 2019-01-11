
/// <summary>
/// Controlador del panel de borrado de datos
/// </summary>
public class DeleteUI : OverlayUI {

    /// <summary>
    /// Borra los datos guardados y los resetea al estado inicial 
    /// </summary>
    public void Delete()
    {
        SaveSystem.DeleteData();
        GameManager.Instance.ResetSaveData();
        LoadSceneManager.Instance.PlayLevel("LogoScene");
    }
}
