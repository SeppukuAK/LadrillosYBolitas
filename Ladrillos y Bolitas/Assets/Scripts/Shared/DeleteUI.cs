using UnityEngine;

/// <summary>
/// Controlador del panel de borrado de datos
/// </summary>
public class DeleteUI : MonoBehaviour {

    /// <summary>
    /// Borra los datos guardados y los resetea al estado inicial 
    /// </summary>
    public void Delete()
    {
        SaveSystem.DeleteData();
        GameManager.Instance.ResetSaveData();
        MenuCanvas.Instance.PlayLevelOptions("LogoScene", true);
    }

    /// <summary>
    /// Elimina el panel de borrado
    /// </summary>
    public void Exit()
    {
        Destroy(gameObject);
    }
}
