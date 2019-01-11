using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manejador del UI del GameOver
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Text levelText;
    [SerializeField] private Text statusText;
    [SerializeField] private Image[] starsImages;

    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button resetLevelButton;

    /// <summary>
    /// Inicializa el Panel con los atributos necesarios
    /// </summary>
    /// <param name="win"></param>
    /// <param name="stars"></param>
    public void Init(bool win, uint stars)
    {
        levelText.text = "Level " + (GameManager.Instance.SelectedMapLevel + 1).ToString();
        if (win)
        {
            //Guardamos los datos
            if (win)
            {
                GameManager.Instance.LevelData[(int)GameManager.Instance.SelectedMapLevel + 1].Blocked = false;
                GameManager.Instance.LevelData[(int)GameManager.Instance.SelectedMapLevel].Stars = stars;
                GameManager.Instance.SaveData();
            }

            statusText.text = "Win";

            for (int i = 0; i < stars; i++)
                starsImages[i].enabled = true;
        }
        else
            statusText.text = "Fail";

        nextLevelButton.gameObject.SetActive(win);
        resetLevelButton.gameObject.SetActive(!win);
    }

    /// <summary>
    /// Pasa al siguiente nivel
    /// </summary>
    public void NextLevel()
    {
        GameManager.Instance.SelectedMapLevel++;
        MenuCanvas.Instance.PlayLevelOptions("GameScene", true);
    }

    /// <summary>
    /// Resetea este nivel
    /// </summary>
    public void ResetLevel()
    {
        MenuCanvas.Instance.PlayLevelOptions("GameScene", true);
    }

    /// <summary>
    /// Vuelve al menu
    /// </summary>
    public void GoMenu()
    {
        MenuCanvas.Instance.PlayLevelOptions("MenuScene", true);
    }

}
