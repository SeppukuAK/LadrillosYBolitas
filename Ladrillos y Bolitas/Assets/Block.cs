using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public Text HealthText;

    private int health;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            HealthText.text = health.ToString();
        }
    }

    private void Start()
    {
        Health = 20;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health--;
        if (health == 0)
            Destroy(gameObject);

    }

}
