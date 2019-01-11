using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Manager del menu
/// Construye las celdas de los niveles
/// </summary>
public class MenuManager : MonoBehaviour
{
    [Header("Prefab References")]
    [SerializeField] private ExitUI exitUIPrefab;
    [SerializeField] private DeleteUI deleteUIPrefab;
    [SerializeField] private ShopManager shopUIPrefab;
    [SerializeField] private CellUI cellUIPrefab;

    [Header("UI References")]
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private RectTransform panelTransform;

    [SerializeField] private Text gemsText;
    [SerializeField] private Text totalScoreText;

    /// <summary>
    /// Panel que se está superponiendo en la escena
    /// </summary>
    private OverlayUI overlayPanel;

    /// <summary>
    /// Crea las celdas de los niveles
    /// </summary>
    private void Start()
    {
        overlayPanel = null;

        for (int i = 0; i < GameManager.Instance.MapData.Length; i++)
           Instantiate(cellUIPrefab, gridLayoutGroup.transform).Init(GameManager.Instance.LevelData[i].Level, GameManager.Instance.LevelData[i].Stars, GameManager.Instance.LevelData[i].Blocked);

        SetGridSize();

        totalScoreText.text = GameManager.Instance.TotalStars.ToString();
        UpdateUI();
    }

    /// <summary>
    /// Establece el tamaño del panel
    /// </summary>
    private void SetGridSize()
    {
        int rowCell;//nFilas
        rowCell = (gridLayoutGroup.transform.childCount / gridLayoutGroup.constraintCount) + 1;

        float tamañoPanel = rowCell * (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y); //BOT

        panelTransform.offsetMin = new Vector2(panelTransform.offsetMin.x, -tamañoPanel);
        panelTransform.offsetMax = new Vector2(panelTransform.offsetMax.x, 0);
    }

    /// <summary>
    /// Si se le da a "return", se spawnea el panel de exit o se destruye el overlayPanel
    /// </summary>
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
