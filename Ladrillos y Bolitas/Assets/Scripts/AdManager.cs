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
                Debug.Log("Has ganado 5 gemas");
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
