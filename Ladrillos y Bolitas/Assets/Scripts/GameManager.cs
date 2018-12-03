using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase persistente entre escenas
/// </summary>
public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }


    private void Start()
    {

    }

}
