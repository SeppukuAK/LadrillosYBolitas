using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

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
                GameManager.Instance.Coins += 5; //Incrementamos el número de monedas
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
