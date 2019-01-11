using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Celda de un nivel
/// </summary>
public class CellUI : MonoBehaviour
{
    [SerializeField] private Text textLevel;
    [SerializeField] private Image[] starsImage;
    [SerializeField] private GameObject UnlockImage;

    private uint _level;
    private bool _isBlocked; //Atributo para saber si el nivel está disponible o no

    /// <summary>
    /// Crea una celda de un nivel
    /// </summary>
    /// <param name="level"></param>
    /// <param name="stars"></param>
    /// <param name="isBlocked"></param>
    public void Init(uint level, uint stars, bool isBlocked)
    {
        _level = level;
        _isBlocked = isBlocked;

        textLevel.text = level.ToString();

        //Inicialización de estrellas
        for (int i = 0; i < stars; i++)
            starsImage[i].color = Color.white;

        UnlockImage.SetActive(!_isBlocked);
    }

    /// <summary>
    /// Empieza el nivel asociado a la celda
    /// </summary>
    public void SetLevel()
    {
        if (!_isBlocked)
        {
            GameManager.Instance.SelectedMapLevel = _level - 1;
            LoadSceneManager.Instance.PlayLevel("GameScene");
        }
    }
}
