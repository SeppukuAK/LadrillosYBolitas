using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Clase singleton que proporciona utilidades para cargar un nivel haciendo un FadeIn/FadeOut
/// </summary>
public class LoadSceneManager : MonoBehaviour {

    #region PersistentSingleton

    public static LoadSceneManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion PersistentSingleton

    /// <summary>
    /// Imagen de fundido
    /// </summary>
    [SerializeField] private Image faderImage;

    /// <summary>
    /// Tiempo de fundido
    /// </summary>
    [SerializeField] private float sceneFadeTime = 1f;

    /// <summary>
    /// Al empezar la escena, se hace un FadeIn
    /// </summary>
    private void Start()
    {
        faderImage.gameObject.SetActive(true);

        if (faderImage)
            StartCoroutine(UtilitiesManager.FadeImage(faderImage, sceneFadeTime, new Color(0, 0, 0, 0)));//Transparent Color
    }

    /// <summary>
    /// Metodo para iniciar un nivel con un FadeOut
    /// </summary>
    /// <param name="level"></param>
    public void PlayLevel(string level)
    {
        StartCoroutine(UtilitiesManager.FadeImage(faderImage, sceneFadeTime, Color.black));//Transparent Color
        StartCoroutine(PlayLevelCo(sceneFadeTime, level));
    }

    /// <summary>
    /// Metodo para cargar un nivel pasado un tiempo determinado
    /// </summary>
    /// <param name="time"></param>
    /// <param name="nameLevel"></param>
    /// <param name="loading"></param>
    /// <returns></returns>
    private IEnumerator PlayLevelCo(float time, string nameLevel)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(nameLevel);
        StartCoroutine(UtilitiesManager.FadeImage(faderImage, sceneFadeTime, new Color(0, 0, 0, 0)));//Transparent Color
    }
}
