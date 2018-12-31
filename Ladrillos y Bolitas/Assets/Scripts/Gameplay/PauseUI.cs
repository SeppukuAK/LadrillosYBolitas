using UnityEngine;

public class PauseUI : MonoBehaviour {

    private LevelManager _levelManager;

    public void Init(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    public void ExitPause()
    {
        _levelManager.Pause = false;
        Destroy(gameObject);
    }

    public void ResetLevel()
    {
        _levelManager.Pause = false;
        MenuCanvas.Instance.PlayLevelOptions("GameScene", true);
        Destroy(gameObject);
    }

    public void GoMenu()
    {
        _levelManager.Pause = false;
        MenuCanvas.Instance.PlayLevelOptions("MenuScene", true);
        Destroy(gameObject);
    }
}
