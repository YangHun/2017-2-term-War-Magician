using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerCircle : MonoBehaviour {

    public float angle;
    public float fadeSpeed;

	// Use this for initialization
	void Start () {
        Color c = Color.white;
        c.a = 0.0f;
        GetComponent<SpriteRenderer>().material.color = c;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0.0f, 0.0f, angle) * Time.deltaTime);
	}

  
}
