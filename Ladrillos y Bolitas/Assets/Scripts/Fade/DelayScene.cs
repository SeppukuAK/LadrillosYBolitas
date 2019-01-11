using UnityEngine;
using System.Collections;


/// <summary>
/// Clase que espera un retardo para cargar la siguiente escena
/// </summary>
public class DelayScene : MonoBehaviour
{
    //Retardo y nombre de la escena a cargar a continuacion
    public float DelayTime;
    public string NameScene;
    //Indica si muestro pantalla de cargando o no
    public bool IsLoading = true;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(this.DelayTime);
        MenuCanvas.Instance.PlayLevelOptions(this.NameScene, this.IsLoading);
    }
}
