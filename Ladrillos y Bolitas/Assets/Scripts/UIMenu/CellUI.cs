using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellUI : MonoBehaviour
{

    private uint _level;
    private bool _isBlocked; //Atributo para saber si el nivel está disponible o no

    [SerializeField] private Text textLevel;
    [SerializeField] private Image[] starsImage;
    [SerializeField] private GameObject UnlockImage;

    public void Init(uint level, uint stars, bool isBlocked)
    {
        _level = level;
        textLevel.text = (level).ToString();
        _isBlocked = isBlocked;

        //Inicialización de estrellas
        for (int i = 0; i < stars; i++)
        {

            starsImage[i].color = Color.white;
            starsImage[i].enabled = true;
        }

        UnlockImage.SetActive(!_isBlocked);

    }

    public void SetLevel()
    {
        if (!_isBlocked)
        {
            GameManager.Instance.MapLevel = _level - 1;
            MenuCanvas.Instance.PlayLevelOptions("GameScene", true);
        }
    }
}
