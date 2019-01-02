using UnityEngine;

public class ExitUI : MonoBehaviour {

    private MenuManager _menuManager;

    public void Init(MenuManager menuManager)
    {
        _menuManager = menuManager;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ExitMenu()
    {
        _menuManager.Exit = false;
        Destroy(gameObject);
    }

}
