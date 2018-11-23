using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Use this for initialization
    void Start () {
        rb.velocity = new Vector2(5.0f, 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
