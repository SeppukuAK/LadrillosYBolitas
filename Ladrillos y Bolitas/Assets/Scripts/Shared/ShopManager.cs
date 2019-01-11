using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

/// <summary>
/// Controlador de la tienda y su interfaz
/// Gestiona las compras
/// GameObject Panel
/// </summary>
public class ShopManager : OverlayUI
{
    [Header("Shop Attributes")]
    [SerializeField] private uint adReward;
    [SerializeField] private uint destroyPowerUpCost;

    [Header("References")]
    [SerializeField] private Text gemsText;
    [SerializeField] private Text gemsRewardText;

    [SerializeField] private Text destroyPowerUpCostText;
    [SerializeField] private Text destroyPowerUpNumText;

    /// <summary>
    /// Inicializa los textos del UI
    /// </summary>
    private void Start()
    {
        gemsRewardText.text = adReward.ToString();
        destroyPowerUpCostText.text = destroyPowerUpCost.ToString();

        gemsText.text = GameManager.Instance.Gems.ToString();
        destroyPowerUpNumText.text = "x" + GameManager.Instance.PowerUps[(int)GameManager.PowerUpType.DestroyRow].ToString();
    }

    /// <summary>
    /// Es llamado cuando se pulsa el botón de comprar powerUp
    /// Comprueba si hay suficientes gemas y lo compra, actualizando el texto
    /// </summary>
    public void BuyDestroyPowerUp()
    {
        if (GameManager.Instance.Gems >= destroyPowerUpCost)
        {
            GameManager.Instance.Gems -= destroyPowerUpCost;
            gemsText.text = GameManager.Instance.Gems.ToString();

            GameManager.Instance.SetNumPowerUp(GameManager.PowerUpType.DestroyRow, GameManager.Instance.PowerUps[(int)GameManager.PowerUpType.DestroyRow] + 1);
            destroyPowerUpNumText.text = "x" + GameManager.Instance.PowerUps[(int)GameManager.PowerUpType.DestroyRow].ToString();
        }
    }


    /// <summary>
    /// Empieza un anuncio
    /// </summary>
    public void ShowAd()
    {
        if (Advertisement.IsReady())
            Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleAdResult });
    }

    /// <summary>
    /// Es llamado cuando acaba un anuncio.
    /// Recompensamos al jugador si lo ha terminado
    /// </summary>
    /// <param name="result"></param>
    private void HandleAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                GameManager.Instance.Gems += adReward; //Incrementamos el número de monedas 
                gemsText.text = GameManager.Instance.Gems.ToString();
                break;
            case ShowResult.Skipped:
                Debug.Log("No se ha visto el video entero");
                break;
            case ShowResult.Failed:
                Debug.Log("No se ha podido ver el anuncio");
                break;
        }
    }
}
