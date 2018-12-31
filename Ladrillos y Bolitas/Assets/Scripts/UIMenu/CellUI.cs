using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellUI : MonoBehaviour {

    private int _level;
    
    [SerializeField] private Text textLevel;
    [SerializeField] private Image[] starsImage;

    public void Init (int level, int stars) {
        _level = level;
        textLevel.text = (level+1).ToString();

        //Inicialización de estrellas
        for (int i = 0; i < stars; i++)
        {
            starsImage[i].enabled = true;
        }
	}
	
    public void OnClick()
    {
        GameManager.Instance.MapLevel = _level;
        MenuCanvas.Instance.PlayLevelOptions("GameScene", true);
    }
}
