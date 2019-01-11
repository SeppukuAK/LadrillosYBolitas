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
    [SerializeField] private uint earthquakePowerUpCost;

    [Header("References")]
    [SerializeField] private Text gemsText;
    [SerializeField] private Text gemsRewardText;

    [SerializeField] private Text destroyPowerUpCostText;
    [SerializeField] private Text destroyPowerUpNumText;

    [SerializeField] private Text earthquakePowerUpCostText;
    [SerializeField] private Text earthquakePowerUpNumText;

    /// <summary>
    /// Inicializa los textos del UI
    /// </summary>
    private void Start()
    {
        gemsRewardText.text = adReward.ToString();
        gemsText.text = GameManager.Instance.Gems.ToString();

        destroyPowerUpCostText.text = destroyPowerUpCost.ToString();
        destroyPowerUpNumText.text = "x" + GameManager.Instance.PowerUps[(int)PowerUpType.DestroyRow].ToString();

        earthquakePowerUpCostText.text = earthquakePowerUpCost.ToString();
        earthquakePowerUpNumText.text = "x" + GameManager.Instance.PowerUps[(int)PowerUpType.Earthquake].ToString();
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

            GameManager.Instance.SetNumPowerUp(PowerUpType.DestroyRow, GameManager.Instance.PowerUps[(int)PowerUpType.DestroyRow] + 1);
            destroyPowerUpNumText.text = "x" + GameManager.Instance.PowerUps[(int)PowerUpType.DestroyRow].ToString();
        }
    }

    /// <summary>
    /// Es llamado cuando se pulsa el botón de comprar powerUp
    /// Comprueba si hay suficientes gemas y lo compra, actualizando el texto
    /// </summary>
    public void BuyEarthquakePowerUp()
    {
        if (GameManager.Instance.Gems >= earthquakePowerUpCost)
        {
            GameManager.Instance.Gems -= earthquakePowerUpCost;
            gemsText.text = GameManager.Instance.Gems.ToString();

            GameManager.Instance.SetNumPowerUp(PowerUpType.Earthquake, GameManager.Instance.PowerUps[(int)PowerUpType.Earthquake] + 1);
            earthquakePowerUpNumText.text = "x" + GameManager.Instance.PowerUps[(int)PowerUpType.Earthquake].ToString();
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
                if (gemsText != null)
                    gemsText.text = GameManager.Instance.Gems.ToString();
                break;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            case ShowResult.Skipped:
                Debug.Log("No se ha visto el video entero");
                break;
            case ShowResult.Failed:
                Debug.Log("No se ha podido ver el anuncio");
                break;
#endif
        }
    }
}
