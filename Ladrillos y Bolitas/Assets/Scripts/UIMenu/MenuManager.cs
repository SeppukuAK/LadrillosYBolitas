using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    [Header("UI References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private ExitUI exitUIPrefab;
    [SerializeField] private CellUI cellUIPrefab;

    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private RectTransform panelTransform;

    [SerializeField] private float offset;

    private List<CellUI> cellList;

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
    
    private void Start()
    {
 
        cellList = new List<CellUI>();

        for (int i = 0; i < GameManager.Instance.MapData.Length; i++)
        {
            CellUI cellAux = Instantiate(cellUIPrefab, gridLayoutGroup.transform);
            cellAux.Init(GameManager.Instance.LevelData[i].Level, GameManager.Instance.LevelData[i].Stars, GameManager.Instance.LevelData[i].Blocked);
            cellList.Add(cellAux);
        }
        SetPanelSize();

    }

    void Update()
    {
        //Borra el ficherito
        if (Input.GetKeyDown(KeyCode.B))
            SaveSystem.DeleteData();

        if (!Exit && Input.GetKeyDown(KeyCode.Escape))
            Exit = true;

    }
    void SetPanelSize()
    {
        int rowCell;//nFilas
        rowCell = (cellList.Count / 5) + 1;

        float tamañoPanel = rowCell * (gridLayoutGroup.cellSize.y + (gridLayoutGroup.spacing.y*2)) - offset ; //BOT
        
        // Rect rect =  new Rect()
        panelTransform.offsetMin = new Vector2(panelTransform.offsetMin.x, -tamañoPanel);
        panelTransform.offsetMax = new Vector2(panelTransform.offsetMax.x, 0);
    }
}
