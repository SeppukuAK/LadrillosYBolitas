using UnityEngine;

public class MenuManager : MonoBehaviour {

    [Header("UI References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private ExitUI exitUIPrefab;

    public bool Exit
    {
        get { return exit; }
        set
        {
            exit = value;

            if (exit)
                Instantiate(exitUIPrefab, canvas.transform).Init(this);

        }
    }
    private bool exit;

    void Update()
    {
        if (!Exit && Input.GetKeyDown(KeyCode.Escape))
            Exit = true;


    }
}
