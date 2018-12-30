using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

    [SerializeField] private Text levelText;
    [SerializeField] private Image [] starsImages;
    [SerializeField] private Text statusText;

    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button resetLevelButton;

    public void Init(bool win, int stars)
    {
        levelText.text = "Level " + (GameManager.Instance.MapLevel + 1).ToString();
        if (win)
        {
            statusText.text = "Win";

            for (int i = 0; i < stars; i++)
                starsImages[i].enabled = true;
        }
        else
        {
            statusText.text = "Fail";
        }
        nextLevelButton.gameObject.SetActive(win);
        resetLevelButton.gameObject.SetActive(!win);
    }

    public void NextLevel()
    {
        GameManager.Instance.MapLevel++;
        MenuCanvas.Instance.PlayLevelOptions("GameScene", true);
    }
    public void ResetLevel()
    {
        MenuCanvas.Instance.PlayLevelOptions("GameScene", true);
    }

    public void GoMenu()
    {
        MenuCanvas.Instance.PlayLevelOptions("MenuScene", true);
    }

}
