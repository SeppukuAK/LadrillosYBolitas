using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {

    public int X { get; set; }
    public int Y { get; set; }

    LevelManager _levelManager;

    public Text HealthText;

    private uint health;
    public uint Health
    {
        get { return health; }
        set
        {
            health = value;
            HealthText.text = health.ToString();
        }
    }

    public void Init(LevelManager levelManager, uint pendingTouches, int x, int y)
    {
        X = x;
        Y = y;
        Health = pendingTouches;
        _levelManager = levelManager;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health--;
        if (health == 0)
            Destroy(gameObject);

    }

}
