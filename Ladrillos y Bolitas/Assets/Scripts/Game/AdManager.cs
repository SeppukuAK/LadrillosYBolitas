using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// Manager de los anuncios
/// Si el usuario ve todo el anuncio, le recompensamos con gemas
/// </summary>
public class AdManager : MonoBehaviour {

    public void ShowAd()
    {
        if (Advertisement.IsReady())      
            Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleAdResult });
        
    }
    private void HandleAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                GameManager.Instance.Gems += GameManager.Instance.AdReward; //Incrementamos el número de monedas 
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
