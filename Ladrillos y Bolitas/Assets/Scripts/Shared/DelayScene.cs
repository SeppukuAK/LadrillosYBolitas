using UnityEngine;
using System.Collections;

/// <summary>
/// Clase que espera un retardo para cargar la siguiente escena
/// </summary>
public class DelayScene : MonoBehaviour
{
    //Retardo y nombre de la escena a cargar a continuacion
    [SerializeField] private float delayTime;
    [SerializeField] private string nameScene;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delayTime);
        LoadSceneManager.Instance.PlayLevel(nameScene);
    }
}
