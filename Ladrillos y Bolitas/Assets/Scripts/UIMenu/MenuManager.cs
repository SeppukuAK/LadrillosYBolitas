using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Manager del menu
/// </summary>
public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private ExitUI exitUIPrefab;
    [SerializeField] private DeleteUI deleteUIPrefab;
    [SerializeField] private ShopManager shopUIPrefab;

    [SerializeField] private CellUI cellUIPrefab;

    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private RectTransform panelTransform;

    [SerializeField] private Text gemsText;
    [SerializeField] private Text totalScoreText;

    [SerializeField] private float offset;

    private List<CellUI> cellList;

    /// <summary>
    /// Panel que se está superponiendo en la escena
    /// </summary>
    private OverlayUI overlayPanel;

    private void Start()
    {
        overlayPanel = null;
        cellList = new List<CellUI>();

        for (int i = 0; i < GameManager.Instance.MapData.Length; i++)
        {
            CellUI cellAux = Instantiate(cellUIPrefab, gridLayoutGroup.transform);
            cellAux.Init(GameManager.Instance.LevelData[i].Level, GameManager.Instance.LevelData[i].Stars, GameManager.Instance.LevelData[i].Blocked);
            cellList.Add(cellAux);
        }
        SetPanelSize();

        totalScoreText.text = GameManager.Instance.TotalStars.ToString();
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (overlayPanel == null)
                overlayPanel = Instantiate(exitUIPrefab);

            else
                overlayPanel.Exit();
        }

    }
    void SetPanelSize()
    {
        int rowCell;//nFilas
        rowCell = (cellList.Count / 5) + 1;

        float tamañoPanel = rowCell * (gridLayoutGroup.cellSize.y + (gridLayoutGroup.spacing.y * 2)) - offset; //BOT

        // Rect rect =  new Rect()
        panelTransform.offsetMin = new Vector2(panelTransform.offsetMin.x, -tamañoPanel);
        panelTransform.offsetMax = new Vector2(panelTransform.offsetMax.x, 0);
    }

    /// <summary>
    /// Cuando el botón de la tienda es pulsado, se spawnea el canvas
    /// </summary>
    public void ShowShop()
    {
        overlayPanel = Instantiate(shopUIPrefab);
        overlayPanel.SetCallbackOnDestroy(UpdateUI);
    }

    /// <summary>
    /// Actualiza el UI a los valores actuales
    /// </summary>
    private void UpdateUI()
    {
        gemsText.text = GameManager.Instance.Gems.ToString();
    }

    /// <summary>
    /// Cuando el botón de ajustes es pulsado, se spawnea el panel de borrado de datos
    /// </summary>
    public void ShowDeletePanel()
    {
        overlayPanel = Instantiate(deleteUIPrefab);
    }

}
